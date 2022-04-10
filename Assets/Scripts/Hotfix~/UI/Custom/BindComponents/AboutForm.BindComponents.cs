using UnityEngine;
using UnityEngine.UI;

//自动生成于：2022/4/10 14:15:55
namespace UGFExtensions.Hotfix
{

	public partial class AboutForm
	{

		private RectTransform m_Transform_Content;
		private RectTransform m_RectTransform_Content;
		private VerticalLayoutGroup m_VerticalLayoutGroup_Content;
		private RectTransform m_Transform_Back;
		private RectTransform m_RectTransform_Back;
		private CommonButton m_CommonButton_Back;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Transform_Content = autoBindTool.GetBindComponent<RectTransform>(0);
			m_RectTransform_Content = autoBindTool.GetBindComponent<RectTransform>(1);
			m_VerticalLayoutGroup_Content = autoBindTool.GetBindComponent<VerticalLayoutGroup>(2);
			m_Transform_Back = autoBindTool.GetBindComponent<RectTransform>(3);
			m_RectTransform_Back = autoBindTool.GetBindComponent<RectTransform>(4);
			m_CommonButton_Back = autoBindTool.GetBindComponent<CommonButton>(5);
		}
	}
}
