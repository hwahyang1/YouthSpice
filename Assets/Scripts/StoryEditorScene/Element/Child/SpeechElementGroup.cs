using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace YouthSpice.StoryEditorScene.Element.Child
{
	/// <summary>
	/// 대사 항목을 정의하는 자식 스크립트입니다.
	/// </summary>
	public class SpeechElementGroup : ElementGroup
	{
		[SerializeField]
		private Dropdown characterNameDropdown;

		[SerializeField]
		private TMP_InputField characterScriptInputField;

		protected override void Init(Dictionary<string, string> data)
		{
			characterNameDropdown.options.Add(new Dropdown.OptionData("(이름 표시하지 않기)"));
			if (data.ContainsKey("AvailableNames") && data["AvailableNames"] != "")
			{
				foreach (string characterName in data["AvailableNames"].Split(" | "))
				{
					characterNameDropdown.options.Add(new Dropdown.OptionData(characterName));
				}
			}

			if (data.ContainsKey("Character")) characterNameDropdown.value = int.Parse(data["Character"]) + 1;
			if (data.ContainsKey("Script")) characterScriptInputField.text = data["Script"];
		}

		public override Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>()
			{
				{ "Character", (characterNameDropdown.value - 1).ToString() },
				{ "Script", characterScriptInputField.text }
			};
		}
	}
}
