using UnityEditor;
using UnityGameFramework.Editor;

namespace UGFExtensions.Editor
{
    public class ILRuntimeScriptingDefineSymbols
    {
        private const string EnableILRuntimeScriptingDefineSymbol = "ILRuntime";
        private const string EnableILRuntimeReleaseScriptingDefineSymbol = "DISABLE_ILRUNTIME_DEBUG";

        /// <summary>
        /// 禁用ILRuntime。
        /// </summary>
        [MenuItem("Tools/ILRuntime/DisableILRuntime", false, 30)]
        public static void DisableILRuntime()
        {
            ScriptingDefineSymbols.RemoveScriptingDefineSymbol(EnableILRuntimeScriptingDefineSymbol);
            ScriptingDefineSymbols.RemoveScriptingDefineSymbol(EnableILRuntimeReleaseScriptingDefineSymbol);
        }

        /// <summary>
        /// 启用ILRuntime。
        /// </summary>
        [MenuItem("Tools/ILRuntime/EnableILRuntime", false, 31)]
        public static void EnableILRuntime()
        {
            ScriptingDefineSymbols.AddScriptingDefineSymbol(EnableILRuntimeScriptingDefineSymbol);
        }

        /// <summary>
        /// 启用ILRuntime Release模式。
        /// </summary>
        [MenuItem("Tools/ILRuntime/EnableILRuntimeRelease", false, 32)]
        public static void EnableILRuntimeRelease()
        {
            BuildTargetGroup BuildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            if (!ScriptingDefineSymbols.HasScriptingDefineSymbol(BuildTargetGroup.Android,
                EnableILRuntimeScriptingDefineSymbol))
            {
                if (EditorUtility.DisplayDialog("", "当前脚本模式不是ILRuntime,是否开启？", "确定", "取消"))
                {
                    EnableILRuntime();
                }
            }
            else
            {
                ScriptingDefineSymbols.AddScriptingDefineSymbol(EnableILRuntimeReleaseScriptingDefineSymbol);
            }
        }
        /// <summary>
        /// 禁用ILRuntime Release模式。
        /// </summary>
        [MenuItem("Tools/ILRuntime/DisableILRuntimeRelease", false, 33)]
        public static void DisableILRuntimeRelease()
        {
            ScriptingDefineSymbols.RemoveScriptingDefineSymbol(EnableILRuntimeReleaseScriptingDefineSymbol);
        }
    }
}