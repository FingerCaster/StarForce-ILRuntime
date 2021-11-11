using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using UnityEngine;
using UnityGameFramework.Runtime;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;
using Object = UnityEngine.Object;

namespace UGFExtensions.Hotfix
{
    public partial class ILRuntimeUtility
    {
        private static unsafe void RegisterComponentCLRRedirection(AppDomain appDomain)
        {
            var arr = typeof(Component).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "GetComponent" && i.GetGenericArguments().Length == 1)
                {
                    appDomain.RegisterCLRMethodRedirection(i, ComponentGetComponent);
                }
            }
        }


        unsafe static StackObject* ComponentGetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            object obj = StackObject.ToObject(ptr, __domain, __mStack);
            GameObject instance = null;
            if (obj is Component component)
            {
                instance = component.gameObject;
            }else if (obj is GameObject gameObject)
            {
                instance = gameObject;
            }
            else if (obj is ILTypeInstance ilTypeInstance)
            {
                if (ilTypeInstance.CLRInstance is MonoBehaviour monoBehaviour)
                {
                    instance = monoBehaviour.gameObject;
                }
            }
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                var type = genericArgument[0];
                object res = null;
                if (type is CLRType)
                {
                    //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                    res = instance.GetComponent(type.TypeForCLR);
                    //但是如果当前是适配器添加的 需要返回ILInstance
                    if (res is CrossBindingAdaptorType crossBindingAdaptorType)
                    {
                        res = crossBindingAdaptorType.ILInstance;
                    }
                }
                else
                {
                    //因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
                    CrossBindingAdaptorType[] clrInstances = instance.GetComponents(typeof(CrossBindingAdaptorType))
                        .Cast<CrossBindingAdaptorType>().ToArray();
                    for (int i = 0; i < clrInstances.Length; i++)
                    {
                        var clrInstance = clrInstances[i];
                        if (clrInstance.ILInstance != null) //ILInstance为null, 表示是无效的MonoBehaviour，要略过
                        {
                            Log.Info(clrInstance.ILInstance.Type.CanAssignTo(type) + $"{type}");
                            if (clrInstance.ILInstance.Type.CanAssignTo(type))
                            {
                                res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance
                                break;
                            }
                        }
                    }
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }
    }
}