

using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace UGFExtensions.Hotfix
{
    public class HPBarItem
    {
        private float m_AnimationSeconds = 0.3f;
        private float m_KeepSeconds = 0.4f;
        private float m_FadeOutSeconds = 0.3f;

        private Slider m_HPBar = null;

        private Canvas m_ParentCanvas = null;
        private RectTransform m_CachedTransform = null;
        private CanvasGroup m_CachedCanvasGroup = null;
        private EntityLogic m_Owner = null;
        private int m_OwnerId = 0;
        private CancellationToken m_CancellationToken;

        public EntityLogic Owner
        {
            get { return m_Owner; }
        }

        public GameObject CacheGameObject { get; }

        public HPBarItem(GameObject gameObject)
        {
            CacheGameObject = gameObject;
            m_AnimationSeconds = 0.3f;
            m_KeepSeconds = 0.4f;
            m_FadeOutSeconds = 0.3f;
            m_ParentCanvas = null;
            m_Owner = null;
            m_OwnerId = 0;

            m_CachedTransform = CacheGameObject.GetComponent<RectTransform>();
            if (m_CachedTransform == null)
            {
                Log.Error("RectTransform is invalid.");
                return;
            }

            m_CachedCanvasGroup = CacheGameObject.GetComponent<CanvasGroup>();
            if (m_CachedCanvasGroup == null)
            {
                Log.Error("CanvasGroup is invalid.");
                return;
            }

            m_HPBar = m_CachedTransform.Find("HPBar").GetComponent<Slider>();
        }

        public void Init(EntityLogic owner, Canvas parentCanvas, float fromHPRatio, float toHPRatio,
            CancellationToken token)
        {
            if (owner == null)
            {
                Log.Error("Owner is invalid.");
                return;
            }

            m_CancellationToken = token;
            m_ParentCanvas = parentCanvas;
            CacheGameObject.SetActive(true);
            m_CachedCanvasGroup.alpha = 1f;
            if (m_Owner != owner || m_OwnerId != owner.Id)
            {
                m_HPBar.value = fromHPRatio;
                m_Owner = owner;
                m_OwnerId = owner.Id;
            }

            Refresh();
            HpBarCo(toHPRatio, m_AnimationSeconds, m_KeepSeconds, m_FadeOutSeconds);
        }

        public bool Refresh()
        {
            if (m_CachedCanvasGroup.alpha <= 0f)
            {
                return false;
            }

            if (m_Owner != null && Owner.Available && Owner.Id == m_OwnerId)
            {
                Vector3 worldPosition = m_Owner.CachedTransform.position + Vector3.forward;
                Vector3 screenPosition = GameEntry.Scene.MainCamera.WorldToScreenPoint(worldPosition);

                Vector2 position;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)m_ParentCanvas.transform,
                    screenPosition,
                    m_ParentCanvas.worldCamera, out position))
                {
                    m_CachedTransform.localPosition = position;
                }
            }

            return true;
        }

        public void Reset()
        {
            //StopAllCoroutines();
            m_CancellationToken?.Cancel();
            m_CancellationToken = null;
            m_CachedCanvasGroup.alpha = 1f;
            m_HPBar.value = 1f;
            m_Owner = null;
            CacheGameObject.SetActive(false);
        }


        private async void HpBarCo(float value, float animationDuration, float keepDuration, float fadeOutDuration)
        {
            bool result = await SmoothValue(value, animationDuration);
            if (result)
            {
                result = await GameEntry.Timer.OnceTimerAsync((long)Math.Ceiling(keepDuration * 1000d),
                    cancellationToken: m_CancellationToken);
            }

            if (result)
            {
                FadeToAlpha(0f, fadeOutDuration);
            }
        }

        async Task<bool> SmoothValue(float value, float duration)
        {
            float originalValue = m_HPBar.value;
            var time = (long)Math.Ceiling(duration * 1000d);
            bool result = await GameEntry.Timer.OnceTimerAsync((long)Math.Ceiling(duration * 1000d),
                l => { m_HPBar.value = Mathf.Lerp(originalValue, value, 1f - ((float)l / time)); },
                m_CancellationToken);
            if (result)
            {
                m_HPBar.value = value;
            }
            return result;
        }

        async void FadeToAlpha(float value, float duration)
        {
            float originalValue = m_CachedCanvasGroup.alpha;
            var time = (long)Math.Ceiling(duration * 1000d);
            bool result = await GameEntry.Timer.OnceTimerAsync((long)Math.Ceiling(duration * 1000d),
                l => { m_CachedCanvasGroup.alpha = Mathf.Lerp(originalValue, value, 1f - ((float)l / time)); },
                m_CancellationToken);
            if (result)
            {
                m_CachedCanvasGroup.alpha = value;
            }
        }
    }
}