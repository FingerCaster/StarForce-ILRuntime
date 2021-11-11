

using System.Collections.Generic;
using UnityGameFramework.Runtime;
using IFsm =  UGFExtensions.Hotfix.IFsm;

namespace UGFExtensions.Hotfix
{
    public class ProcedureMain : ProcedureBase
    {
        private const float GameOverDelayedSeconds = 2f;
        
        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        private GameBase m_CurrentGame = null;
        private bool m_GotoMenu = false;
        private float m_GotoMenuDelaySeconds = 0f;
        
        
        
        public void GotoMenu()
        {
            m_GotoMenu = true;
        }
        
        protected internal override void OnInit(IFsm fsm)
        {
            base.OnInit(fsm);
        
            m_Games.Add(GameMode.Survival, new SurvivalGame());
        }
        
        protected internal override void OnDestroy(IFsm fsm)
        {
            base.OnDestroy(fsm);
        
            m_Games.Clear();
        }
        
        protected internal override void OnEnter(IFsm fsm)
        {
            base.OnEnter(fsm);
        
            m_GotoMenu = false;
            GameMode gameMode = (GameMode)fsm.GetData<VarByte>("GameMode").Value;
            m_CurrentGame = m_Games[gameMode];
            m_CurrentGame.Initialize();
        }
        
        protected internal override void OnLeave(IFsm IFsm, bool isShutdown)
        {
            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }
        
            base.OnLeave(IFsm, isShutdown);
        }
        
        protected internal override void OnUpdate(IFsm fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
        
            if (m_CurrentGame != null && !m_CurrentGame.GameOver)
            {
                m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
                return;
            }
        
            if (!m_GotoMenu)
            {
                m_GotoMenu = true;
                m_GotoMenuDelaySeconds = 0;
            }
        
            m_GotoMenuDelaySeconds += elapseSeconds;
            if (m_GotoMenuDelaySeconds >= GameOverDelayedSeconds)
            {
                fsm.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Menu"));
                ChangeState<ProcedureChangeScene>(fsm);
            }
        }
    }
}
