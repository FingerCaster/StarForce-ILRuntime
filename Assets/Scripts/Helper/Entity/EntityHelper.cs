//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Reflection;
using GameFramework.Entity;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Reflection;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;
using UnityGameFramework.Runtime;
using Object = UnityEngine.Object;

namespace UGFExtensions.Hotfix
{
    /// <summary>
    /// 默认实体辅助器。
    /// </summary>
    public class EntityHelper : EntityHelperBase
    {
        private ResourceComponent m_ResourceComponent = null;

        /// <summary>
        /// 实例化实体。
        /// </summary>
        /// <param name="entityAsset">要实例化的实体资源。</param>
        /// <returns>实例化后的实体。</returns>
        public override object InstantiateEntity(object entityAsset)
        {
            return Instantiate((Object)entityAsset);
        }

        /// <summary>
        /// 创建实体。
        /// </summary>
        /// <param name="entityInstance">实体实例。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>实体。</returns>
        public override IEntity CreateEntity(object entityInstance, IEntityGroup entityGroup, object userData)
        {
            GameObject gameObject = entityInstance as GameObject;
            if (gameObject == null)
            {
                Log.Error("Entity instance is invalid.");
                return null;
            }

            Transform transform = gameObject.transform;
            transform.SetParent(((MonoBehaviour)entityGroup.Helper).transform);
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            Type entityLogicType = showEntityInfo.EntityLogicType;
            EntityLogic entityLogic = gameObject.GetComponent<EntityLogic>();
            if (entityLogic != null)
            {
                if (entityLogic is CrossBindingAdaptorType crossBindingAdaptorType)
                {
                    var type1 = crossBindingAdaptorType.ILInstance.Type;
                    if (entityLogicType is ILRuntimeType type2 && type1 == type2.ILType)
                    {
                        entityLogic.enabled = true;
                    }
                    else
                    {
                        Destroy(entityLogic);
                        entityLogic = null;
                    }
                }
                else
                {
                    if (entityLogic.GetType() == entityLogicType)
                    {
                        entityLogic.enabled = true;
                    }
                    else
                    {
                        Destroy(entityLogic);
                        entityLogic = null;
                    }
                }
            }

            if (entityLogic == null)
            {
                if (entityLogicType is ILRuntimeType ilRuntimeType)
                {
                    ILType type = ilRuntimeType.ILType;
                    if (type == null)
                    {
                        throw new Exception($"Can not find hotfix mono behaviour {type}");
                    }

                    //热更DLL内的类型比较麻烦。首先我们得自己手动创建实例
                    var ilInstance = new ILTypeInstance(type, false); //手动创建实例是因为默认方式会new MonoBehaviour，这在Unity里不允许
                    //接下来创建Adapter实例
                    Type adapterType = type.FirstCLRBaseType.TypeForCLR;
                    EntityLogic clrInstance = gameObject.AddComponent(adapterType) as EntityLogic;
                    //unity创建的实例并没有热更DLL里面的实例，所以需要手动赋值
                    if (clrInstance is IAdapterProperty adapterProperty)
                    {
                        adapterProperty.ILInstance = ilInstance;
                        adapterProperty.AppDomain = ilRuntimeType.ILType.AppDomain;
                    }

                    //这个实例默认创建的CLRInstance不是通过AddComponent出来的有效实例，所以得手动替换
                    ilInstance.CLRInstance = clrInstance;
                }
                else
                {
                    gameObject.AddComponent(entityLogicType);
                }
            }

            return gameObject.GetOrAddComponent<Entity>();
        }

        /// <summary>
        /// 释放实体。
        /// </summary>
        /// <param name="entityAsset">要释放的实体资源。</param>
        /// <param name="entityInstance">要释放的实体实例。</param>
        public override void ReleaseEntity(object entityAsset, object entityInstance)
        {
            m_ResourceComponent.UnloadAsset(entityAsset);
            Destroy((Object)entityInstance);
        }

        private void Start()
        {
            m_ResourceComponent = UnityGameFramework.Runtime.GameEntry.GetComponent<ResourceComponent>();
            if (m_ResourceComponent == null)
            {
                Log.Fatal("Resource component is invalid.");
                return;
            }
        }
    }
}