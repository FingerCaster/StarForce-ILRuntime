﻿using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    /// <summary>
    /// 可作为目标的实体类。
    /// </summary>
    public abstract class TargetableObject : EntityLogic
    {
        [SerializeField] private TargetableObjectData m_TargetableObjectData = null;

        public bool IsDead
        {
            get { return m_TargetableObjectData.HP <= 0; }
        }

        public abstract ImpactData GetImpactData();

        public void ApplyDamage(EntityLogic attacker, int damageHP)
        {
            float fromHPRatio = m_TargetableObjectData.HPRatio;
            m_TargetableObjectData.HP -= damageHP;
            float toHPRatio = m_TargetableObjectData.HPRatio;
            if (fromHPRatio > toHPRatio)
            {
                HotfixGameEntry.GameEntry.HpBar.ShowHPBar(this, fromHPRatio, toHPRatio);
            }

            if (m_TargetableObjectData.HP <= 0)
            {
                OnDead(attacker);
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            gameObject.SetLayerRecursively(Constant.Layer.TargetableObjectLayerId);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_TargetableObjectData = userData as TargetableObjectData;
            if (m_TargetableObjectData == null)
            {
                Log.Error("Targetable object data is invalid.");
                return;
            }
        }

        protected virtual void OnDead(EntityLogic attacker)
        {
            GameEntry.Entity.HideEntity(this);
        }

        protected void OnTriggerEnter(Collider other)
        {
            EntityLogic entity = other.gameObject.GetComponent<EntityLogic>();
            if (entity == null)
            {
                return;
            }

            if (entity is TargetableObject && entity.Id >= Id)
            {
                // 碰撞事件由 Id 小的一方处理，避免重复处理
                return;
            }

            AIUtility.PerformCollision(this, entity);
        }
    }
}