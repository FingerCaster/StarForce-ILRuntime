using System;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    /// <summary>
    ///     热更新层游戏入口
    /// </summary>
    public partial class HotfixGameEntry
    {
        /// <summary>
        ///     有限状态机
        /// </summary>
        public FsmManager Fsm { get; private set; }

        /// <summary>
        ///     流程
        /// </summary>
        public ProcedureManager Procedure { get; private set; }

        /// <summary>
        ///     事件
        /// </summary>
        public EventManager Event { get; private set; }

        private bool IsShutDown { get; set; }

        public event Action<float, float> UpdateEvent = null;
        public event Action<bool> OnApplicationPauseEvent = null;
        public event Action OnApplicationQuitEvent = null;

        private bool IsStarted = false;

        private void Start()
        {
            IsShutDown = false;

            Fsm = new FsmManager();
            Procedure = new ProcedureManager();
            Event = new EventManager();
            //初始化流程管理器
            var procedure = new List<ProcedureBase>();
            var typeBase = typeof(ProcedureBase);
            var types = UGFExtensions.GameEntry.Hotfix.GetAllTypes();
            foreach (var type in types)
            {
                if (type.IsClass && !type.IsAbstract && typeBase.IsAssignableFrom(type))
                {
                    procedure.Add((ProcedureBase)Activator.CreateInstance(type));
                }
            }

            Procedure.Initialize(Fsm, procedure.ToArray());
            InitCustomComponents();
            //开始热更新层入口流程
            Procedure.StartProcedure<ProcedureEntry>();
            IsStarted = true;
        }

        private void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (!IsStarted)
            {
                return;
            }
            Fsm.Update(elapseSeconds, realElapseSeconds);
            Event.Update(elapseSeconds, realElapseSeconds);
            UpdateEvent?.Invoke(elapseSeconds, realElapseSeconds);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Log.Info($"Hotfix OnApplicationPause pauseStatus:{pauseStatus}");
            OnApplicationPauseEvent?.Invoke(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            Log.Info("Hotfix OnApplicationQuit");
            OnApplicationQuitEvent?.Invoke();
        }

        private void Shutdown()
        {
            IsShutDown = true;
            ShutDownCustomComponents();
            Procedure.Shutdown();
            Fsm.Shutdown();
            Event.Shutdown();
            s_GameEntry = null;
        }

        private static HotfixGameEntry s_GameEntry;
        public static HotfixGameEntry GameEntry
        {
            get
            {
                if (s_GameEntry == null)
                {
                    s_GameEntry = (HotfixGameEntry)UGFExtensions.GameEntry.Hotfix.GetHotfixGameEntry();
                }

                return s_GameEntry;
            }
        }
    }
}