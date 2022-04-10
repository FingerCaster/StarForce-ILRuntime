using System.IO;
using GameFramework.DataTable;

namespace UGFExtensions
{
    public partial class DataTableExtensionComponent
    {
        public DRHotfix GetHotfixDataRow(string dataTableName,int id)
        {
            m_DataTableRowConfigs.TryGetValue(new TypeNamePair(typeof(DRHotfix),dataTableName), out var config);
            if (config == null) return default;
            config.DataTableRowSettings.TryGetValue(id, out var value);
            if (value == null) return default;
            IDataTable<DRHotfix> dataTableBase = GameEntry.DataTable.GetDataTable<DRHotfix>(dataTableName);
            if (dataTableBase.HasDataRow(id))
            {
                return dataTableBase.GetDataRow(id);
            }

            config.DataProvider.ReadFileSegment(value.StartIndex, ref m_Buffer, 0, value.Length);
            var realLength = config.DataProvider.ReadFileSegment(value.StartIndex, ref m_Buffer, 0,
                value.Length);
            dataTableBase.AddDataRow(m_Buffer, 0, (int)realLength, new LoadHotfixDataTableUserData(dataTableName,null));
            return dataTableBase.GetDataRow(id);
        }
    }
}