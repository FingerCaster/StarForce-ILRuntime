using UGFExtensions;

//自动生成于：2021/10/31 15:54:13
namespace UGFExtensions.Hotfix
{

	public partial class MenuForm
	{

		private CommonButton m_CBtn_Start;
		private CommonButton m_CBtn_Setting;
		private CommonButton m_CBtn_About;
		private CommonButton m_CBtn_Quit;

		public void GetBindComponents(ComponentAutoBindTool autoBindTool)
		{
			m_CBtn_Start = autoBindTool.GetBindComponent<CommonButton>(0);
			m_CBtn_Setting = autoBindTool.GetBindComponent<CommonButton>(1);
			m_CBtn_About = autoBindTool.GetBindComponent<CommonButton>(2);
			m_CBtn_Quit = autoBindTool.GetBindComponent<CommonButton>(3);
		}

	}
}
