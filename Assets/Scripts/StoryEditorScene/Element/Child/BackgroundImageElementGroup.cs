using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.StoryEditorScene.Element.Child
{
	/// <summary>
	/// 배경이미지를 정의하는 자식 스크립트입니다.
	/// </summary>
	public class BackgroundImageElementGroup : ElementGroup
	{
		[SerializeField]
		private Dropdown backgroundImageDropdown;
		
		[SerializeField]
		private Dropdown imageTransitionDropdown;

		protected override void Init(Dictionary<string, string> data)
		{
			if (data.ContainsKey("AvailableImages") && data["AvailableImages"] != "")
			{
				foreach (string imageName in data["AvailableImages"].Split(" | "))
				{
					backgroundImageDropdown.options.Add(new Dropdown.OptionData(imageName));
				}
			}
			
			if (data.ContainsKey("AvailableTransitions") && data["AvailableTransitions"] != "")
			{
				foreach (string transitionName in data["AvailableTransitions"].Split(" | "))
				{
					imageTransitionDropdown.options.Add(new Dropdown.OptionData(transitionName));
				}
			}
			
			if (data.ContainsKey("Background")) backgroundImageDropdown.value = int.Parse(data["Background"]);
			if (data.ContainsKey("Transition")) imageTransitionDropdown.value = int.Parse(data["Transition"]) + 1;
		}
		
		public override Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>(){{"Background", backgroundImageDropdown.value.ToString()},{ "Transition", (imageTransitionDropdown.value - 1).ToString() }};
		}
	}
}
