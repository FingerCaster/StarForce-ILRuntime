using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    public class ProcedureLoadHotfixDataTable : ProcedureBase
    {
        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);
            //1. gf 事件加载
            // GameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
            // GameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
            // GameEntry.DataTable.LoadHotfixDataTable("Entity1", true, this);
            
            //2. await 方式加载热更数据表
            // var drHotfixes = await GameEntry.DataTable.LoadHotfixDataTableAsync("Entity1", true);
            // var drTestEnum= drHotfixes.GetDataRow<DREntity1>(10001);
            
            //3. 按需加载
            // GameEntry.DataTableExtension.LoadDataTableRowConfig<DREntity1>(HotfixAssetUtility.GetDataTable("Entity1",true),"Entity1");
            // DREntity1 drEntity1 =  GameEntry.DataTableExtension.GetDataRow<DREntity1>("Entity1",10001);
            // Debug.Log(drEntity1.AssetName);
            // Debug.Log(drEntity1.Id);
        }
        
        // private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
        // {
        //     LoadDataTableSuccessEventArgs eventArgs = (LoadDataTableSuccessEventArgs)e;
        //     if (!(eventArgs.UserData is LoadHotfixDataTableUserData hotfixDataTableUserData))
        //     {
        //         return;
        //     }
        //
        //     if (hotfixDataTableUserData.UserData != this)
        //     {
        //         return;
        //     }
        //
        //     var drHotfixes = GameEntry.DataTable.GetDataTable<DRHotfix>("Entity1");
        //     var drEntity1 = drHotfixes.GetDataRow<DREntity1>(10001);
        //     if (drEntity1 == null)
        //         return;
        //     Log.Info(
        //         $"{drEntity1.Id}    TestEnum:{drEntity1.AssetName}   TestEnum1:{drEntity1.EntityGroupName}");
        // }
        //
        // private void OnLoadDataTableFailure(object sender, GameEventArgs e)
        // {
        //     LoadDataTableFailureEventArgs eventArgs = (LoadDataTableFailureEventArgs)e;
        //     if (!(eventArgs.UserData is LoadHotfixDataTableUserData hotfixDataTableUserData))
        //     {
        //         return;
        //     }
        //
        //     if (hotfixDataTableUserData.UserData != this)
        //     {
        //         return;
        //     }
        //
        //     Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.",
        //         eventArgs.DataTableAssetName, eventArgs.DataTableAssetName, eventArgs.ErrorMessage);
        // }
    }
}