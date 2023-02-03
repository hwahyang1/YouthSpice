using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json;
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

		public Transform SelectionParent => selectionParent;

		[SerializeField]
		private GameObject subElementPrefab;

		protected override void Init(Dictionary<string, string> data)
		{
			if (data.ContainsKey("Count") && data["Count"] != "")
			{
				currentSelectionCount = int.Parse(data["Count"]) - 1;
				selectionDropdown.value = currentSelectionCount;
				// 짜피 data에 저장되는 거라 안해도 될듯?
				/*for (int i = 0; i < currentSelectionCount + 1; i++)
				{
					base.ElementManager.NewElement(ChapterElementType.SelectionName, null, selectionParent);
				}*/

				for (int i = 0;; i++)
				{
					if (!data.ContainsKey(i.ToString())) break;
					ChapterElement currentData = JsonConvert.DeserializeObject<ChapterElement>(data[i.ToString()]);
					base.ElementManager.NewElement(currentData.Type, currentData.Data, selectionParent);
				}
			}
			else
			{
				base.ElementManager.NewElement(ChapterElementType.SelectionName, null, selectionParent);
			}
		}

		public override Dictionary<string, string> GetData()
		{
			Dictionary<string, string> data = new Dictionary<string, string>()
				{ { "Count", (selectionDropdown.value + 1).ToString() } };
			
			for (int i = 0; i < selectionParent.childCount; i++)
			{
				ElementGroup targetElement = selectionParent.GetChild(i).GetComponent<ElementGroup>();
				Dictionary<string, string> elementData = base.ElementManager.ElementConverter.GetData(targetElement);
				data.Add(i.ToString(), JsonConvert.SerializeObject(new ChapterElement(targetElement.Type, elementData)));
			}

			return data;
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
					base.ElementManager.NewElement(ChapterElementType.SelectionName, null, selectionParent);
				}
			}

			currentSelectionCount = newSelectionCount;
		}
	}
}
