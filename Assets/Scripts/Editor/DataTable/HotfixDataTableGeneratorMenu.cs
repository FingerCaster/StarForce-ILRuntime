using System.IO;
using GameFramework;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

namespace DE.Editor.DataTableTools
{
    public class HotfixDataTableGeneratorMenu
    {
        [MenuItem("Tools/DataTable/Generate Hotfix DataTables/From Txt", priority = 2)]
        public static void GenerateDataTablesFromTxtNotFileSystem()
        {
            HotfixDataTableConfig.RefreshDataTables();
            ExtensionsGenerate.GenerateExtensionByAnalysis(ExtensionsGenerate.DataTableType.Txt,DataTableConfig.TxtFilePaths, 2);
            foreach (var dataTableName in HotfixDataTableConfig.DataTableNames)
            {
                var dataTableProcessor = HotfixDataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!HotfixDataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                HotfixDataTableGenerator.GenerateDataFile(dataTableProcessor, dataTableName);
                HotfixDataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/DataTable/Generate Hotfix DataTables/From Excel", priority = 2)]
        public static void GenerateDataTablesFormExcelNotFileSystem()
        {
            HotfixDataTableConfig.RefreshDataTables();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExtensionsGenerate.GenerateExtensionByAnalysis(ExtensionsGenerate.DataTableType.Excel,DataTableConfig.ExcelFilePaths, 2);
            foreach (var excelFile in HotfixDataTableConfig.ExcelFilePaths)
            {
                using (FileStream fileStream =
                    new FileStream(excelFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(fileStream))
                    {
                        for (int i = 0; i < excelPackage.Workbook.Worksheets.Count; i++)
                        {
                            ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[i];
                            var dataTableProcessor = HotfixDataTableGenerator.CreateExcelDataTableProcessor(sheet);
                            if (!HotfixDataTableGenerator.CheckRawData(dataTableProcessor, sheet.Name))
                            {
                                Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'",
                                    sheet.Name));
                                break;
                            }

                            HotfixDataTableGenerator.GenerateDataFile(dataTableProcessor, sheet.Name);
                            HotfixDataTableGenerator.GenerateCodeFile(dataTableProcessor, sheet.Name);
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/DataTable/Generate Hotfix DataTables/From Txt Use FileSystem", priority = 20)]
        public static void GenerateDataTablesFromTxtFileSystem()
        {
            HotfixDataTableConfig.RefreshDataTables();
            ExtensionsGenerate.GenerateExtensionByAnalysis(ExtensionsGenerate.DataTableType.Txt,DataTableConfig.TxtFilePaths, 2);
            foreach (var dataTableName in HotfixDataTableConfig.DataTableNames)
            {
                var dataTableProcessor = HotfixDataTableGenerator.CreateDataTableProcessor(dataTableName);
                if (!HotfixDataTableGenerator.CheckRawData(dataTableProcessor, dataTableName))
                {
                    Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'", dataTableName));
                    break;
                }

                HotfixDataTableGenerator.GenerateFileSystemFile(dataTableProcessor, dataTableName);
                HotfixDataTableGenerator.GenerateCodeFile(dataTableProcessor, dataTableName);
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/DataTable/Generate Hotfix DataTables/From Excel Use FileSystem", priority = 20)]
        public static void GenerateDataTablesFormExcelFileSystem()
        {
            HotfixDataTableConfig.RefreshDataTables();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExtensionsGenerate.GenerateExtensionByAnalysis(ExtensionsGenerate.DataTableType.Excel, DataTableConfig.ExcelFilePaths,2);
            foreach (var excelFile in HotfixDataTableConfig.ExcelFilePaths)
            {
                using (FileStream fileStream =
                    new FileStream(excelFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (ExcelPackage excelPackage = new ExcelPackage(fileStream))
                    {
                        for (int i = 0; i < excelPackage.Workbook.Worksheets.Count; i++)
                        {
                            ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[i];
                            var dataTableProcessor = HotfixDataTableGenerator.CreateExcelDataTableProcessor(sheet);
                            if (!HotfixDataTableGenerator.CheckRawData(dataTableProcessor, sheet.Name))
                            {
                                Debug.LogError(Utility.Text.Format("Check raw data failure. DataTableName='{0}'",
                                    sheet.Name));
                                break;
                            }

                            HotfixDataTableGenerator.GenerateFileSystemFile(dataTableProcessor, sheet.Name);
                            HotfixDataTableGenerator.GenerateCodeFile(dataTableProcessor, sheet.Name);
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }
    }
}