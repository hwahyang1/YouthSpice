using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.StoryEditorScene.Element.Child
{
	/// <summary>
	/// 낧짜 이미지를 정의하는 자식 스크립트입니다.
	/// </summary>
	public class DayImageElementGroup : ElementGroup
	{
		[SerializeField]
		private Dropdown dayImageDropdown;

		[SerializeField]
		private Dropdown imageTransitionDropdown;

		protected override void Init(Dictionary<string, string> data)
		{
			dayImageDropdown.options.Add(new Dropdown.OptionData("(사용하지 않음)"));
			if (data.ContainsKey("AvailableImages") && data["AvailableImages"] != "")
			{
				foreach (string imageName in data["AvailableImages"].Split(" | "))
				{
					dayImageDropdown.options.Add(new Dropdown.OptionData(imageName));
				}
			}

			if (data.ContainsKey("AvailableTransitions") && data["AvailableTransitions"] != "")
			{
				foreach (string transitionName in data["AvailableTransitions"].Split(" | "))
				{
					imageTransitionDropdown.options.Add(new Dropdown.OptionData(transitionName));
				}
			}

			if (data.ContainsKey("Day")) dayImageDropdown.value = int.Parse(data["Day"]) + 1;
			if (data.ContainsKey("Transition")) imageTransitionDropdown.value = int.Parse(data["Transition"]) + 1;
		}

		public override Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>()
			{
				{ "Day", (dayImageDropdown.value - 1).ToString() },
				{ "Transition", (imageTransitionDropdown.value - 1).ToString() }
			};
		}
	}
}
