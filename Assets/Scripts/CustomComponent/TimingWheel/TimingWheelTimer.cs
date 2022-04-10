﻿using System;
using System.Threading;
using System.Threading.Tasks;
using DelayQueue;
using ET;
using TimingWheel.Extensions;
using TimingWheel.Interfaces;
using UGFExtensions;

namespace TimingWheel
{
    /// <summary>
    /// 时间轮计时器，参考kafka时间轮算法实现
    /// </summary>
    public class TimingWheelTimer : ITimer
    {
        /// <summary>
        /// 时间槽延时队列，和时间轮共用
        /// </summary>
        private readonly DelayQueue<TimeSlot> m_DelayQueue = new DelayQueue<TimeSlot>();

        /// <summary>
        /// 时间轮
        /// </summary>
        private readonly TimingWheel m_TimingWheel;

        /// <summary>
        /// 任务总数
        /// </summary>
        private readonly AtomicInt m_TaskCount = new AtomicInt();

        /// <summary>
        /// 任务总数
        /// </summary>
        public int TaskCount => m_TaskCount.Get();

        private CancellationTokenSource m_CancelTokenSource;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tickSpan">时间槽大小，毫秒</param>
        /// <param name="slotCount">时间槽数量</param>
        /// <param name="startMs">起始时间戳，标识时间轮创建时间</param>
        private TimingWheelTimer(long tickSpan, int slotCount, long startMs)
        {
            m_TimingWheel = new TimingWheel(tickSpan, slotCount, startMs, m_TaskCount, m_DelayQueue);
        }

        /// <summary>
        /// 构建时间轮计时器
        /// </summary>
        /// <param name="tickSpan">时间槽大小</param>
        /// <param name="slotCount">时间槽数量</param>
        /// <param name="startMs">起始时间戳，标识时间轮创建时间，默认当前时间</param>
        public static ITimer Build(TimeSpan tickSpan, int slotCount, long? startMs = null)
        {
            return new TimingWheelTimer((long) tickSpan.TotalMilliseconds,
                slotCount,
                startMs ?? DateTimeHelper.GetTimestamp());
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="timeout">过期时间，相对时间</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public ETTask<bool> AddTask(TimeSpan timeout, ETCancellationToken cancellationToken = default)
        {
            var timeoutMs = DateTimeHelper.GetTimestamp() + (long) timeout.TotalMilliseconds;
            return AddTask(timeoutMs, cancellationToken);
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="timeout">过期时间，相对时间</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public ITimeTask AddTask(TimeSpan timeout, Action<bool> action)
        {
            var timeoutMs = DateTimeHelper.GetTimestamp() + (long) timeout.TotalMilliseconds;
            return AddTask(timeoutMs,action);
        }


        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="timeoutMs">过期时间戳，绝对时间</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ETTask<bool> AddTask(long timeoutMs, ETCancellationToken cancellationToken = default)
        {
            var task = TimeTask.Create(timeoutMs);
            AddTask(task);
            void CancelAction()
            {
                task.Remove();
                ETTask<bool> etTask = (ETTask<bool>) task.DelayTask;
                etTask.SetResult(false);
            }
            bool result;
            try
            {
                cancellationToken?.Add(CancelAction);
                result = await (ETTask<bool>) task.DelayTask;
            }
            finally
            {
                cancellationToken?.Remove(CancelAction);
            }
            if (task.TimeSlot == null)
            {
                return true;
            }
            return result;
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="timeoutMs">过期时间戳，绝对时间</param>
        /// <param name="action"></param>
        /// <returns></returns>
        public ITimeTask AddTask(long timeoutMs, Action<bool> action)
        {
            var task = TimeTask.Create(timeoutMs, action);
            AddTask(task);
            return task;
        }

        /// <summary>
        /// 启动
        /// </summary>
        public void Start()
        {
            if (m_CancelTokenSource != null)
            {
                return;
            }

            m_CancelTokenSource = new CancellationTokenSource();

            // 时间轮运行线程
            Task.Factory.StartNew(() => Run(m_CancelTokenSource.Token),
                m_CancelTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            Cancel();
            m_DelayQueue.Clear();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            Cancel();
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void Resume()
        {
            Start();
        }

        /// <summary>
        /// 取消任务
        /// </summary>
        private void Cancel()
        {
            if (m_CancelTokenSource != null)
            {
                m_CancelTokenSource.Cancel();
                m_CancelTokenSource.Dispose();
                m_CancelTokenSource = null;
            }
        }

        /// <summary>
        /// 运行
        /// </summary>
        private void Run(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    Step(token);
                }
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    return;
                }

                throw;
            }
        }

        /// <summary>
        /// 推进时间轮
        /// </summary>
        /// <param name="token"></param>
        private void Step(CancellationToken token)
        {
            // 阻塞式获取，到期的时间槽才会出队
            if (m_DelayQueue.TryTake(out var slot, token))
            {
                while (!token.IsCancellationRequested)
                {
                    // 推进时间轮
                    m_TimingWheel.Step(slot.TimeoutMs.Get());

                    // 到期的任务会重新添加进时间轮，那么下一层时间轮的任务重新计算后可能会进入上层时间轮。
                    // 这样就实现了任务在时间轮中的传递，由大精度的时间轮进入小精度的时间轮。
                    slot.Flush(AddTask);

                    // Flush之后可能有新的slot入队，可能仍旧过期，因此尝试继续处理，直到没有过期项。
                    if (!m_DelayQueue.TryTakeNoBlocking(out slot))
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="timeTask">延时任务</param>
        private void AddTask(TimeTask timeTask)
        {
            // 添加失败，说明该任务已到期，需要执行了
            if (m_TimingWheel.AddTask(timeTask)) return;
            if (timeTask.IsWaiting)
            {
                Loom.Instance.Post(timeTask.Run);
            }
        }
    }
}