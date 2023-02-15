using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Config;

namespace YouthSpice.ConfigScene.SettingElements.SubElements
{
	/// <summary>
	/// "전체화면 모드" 항목을 정의합니다.
	/// </summary>
	public class FullScreenSelectElement : SelectionElement
	{
		protected override void Start()
		{
			currentSelection = ConfigManager.Instance.GetConfig().useFullscreen;
			base.Start();
		}

		protected override void OnValueChanged()
		{
			DefineConfig config = ConfigManager.Instance.GetConfig();
			config.useFullscreen = currentSelection;
			ConfigManager.Instance.SetConfig(config);
			ConfigManager.Instance.ApplyConfig();
			ConfigManager.Instance.SaveConfig();
		}
	}
}
