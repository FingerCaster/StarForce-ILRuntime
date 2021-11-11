//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using GameFramework;

namespace UGFExtensions.Hotfix
{
    /// <summary>
    ///     有限状态机。
    /// </summary>
    public sealed class Fsm : FsmBase, IFsm
    {
        private readonly Dictionary<string, Variable> m_Datas;
        private readonly Dictionary<string, FsmState> m_States;
        private float m_CurrentStateTime;
        private bool m_IsDestroyed;

        /// <summary>
        ///     初始化有限状态机的新实例。
        /// </summary>
        /// <param name="name">有限状态机名称。</param>
        /// <param name="owner">有限状态机持有者。</param>
        /// <param name="states">有限状态机状态集合。</param>
        public Fsm(string name, object owner, params FsmState[] states)
            : base(name)
        {
            if (owner == null) throw new GameFrameworkException("FSM owner is invalid.");

            if (states == null || states.Length < 1) throw new GameFrameworkException("FSM states is invalid.");

            Owner = owner;
            m_States = new Dictionary<string, FsmState>();
            m_Datas = new Dictionary<string, Variable>();

            foreach (var state in states)
            {
                if (state == null) throw new GameFrameworkException("FSM states is invalid.");

                var stateName = state.GetType().FullName;
                if (m_States.ContainsKey(stateName))
                    throw new GameFrameworkException(Utility.Text.Format("FSM '{0}' state '{1}' is already exist.",
                        TextUtility.GetFullName(OwnerType, name), stateName));

                m_States.Add(stateName, state);
                state.OnInit(this);
            }

            m_CurrentStateTime = 0f;
            CurrentState = null;
            m_IsDestroyed = false;
        }

        /// <summary>
        ///     获取有限状态机持有者类型。
        /// </summary>
        public override Type OwnerType => Owner.GetType();

        /// <summary>
        ///     获取当前有限状态机状态名称。
        /// </summary>
        public override string CurrentStateName => CurrentState != null ? CurrentState.GetType().FullName : null;

        /// <summary>
        ///     获取有限状态机持有者。
        /// </summary>
        public object Owner { get; }

        /// <summary>
        ///     获取有限状态机中状态的数量。
        /// </summary>
        public override int FsmStateCount => m_States.Count;

        /// <summary>
        ///     获取有限状态机是否正在运行。
        /// </summary>
        public override bool IsRunning => CurrentState != null;

        /// <summary>
        ///     获取有限状态机是否被销毁。
        /// </summary>
        public override bool IsDestroyed => m_IsDestroyed;

        /// <summary>
        ///     获取当前有限状态机状态。
        /// </summary>
        public FsmState CurrentState { get; private set; }

        /// <summary>
        ///     获取当前有限状态机状态持续时间。
        /// </summary>
        public override float CurrentStateTime => m_CurrentStateTime;

        /// <summary>
        ///     开始有限状态机。
        /// </summary>
        /// <typeparam name="TState">要开始的有限状态机状态类型。</typeparam>
        public void Start<TState>() where TState : FsmState
        {
            if (IsRunning) throw new GameFrameworkException("FSM is running, can not start again.");

            FsmState state = GetState<TState>();
            if (state == null)
                throw new GameFrameworkException(Utility.Text.Format(
                    "FSM '{0}' can not start state '{1}' which is not exist.", TextUtility.GetFullName(OwnerType, Name),
                    typeof(TState).FullName));

            m_CurrentStateTime = 0f;
            CurrentState = state;
            CurrentState.OnEnter(this);
        }

        /// <summary>
        ///     开始有限状态机。
        /// </summary>
        /// <param name="stateType">要开始的有限状态机状态类型。</param>
        public void Start(Type stateType)
        {
            if (IsRunning) throw new GameFrameworkException("FSM is running, can not start again.");

            if (stateType == null) throw new GameFrameworkException("State type is invalid.");

            if (!typeof(FsmState).IsAssignableFrom(stateType))
                throw new GameFrameworkException(
                    Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));

            var state = GetState(stateType);
            if (state == null)
                throw new GameFrameworkException(Utility.Text.Format(
                    "FSM '{0}' can not start state '{1}' which is not exist.", TextUtility.GetFullName(OwnerType, Name),
                    stateType.FullName));

            m_CurrentStateTime = 0f;
            CurrentState = state;
            CurrentState.OnEnter(this);
        }

        /// <summary>
        ///     是否存在有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要检查的有限状态机状态类型。</typeparam>
        /// <returns>是否存在有限状态机状态。</returns>
        public bool HasState<TState>() where TState : FsmState
        {
            return m_States.ContainsKey(typeof(TState).FullName);
        }

        /// <summary>
        ///     是否存在有限状态机状态。
        /// </summary>
        /// <param name="stateType">要检查的有限状态机状态类型。</param>
        /// <returns>是否存在有限状态机状态。</returns>
        public bool HasState(Type stateType)
        {
            if (stateType == null) throw new GameFrameworkException("State type is invalid.");

            if (!typeof(FsmState).IsAssignableFrom(stateType))
                throw new GameFrameworkException(
                    Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));

