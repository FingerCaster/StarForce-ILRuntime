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
    /// 推进器类。
    /// </summary>
    public class Thruster : EntityLogic
    {
        private const string AttachPoint = "Thruster Point";

        [SerializeField] private ThrusterData m_ThrusterData = null;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_ThrusterData = userData as ThrusterData;
            if (m_ThrusterData == null)
            {
                Log.Error("Thruster data is invalid.");
                return;
            }

            GameEntry.Entity.AttachEntity(this.Entity, m_ThrusterData.OwnerId, AttachPoint);
        }

        protected override void OnAttachTo(UnityGameFramework.Runtime.EntityLogic parentEntity,
            Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            Name = Utility.Text.Format("Thruster of {0}", parentEntity.Name);
            CachedTransform.localPosition = Vector3.zero;
        }
    }
}