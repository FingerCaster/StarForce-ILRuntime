

using UnityEngine;

namespace UGFExtensions.Hotfix
{
    public class ScrollableBackground
    {
        private float m_ScrollSpeed;
        private float m_TileSize;
        private BoxCollider m_VisibleBoundary;
        private BoxCollider m_PlayerMoveBoundary;
        private BoxCollider m_EnemySpawnBoundary;
        private Transform m_CachedTransform;
        private Vector3 m_StartPosition;


        public void Init(Transform cacheTrans)
        {
            m_ScrollSpeed = -0.25f;
            m_TileSize = 30f;
            m_CachedTransform = cacheTrans;
            m_StartPosition = m_CachedTransform.position;
            GameObject boundary = GameObject.Find("Boundary");
            m_VisibleBoundary = boundary.transform.Find("VisibleBoundary").GetComponent<BoxCollider>();
            m_VisibleBoundary.gameObject.GetOrAddComponent<HideByBoundary>();
            m_PlayerMoveBoundary = boundary.transform.Find("PlayerMoveBoundary").GetComponent<BoxCollider>();
            m_EnemySpawnBoundary = boundary.transform.Find("EnemySpawnBoundary").GetComponent<BoxCollider>();
        }

        public void Update()
        {
            float newPosition = Mathf.Repeat(Time.time * m_ScrollSpeed, m_TileSize);
            m_CachedTransform.position = m_StartPosition + Vector3.forward * newPosition;
        }

        public BoxCollider VisibleBoundary
        {
            get { return m_VisibleBoundary; }
        }

        public BoxCollider PlayerMoveBoundary
        {
            get { return m_PlayerMoveBoundary; }
        }

        public BoxCollider EnemySpawnBoundary
        {
            get { return m_EnemySpawnBoundary; }
        }
    }
}