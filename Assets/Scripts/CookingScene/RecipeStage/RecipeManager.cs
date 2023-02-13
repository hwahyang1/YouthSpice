using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.CookingScene.RecipeStage.UI;

using NaughtyAttributes;

namespace YouthSpice.CookingScene.RecipeStage
{
	/// <summary>
	/// 조리 순서를 관리합니다.
	/// </summary>
	public class RecipeManager : MonoBehaviour
	{
		[SerializeField, ReadOnly]
		private List<int> selectedItems;

		private ItemSelectManager itemSelectManager;

		private void Start()
		{
			itemSelectManager = GetComponent<ItemSelectManager>();
		}

		public void SelectItem(int itemID)
		{
			
		}
	}
}
