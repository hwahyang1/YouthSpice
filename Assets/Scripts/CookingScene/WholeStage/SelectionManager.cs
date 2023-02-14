using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.CookingScene.RecipeStage;
using YouthSpice.CookingScene.WholeStage.UI;

namespace YouthSpice.CookingScene.WholeStage
{
	/// <summary>
	/// 선택된 항목들을 관리합니다
	/// </summary>
	public class SelectionManager : MonoBehaviour
	{
		[Header("Config")]
		[SerializeField]
		private int selectionCount = 5;

		[Header("Status")]
		[SerializeField]
		private int[] items;
		
		[Header("Prefabs")]
		[SerializeField]
		private GameObject elementPrefab;
	
		[Header("GameObjects")]
		[SerializeField]
		private Transform elementParent;

		[Header("Classes")]
		[SerializeField]
		private GameManager gameManager;
		[SerializeField]
		private RecipeManager recipeManager;
		private ButtonManager buttonManager;

		private void Awake()
		{
			for (int i = 0; i < elementParent.childCount; i++)
			{
				Destroy(elementParent.GetChild(i).gameObject);
			}

			buttonManager = GetComponent<ButtonManager>();
		}

		private void Start()
		{
			items = new int[selectionCount];
			Array.Fill(items, -2);
			for (int i = 0; i < selectionCount; i++)
			{
				ShowSelection child = Instantiate(elementPrefab, Vector3.zero, Quaternion.identity, elementParent).GetComponent<ShowSelection>();
				child.Init();
				child.Set(-2);
			}
		}

		public void GoNext()
		{
			CookingFlow current = gameManager.CurrentChapter;

			switch (current)
			{
				case CookingFlow.Selection:
					break;
				case CookingFlow.Recipe:
					SetCloseButton(false);
					break;
				case CookingFlow.Result:
					break;
			}
		}
		
		/* ====================================== Selection Stage ====================================== */

		/// <summary>
		/// 빈 Element를 반환합니다.
		/// -1인 경우, 빈 ELement가 없는 경우입니다.
		/// </summary>
		public int GetEmptyElement()
		{
			for (int i = 0; i < selectionCount; i++)
			{
				ShowSelection child = elementParent.GetChild(i).GetComponent<ShowSelection>();
				if (child.CurrentItemNumber == -2) return i;
			}

			return -1;
		}

		public void AddItem(int id, Action cancelClickedCallback = null)
		{
			int elementIndex = GetEmptyElement();
			if (elementIndex == -1) return;

			items[elementIndex] = id;
			
			ShowSelection child = elementParent.GetChild(elementIndex).GetComponent<ShowSelection>();
			child.Set(id, cancelClickedCallback, () => { RemoveItem(elementIndex); }, () => { SelectItem(elementIndex, id); });
			
			CanGoNext();
		}

		private void CanGoNext()
		{
			int elementIndex = GetEmptyElement();
			buttonManager.SetButtonActive(elementIndex == -1);
		}

		private void RemoveItem(int index)
		{
			items[index] = -2;
			
			ShowSelection child = elementParent.GetChild(index).GetComponent<ShowSelection>();
			child.Set(-2);
			
			CanGoNext();
		}
		
		/* ====================================== Recipe Stage ====================================== */

		private void SetCloseButton(bool set)
		{
			for (int i = 0; i < selectionCount; i++)
			{
				ShowSelection child = elementParent.GetChild(i).GetComponent<ShowSelection>();
				child.SetCancelActive(false);
			}
		}

		private void SelectItem(int index, int itemID)
		{
			if (gameManager.CurrentChapter != CookingFlow.Recipe) return;
			
			recipeManager.SelectItem(index, itemID);
		}

		public void EnableItem(int index)
		{
			if (gameManager.CurrentChapter != CookingFlow.Recipe) return;
			
			ShowSelection child = elementParent.GetChild(index).GetComponent<ShowSelection>();
			child.SetSelectInteractable(true);
		}

		public void DisableItem(int index)
		{
			if (gameManager.CurrentChapter != CookingFlow.Recipe) return;
			
			ShowSelection child = elementParent.GetChild(index).GetComponent<ShowSelection>();
			child.SetSelectInteractable(false);
		}
		
		/* ====================================== Result Stage ====================================== */
	}
}
