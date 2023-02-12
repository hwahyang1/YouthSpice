using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Config;

namespace YouthSpice.ConfigScene.SettingElements.SubElements
{
	/// <summary>
	/// "BGM 음량" 항목을 정의합니다.
	/// </summary>
	public class BackgroundAudioScrollElement : ScrollElement
	{
		protected override void Start()
		{
			currentValue = ConfigManager.Instance.GetConfig().backgroundVolume * 100f;
			base.Start();
		}

		protected override void OnValueChanged(float currentValue)
		{
			base.OnValueChanged(currentValue);
			DefineConfig config = ConfigManager.Instance.GetConfig();
			float adjustedValue = Mathf.Floor(currentValue) / 100f;
			config.backgroundVolume = adjustedValue;
			ConfigManager.Instance.SetConfig(config);
			ConfigManager.Instance.ApplyConfig();
			ConfigManager.Instance.SaveConfig();
		}
	}
}
