using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    public class ProcedureEntry : ProcedureBase
    {
        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);
            // 编辑器下进入断点调试等待流程
            if (Application.platform == RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.OSXEditor
                || Application.platform == RuntimePlatform.LinuxEditor)
            {
                VarObject varObject = new VarObject()
                {
                    Value = typeof(ProcedurePreload)
                };
                procedureOwner.SetData(Constant.ProcedureData.ProcedureType, varObject);
                ChangeState<ProcedureWaitForDebug>(procedureOwner);
            }
            else
            {
                // //TODO:真机在这里切换到游戏的正式开始场景(流程)
                // procedureOwner.SetData<VarInt32>(Constant.ProcedureData.NextSceneId, (int)SceneId.TestScene);
                // ChangeState<ProcedureChangeScene>(procedureOwner);
               ChangeState<ProcedurePreload>(procedureOwner);
            }
        }
    }
}