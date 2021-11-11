using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    public class TestForm : UGuiForm
    {
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            Log.Info("OnInit");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            Log.Info("OnOpen");
        }
        
    }
}