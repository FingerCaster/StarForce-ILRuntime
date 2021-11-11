using System.IO;
using GameFramework;

namespace UGFExtensions.Editor
{
    public abstract class CodeGeneratorBase
    {
        protected static string HotfixProjectPath = "Assets/Scripts/Hotfix~";
        protected string Suffix = ".cs";
        protected CodeGeneratorBase(string mainCodeFolder, string hotfixCodeFolder)
        {
            MainCodeFolder = mainCodeFolder;
            HotfixCodeFolder = hotfixCodeFolder;
            IsHotfix = false;
        }

        public void SetSuffix(string suffix)
        {
            Suffix = suffix;
        }

        public virtual string GetCodePath()
        {
            return IsHotfix
                ? Utility.Path.GetRegularPath(Path.Combine(HotfixProjectPath, HotfixCodeFolder, CodeName+Suffix))
                : Utility.Path.GetRegularPath(Path.Combine(MainCodeFolder, CodeName+Suffix));
        }

        public virtual string GetCodeFolder()
        {
            return IsHotfix
                ? Utility.Path.GetRegularPath(Path.Combine(HotfixProjectPath, HotfixCodeFolder))
                : MainCodeFolder;
        }

        public void SetCodeName(string codeName)
        {
            CodeName = codeName;
        }

        /// <summary>
        /// 主工程生成代码路径
        /// </summary>
        public string MainCodeFolder { get;protected set; }
        /// <summary>
        /// 热更工程生成代码路径
        /// </summary>
        public string HotfixCodeFolder { get;protected set; }

        /// <summary>
        /// 脚本名称
        /// </summary>
        public string CodeName { get;protected set; }
        /// <summary>
        /// 是否热更
        /// </summary>
        public bool IsHotfix { get; protected set; }

        /// <summary>
        /// 编辑器界面
        /// </summary>
        public abstract void Draw();

        /// <summary>
        /// 生成代码
        /// </summary>
        public abstract bool GenCode();

        public void UpdateHotfixCompile()
        {
            HotfixProjectUtility.AddCompileItem(Path.Combine(HotfixCodeFolder,CodeName+Suffix));
        }
    }
}