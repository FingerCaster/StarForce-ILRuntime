#if ILRuntime
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UGFExtensions.Await;
using UnityEngine;
using UnityGameFramework.Runtime;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGFExtensions.Hotfix
{
    public class ILRuntimeHelper : HotfixHelperBase
    {
        private AppDomain m_appDomain;

        /// <summary>
        /// ILRuntime入口对象
        /// </summary>
        public AppDomain AppDomain
        {
            get
            {
                if (m_appDomain == null)
                {
                    m_appDomain = new AppDomain(ILRuntimeJITFlags.JITOnDemand);
                }

                return m_appDomain;
            }
        }

        private IMethod m_Update;
        private IMethod m_Shutdown;
        private IMethod m_ApplicationPause;
        private IMethod m_ApplicationQuit;

        private List<Type> m_HotfixTypes;
        private bool m_IsStarted;
        private Action m_RunOnStarted;
        private ILTypeInstance m_HotfixGameEntry;

        
        public override T CreateHotfixMonoBehaviour<T>(GameObject go,string hotfixFullTypeName)
        {
            var appDomain = GameEntry.Hotfix.GetAppDomain();
            ILType type = appDomain.LoadedTypes[hotfixFullTypeName] as ILType;
            if (type == null)
            {
                throw new Exception($"Can not find hotfix mono behaviour {hotfixFullTypeName}");
            }
            //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
            var ilInstance = new ILTypeInstance(type, false); //手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
            //接下来创建Adapter实例
            Type adapterType = type.FirstCLRBaseType.TypeForCLR;
            T clrInstance = go.AddComponent(adapterType) as T;
            //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
            if (clrInstance is IAdapterProperty adapterProperty)
            {
                adapterProperty.ILInstance = ilInstance;
                adapterProperty.AppDomain = appDomain;
            }
            
            //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
            ilInstance.CLRInstance = clrInstance;
            return clrInstance;
        }

        /// <summary>
        /// 获取热更新层类的Type对象
        /// </summary>
        public override Type GetHotfixType(string hotfixTypeFullName)
        {
            return m_HotfixTypes.Find(x => x.FullName != null && x.FullName.Equals(hotfixTypeFullName));
        }

        /// <summary>
        /// 获取所有热更新层类的Type对象
        /// </summary>
        public override List<Type> GetAllTypes()
        {
            return m_HotfixTypes;
        }

        public override object GetHotfixGameEntry => m_HotfixGameEntry;

        public override async void Initialize()
        {
            TextAsset dllAsset = await GameEntry.Resource.LoadAssetAsync<TextAsset>(AssetUtility.GetHotfixDLLAsset("Hotfix.dll"));
            byte[] dll = dllAsset.bytes;
            Log.Info("hotfix dll加载完毕");
            ILRuntimeUtility.InitILRuntime(AppDomain);
#if !DISABLE_ILRUNTIME_DEBUG
            TextAsset pdbAsset = await GameEntry.Resource.LoadAssetAsync<TextAsset>(AssetUtility.GetHotfixDLLAsset("Hotfix.pdb"));
            if (pdbAsset != null)
            {
                byte[] pdb = pdbAsset.bytes;
                Log.Info("hotfix pdb加载完毕");
                AppDomain.LoadAssembly(new MemoryStream(dll), new MemoryStream(pdb),
                    new ILRuntime.Mono.Cecil.Pdb.PdbReaderProvider());
                //启动调试服务器
                AppDomain.DebugService.StartDebugService(56789);
            }
#else
            AppDomain.LoadAssembly(new MemoryStream(dll));
#endif
            //设置Unity主线程ID 这样就可以用Profiler看性能消耗了
#if DEBUG && !NO_PROFILER
            AppDomain.UnityMainThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif
            m_HotfixTypes = AppDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();
            Enter();
        }

        public override void Enter()
        {
            string typeFullName = "UGFExtensions.Hotfix.HotfixGameEntry";
            IType type = AppDomain.LoadedTypes[typeFullName];
            m_HotfixGameEntry = (ILTypeInstance)CreateInstance(typeFullName);
            IMethod start = type.GetMethod("Start", 0);
            m_Update = type.GetMethod("Update", 2);
            m_Shutdown = type.GetMethod("Shutdown", 0);
            m_ApplicationPause = type.GetMethod("OnApplicationPause", 1);
            m_ApplicationQuit = type.GetMethod("OnApplicationQuit", 0);
            AppDomain.Invoke(start,m_HotfixGameEntry, null);
        }

        public override void ShutDown()
        {
            if (m_Shutdown == null)
            {
                return;
            }

            AppDomain.Invoke(m_Shutdown, m_HotfixGameEntry, null);
        }

        public override object CreateInstance(string typeName)
        {
            IType type = AppDomain.LoadedTypes[typeName];
            object instance = ((ILType)type).Instantiate();
            return instance;
        }

        public override object GetMethod(string typeName, string methodName, int paramCount)
        {
            IType type = AppDomain.LoadedTypes[typeName];
            return type.GetMethod(methodName, paramCount);
        }

        public override object InvokeMethod(object method, object instance, params object[] objects)
        {
           return AppDomain.Invoke((IMethod)method, instance, objects);
        }
        
        public InvocationContext BeginInvoke(IMethod m)
        {
            return AppDomain.BeginInvoke(m);
        }
        private void Update()
        {
            if (m_Update == null)
            {
                return;
            }

            using (var ctx = AppDomain.BeginInvoke(m_Update))
            {
                ctx.PushObject(m_HotfixGameEntry);
                ctx.PushFloat(Time.deltaTime);
                ctx.PushFloat(Time.unscaledDeltaTime);
                ctx.Invoke();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (m_ApplicationPause == null)
            {
                return;
            }

            AppDomain.Invoke(m_ApplicationPause, m_HotfixGameEntry, pauseStatus);
        }

        private void OnApplicationQuit()
        {
            if (m_ApplicationQuit == null)
            {
                return;
            }

            AppDomain.Invoke(m_ApplicationQuit, m_HotfixGameEntry, null);
        }
    }
}
#endif
