using System.Collections;
using System.Collections.Generic;
using GameFramework.Localization;
using UnityEngine;
using UnityGameFramework.Runtime;

//自动生成于：2021/10/29 18:14:10
namespace UGFExtensions.Hotfix
{

	public partial class SettingForm : UGuiForm
	{
        private Language m_SelectedLanguage;
		
        protected override void OnInit(object userdata)
		{
			base.OnInit(userdata);

			GetBindComponents(gameObject);
            m_Toggle_MusicMute.onValueChanged.AddListener(OnMusicMuteChanged);
            m_Slider_MusicVolume.onValueChanged.AddListener(OnMusicVolumeChanged);
            
            m_Toggle_SoundMute.onValueChanged.AddListener(OnSoundMuteChanged);
            m_Slider_SoundVolume.onValueChanged.AddListener(OnMusicVolumeChanged);
            
            m_Toggle_UISoundMute.onValueChanged.AddListener(OnUISoundMuteChanged);
            m_Slider_UISoundVolume.onValueChanged.AddListener(OnUISoundVolumeChanged);
            
            m_Toggle_English.onValueChanged.AddListener(OnEnglishSelected);
            m_Toggle_ChineseSimplified.onValueChanged.AddListener(OnChineseSimplifiedSelected);
            m_Toggle_ChineseTraditional.onValueChanged.AddListener(OnChineseTraditionalSelected);
            m_Toggle_Korean.onValueChanged.AddListener(OnKoreanSelected);
            
            m_CommonButton_Confirm.OnHover.AddListener(()=>PlayUISound(10000));
            m_CommonButton_Confirm.OnClick.AddListener(OnSubmitButtonClick);
            m_CommonButton_Cancel.OnHover.AddListener(()=>PlayUISound(10000));
            m_CommonButton_Cancel.OnClick.AddListener(Close);
            m_SelectedLanguage = Language.Unspecified;
        }
		
        public void OnMusicMuteChanged(bool isOn)
        {
            PlayUISound(10001);
            GameEntry.Sound.Mute("Music", !isOn);
            m_Slider_MusicVolume.gameObject.SetActive(isOn);
        }

        public void OnMusicVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Music", volume);
        }

        public void OnSoundMuteChanged(bool isOn)
        {
            PlayUISound(10001);
            GameEntry.Sound.Mute("Sound", !isOn);
            m_Slider_SoundVolume.gameObject.SetActive(isOn);
        }

        public void OnSoundVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("Sound", volume);
        }

        public void OnUISoundMuteChanged(bool isOn)
        {
            PlayUISound(10001);
            GameEntry.Sound.Mute("UISound", !isOn);
            m_Slider_UISoundVolume.gameObject.SetActive(isOn);
        }

        public void OnUISoundVolumeChanged(float volume)
        {
            GameEntry.Sound.SetVolume("UISound", volume);
        }

        public void OnEnglishSelected(bool isOn)
        {
            PlayUISound(10001);

            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.English;
            RefreshLanguageTips();
        }

        public void OnChineseSimplifiedSelected(bool isOn)
        {
            PlayUISound(10001);

            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.ChineseSimplified;
            RefreshLanguageTips();
        }

        public void OnChineseTraditionalSelected(bool isOn)
        {
            PlayUISound(10001);

            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.ChineseTraditional;
            RefreshLanguageTips();
        }

        public void OnKoreanSelected(bool isOn)
        {
            PlayUISound(10001);

            if (!isOn)
            {
                return;
            }

            m_SelectedLanguage = Language.Korean;
            RefreshLanguageTips();
        }

        public void OnSubmitButtonClick()
        {
            PlayUISound(10001);

            if (m_SelectedLanguage == GameEntry.Localization.Language)
            {
                Close();
                return;
            }

            GameEntry.Setting.SetString(Constant.Setting.Language, m_SelectedLanguage.ToString());
            GameEntry.Setting.Save();

            GameEntry.Sound.StopMusic();
            UnityGameFramework.Runtime.GameEntry.Shutdown(ShutdownType.Restart);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_Toggle_MusicMute.isOn = !GameEntry.Sound.IsMuted("Music");
            m_Slider_MusicVolume.value = GameEntry.Sound.GetVolume("Music");

            m_Toggle_SoundMute.isOn = !GameEntry.Sound.IsMuted("Sound");
            m_Slider_SoundVolume.value = GameEntry.Sound.GetVolume("Sound");

            m_Toggle_UISoundMute.isOn = !GameEntry.Sound.IsMuted("UISound");
            m_Slider_UISoundVolume.value = GameEntry.Sound.GetVolume("UISound");

            m_SelectedLanguage = GameEntry.Localization.Language;
            switch (m_SelectedLanguage)
            {
                case Language.English:
                    m_Toggle_English.isOn = true;
                    break;

                case Language.ChineseSimplified:
                    m_Toggle_ChineseSimplified.isOn = true;
                    break;

                case Language.ChineseTraditional:
                    m_Toggle_ChineseTraditional.isOn = true;
                    break;

                case Language.Korean:
                    m_Toggle_Korean.isOn = true;
                    break;

                default:
                    break;
            }
        }


        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)

        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_CanvasGroup_LanguageTips.gameObject.activeSelf)
            {
                m_CanvasGroup_LanguageTips.alpha = 0.5f + 0.5f * Mathf.Sin(Mathf.PI * Time.time);
            }
        }

        private void RefreshLanguageTips()
        {
            m_CanvasGroup_LanguageTips.gameObject.SetActive(m_SelectedLanguage != GameEntry.Localization.Language);
        }
	}
}
