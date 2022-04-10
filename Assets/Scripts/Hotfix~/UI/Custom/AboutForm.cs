using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

//自动生成于：2021/10/29 18:11:55
namespace UGFExtensions.Hotfix
{

	public partial class AboutForm : UGuiForm
	{
		private float m_ScrollSpeed;
		private float m_InitPosition;

		protected override void OnInit(object userdata)
		{
			base.OnInit(userdata);
			GetBindComponents(gameObject);
			CanvasScaler canvasScaler = gameObject.GetComponentInParent<CanvasScaler>();
			if (canvasScaler == null)
			{
				Log.Warning("Can not find CanvasScaler component.");
				return;
			}

			m_InitPosition = -0.5f * canvasScaler.referenceResolution.x * Screen.height / Screen.width;
			m_ScrollSpeed = 20;
			m_CommonButton_Back.OnHover.AddListener(()=>PlayUISound(10000));
			m_CommonButton_Back.OnClick.AddListener(Close);
		}

		protected override void OnOpen(object userdata)
		{
			base.OnOpen(userdata);
			m_Transform_Content.SetLocalPositionY(m_InitPosition);

			// 换个音乐
			GameEntry.Sound.PlayMusic(3);
		}

		protected override void OnClose(bool isShutdown, object userdata)
		{
			base.OnClose(isShutdown, userdata);

			// 还原音乐
			GameEntry.Sound.PlayMusic(1);
		}

		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);
			m_Transform_Content.AddLocalPositionY(m_ScrollSpeed * elapseSeconds);
			if (m_Transform_Content.localPosition.y > m_Transform_Content.sizeDelta.y - m_InitPosition)
			{
				m_Transform_Content.SetLocalPositionY(m_InitPosition);
			}
		}
	}
}
