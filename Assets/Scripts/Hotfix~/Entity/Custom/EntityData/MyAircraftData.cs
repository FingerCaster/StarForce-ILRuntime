//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using UnityEngine;

namespace UGFExtensions.Hotfix
{
    [Serializable]
    public class MyAircraftData : AircraftData
    {
        [SerializeField]
        private string m_Name = null;
        private ScrollableBackground m_ScrollableBackground = null;
        public MyAircraftData(int entityId, int typeId)
            : base(entityId, typeId, CampType.Player)
        {
        }

        /// <summary>
        /// 角色名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                m_Name = value;
            }
        }
        /// <summary>
        /// 背景滑动。
        /// </summary>
        public ScrollableBackground ScrollableBackground
        {
            get
            {
                return m_ScrollableBackground;
            }
            set
            {
                m_ScrollableBackground = value;
            }
        }
    }
}
