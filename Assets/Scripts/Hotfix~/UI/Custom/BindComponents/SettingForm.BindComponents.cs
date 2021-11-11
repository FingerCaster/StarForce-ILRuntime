using UnityEngine.UI;
using UnityEngine;
using UGFExtensions;

//自动生成于：2021/11/1 12:27:20
namespace UGFExtensions.Hotfix
{

	public partial class SettingForm
	{

		private Toggle m_Tog_MusicMute;
		private Slider m_Slider_MusicVolume;
		private Toggle m_Tog_SoundMute;
		private Slider m_Slider_SoundVolume;
		private Toggle m_Tog_UISoundMute;
		private Slider m_Slider_UISoundVolume;
		private Toggle m_Tog_English;
		private Toggle m_Tog_ChineseSimplified;
		private Toggle m_Tog_ChineseTraditional;
		private Toggle m_Tog_Korean;
		private CanvasGroup m_Group_LanguageTips;
		private CommonButton m_CBtn_Confirm;
		private CommonButton m_CBtn_Cancel;

		public void GetBindComponents(ComponentAutoBindTool autoBindTool)
		{
			m_Tog_MusicMute = autoBindTool.GetBindComponent<Toggle>(0);
			m_Slider_MusicVolume = autoBindTool.GetBindComponent<Slider>(1);
			m_Tog_SoundMute = autoBindTool.GetBindComponent<Toggle>(2);
			m_Slider_SoundVolume = autoBindTool.GetBindComponent<Slider>(3);
			m_Tog_UISoundMute = autoBindTool.GetBindComponent<Toggle>(4);
			m_Slider_UISoundVolume = autoBindTool.GetBindComponent<Slider>(5);
			m_Tog_English = autoBindTool.GetBindComponent<Toggle>(6);
			m_Tog_ChineseSimplified = autoBindTool.GetBindComponent<Toggle>(7);
			m_Tog_ChineseTraditional = autoBindTool.GetBindComponent<Toggle>(8);
			m_Tog_Korean = autoBindTool.GetBindComponent<Toggle>(9);
			m_Group_LanguageTips = autoBindTool.GetBindComponent<CanvasGroup>(10);
			m_CBtn_Confirm = autoBindTool.GetBindComponent<CommonButton>(11);
			m_CBtn_Cancel = autoBindTool.GetBindComponent<CommonButton>(12);
		}

	}
}