            return m_States.ContainsKey(stateType.FullName);
        }

        /// <summary>
        ///     获取有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要获取的有限状态机状态类型。</typeparam>
        /// <returns>要获取的有限状态机状态。</returns>
        public TState GetState<TState>() where TState : FsmState
        {
            FsmState state = null;
            if (m_States.TryGetValue(typeof(TState).FullName, out state)) return (TState) state;

            return null;
        }

        /// <summary>
        ///     获取有限状态机状态。
        /// </summary>
        /// <param name="stateType">要获取的有限状态机状态类型。</param>
        /// <returns>要获取的有限状态机状态。</returns>
        public FsmState GetState(Type stateType)
        {
            if (stateType == null) throw new GameFrameworkException("State type is invalid.");

            if (!typeof(FsmState).IsAssignableFrom(stateType))
                throw new GameFrameworkException(
                    Utility.Text.Format("State type '{0}' is invalid.", stateType.FullName));

            FsmState state = null;
            if (m_States.TryGetValue(stateType.FullName, out state)) return state;

            return null;
        }

        /// <summary>
        ///     获取有限状态机的所有状态。
        /// </summary>
        /// <returns>有限状态机的所有状态。</returns>
        public FsmState[] GetAllStates()
        {
            var index = 0;
            var results = new FsmState[m_States.Count];
            foreach (var state in m_States) results[index++] = state.Value;

            return results;
        }

        /// <summary>
        ///     获取有限状态机的所有状态。
        /// </summary>
        /// <param name="results">有限状态机的所有状态。</param>
        public void GetAllStates(List<FsmState> results)
        {
            if (results == null) throw new GameFrameworkException("Results is invalid.");

            results.Clear();
            foreach (var state in m_States) results.Add(state.Value);
        }

        /// <summary>
        ///     抛出有限状态机事件。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="eventId">事件编号。</param>
        public void FireEvent(object sender, int eventId)
        {
            if (CurrentState == null) throw new GameFrameworkException("Current state is invalid.");

            CurrentState.OnEvent(this, sender, eventId, null);
        }

        /// <summary>
        ///     抛出有限状态机事件。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="eventId">事件编号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void FireEvent(object sender, int eventId, object userData)
        {
            if (CurrentState == null) throw new GameFrameworkException("Current state is invalid.");

            CurrentState.OnEvent(this, sender, eventId, userData);
        }

        /// <summary>
        ///     是否存在有限状态机数据。
        /// </summary>
        /// <param name="name">有限状态机数据名称。</param>
        /// <returns>有限状态机数据是否存在。</returns>
        public bool HasData(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new GameFrameworkException("Data name is invalid.");

            return m_Datas.ContainsKey(name);
        }

        /// <summary>
        ///     获取有限状态机数据。
        /// </summary>
        /// <typeparam name="TData">要获取的有限状态机数据的类型。</typeparam>
        /// <param name="name">有限状态机数据名称。</param>
        /// <returns>要获取的有限状态机数据。</returns>
        public TData GetData<TData>(string name) where TData : Variable
        {
            return (TData) GetData(name);
        }

        /// <summary>
        ///     获取有限状态机数据。
        /// </summary>
        /// <param name="name">有限状态机数据名称。</param>
        /// <returns>要获取的有限状态机数据。</returns>
        public Variable GetData(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new GameFrameworkException("Data name is invalid.");

            Variable data = null;
            if (m_Datas.TryGetValue(name, out data)) return data;

            return null;
        }

        /// <summary>
        ///     设置有限状态机数据。
        /// </summary>
        /// <typeparam name="TData">要设置的有限状态机数据的类型。</typeparam>
        /// <param name="name">有限状态机数据名称。</param>
        /// <param name="data">要设置的有限状态机数据。</param>
        public void SetData<TData>(string name, TData data) where TData : Variable
        {
            if (string.IsNullOrEmpty(name)) throw new GameFrameworkException("Data name is invalid.");

            m_Datas[name] = data;
        }

        /// <summary>
        ///     设置有限状态机数据。
        /// </summary>
        /// <param name="name">有限状态机数据名称。</param>
        /// <param name="data">要设置的有限状态机数据。</param>
        public void SetData(string name, Variable data)
        {
            if (string.IsNullOrEmpty(name)) throw new GameFrameworkException("Data name is invalid.");

            m_Datas[name] = data;
        }

        /// <summary>
        ///     移除有限状态机数据。
        /// </summary>
        /// <param name="name">有限状态机数据名称。</param>
        /// <returns>是否移除有限状态机数据成功。</returns>
        public bool RemoveData(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new GameFrameworkException("Data name is invalid.");

            return m_Datas.Remove(name);
        }

        /// <summary>
        ///     有限状态机轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (CurrentState == null) return;

            m_CurrentStateTime += elapseSeconds;
            CurrentState.OnUpdate(this, elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        ///     关闭并清理有限状态机。
        /// </summary>
        internal override void Shutdown()
        {
            if (CurrentState != null)
            {
                CurrentState.OnLeave(this, true);
                CurrentState = null;
                m_CurrentStateTime = 0f;
            }

            foreach (var state in m_States) state.Value.OnDestroy(this);

            m_States.Clear();
            m_Datas.Clear();

            m_IsDestroyed = true;
        }

        /// <summary>
        ///     切换当前有限状态机状态。
        /// </summary>
        /// <typeparam name="TState">要切换到的有限状态机状态类型。</typeparam>
        internal void ChangeState<TState>() where TState : FsmState
        {
            ChangeState(typeof(TState));
        }

        /// <summary>
        ///     切换当前有限状态机状态。
        /// </summary>
        /// <param name="stateType">要切换到的有限状态机状态类型。</param>
        internal void ChangeState(Type stateType)
        {
            if (CurrentState == null) throw new GameFrameworkException("Current state is invalid.");

            var state = GetState(stateType);
            if (state == null)
                throw new GameFrameworkException(Utility.Text.Format(
                    "FSM '{0}' can not change state to '{1}' which is not exist.",
                    TextUtility.GetFullName(OwnerType, Name), stateType.FullName));

            CurrentState.OnLeave(this, false);
            m_CurrentStateTime = 0f;
            CurrentState = state;
            CurrentState.OnEnter(this);
        }
    }
}