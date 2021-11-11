

using System;
using GameFramework.Event;
using UGFExtensions.Hotfix;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UGFExtensions
{
    public abstract class GameBase
    {
        public abstract GameMode GameMode
        {
            get;
        }

        protected ScrollableBackground SceneBackground
        {
            get;
            private set;
        }
        public bool GameOver
        {
            get;
            protected set;
        }
        
        private MyAircraft m_MyAircraft = null;
        
        public virtual void Initialize()
        {
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
           
            GameObject go = GameObject.Find("Background");
            if (SceneBackground ==  null)
            {
                SceneBackground = new ScrollableBackground();
            }
            SceneBackground.Init(go.transform);
            //
            // if (SceneBackground == null)
            // {
            //     Log.Warning("Can not find scene background.");
            //     return;
            // }
            GameEntry.Entity.ShowMyAircraft(new MyAircraftData(GameEntry.Entity.GenerateSerialId(), 10000)
            {
                Name = "My Aircraft",
                Position = Vector3.zero,
                ScrollableBackground = SceneBackground,
            });
        
            GameOver = false;
            m_MyAircraft = null;
        }
        
        public virtual void Shutdown()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }
        
        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
            SceneBackground?.Update();
            if (m_MyAircraft != null && m_MyAircraft.IsDead)
            {
                GameOver = true;
                return;
            }
        }
        
        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            if (ne.EntityLogicType == typeof(MyAircraft))
            {
                m_MyAircraft = (MyAircraft)ne.Entity.Logic;
            }
        }
        
        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }
    }
}
