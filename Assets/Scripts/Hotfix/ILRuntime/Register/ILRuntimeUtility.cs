#if ILRuntime

using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GameFramework;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Stack;
using UnityEngine;
using UnityEngine.Events;
using UnityGameFramework.Runtime;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGFExtensions.Hotfix
{
    public  static partial class ILRuntimeUtility
    {
        public static void InitILRuntime(AppDomain appDomain)
        {
            MethodInfo[] methodInfos = typeof(ILRuntimeUtility)
                .GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(_ => _.IsDefined(typeof(ILRunTimeRegisterAttribute))).ToArray();
            object[] objects = { appDomain };
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.GetCustomAttribute<ILRunTimeRegisterAttribute>().ILRegister == ILRegister.Redirection)
                {
                    methodInfo.Invoke(null, objects);
                }
            }
            
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.GetCustomAttribute<ILRunTimeRegisterAttribute>().ILRegister == ILRegister.Delegate)
                {
                    methodInfo.Invoke(null, objects);
                }
            }
            
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.GetCustomAttribute<ILRunTimeRegisterAttribute>().ILRegister == ILRegister.ValueType)
                {
                    methodInfo.Invoke(null, objects);
                }
            }
            
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.GetCustomAttribute<ILRunTimeRegisterAttribute>().ILRegister == ILRegister.Adaptor)
                {
                    methodInfo.Invoke(null, objects);
                }
            }
            
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.GetCustomAttribute<ILRunTimeRegisterAttribute>().ILRegister == ILRegister.Other)
                {
                    methodInfo.Invoke(null, objects);
                }
            }
            
            CLRBindings.Initialize(appDomain);
        }
    }
}
#endif