//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    /// <summary>
    /// 装甲类。
    /// </summary>
    public class Armor : EntityLogic
    {
        private const string AttachPoint = "Armor Point";

        [SerializeField] private ArmorData m_ArmorData = null;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_ArmorData = userData as ArmorData;
            if (m_ArmorData == null)
            {
                Log.Error("Armor data is invalid.");
                return;
            }

            GameEntry.Entity.AttachEntity(Entity, m_ArmorData.OwnerId, AttachPoint);
        }

        protected override void OnAttachTo(UnityGameFramework.Runtime.EntityLogic parentEntity,
            Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Armor of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }
    }
}