using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Config;

namespace YouthSpice.ConfigScene.SettingElements.SubElements
{
	/// <summary>
	/// "타이핑 효과" 항목을 정의합니다.
	/// </summary>
	public class TypingEffectSelectElement : SelectionElement
	{
		protected override void Start()
		{
			currentSelection = ConfigManager.Instance.GetConfig().useTypingEffect;
			base.Start();
		}

		protected override void OnValueChanged()
		{
			DefineConfig config = ConfigManager.Instance.GetConfig();
			config.useTypingEffect = currentSelection;
			ConfigManager.Instance.SetConfig(config);
			ConfigManager.Instance.ApplyConfig();
			ConfigManager.Instance.SaveConfig();
		}
	}
}
