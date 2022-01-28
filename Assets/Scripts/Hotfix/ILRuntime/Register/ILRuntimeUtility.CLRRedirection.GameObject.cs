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
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace UGFExtensions.Hotfix
{
    public static partial class ILRuntimeUtility
    {
        [ILRunTimeRegister(ILRegister.Redirection)]
        public static unsafe void RegisterGameObjectCLRRedirection(AppDomain appDomain)
        {
            var arr = typeof(GameObject).GetMethods();
            foreach (var i in arr)
            {
                if (i.Name == "AddComponent" && i.GetGenericArguments().Length == 1)
                {
                    appDomain.RegisterCLRMethodRedirection(i, AddComponent);
                }

                if (i.Name == "GetComponent" && i.GetGenericArguments().Length == 1)
                {
                    appDomain.RegisterCLRMethodRedirection(i, GetComponent);
                }

                if (i.Name == "GetOrAddComponent" && i.GetGenericArguments().Length == 1)
                {
                    appDomain.RegisterCLRMethodRedirection(i, GetOrAddComponent);
                }
            }
        }

        private static unsafe StackObject* GetOrAddComponent(ILIntepreter __intp, StackObject* __esp,
            IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
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
                    //但是如果当前monobehaviour 是适配器添加的 需要返回ILInstance
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
                            if (clrInstance.ILInstance.Type.CanAssignTo(type))
                            {
                                res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance
                                break;
                            }
                        }
                    }
                }

                if (res == null)
                {
                    if (type is CLRType)
                    {
                        //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                        res = instance.AddComponent(type.TypeForCLR);
                    }
                    else
                    {
                        //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
                        ILType ilType = type as ILType;
                        var ilInstance = new ILTypeInstance(ilType, false); //手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                        //接下来创建Adapter实例
                        var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adaptor>();
                        //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                        clrInstance.ILInstance = ilInstance;
                        clrInstance.AppDomain = __domain;
                        //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                        ilInstance.CLRInstance = clrInstance;

                        res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance
                        // 需要手动调用一下构造函数 否则MonoBehaviour 字段初始化 只会是默认值
                        var constructor = ilType.GetConstructor(0);
                        __domain.Invoke(constructor, ilInstance);
                        clrInstance.Awake(); //因为Unity调用这个方法时还没准备好所以这里补调一次
                    }
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }

        private static unsafe StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
            if (instance == null)
                throw new System.NullReferenceException();
            __intp.Free(ptr);

            var genericArgument = __method.GenericArguments;
            //AddComponent应该有且只有1个泛型参数
            if (genericArgument != null && genericArgument.Length == 1)
            {
                IType type = genericArgument[0];
                object res;
                if (type is CLRType)
                {
                    //Unity主工程的类不需要任何特殊处理，直接调用Unity接口
                    res = instance.AddComponent(type.TypeForCLR);
                }
                else
                {
                    ILType ilType = type as ILType;
                    //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
                    var ilInstance = new ILTypeInstance(ilType, false); //手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                    Type adapterType = ilType.FirstCLRBaseType.TypeForCLR;

                    CrossBindingAdaptorType clrInstance = instance.AddComponent(adapterType) as CrossBindingAdaptorType;
                    //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                    if (clrInstance is IAdapterProperty adapterProperty)
                    {
                        adapterProperty.ILInstance = ilInstance;
                        adapterProperty.AppDomain = __domain;
                    }

                    //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                    ilInstance.CLRInstance = clrInstance;

                    res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance
                    var constructor = ilType.GetConstructor(0);
                    __domain.Invoke(constructor, ilInstance);
                    var awake = clrInstance.GetType().GetMethod("Awake", BindingFlags.Public | BindingFlags.Instance);
                    awake?.Invoke(clrInstance, null); //因为Unity调用这个方法时还没准备好所以这里补调一次
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }


        private static unsafe StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
            CLRMethod __method, bool isNewObj)
        {
            //CLR重定向的说明请看相关文档和教程，这里不多做解释
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;

            var ptr = __esp - 1;
            //成员方法的第一个参数为this
            GameObject instance = StackObject.ToObject(ptr, __domain, __mStack) as GameObject;
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
                    //但是如果当前monobehaviour 是适配器添加的 需要返回ILInstance
                    if (res is CrossBindingAdaptorType crossBindingAdaptorType)
                    {
                        res = crossBindingAdaptorType.ILInstance;
                    }
                }
                else
                {
                    //因为所有DLL里面的MonoBehaviour实际都是这个Component，所以我们只能全取出来遍历查找
                    CrossBindingAdaptorType[] clrInstances = instance.GetComponents<CrossBindingAdaptorType>();
                    for (int i = 0; i < clrInstances.Length; i++)
                    {
                        var clrInstance = clrInstances[i];
                        if (clrInstance.ILInstance != null) //ILInstance为null, 表示是无效的MonoBehaviour，要略过
                        {
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