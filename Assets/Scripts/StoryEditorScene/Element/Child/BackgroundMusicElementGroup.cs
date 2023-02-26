using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.StoryEditorScene.Element.Child
{
	/// <summary>
	/// 배경음악을 정의하는 자식 스크립트입니다.
	/// </summary>
	public class BackgroundMusicElementGroup : ElementGroup
	{
		[SerializeField]
		private Dropdown backgroundMusicDropdown;

		[SerializeField]
		private Dropdown audioTransitionDropdown;

		protected override void Init(Dictionary<string, string> data)
		{
			backgroundMusicDropdown.options.Add(new Dropdown.OptionData("(음소거)"));
			if (data.ContainsKey("AvailableAudios") && data["AvailableAudios"] != "")
			{
				foreach (string musicName in data["AvailableAudios"].Split(" | "))
				{
					backgroundMusicDropdown.options.Add(new Dropdown.OptionData(musicName));
				}
			}

			if (data.ContainsKey("AvailableTransitions") && data["AvailableTransitions"] != "")
			{
				foreach (string transitionName in data["AvailableTransitions"].Split(" | "))
				{
					audioTransitionDropdown.options.Add(new Dropdown.OptionData(transitionName));
				}
			}

			if (data.ContainsKey("Background")) backgroundMusicDropdown.value = int.Parse(data["Background"]) + 1;
			if (data.ContainsKey("Transition")) audioTransitionDropdown.value = int.Parse(data["Transition"]) + 1;
		}

		public override Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>()
			{
				{ "Background", (backgroundMusicDropdown.value - 1).ToString() },
				{ "Transition", (audioTransitionDropdown.value - 1).ToString() }
			};
		}
	}
}
