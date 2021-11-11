﻿

using System;
using GameFramework;
using UnityEngine;

namespace UGFExtensions.Hotfix
{
    [Serializable]
    public abstract class EntityData : IReference
    {
        [SerializeField]
        private int m_Id = 0;

        [SerializeField]
        private int m_TypeId = 0;

        [SerializeField]
        private Vector3 m_Position = Vector3.zero;

        [SerializeField]
        private Quaternion m_Rotation = Quaternion.identity;

        public EntityData()
        {
        }

        public EntityData(int entityId, int typeId)
        {
            m_Id = entityId;
            m_TypeId = typeId;
        }

        /// <summary>
        /// 实体编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 实体类型编号。
        /// </summary>
        public int TypeId
        {
            get
            {
                return m_TypeId;
            }
        }

        /// <summary>
        /// 实体位置。
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        /// <summary>
        /// 实体朝向。
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                m_Rotation = value;
            }
        }
        /// <summary>
        /// 填充实体数据
        /// </summary>
        protected void Fill(int id, int typeId)
        {
            m_Id = id;
            m_TypeId = typeId;
        }


        public virtual void Clear()
        {
            m_Id = 0;
            m_TypeId = 0;
            m_Position = default(Vector3);
            m_Rotation = default(Quaternion);
        }
    }
}
