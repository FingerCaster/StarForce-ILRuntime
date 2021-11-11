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

            if (config.FileStream != null)
            {
                AddHotfixDataRow(dataTableBase,dataTableName, config.FileStream, value.StartIndex, value.Length);
            }
            else
            {
                using (IFileStream fileStream = FileStreamHelper.CreateFileStream(config.Path))
                {
                    AddHotfixDataRow(dataTableBase,dataTableName, fileStream, value.StartIndex, value.Length);
                }
            }

            return dataTableBase.GetDataRow(id);
        }
        private void AddHotfixDataRow(IDataTable<DRHotfix> dataTable,string dataTableName, IFileStream fileStream, int startIndex, int length)
        {
            fileStream.Seek(startIndex, SeekOrigin.Begin);
            EnsureBufferSize(length);
            long readLength = fileStream.Read(m_Buffer, 0, length);
            dataTable.AddDataRow(m_Buffer, 0, (int)readLength, new LoadHotfixDataTableUserData(dataTableName,null));
        }
    }
}