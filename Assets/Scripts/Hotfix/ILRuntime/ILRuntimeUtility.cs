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
            //注册重定向方法
            RegisterILRuntimeCLRRedirection(appDomain);
            RegisterComponentCLRRedirection(appDomain);
            RegisterGameObjectCLRRedirection(appDomain);
            RegisterReferencePoolCLRRedirection(appDomain);
            //适配委托

            //GF用
            appDomain.DelegateManager.RegisterMethodDelegate<float>();
            appDomain.DelegateManager.RegisterMethodDelegate<object, ILTypeInstance>();
            appDomain.DelegateManager.RegisterMethodDelegate<object, GameFramework.Event.GameEventArgs>();
            appDomain.DelegateManager.RegisterMethodDelegate<UnityEngine.U2D.SpriteAtlas>();
            appDomain.DelegateManager
                .RegisterMethodDelegate<System.String, System.Object, System.Single, System.Object>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Int32>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.String>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Single, System.Single>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Boolean>();
            appDomain.DelegateManager
                .RegisterMethodDelegate<System.String, GameFramework.Resource.LoadResourceStatus, System.String,
                    System.Object>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Object>();
            appDomain.DelegateManager.RegisterMethodDelegate<System.Int64>();

            //注册委托
            appDomain.DelegateManager.RegisterDelegateConvertor<GameFramework.GameFrameworkAction<System.Object>>(
                (act) =>
                {
                    return new GameFramework.GameFrameworkAction<System.Object>((obj) =>
                    {
                        ((Action<System.Object>)act)(obj);
                    });
                });
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.Boolean>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.Boolean>((arg0) =>
                {
                    ((Action<System.Boolean>)act)(arg0);
                });
            });


            appDomain.DelegateManager.RegisterDelegateConvertor<UnityAction>((action) =>
            {
                return new UnityAction(() => { ((Action)action)(); });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<UnityAction<float>>((action) =>
            {
                return new UnityAction<float>((a) => { ((Action<float>)action)(a); });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<EventHandler<GameFramework.Event.GameEventArgs>>(
                (act) =>
                {
                    return new EventHandler<GameFramework.Event.GameEventArgs>((sender, e) =>
                    {
                        ((Action<object, GameFramework.Event.GameEventArgs>)act)(sender, e);
                    });
                });

            appDomain.DelegateManager.RegisterDelegateConvertor<EventHandler<ILTypeInstance>>((act) =>
            {
                return new EventHandler<ILTypeInstance>((sender, e) =>
                {
                    ((Action<object, ILTypeInstance>)act)(sender, e);
                });
            });
            appDomain.DelegateManager.RegisterDelegateConvertor<GameFramework.Resource.LoadAssetSuccessCallback>(
                (act) =>
                {
                    return new GameFramework.Resource.LoadAssetSuccessCallback((assetName, asset, duration, userData) =>
                    {
                        ((Action<System.String, System.Object, System.Single, System.Object>)act)(assetName, asset,
                            duration, userData);
                    });
                });
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityEngine.Events.UnityAction<System.String>>((act) =>
            {
                return new UnityEngine.Events.UnityAction<System.String>((arg0) =>
                {
                    ((Action<System.String>)act)(arg0);
                });
            });
            appDomain.DelegateManager.RegisterDelegateConvertor<GameFramework.Resource.LoadAssetFailureCallback>(
                (act) =>
                {
                    return new GameFramework.Resource.LoadAssetFailureCallback(
                        (assetName, status, errorMessage, userData) =>
                        {
                            ((Action<System.String, GameFramework.Resource.LoadResourceStatus, System.String,
                                System.Object>)act)(assetName, status, errorMessage, userData);
                        });
                });


            //注册值类型绑定
            appDomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            appDomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            appDomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());

            //注册跨域继承适配器
            // appDomain.RegisterCrossBindingAdaptor(new IDisposableAdaptor());
            appDomain.RegisterCrossBindingAdaptor(new CoroutineAdapter());
            appDomain.RegisterCrossBindingAdaptor(new MonoBehaviourAdapter());
            appDomain.RegisterCrossBindingAdaptor(new IAsyncStateMachineAdaptor());
            appDomain.RegisterCrossBindingAdaptor(new ObjectBaseAdapter());
            appDomain.RegisterCrossBindingAdaptor(new UGuiFormAdapter());
            appDomain.RegisterCrossBindingAdaptor(new EntityLogicAdapter());

            //注册CLR绑定代码
            CLRBindings.Initialize(appDomain);


            //注册CatJson
            CatJson.JsonParser.RegisterILRuntimeCLRRedirection(appDomain);
        }

        private static unsafe void RegisterReferencePoolCLRRedirection(AppDomain appDomain)
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
        
        private static  unsafe StackObject* Acquire_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
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
        /// <summary>
        /// Utility.Json 重定向注册
        /// </summary>
        /// <param name="appDomain"></param>
        private static unsafe void RegisterILRuntimeCLRRedirection(AppDomain appDomain)
        {
            if (!Application.isPlaying) return;
            var fieldInfo = typeof(BaseComponent).GetField("m_JsonHelperTypeName",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null)
            {
                return;
            }

            BaseComponent baseComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<BaseComponent>();
            string jsonHelperTypeName = (string)fieldInfo.GetValue(baseComponent);
            if (jsonHelperTypeName == "UGFExtensions.Helper.CatJsonHelper")
            {
                foreach (var i in typeof(Utility.Json).GetMethods())
                {
                    if (i.Name == "ToObject" && i.IsGenericMethodDefinition)
                    {
                        appDomain.RegisterCLRMethodRedirection(i, CatJsonExtensions.RedirectionParseJson);
                    }

                    if (i.Name == "ToJson")
                    {
                        appDomain.RegisterCLRMethodRedirection(i, CatJsonExtensions.RedirectionToJson);
                    }
                }
            }
            else
            {
                throw new Exception("On ILRuntime Use Utility.Json Please Register ILRuntime CLRRedirection!");
            }
        }

        private static unsafe void RegisterGameObjectCLRRedirection(AppDomain appDomain)
        {
            //这里面的通常应该写在InitializeILRuntime，这里为了演示写这里
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

        unsafe static StackObject* GetOrAddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
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
                    CrossBindingAdaptorType[] clrInstances = instance.GetComponents(typeof(CrossBindingAdaptorType)).Cast<CrossBindingAdaptorType>().ToArray();
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
                        var ilInstance =
                            new ILTypeInstance(type as ILType, false); //手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                        //接下来创建Adapter实例
                        var clrInstance = instance.AddComponent<MonoBehaviourAdapter.Adaptor>();
                        //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                        clrInstance.ILInstance = ilInstance;
                        clrInstance.AppDomain = __domain;
                        //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                        ilInstance.CLRInstance = clrInstance;

                        res = clrInstance.ILInstance; //交给ILRuntime的实例应该为ILInstance

                        clrInstance.Awake(); //因为Unity调用这个方法时还没准备好所以这里补调一次
                    }
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }

        unsafe static StackObject* AddComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
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

                    var awake = clrInstance.GetType().GetMethod("Awake", BindingFlags.Public | BindingFlags.Instance);
                    awake?.Invoke(clrInstance, null); //因为Unity调用这个方法时还没准备好所以这里补调一次
                }

                return ILIntepreter.PushObject(ptr, __mStack, res);
            }

            return __esp;
        }


        unsafe static StackObject* GetComponent(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack,
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
                    CrossBindingAdaptorType[] clrInstances = instance.GetComponents(typeof(CrossBindingAdaptorType)).Cast<CrossBindingAdaptorType>().ToArray();
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
#endif