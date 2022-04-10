using UnityEngine;
using UnityEngine.UI;

//自动生成于：2022/4/10 14:15:55
namespace UGFExtensions.Hotfix
{

	public partial class SettingForm
	{

		private RectTransform m_Transform_MusicMute;
		private RectTransform m_RectTransform_MusicMute;
		private Toggle m_Toggle_MusicMute;
		private RectTransform m_Transform_MusicVolume;
		private RectTransform m_RectTransform_MusicVolume;
		private Slider m_Slider_MusicVolume;
		private RectTransform m_Transform_SoundMute;
		private RectTransform m_RectTransform_SoundMute;
		private Toggle m_Toggle_SoundMute;
		private RectTransform m_Transform_SoundVolume;
		private RectTransform m_RectTransform_SoundVolume;
		private Slider m_Slider_SoundVolume;
		private RectTransform m_Transform_UISoundMute;
		private RectTransform m_RectTransform_UISoundMute;
		private Toggle m_Toggle_UISoundMute;
		private RectTransform m_Transform_UISoundVolume;
		private RectTransform m_RectTransform_UISoundVolume;
		private Slider m_Slider_UISoundVolume;
		private RectTransform m_Transform_English;
		private RectTransform m_RectTransform_English;
		private Toggle m_Toggle_English;
		private RectTransform m_Transform_ChineseSimplified;
		private RectTransform m_RectTransform_ChineseSimplified;
		private Toggle m_Toggle_ChineseSimplified;
		private RectTransform m_Transform_ChineseTraditional;
		private RectTransform m_RectTransform_ChineseTraditional;
		private Toggle m_Toggle_ChineseTraditional;
		private RectTransform m_Transform_Korean;
		private RectTransform m_RectTransform_Korean;
		private Toggle m_Toggle_Korean;
		private RectTransform m_Transform_LanguageTips;
		private RectTransform m_RectTransform_LanguageTips;
		private CanvasGroup m_CanvasGroup_LanguageTips;
		private RectTransform m_Transform_Confirm;
		private RectTransform m_RectTransform_Confirm;
		private CommonButton m_CommonButton_Confirm;
		private RectTransform m_Transform_Cancel;
		private RectTransform m_RectTransform_Cancel;
		private CommonButton m_CommonButton_Cancel;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Transform_MusicMute = autoBindTool.GetBindComponent<RectTransform>(0);
			m_RectTransform_MusicMute = autoBindTool.GetBindComponent<RectTransform>(1);
			m_Toggle_MusicMute = autoBindTool.GetBindComponent<Toggle>(2);
			m_Transform_MusicVolume = autoBindTool.GetBindComponent<RectTransform>(3);
			m_RectTransform_MusicVolume = autoBindTool.GetBindComponent<RectTransform>(4);
			m_Slider_MusicVolume = autoBindTool.GetBindComponent<Slider>(5);
			m_Transform_SoundMute = autoBindTool.GetBindComponent<RectTransform>(6);
			m_RectTransform_SoundMute = autoBindTool.GetBindComponent<RectTransform>(7);
			m_Toggle_SoundMute = autoBindTool.GetBindComponent<Toggle>(8);
			m_Transform_SoundVolume = autoBindTool.GetBindComponent<RectTransform>(9);
			m_RectTransform_SoundVolume = autoBindTool.GetBindComponent<RectTransform>(10);
			m_Slider_SoundVolume = autoBindTool.GetBindComponent<Slider>(11);
			m_Transform_UISoundMute = autoBindTool.GetBindComponent<RectTransform>(12);
			m_RectTransform_UISoundMute = autoBindTool.GetBindComponent<RectTransform>(13);
			m_Toggle_UISoundMute = autoBindTool.GetBindComponent<Toggle>(14);
			m_Transform_UISoundVolume = autoBindTool.GetBindComponent<RectTransform>(15);
			m_RectTransform_UISoundVolume = autoBindTool.GetBindComponent<RectTransform>(16);
			m_Slider_UISoundVolume = autoBindTool.GetBindComponent<Slider>(17);
			m_Transform_English = autoBindTool.GetBindComponent<RectTransform>(18);
			m_RectTransform_English = autoBindTool.GetBindComponent<RectTransform>(19);
			m_Toggle_English = autoBindTool.GetBindComponent<Toggle>(20);
			m_Transform_ChineseSimplified = autoBindTool.GetBindComponent<RectTransform>(21);
			m_RectTransform_ChineseSimplified = autoBindTool.GetBindComponent<RectTransform>(22);
			m_Toggle_ChineseSimplified = autoBindTool.GetBindComponent<Toggle>(23);
			m_Transform_ChineseTraditional = autoBindTool.GetBindComponent<RectTransform>(24);
			m_RectTransform_ChineseTraditional = autoBindTool.GetBindComponent<RectTransform>(25);
			m_Toggle_ChineseTraditional = autoBindTool.GetBindComponent<Toggle>(26);
			m_Transform_Korean = autoBindTool.GetBindComponent<RectTransform>(27);
			m_RectTransform_Korean = autoBindTool.GetBindComponent<RectTransform>(28);
			m_Toggle_Korean = autoBindTool.GetBindComponent<Toggle>(29);
			m_Transform_LanguageTips = autoBindTool.GetBindComponent<RectTransform>(30);
			m_RectTransform_LanguageTips = autoBindTool.GetBindComponent<RectTransform>(31);
			m_CanvasGroup_LanguageTips = autoBindTool.GetBindComponent<CanvasGroup>(32);
			m_Transform_Confirm = autoBindTool.GetBindComponent<RectTransform>(33);
			m_RectTransform_Confirm = autoBindTool.GetBindComponent<RectTransform>(34);
			m_CommonButton_Confirm = autoBindTool.GetBindComponent<CommonButton>(35);
			m_Transform_Cancel = autoBindTool.GetBindComponent<RectTransform>(36);
			m_RectTransform_Cancel = autoBindTool.GetBindComponent<RectTransform>(37);
			m_CommonButton_Cancel = autoBindTool.GetBindComponent<CommonButton>(38);
		}
	}
}
