using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    public class HotfixComponent : GameFrameworkComponent
    {
        [SerializeField] private string m_HotfixHelperTypeName = "UGFExtensions.Hotfix.ILRuntimeHelper";
        [SerializeField] private HotfixHelperBase m_CustomHotfixHelper;
        private void Start()
        {
            m_CustomHotfixHelper =
                UnityGameFramework.Runtime.Helper.CreateHelper(m_HotfixHelperTypeName, m_CustomHotfixHelper);
            if (m_CustomHotfixHelper == null)
            {
                Log.Error("Can not create hotfix helper.");
                return;
            }

            m_CustomHotfixHelper.name = "Hotfix Helper";
            Transform customHelperTrans = m_CustomHotfixHelper.transform;
            customHelperTrans.SetParent(transform);
            customHelperTrans.localScale = Vector3.one;
            
        }

        public void Enter()
        {
            m_CustomHotfixHelper.Initialize();
        }

        private void ShutDown()
        {
            m_CustomHotfixHelper.ShutDown();
        }

        public object CreateHotfixInstance(string typeName)
        {
            return m_CustomHotfixHelper.CreateInstance(typeName);
        }

        public object GetMethod(string typeName, string methodName, int paramCount)
        {
            return m_CustomHotfixHelper.GetMethod(typeName, methodName, paramCount);
        }

        public object InvokeMethod(object method,object instance,params object[] objects)
        {
           return m_CustomHotfixHelper.InvokeMethod(method,instance, objects);
        }
        
        public List<Type> GetAllTypes()
        {
           return m_CustomHotfixHelper.GetAllTypes();
        }
        
        public Type GetHotfixType(string typeName)
        {
            return m_CustomHotfixHelper.GetHotfixType(typeName);
        }

        public object GetHotfixGameEntry()
        {
            return m_CustomHotfixHelper.GetHotfixGameEntry;
        }
        public T AddHotfixMonoBehaviour<T>(GameObject go,string hotfixFullTypeName) where T : MonoBehaviour
        {
            return m_CustomHotfixHelper.CreateHotfixMonoBehaviour<T>(go,hotfixFullTypeName);
        }
#if ILRuntime
        public ILRuntime.Runtime.Enviorment.InvocationContext BeginInvoke(ILRuntime.CLR.Method.IMethod m)
        {
            return ((ILRuntimeHelper)m_CustomHotfixHelper).BeginInvoke(m);
        }
        public ILRuntime.Runtime.Enviorment.AppDomain GetAppDomain()
        {
            return ((ILRuntimeHelper)m_CustomHotfixHelper).AppDomain;
        }
#endif
    }
}