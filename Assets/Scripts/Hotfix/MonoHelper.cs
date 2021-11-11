using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UGFExtensions.Await;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    public class MonoHelper : HotfixHelperBase
    {
        private Assembly m_Assembly;
        private MethodInfo m_Update;
        private MethodInfo m_Shutdown;
        private MethodInfo m_ApplicationPause;
        private MethodInfo m_ApplicationQuit;
        private List<Type> m_HotfixTypes;
        private object[] m_UpdateParams;
        private object m_HotfixGameEntry;
        public override object GetHotfixGameEntry => m_HotfixGameEntry;

        public override T CreateHotfixMonoBehaviour<T>(GameObject go,string hotfixFullTypeName)
        {
            Type type = GetHotfixType(hotfixFullTypeName);
            T instance = go.AddComponent(type) as T;
            return instance;
        }

        public override Type GetHotfixType(string hotfixTypeFullName)
        {
            return m_HotfixTypes.Find(x => x.FullName != null && x.FullName.Equals(hotfixTypeFullName));
        }

        public override List<Type> GetAllTypes()
        {
            return m_HotfixTypes;
        }

        public override async void Initialize()
        {
            TextAsset dllAsset =
                await GameEntry.Resource.LoadAssetAsync<TextAsset>(AssetUtility.GetHotfixDLLAsset("Hotfix.dll"));
            byte[] dll = dllAsset.bytes;
            Log.Info("hotfix dll加载完毕");
            TextAsset pdbAsset =
                await GameEntry.Resource.LoadAssetAsync<TextAsset>(AssetUtility.GetHotfixDLLAsset("Hotfix.pdb"));
            byte[] pdb = pdbAsset.bytes;
            Log.Info("hotfix pdb加载完毕");
            m_Assembly = Assembly.Load(dll, pdb);
            m_HotfixTypes = m_Assembly.GetTypes().ToList();
            m_UpdateParams = new object[2];
            Enter();
        }

        public override void Enter()
        {
            string typeFullName = "UGFExtensions.Hotfix.HotfixGameEntry";
            Type hotfixInit = m_Assembly.GetType(typeFullName);
            m_HotfixGameEntry = Activator.CreateInstance(hotfixInit);
            MethodInfo start = hotfixInit.GetMethod("Start",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            m_Update = hotfixInit.GetMethod("Update",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            m_Shutdown = hotfixInit.GetMethod("Shutdown",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            m_ApplicationPause = hotfixInit.GetMethod("OnApplicationPause",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            m_ApplicationQuit = hotfixInit.GetMethod("OnApplicationQuit",BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            start?.Invoke(m_HotfixGameEntry, null);
        }

        public override void ShutDown()
        {
            if (m_Shutdown == null)
            {
                return;
            }

            m_Shutdown.Invoke(m_HotfixGameEntry, null);
        }

        public override object CreateInstance(string typeName)
        {
            Type type = GetHotfixType(typeName);
            object hotfixInstance = Activator.CreateInstance(type);
            return hotfixInstance;
        }

        public override object GetMethod(string typeName, string methodName, int paramCount)
        {
            Type type = GetHotfixType(typeName);
            return type
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.Name == methodName)
                .FirstOrDefault(x => x.GetParameters().Length == paramCount);
        }

        public override object InvokeMethod(object method, object instance, params object[] objects)
        {
            MethodInfo methodInfo = (MethodInfo)method;
            return methodInfo.Invoke(instance, objects);
        }

        private void Update()
        {
            if (m_Update == null)
            {
                return;
            }

            m_UpdateParams[0] = Time.deltaTime;
            m_UpdateParams[1] = Time.unscaledDeltaTime;
            m_Update.Invoke(m_HotfixGameEntry, m_UpdateParams);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (m_ApplicationPause == null)
            {
                return;
            }

            m_ApplicationPause.Invoke(m_HotfixGameEntry, new object[] { pauseStatus });
        }

        private void OnApplicationQuit()
        {
            if (m_ApplicationQuit == null)
            {
                return;
            }

            m_ApplicationQuit.Invoke(m_HotfixGameEntry, null);
        }
        
    }
}