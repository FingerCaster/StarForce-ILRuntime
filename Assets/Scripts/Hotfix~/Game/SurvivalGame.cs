

using System;
using GameFramework;
using GameFramework.DataTable;
using UGFExtensions.Hotfix;
using UnityEngine;

namespace UGFExtensions
{
    public class SurvivalGame : GameBase
    {
        private float m_ElapseSeconds = 0f;
        
        public override GameMode GameMode
        {
            get
            {
                return GameMode.Survival;
            }
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
        
            m_ElapseSeconds += elapseSeconds;
            if (m_ElapseSeconds >= 1f)
            {
                m_ElapseSeconds = 0f;
                var dtAsteroids = GameEntry.DataTableExtension.GetCount<DRAsteroid>();
                Bounds bounds = SceneBackground.EnemySpawnBoundary.bounds;
                float randomPositionX = bounds.min.x + bounds.size.x * (float)Utility.Random.GetRandomDouble();
                float randomPositionZ = bounds.min.z + bounds.size.z * (float)Utility.Random.GetRandomDouble();
                GameEntry.Entity.ShowAsteroid(new AsteroidData(GameEntry.Entity.GenerateSerialId(), 60000 + Utility.Random.GetRandom(dtAsteroids))
                {
                    Position = new Vector3(randomPositionX, 0f, randomPositionZ),
                });
            }
        }
    }
}
