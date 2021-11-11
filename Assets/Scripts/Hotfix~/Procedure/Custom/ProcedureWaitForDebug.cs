using System;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    /// <summary>
    /// 等待断点进入下一流程
    /// </summary>
    public class ProcedureWaitForDebug : ProcedureBase
    {
        private Type m_ProcedureType = null;
        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_ProcedureType = (Type)procedureOwner.GetData<VarObject>(Constant.ProcedureData.ProcedureType).Value;
            procedureOwner.RemoveData(Constant.ProcedureData.ProcedureType);
        }

        protected internal override void OnUpdate(IFsm procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (Input.GetKeyDown(KeyCode.A)) 
            {
                ChangeState(procedureOwner,m_ProcedureType);
            }
        }

        protected internal override void OnLeave(IFsm procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            m_ProcedureType = null;
        }
    }
}