using System.Threading.Tasks;
using UGFExtensions.Await;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    public static class HotfixUIExtensions
    {
        /// <summary>
        /// 打开界面
        /// </summary>
        public static int? OpenUIForm(this UIComponent uiComponent, HotfixUIFormId uiFormId, object userData = null)
        {
            return uiComponent.OpenUIForm((int) uiFormId, userData);
        }
        
        /// <summary>
        /// 打开界面（可等待）
        /// </summary>
        public static Task<UIForm> OpenUIFormAsync(this UIComponent self, HotfixUIFormId uiFormId, object userData = null)
        {
            return self.OpenUIFormAsync((int) uiFormId, userData);
        }
    }
}