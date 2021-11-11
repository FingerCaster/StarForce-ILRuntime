//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Threading.Tasks;
using UnityGameFramework.Runtime;
using ProcedureOwner = UGFExtensions.Hotfix.IFsm;

namespace UGFExtensions.Hotfix
{
    /// <summary>
    ///     流程基类。
    /// </summary>
    public abstract class ProcedureBase : FsmState
    {
        /// <summary>
        ///     状态初始化时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        protected internal override void OnInit(IFsm procedureOwner)
        {
            base.OnInit(procedureOwner);
        }


        /// <summary>
        ///     进入状态时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);
            Log.Info("Enter '{0}' procedure.", GetType().FullName);
        }

        /// <summary>
        ///     状态轮询时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        protected internal override void OnUpdate(IFsm procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        ///     离开状态时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        /// <param name="isShutdown">是否是关闭状态机时触发。</param>
        protected internal override void OnLeave(IFsm procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            Log.Info("Leave '{0}' procedure.", GetType().FullName);
        }

        /// <summary>
        ///     状态销毁时调用。
        /// </summary>
        /// <param name="procedureOwner">流程持有者。</param>
        protected internal override void OnDestroy(IFsm procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}