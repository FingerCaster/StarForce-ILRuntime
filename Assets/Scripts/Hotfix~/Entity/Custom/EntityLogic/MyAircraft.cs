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
    public class MyAircraft : Aircraft
    {
        [SerializeField] private MyAircraftData m_MyAircraftData = null;

        private Rect m_PlayerMoveBoundary;
        private Vector3 m_TargetPosition = Vector3.zero;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_MyAircraftData = userData as MyAircraftData;
            m_PlayerMoveBoundary = Rect.zero;
            if (m_MyAircraftData == null)
            {
                Log.Error("My aircraft data is invalid.");
                return;
            }

            ScrollableBackground sceneBackground = m_MyAircraftData.ScrollableBackground;
            if (sceneBackground == null)
            {
                Log.Warning("Can not find scene background.");
                return;
            }

            m_PlayerMoveBoundary = new Rect(sceneBackground.PlayerMoveBoundary.bounds.min.x,
                sceneBackground.PlayerMoveBoundary.bounds.min.z,
                sceneBackground.PlayerMoveBoundary.bounds.size.x, sceneBackground.PlayerMoveBoundary.bounds.size.z);

            Log.Warning(m_PlayerMoveBoundary);
            Log.Warning(Utility.Json.ToJson(m_PlayerMoveBoundary));
            
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
            if (Input.GetMouseButton(0))
            {
                Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                m_TargetPosition = new Vector3(point.x, 0f, point.z);

                for (int i = 0; i < m_Weapons.Count; i++)
                {
                    m_Weapons[i].TryAttack();
                }
            }

            Vector3 direction = m_TargetPosition - CachedTransform.localPosition;
            if (direction.sqrMagnitude <= Vector3.kEpsilon)
            {
                return;
            }

            Vector3 speed = Vector3.ClampMagnitude(direction.normalized * m_MyAircraftData.Speed * elapseSeconds,
                direction.magnitude);
            var localPosition = CachedTransform.localPosition;
            localPosition = new Vector3
            (
                Mathf.Clamp(localPosition.x + speed.x, m_PlayerMoveBoundary.xMin, m_PlayerMoveBoundary.xMax),
                0f,
                Mathf.Clamp(localPosition.z + speed.z, m_PlayerMoveBoundary.yMin, m_PlayerMoveBoundary.yMax)
            );
            CachedTransform.localPosition = localPosition;
        }
    }
}