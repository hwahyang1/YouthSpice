using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

using Newtonsoft.Json;

namespace YouthSpice.PreloadScene.Config
{
	/// <summary>
	/// 설정을 관리합니다.
	/// </summary>
	public class ConfigManager : Singleton<ConfigManager>
	{
		private DefineConfig config;
		private string configPath;

		private FullScreenMode fullScreenMode;

		public float BackgroundVolume => config.backgroundVolume;
		public float EffectVolume => config.effectVolume;

		protected override void Awake()
		{
			base.Awake();

			configPath = Application.persistentDataPath + @"\YouthSpice.cfg";

			LoadConfig();
			ApplyConfig();
		}

		private int lastWidth;
		private int lastHeight;

		protected override void Update()
		{
			if (lastWidth != Screen.width)
			{
				Screen.SetResolution(Screen.width, Mathf.RoundToInt(Screen.width * (9f / 16f)), fullScreenMode);
			}
			else if (lastHeight != Screen.height)
			{
				Screen.SetResolution(Mathf.RoundToInt(Screen.height * (16f / 9f)), Screen.height, fullScreenMode);
			}

			lastWidth = Screen.width;
			lastHeight = Screen.height;
		}

		/// <summary>
		/// 파일에서 설정을 가져옵니다.
		/// 파일이 존재하지 않을 경우, 기본값을 불러오고 파일로 저장합니다.
		/// </summary>
		public void LoadConfig()
		{
			if (!File.Exists(configPath))
			{
				config = new DefineConfig();
				SaveConfig();
			}
			else
			{
				string data = File.ReadAllText(configPath);
				config = JsonConvert.DeserializeObject<DefineConfig>(data);
			}
		}

		/// <summary>
		/// 현재 설정값을 반환합니다.
		/// </summary>
		/// <returns>현재 저장된 설정값입니다.</returns>
		public DefineConfig GetConfig() => config;

		/// <summary>
		/// 현재 설정을 변경합니다.
		/// </summary>
		/// <param name="changeTo">변경할 설정값을 지정합니다.</param>
		public void SetConfig(DefineConfig changeTo) => config = changeTo;

		/// <summary>
		/// 현재 저장된 설정값을 적용합니다.
		/// </summary>
		public void ApplyConfig()
		{
			fullScreenMode = (config.useFullscreen) ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
			Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, fullScreenMode);

			QualitySettings.vSyncCount = 1;
			Application.targetFrameRate = 144; // 사실 의미 없음
		}

		/// <summary>
		/// 현재 저장된 설정값을 파일로 저장합니다.
		/// </summary>
		public void SaveConfig()
		{
			string data = JsonConvert.SerializeObject(config);
			File.WriteAllText(configPath, data);
		}
	}
}
