using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Config;

namespace YouthSpice.ConfigScene.SettingElements.SubElements
{
	/// <summary>
	/// "타이핑 효과 속도" 항목을 정의합니다.
	/// </summary>
	public class TypingSpeedScrollElement : ScrollElement
	{
		protected override void Start()
		{
			currentValue = ConfigManager.Instance.GetConfig().typingSpeed * 100f;
			base.Start();
		}

		protected override void OnValueChanged(float currentValue)
		{
			base.OnValueChanged(currentValue);
			DefineConfig config = ConfigManager.Instance.GetConfig();
			float adjustedValue = Mathf.Floor(currentValue) / 100f;
			config.typingSpeed = adjustedValue;
			ConfigManager.Instance.SetConfig(config);
			ConfigManager.Instance.ApplyConfig();
			ConfigManager.Instance.SaveConfig();
		}
	}
}
