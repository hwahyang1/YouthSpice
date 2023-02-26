using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;

namespace YouthSpice.StoryEditorScene.Element.Child
{
	/// <summary>
	/// 선택지를 정의하는 자식 스크립트입니다.
	/// </summary>
	public class SelectionNameSubElementGroup : ElementGroup
	{
		[SerializeField]
		private TMP_InputField selectionNameInputField;

		protected override void Init(Dictionary<string, string> data)
		{
			if (data != null && data.ContainsKey("SelectionName") && data["SelectionName"] != "")
			{
				selectionNameInputField.text = data["SelectionName"];
			}
		}

		public override Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>() { { "SelectionName", selectionNameInputField.text } };
		}
	}
}
