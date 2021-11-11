using System;
using System.IO;
using System.Reflection;
using System.Text;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;
using GameEntry = UGFExtensions.GameEntry;

namespace UGFExtensions
{
    public class DRHotfix : DataRowBase
    {
        private int m_Id = 0;

        /// <summary>
        /// 对应的热更新层实体逻辑类实例
        /// </summary>
        private object m_HotfixInstance;

        /// <summary>
        /// 获取编号。
        /// </summary>
        public override int Id => m_Id;

        private PropertyInfo m_IdPropertyInfo;
        

        public override bool ParseDataRow(string dataRowString, object userData)
        {
            LoadHotfixDataTableUserData hotfixDataTableUserData = userData as LoadHotfixDataTableUserData;
            if (hotfixDataTableUserData == null)
            {
                return false;
            }

            m_HotfixInstance = GameEntry.Hotfix.CreateHotfixInstance(hotfixDataTableUserData.DataTableName);
            m_IdPropertyInfo = m_HotfixInstance.GetType().GetProperty("Id");
            object parseDataRow =  GameEntry.Hotfix.GetMethod(hotfixDataTableUserData.DataTableName,"ParseDataRow",2);

            //调用热更新层的解析
            GameEntry.Hotfix.InvokeMethod(parseDataRow,m_HotfixInstance,dataRowString,userData);
            if (m_IdPropertyInfo == null)
            {
                return false;
            }
            m_Id = (int) m_IdPropertyInfo.GetValue(m_HotfixInstance);
            return true;
        }

        public override bool ParseDataRow(byte[] dataRowBytes, int startIndex, int length, object userData)
        {
            LoadHotfixDataTableUserData hotfixDataTableUserData = userData as LoadHotfixDataTableUserData;
            if (hotfixDataTableUserData == null)
            {
                return false;
            }
            m_HotfixInstance = GameEntry.Hotfix.CreateHotfixInstance(hotfixDataTableUserData.DataTableName);
            m_IdPropertyInfo = GameEntry.Hotfix.GetHotfixType(hotfixDataTableUserData.DataTableName).GetProperty("Id");
            object parseDataRow =  GameEntry.Hotfix.GetMethod(hotfixDataTableUserData.DataTableName,"ParseDataRow",4);

            //调用热更新层的解析
            GameEntry.Hotfix.InvokeMethod(parseDataRow,m_HotfixInstance,dataRowBytes, startIndex,length,userData);
            if (m_IdPropertyInfo == null)
            {
                return false;
            }
            m_Id = (int) m_IdPropertyInfo.GetValue(m_HotfixInstance);
            return true;
        }

        public object GetHotfixData()
        {
            return m_HotfixInstance;
        }
    }

    public class LoadHotfixDataTableUserData
    {
        public LoadHotfixDataTableUserData(string dataTableName, object userData)
        {
            DataTableName = $"UGFExtensions.Hotfix.DR{dataTableName}";
            UserData = userData;
        }

        public string DataTableName { get; }
        public object UserData { get; }
    }
}