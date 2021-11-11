

using GameFramework.Event;
using UGFExtensions.Await;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool m_StartGame = false;
        private MenuForm m_MenuForm = null;
        

        public void StartGame()
        {
            m_StartGame = true;
        }
        
        protected internal override async void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);
            
            m_StartGame = false;
            
            UIForm uiForm = await GameEntry.UI.OpenUIFormAsync(HotfixUIFormId.MenuForm, this);
            m_MenuForm = (MenuForm) uiForm.Logic;
        }
        
        protected internal override void OnLeave(IFsm procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            if (m_MenuForm != null)
            {
                m_MenuForm.Close();
                m_MenuForm = null;
            }
        }
        
        protected internal override void OnUpdate(IFsm procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        
            if (m_StartGame)
            {
                procedureOwner.SetData<VarInt32>("NextSceneId", GameEntry.Config.GetInt("Scene.Main"));
                procedureOwner.SetData<VarByte>("GameMode", (byte)GameMode.Survival);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }
    }
}
