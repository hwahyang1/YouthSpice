using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.CookingScene.WholeStage;
using YouthSpice.PreloadScene.Item;

namespace YouthSpice.CookingScene.SelectionStage.UI
{
	/// <summary>
	/// 식재료 선택에 사용되는 항목들을 관리합니다
	/// </summary>
	public class ElementsManager : MonoBehaviour
	{
		[Header("Prefab")]
		[SerializeField]
		private GameObject horizontalElementGroupPrefab;

		[SerializeField]
		private GameObject selectNoneElementPrefab;

		[SerializeField]
		private GameObject elementPrefab;

		[Header("GameObjects")]
		[SerializeField]
		private Transform rootParent;

		[Header("External Classes")]
		[SerializeField]
		private SelectionManager selectionManager;

		private List<List<Element>> displayItems = new List<List<Element>>();

		private List<int> usedItems = new List<int>();
		private Transform currentParent = null;

		private void Awake()
		{
			for (int i = 0; i < rootParent.childCount; i++)
			{
				Destroy(rootParent.GetChild(i).gameObject);
			}

			displayItems.Add(new List<Element>());

			currentParent = Instantiate(horizontalElementGroupPrefab, Vector3.zero, Quaternion.identity, rootParent)
				.transform;
			Element child = Instantiate(selectNoneElementPrefab, Vector3.zero, Quaternion.identity, currentParent)
				.GetComponent<Element>();
			child.Init(0, 0, -1, Select);
			displayItems[0].Add(child);
		}

		private void Start()
		{
			int majorIndex = 0;
			int minorIndex = 1;

			foreach (int item in GameInfo.Instance.inventory)
			{
				if (usedItems.Contains(item)) continue;

				usedItems.Add(item);

				if (currentParent.childCount >= 7)
				{
					currentParent = Instantiate(horizontalElementGroupPrefab, Vector3.zero, Quaternion.identity,
						rootParent).transform;
					displayItems.Add(new List<Element>());
					majorIndex++;
					minorIndex = 0;
				}

				Element child = Instantiate(elementPrefab, Vector3.zero, Quaternion.identity, currentParent)
					.GetComponent<Element>();
				child.Init(majorIndex, minorIndex, item, Select);
				child.gameObject.name = $"{item} - {ItemBuffer.Instance.items[item].name}";
				displayItems[majorIndex].Add(child);
				minorIndex++;
			}

			usedItems.Clear();
		}

		/// <summary>
		/// 항목이 선택되었을 때의 이벤트를 처리합니다.
		/// </summary>
		/// <param name="majorIndex">선택된 항목의 행을 지정합니다.</param>
		/// <param name="minorIndex">선택된 항목의 열을 지정합니다.</param>
		private void Select(int majorIndex, int minorIndex)
		{
			if (selectionManager.GetEmptyElement() == -1) return;

			Element target = displayItems[majorIndex][minorIndex];
			selectionManager.AddItem(target.ItemNumber, () => { ReturnItem(majorIndex, minorIndex); });
			// -1은 선택 포기
			if (target.ItemNumber != -1) target.SetInteractable(false);
		}

		/// <summary>
		/// 항목 선택을 취소합니다.
		/// </summary>
		/// <param name="majorIndex">취소할 항목의 행을 지정합니다.</param>
		/// <param name="minorIndex">취소할 항목의 열을 지정합니다.</param>
		private void ReturnItem(int majorIndex, int minorIndex)
		{
			Element target = displayItems[majorIndex][minorIndex];
			target.SetInteractable(true);
		}
	}
}
