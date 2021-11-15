//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Entity;
using System;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Reflection;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 实体。
    /// </summary>
    public sealed class Entity : MonoBehaviour, IEntity
    {
        private int m_Id;
        private string m_EntityAssetName;
        private IEntityGroup m_EntityGroup;
        private EntityLogic m_EntityLogic;

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int Id
        {
            get { return m_Id; }
        }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName
        {
            get { return m_EntityAssetName; }
        }

        /// <summary>
        /// 获取实体实例。
        /// </summary>
        public object Handle
        {
            get { return gameObject; }
        }

        /// <summary>
        /// 获取实体所属的实体组。
        /// </summary>
        public IEntityGroup EntityGroup
        {
            get { return m_EntityGroup; }
        }

        /// <summary>
        /// 获取实体逻辑。
        /// </summary>
        public EntityLogic Logic
        {
            get { return m_EntityLogic; }
        }

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="isNewInstance">是否是新实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnInit(int entityId, string entityAssetName, IEntityGroup entityGroup, bool isNewInstance,
            object userData)
        {
            m_Id = entityId;
            m_EntityAssetName = entityAssetName;
            if (isNewInstance)
            {
                m_EntityGroup = entityGroup;
            }
            else if (m_EntityGroup != entityGroup)
            {
                Log.Error("Entity group is inconsistent for non-new-instance entity.");
                return;
            }

            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            Type entityLogicType = showEntityInfo.EntityLogicType;
            if (entityLogicType == null)
            {
                Log.Error("Entity logic type is invalid.");
                return;
            }

            if (!CheckLogicChanged(showEntityInfo))
            {
                return;
            }
            if (m_EntityLogic == null)
            {
                Log.Error("Entity '{0}' can not add entity logic.", entityAssetName);
                return;
            }

            try
            {
                m_EntityLogic.OnInit(showEntityInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnInit with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 检查逻辑是否变更
        /// </summary>
        /// <param name="showEntityInfo"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool CheckLogicChanged(ShowEntityInfo showEntityInfo)
        {
            if (m_EntityLogic != null)
            {
                if (m_EntityLogic is CrossBindingAdaptorType crossBindingAdaptorType)
                {
                    var type1 = crossBindingAdaptorType.ILInstance.Type;
                    if (showEntityInfo.EntityLogicType is ILRuntimeType type2 && type1 == type2.ILType)
                    {
                        m_EntityLogic.enabled = true;
                    }
                    else
                    {
                        Destroy(m_EntityLogic);
                        m_EntityLogic = null;
                    }
                }
                else
                {
                    if (m_EntityLogic.GetType() == showEntityInfo.EntityLogicType)
                    {
                        m_EntityLogic.enabled = true;
                    }
                    else
                    {
                        Destroy(m_EntityLogic);
                        m_EntityLogic = null;
                    }
                }
            }

            if (m_EntityLogic == null)
            {
                if (showEntityInfo.EntityLogicType is ILRuntimeType ilRuntimeType)
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
                    m_EntityLogic = clrInstance;
                }
                else
                {
                    m_EntityLogic =  (EntityLogic) gameObject.AddComponent(showEntityInfo.EntityLogicType);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 实体回收。
        /// </summary>
        public void OnRecycle()
        {
            try
            {
                m_EntityLogic.OnRecycle();
                m_EntityLogic.enabled = false;
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnRecycle with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }

            m_Id = 0;
        }

        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void OnShow(object userData)
        {
            ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            try
            {
                m_EntityLogic.OnShow(showEntityInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnShow with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnHide(bool isShutdown, object userData)
        {
            try
            {
                m_EntityLogic.OnHide(isShutdown, userData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnHide with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="childEntity">附加的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnAttached(IEntity childEntity, object userData)
        {
            AttachEntityInfo attachEntityInfo = (AttachEntityInfo)userData;
            try
            {
                m_EntityLogic.OnAttached(((Entity)childEntity).Logic, attachEntityInfo.ParentTransform,
                    attachEntityInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnAttached with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="childEntity">解除的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnDetached(IEntity childEntity, object userData)
        {
            try
            {
                m_EntityLogic.OnDetached(((Entity)childEntity).Logic, userData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnDetached with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnAttachTo(IEntity parentEntity, object userData)
        {
            AttachEntityInfo attachEntityInfo = (AttachEntityInfo)userData;
            try
            {
                m_EntityLogic.OnAttachTo(((Entity)parentEntity).Logic, attachEntityInfo.ParentTransform,
                    attachEntityInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnAttachTo with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }

            ReferencePool.Release(attachEntityInfo);
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnDetachFrom(IEntity parentEntity, object userData)
        {
            try
            {
                m_EntityLogic.OnDetachFrom(((Entity)parentEntity).Logic, userData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnDetachFrom with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }

        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            try
            {
                m_EntityLogic.OnUpdate(elapseSeconds, realElapseSeconds);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnUpdate with exception '{2}'.", m_Id, m_EntityAssetName, exception);
            }
        }
    }
}