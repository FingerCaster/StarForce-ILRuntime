using UnityEngine;
using UGFExtensions;

//自动生成于：2021/10/31 16:10:29
namespace UGFExtensions.Hotfix
{

	public partial class AboutForm
	{

		private RectTransform m_Trans_Content;
		private CommonButton m_CBtn_Back;

		public void GetBindComponents(ComponentAutoBindTool autoBindTool)
		{
			m_Trans_Content = autoBindTool.GetBindComponent<RectTransform>(0);
			m_CBtn_Back = autoBindTool.GetBindComponent<CommonButton>(1);
		}

	}
}
