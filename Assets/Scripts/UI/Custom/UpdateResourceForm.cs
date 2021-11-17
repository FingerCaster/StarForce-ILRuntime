using UnityEngine;
using UnityEngine.UI;

namespace UGFExtensions
{
    public class UpdateResourceForm : MonoBehaviour
    {
        [SerializeField]
        private Text m_DescriptionText = null;

        [SerializeField]
        private Slider m_ProgressSlider = null;

        [SerializeField] private Canvas m_Canvas;

        public void SetCamera(Camera uiCamera)
        {
            m_Canvas.worldCamera = uiCamera;
        }

        public void SetProgress(float progress, string description)
        {
            m_ProgressSlider.value = progress;
            m_DescriptionText.text = description;
        }
    }
}
