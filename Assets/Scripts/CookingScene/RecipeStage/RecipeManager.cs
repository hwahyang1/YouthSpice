using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

using YouthSpice.CookingScene.RecipeStage.UI;
using YouthSpice.CookingScene.Extern;
using YouthSpice.CookingScene.WholeStage;
using YouthSpice.CookingScene.WholeStage.UI;
using YouthSpice.PreloadScene.Item;

namespace YouthSpice.CookingScene.RecipeStage
{
	/// <summary>
	/// 조리 순서를 관리합니다.
	/// </summary>
	public class RecipeManager : MonoBehaviour
	{
		public bool IsEnded => currentIndex == currentTexts.Count - 1;

		[Header("Statuses")]
		[SerializeField, ReadOnly]
		private List<int> selectedItems;

		public List<int> SelectedItems => selectedItems;

		[SerializeField, ReadOnly]
		private int currentIndex;

		[SerializeField, ReadOnly]
		private List<int> currentItems;

		[SerializeField, ReadOnly]
		private List<string> currentTexts;

		[Header("Classes")]
		[SerializeField]
		private RecipeStorage recipeStorage;

		[SerializeField]
		private SelectionManager selectionManager;

		[SerializeField]
		private ButtonManager buttonManager;

		private ItemSelectManager itemSelectManager;

		private int tempIndex;

		private void Start()
		{
			itemSelectManager = GetComponent<ItemSelectManager>();

			DefineRecipe current = recipeStorage.GetRecipe(CookingLoadParams.Instance.menu);
			currentItems = current.stepCorrectItems;
			currentTexts = current.stepDescriptions;

			tempIndex = -2;
			Set();
		}

		public void SelectItem(int index, int itemID)
		{
			if (currentItems[currentIndex] == -2) return;

			if (tempIndex != -2) selectionManager.EnableItem(tempIndex);
			tempIndex = index;
			selectionManager.DisableItem(tempIndex);

			ItemProperty data;
			// -1 == 선택 포기
			if (itemID == -1)
			{
				data = new ItemProperty() { name = "(선택 안함)" };
			}
			else
			{
				data = ItemBuffer.Instance.items[itemID];
			}

			string descriptionText = currentTexts[currentIndex].Replace("???", data.name);

			buttonManager.SetButtonActive(true);
			itemSelectManager.ChangeDescription(descriptionText);
			itemSelectManager.ChangeFoodImage(itemID);
		}

		public void GoNext()
		{
			currentIndex++;

			tempIndex = -2;

			Set();
		}

		private void Set()
		{
			if (currentIndex == currentTexts.Count - 1) buttonManager.SetButtonText(2);
			buttonManager.SetButtonActive(false);
			itemSelectManager.ChangeDescription(currentTexts[currentIndex]);
			itemSelectManager.ChangeFoodImage(null);

			// -2 == 선택 없음
			if (currentItems[currentIndex] == -2)
			{
				selectedItems.Add(-2);
				buttonManager.SetButtonActive(true);
			}
		}
	}
}
