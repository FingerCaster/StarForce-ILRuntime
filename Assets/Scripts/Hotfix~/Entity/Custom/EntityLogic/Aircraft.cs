using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    /// <summary>
    /// 战机类。
    /// </summary>
    public abstract class Aircraft : TargetableObject
    {
        [SerializeField] private AircraftData m_AircraftData = null;

        [SerializeField] protected Thruster m_Thruster = null;

        [SerializeField] protected List<Weapon> m_Weapons;

        [SerializeField] protected List<Armor> m_Armors;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_Weapons = new List<Weapon>();
            m_Armors = new List<Armor>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            m_AircraftData = userData as AircraftData;
            if (m_AircraftData == null)
            {
                Log.Error("Aircraft data is invalid.");
                return;
            }

            Name = Utility.Text.Format("Aircraft ({0})", Id);

            GameEntry.Entity.ShowThruster(m_AircraftData.GetThrusterData());

            List<WeaponData> weaponDatas = m_AircraftData.GetAllWeaponDatas();
            for (int i = 0; i < weaponDatas.Count; i++)
            {
                GameEntry.Entity.ShowWeapon(weaponDatas[i]);
            }

            List<ArmorData> armorDatas = m_AircraftData.GetAllArmorDatas();
            for (int i = 0; i < armorDatas.Count; i++)
            {
                GameEntry.Entity.ShowArmor(armorDatas[i]);
            }
        }

        protected override void OnAttached(UnityGameFramework.Runtime.EntityLogic childEntity,
            Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
            if (childEntity is Thruster entity)
            {
                m_Thruster = entity;
                return;
            }

            if (childEntity is Weapon weapon)
            {
                m_Weapons.Add(weapon);
                return;
            }

            if (childEntity is Armor armor)
            {
                m_Armors.Add(armor);
                return;
            }
        }

        protected override void OnDetached(UnityGameFramework.Runtime.EntityLogic childEntity, object userData)

        {
            base.OnDetached(childEntity, userData);

            if (childEntity is Thruster)
            {
                m_Thruster = null;
                return;
            }

            if (childEntity is Weapon)
            {
                m_Weapons.Remove((Weapon)childEntity);
                return;
            }

            if (childEntity is Armor)
            {
                m_Armors.Remove((Armor)childEntity);
                return;
            }
        }

        protected override void OnDead(EntityLogic attacker)
        {
            base.OnDead(attacker);

            GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_AircraftData.DeadEffectId)
            {
                Position = CachedTransform.localPosition,
            });
            GameEntry.Sound.PlaySound(m_AircraftData.DeadSoundId);
        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_AircraftData.Camp, m_AircraftData.HP, 0, m_AircraftData.Defense);
        }
    }
}