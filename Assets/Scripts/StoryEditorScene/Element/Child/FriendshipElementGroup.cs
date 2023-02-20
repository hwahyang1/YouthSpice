using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace YouthSpice.StoryEditorScene.Element.Child
{
	/// <summary>
	/// 호감도를 정의하는 자식 스크립트입니다.
	/// </summary>
	public class FriendshipElementGroup : ElementGroup
	{
		[SerializeField]
		private Dropdown characterNameDropdown;

		[SerializeField]
		private TMP_InputField characterFriendshipInputField;

		protected override void Init(Dictionary<string, string> data)
		{
			if (data["AvailableNames"] != "")
			{
				foreach (string characterName in data["AvailableNames"].Split(" | "))
				{
					characterNameDropdown.options.Add(new Dropdown.OptionData(characterName));
				}
			}

			if (data.ContainsKey("Character")) characterNameDropdown.value = int.Parse(data["Character"]);
			if (data.ContainsKey("Friendship")) characterFriendshipInputField.text = data["Friendship"];
		}

		public override Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>()
			{
				{ "Character", characterNameDropdown.value.ToString() },
				{ "Friendship", characterFriendshipInputField.text }
			};
		}
	}
}
