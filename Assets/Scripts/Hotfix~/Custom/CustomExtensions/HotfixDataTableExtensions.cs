using System.Threading.Tasks;
using GameFramework.DataTable;
using UGFExtensions.Await;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    public static class HotfixDataTableExtensions
    {
        /// <summary>
        /// 加载热更数据表
        /// </summary>
        /// <param name="dataTableComponent"></param>
        /// <param name="dataTableName"></param>
        /// <param name="fromBytes"></param>
        /// <param name="userData"></param>
        public static void LoadHotfixDataTable(this DataTableComponent dataTableComponent, string dataTableName,
            bool fromBytes, object userData = null)
        {
            if (string.IsNullOrEmpty(dataTableName))
            {
                Log.Warning("Data table name is invalid.");
                return;
            }

            string[] splitedNames = dataTableName.Split('_');
            if (splitedNames.Length > 1)
            {
                Log.Warning("Data table name is invalid.");
                return;
            }
            
            string dataTableAssetName = HotfixAssetUtility.GetDataTable(dataTableName, fromBytes);
            DataTableBase dataTable = dataTableComponent.CreateHotfixDataTable(dataTableName);
            dataTable.ReadData(dataTableAssetName, Constant.AssetPriority.DataTableAsset,
                new LoadHotfixDataTableUserData(dataTableName, userData));
        }

        /// <summary>
        /// 加载数据表（可等待）
        /// </summary>
        public static Task<IDataTable<DRHotfix>> LoadHotfixDataTableAsync(this DataTableComponent dataTableComponent,
            string dataTableName, bool fromBytes, object userData = null)
        {
            string dataTableAssetName = HotfixAssetUtility.GetDataTable(dataTableName, fromBytes);
            return dataTableComponent.LoadHotfixDataTableAsync(dataTableName, dataTableAssetName,
                new LoadHotfixDataTableUserData(dataTableName, userData));
        }

        public static T GetDataRow<T>(this DataTableExtensionComponent dataTableExtension, string dataTableName, int id)
            where T : HotfixDataRowBase
        {
            return dataTableExtension.GetHotfixDataRow(dataTableName, id).GetHotfixData() as T;
        }
        public static T GetDataRow<T>(this IDataTable<DRHotfix> drHotfixes, int id)
            where T : HotfixDataRowBase
        {
            return drHotfixes.GetDataRow(id).GetHotfixData() as T;
        }
        

        private static DataTableBase CreateHotfixDataTable(this DataTableComponent dataTableComponent,
            string dataTableName)
        {
            return dataTableComponent.CreateDataTable(typeof(DRHotfix), dataTableName);
        }

        public static object GetHotfixDataTableUserData(object userData)
        {
            if (userData is LoadHotfixDataTableUserData loadHotfixDataTableUserData)
            {
                return loadHotfixDataTableUserData.UserData;
            }

            return null;
        }
    }
}