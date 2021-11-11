using System.IO;
using System.Linq;
using GameFramework;
using UnityEngine;

namespace DE.Editor.DataTableTools
{
    /// <summary>
    /// 热更数据表配置类
    /// </summary>
    public static class HotfixDataTableConfig
    {
        /// <summary>
        /// 数据表存放文件夹路径
        /// </summary>
        public const string DataTableFolderPath = "Assets/Res/DataTables/Hotfix";

        /// <summary>
        /// Excel存放文件夹路径
        /// </summary>
        public static readonly string ExcelsFolder = $"{Application.dataPath}/../Excels/Hotfix";

        /// <summary>
        /// 热更工程所在目录
        /// </summary>
        public const string HotfixProjectFolder= "Assets/Scripts/Hotfix~";
        /// <summary>
        /// 热更工程数据表存放目录（相对于热更工程目录的相对路径）
        /// </summary>
        public const string HotfixDataTableFolder = "DataTable";
        
        /// <summary>
        /// 数据表C#实体类模板存放路径
        /// </summary>
        public const string CSharpCodeTemplateFileName = "Assets/Res/Configs/HotfixDataTableCodeTemplate.txt";

        /// <summary>
        /// 数据表扩展类文件夹路径
        /// </summary>
        public static readonly string ExtensionDirectoryPath = "Assets/Scripts/CustomComponent/DataTable/Runtime/Extensions";
        
        /// <summary>
        /// 数据表命名空间
        /// </summary>
        public const string NameSpace = "UGFExtensions.Hotfix";

        /// <summary>
        /// 数据表中使用类型 所在的所有程序集
        /// </summary>
        public static readonly string[] AssemblyNames =
        {
#if UNITY_2017_3_OR_NEWER
            //asmdef
            "Test",
#endif
            "Assembly-CSharp"
        };

        /// <summary>
        /// 编辑器中使用到的程序集
        /// </summary>
        public static readonly string[] EditorAssemblyNames =
        {
#if UNITY_2017_3_OR_NEWER
            "UnityGameFramework.Editor",
            "DE.Editor",
#endif
            "Assembly-CSharp-Editor"
        };

        /// <summary>
        /// 数据表文件路径
        /// </summary>
        public static string[] TxtFilePaths;

        /// <summary>
        /// 数据表文件名
        /// </summary>
        public static string[] DataTableNames;
        

        /// <summary>
        /// 非热更Excel表文件路径
        /// </summary>
        public static string[] ExcelFilePaths;
        

        static HotfixDataTableConfig()
        {
            RefreshDataTables();
        }

        public static int NameRow = 1;
        public static int TypeRow = 2;
        public static int CommentRow = 3;
        public static int ContentStartRow = 4;

        public static int IdColumn = 1;

        public static void RefreshDataTables()
        {
            if (Directory.Exists(DataTableFolderPath))
            {
                var txtFolder = new DirectoryInfo(DataTableFolderPath);
                TxtFilePaths = txtFolder.GetFiles("*.txt", SearchOption.TopDirectoryOnly)
                    .Select(_ => Utility.Path.GetRegularPath(_.FullName))
                    .ToArray();
                DataTableNames = txtFolder.GetFiles("*.txt", SearchOption.TopDirectoryOnly)
                    .Select(file => Path.GetFileNameWithoutExtension(file.Name))
                    .ToArray();
            }

            if (Directory.Exists(ExcelsFolder))
            {
                var excelFolder = new DirectoryInfo(ExcelsFolder);
                ExcelFilePaths = excelFolder.GetFiles("*.xlsx", SearchOption.AllDirectories)
                    .Where(_ => !_.Name.StartsWith("~$")).Select(_ => Utility.Path.GetRegularPath(_.FullName))
                    .ToArray();
            }
        }
    }
}