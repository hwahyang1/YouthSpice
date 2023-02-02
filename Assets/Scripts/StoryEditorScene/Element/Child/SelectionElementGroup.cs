using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

namespace YouthSpice.StoryEditorScene.Element.Child
{
	/// <summary>
	/// 분기점을 정의하는 자식 스크립트입니다.
	/// </summary>
	public class SelectionElementGroup : ElementGroup
	{
		[SerializeField, ReadOnly]
		private int currentSelectionCount = 0;

		[SerializeField]
		private Dropdown selectionDropdown;

		[SerializeField]
		private Transform selectionParent;

		[SerializeField]
		private GameObject subElementPrefab;

		public void CreateSelectionNameSubElementGroup(Dictionary<string, string> data)
		{
			Instantiate(subElementPrefab, selectionParent).GetComponent<SelectionNameSubElementGroup>().Init(base.GetElementManager(), data);
		}
		
		protected override void Init(Dictionary<string, string> data)
		{
			if (data.ContainsKey("Count") && data["Count"] != "")
			{
				currentSelectionCount = int.Parse(data["Count"]) - 1;
				selectionDropdown.value = currentSelectionCount;
				for (int i = 0; i < currentSelectionCount + 1; i++)
				{
					CreateSelectionNameSubElementGroup(null);
				}
			}
			else
			{
				CreateSelectionNameSubElementGroup(null);
			}

			LayoutRebuilder.ForceRebuildLayoutImmediate(selectionParent.GetComponent<RectTransform>());
		}

		public override Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>() { { "Count", (selectionDropdown.value + 1).ToString() } };
		}

		public void OnDropdownValueChanged()
		{
			int newSelectionCount = selectionDropdown.value;
			int diff = newSelectionCount - currentSelectionCount;

			if (currentSelectionCount == newSelectionCount) return;
			if (currentSelectionCount > newSelectionCount) // 줄여야 하는 경우
			{
				for (int i = selectionParent.childCount - 1; i >= 0; i--)
				{
					SelectionNameSubElementGroup currentObject = selectionParent.GetChild(i).gameObject
						.GetComponent<SelectionNameSubElementGroup>();
					if (currentObject == null) continue; // 다른 Element인 경우
					Destroy(currentObject.gameObject);
					diff++;
					if (diff == 0) break; // 더 지울 필요가 없는 경우
				}
			}
			else // 늘려야 하는 경우
			{
				for (int i = 0; i < diff; i++)
				{
					CreateSelectionNameSubElementGroup(null);
				}
			}

			currentSelectionCount = newSelectionCount;

			LayoutRebuilder.ForceRebuildLayoutImmediate(selectionParent.GetComponent<RectTransform>());
		}
	}
}
