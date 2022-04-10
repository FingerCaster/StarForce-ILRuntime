using UnityEngine;
using UnityEngine.UI;

//自动生成于：2022/4/10 14:15:55
namespace UGFExtensions.Hotfix
{

	public partial class MenuForm
	{

		private RectTransform m_Transform_Start;
		private RectTransform m_RectTransform_Start;
		private CommonButton m_CommonButton_Start;
		private RectTransform m_Transform_Setting;
		private RectTransform m_RectTransform_Setting;
		private CommonButton m_CommonButton_Setting;
		private RectTransform m_Transform_About;
		private RectTransform m_RectTransform_About;
		private CommonButton m_CommonButton_About;
		private RectTransform m_Transform_Quit;
		private RectTransform m_RectTransform_Quit;
		private CommonButton m_CommonButton_Quit;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Transform_Start = autoBindTool.GetBindComponent<RectTransform>(0);
			m_RectTransform_Start = autoBindTool.GetBindComponent<RectTransform>(1);
			m_CommonButton_Start = autoBindTool.GetBindComponent<CommonButton>(2);
			m_Transform_Setting = autoBindTool.GetBindComponent<RectTransform>(3);
			m_RectTransform_Setting = autoBindTool.GetBindComponent<RectTransform>(4);
			m_CommonButton_Setting = autoBindTool.GetBindComponent<CommonButton>(5);
			m_Transform_About = autoBindTool.GetBindComponent<RectTransform>(6);
			m_RectTransform_About = autoBindTool.GetBindComponent<RectTransform>(7);
			m_CommonButton_About = autoBindTool.GetBindComponent<CommonButton>(8);
			m_Transform_Quit = autoBindTool.GetBindComponent<RectTransform>(9);
			m_RectTransform_Quit = autoBindTool.GetBindComponent<RectTransform>(10);
			m_CommonButton_Quit = autoBindTool.GetBindComponent<CommonButton>(11);
		}
	}
}
