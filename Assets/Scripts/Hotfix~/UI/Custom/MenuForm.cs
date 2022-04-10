using UnityEngine;
using UnityGameFramework.Runtime;

//自动生成于：2021/10/29 18:13:15
namespace UGFExtensions.Hotfix
{
    public partial class MenuForm : UGuiForm
    {
        private ProcedureMenu m_ProcedureMenu;
        protected override void OnInit(object userdata)
        {
            base.OnInit(userdata);

            GetBindComponents(gameObject);
            m_CommonButton_Setting.OnHover.AddListener(OnHoverPlaySound);
            m_CommonButton_Setting.OnClick.AddListener(OnSettingButtonClick);
            
            m_CommonButton_Quit.OnHover.AddListener(OnHoverPlaySound);
            m_CommonButton_Quit.OnClick.AddListener(OnQuitButtonClick);
            
            m_CommonButton_About.OnHover.AddListener(OnHoverPlaySound);
            m_CommonButton_About.OnClick.AddListener(OnAboutButtonClick);
                
            m_CommonButton_Start.OnHover.AddListener(OnHoverPlaySound);
            m_CommonButton_Start.OnClick.AddListener(OnStartButtonClick);
        }

        private void OnHoverPlaySound()
        {
            PlayUISound(10000);
        }

        private void OnStartButtonClick()
        {
            PlayUISound(10001);
            m_ProcedureMenu.StartGame();
        }

        private void OnSettingButtonClick()
        {
            PlayUISound(10001);
            GameEntry.UI.OpenUIForm(HotfixUIFormId.SettingForm);
        }

        private void OnAboutButtonClick()
        {
            PlayUISound(10001);
            GameEntry.UI.OpenUIForm(HotfixUIFormId.AboutForm);
        }
        private void OnQuitButtonClick()
        {
            PlayUISound(10001);
            GameEntry.UI.OpenDialog(new DialogParams()
            {
                Mode = 2,
                Title = GameEntry.Localization.GetString("AskQuitGame.Title"),
                Message = GameEntry.Localization.GetString("AskQuitGame.Message"),
                OnClickConfirm = obj =>
                {
                    UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Quit);
                },
            });
        }

        protected override void OnOpen(object userdata)
        {
            base.OnOpen(userdata);

            m_ProcedureMenu = (ProcedureMenu)userdata;
            if (m_ProcedureMenu == null)
            {
                Log.Warning("ProcedureMenu is invalid when open MenuForm.");
                return;
            }

            m_CommonButton_Quit.gameObject.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
        }

        protected override void OnClose(bool isShutdown, object userdata)
        {
            m_ProcedureMenu = null;

            base.OnClose(isShutdown, userdata);
        }
    }
}