using System;
using System.Collections.Generic;
using GameFramework;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGFExtensions.Hotfix
{
    public static partial class ILRuntimeUtility
    {
        [ILRunTimeRegister(ILRegister.Redirection)]
        public static unsafe void RegisterReferencePoolCLRRedirection(AppDomain appDomain)
        {
            var arr = typeof(ReferencePool).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "Acquire" && i.GetGenericArguments().Length == 1)
                {
                    appDomain.RegisterCLRMethodRedirection(i, Acquire_0);
                }
            }
        }

        private static unsafe StackObject* Acquire_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                StackObject* __ret = ILIntepreter.Minus(__esp, 0);
                IType type = genericArgument[0];
                object res;
                if (type is CLRType)
                {
                    //Unity主工程的类不需要任何特殊处理
                    res = ReferencePool.Acquire(type.TypeForCLR);
                    //但是如果当前是适配器添加的 需要返回ILInstance
                    if (res is CrossBindingAdaptorType crossBindingAdaptorType)
                    {
                        res = crossBindingAdaptorType.ILInstance;
                    }
                }
                else
                {
                    ILType ilType = type as ILType;
                    //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
                    var ilInstance = new ILTypeInstance(ilType, false); //手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                    Type adapterType = ilType.FirstCLRBaseType.TypeForCLR;

                    CrossBindingAdaptorType clrInstance = ReferencePool.Acquire(adapterType) as CrossBindingAdaptorType;
                    //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                    if (clrInstance is IAdapterProperty adapterProperty)
                    {
                        adapterProperty.ILInstance = ilInstance;
                        adapterProperty.AppDomain = __domain;
                    }

                    //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                    ilInstance.CLRInstance = clrInstance;

                    res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance
                }

                return ILIntepreter.PushObject(__ret, __mStack, res);
            }

            return __esp;
        }
    }
}