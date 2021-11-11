using System.Threading.Tasks;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Await
{
    public static partial class AwaitableExtension
    {
        /// <summary>
        /// 加载热更数据表（可等待）
        /// </summary>
        public static async Task<IDataTable<DRHotfix>> LoadHotfixDataTableAsync(this DataTableComponent dataTableComponent,
            string dataTableName, string dataTableAssetName, object userData)
        {
#if UNITY_EDITOR
            TipsSubscribeEvent();
#endif
            IDataTable<DRHotfix> dataTable = dataTableComponent.GetDataTable<DRHotfix>(dataTableName);
            if (dataTable != null)
            {
                return await Task.FromResult(dataTable);
            }

            var loadTcs = new TaskCompletionSource<bool>();
            s_DataTableTcs.Add(dataTableAssetName, loadTcs);
            dataTableComponent.LoadDataTable($"Hotfix_{dataTableName}", dataTableAssetName, userData);
            bool isLoaded = await loadTcs.Task;
            dataTable = isLoaded ? dataTableComponent.GetDataTable<DRHotfix>(dataTableName) : null;
            return await Task.FromResult(dataTable);
        }
    }
}