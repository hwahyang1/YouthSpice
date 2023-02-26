using System.Collections;
using System.Collections.Generic;
using Serializable = System.SerializableAttribute;

using UnityEngine;

namespace YouthSpice.CookingScene
{
	/// <summary>
	/// 스테이지 순서를 정의합니다.
	/// </summary>
	public enum CookingFlow
	{
		Selection,
		Recipe,
		Result
	}

	/// <summary>
	/// 조리 가능한 메뉴들을 정의합니다.
	/// </summary>
	public enum AvailableMenus
	{
		떡볶이,
		약과,
		우럭매운탕
	}

	/// <summary>
	/// 레시피를 정의합니다.
	/// </summary>
	[Serializable]
	public class DefineRecipe
	{
		public AvailableMenus menu;

		public List<string> stepDescriptions;
		public List<int> stepCorrectItems;

		public DefineRecipe(AvailableMenus menu, List<string> stepDescriptions, List<int> stepCorrectItems)
		{
			this.menu = menu;
			this.stepDescriptions = stepDescriptions;
			this.stepCorrectItems = stepCorrectItems;
		}
	}

	/// <summary>
	/// 레시피를 저장합니다.
	/// </summary>
	public class RecipeStorage : MonoBehaviour
	{
		[SerializeField]
		private List<DefineRecipe> recipes;

		public DefineRecipe GetRecipe(AvailableMenus menu)
		{
			return recipes.Find(target => target.menu == menu);
		}
	}
}
