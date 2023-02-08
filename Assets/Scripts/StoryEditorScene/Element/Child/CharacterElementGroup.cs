using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.StoryEditorScene.Element.Child
{
	/// <summary>
	/// 스탠딩 일러스트를 정의하는 자식 스크립트입니다.
	/// </summary>
	public class CharacterElementGroup : ElementGroup
	{
		[SerializeField]
		private Dropdown characterSlot1Dropdown;

		[SerializeField]
		private Dropdown characterSlot2Dropdown;

		[SerializeField]
		private Dropdown characterSlot3Dropdown;

		[SerializeField]
		private Dropdown imageTransitionDropdown;

		protected override void Init(Dictionary<string, string> data)
		{
			characterSlot1Dropdown.options.Add(new Dropdown.OptionData("(슬롯 비워두기)"));
			characterSlot2Dropdown.options.Add(new Dropdown.OptionData("(슬롯 비워두기)"));
			characterSlot3Dropdown.options.Add(new Dropdown.OptionData("(슬롯 비워두기)"));

			if (data.ContainsKey("AvailableCharacters") && data["AvailableCharacters"] != "")
			{
				foreach (string characterName in data["AvailableCharacters"].Split(" | "))
				{
					characterSlot1Dropdown.options.Add(new Dropdown.OptionData(characterName));
					characterSlot2Dropdown.options.Add(new Dropdown.OptionData(characterName));
					characterSlot3Dropdown.options.Add(new Dropdown.OptionData(characterName));
				}
			}

			if (data.ContainsKey("AvailableTransitions") && data["AvailableTransitions"] != "")
			{
				foreach (string transitionName in data["AvailableTransitions"].Split(" | "))
				{
					imageTransitionDropdown.options.Add(new Dropdown.OptionData(transitionName));
				}
			}

			if (data.ContainsKey("CharacterSlot1"))
				characterSlot1Dropdown.value = int.Parse(data["CharacterSlot1"]) + 1;
			if (data.ContainsKey("CharacterSlot2"))
				characterSlot2Dropdown.value = int.Parse(data["CharacterSlot2"]) + 1;
			if (data.ContainsKey("CharacterSlot3"))
				characterSlot3Dropdown.value = int.Parse(data["CharacterSlot3"]) + 1;
			if (data.ContainsKey("Transition")) imageTransitionDropdown.value = int.Parse(data["Transition"]) + 1;
		}

		public override Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>()
			{
				{ "CharacterSlot1", (characterSlot1Dropdown.value - 1).ToString() },
				{
					"CharacterSlot2",
					(characterSlot2Dropdown.value - 1).ToString()
				},
				{ "CharacterSlot3", (characterSlot3Dropdown.value - 1).ToString() },
				{ "Transition", (imageTransitionDropdown.value - 1).ToString() }
			};
		}
	}
}
