using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

using YouthSpice.CookingScene.Extern;
using YouthSpice.CookingScene.RecipeStage;
using YouthSpice.PreloadScene.Item;

namespace YouthSpice.CookingScene.ResultStage.UI
{
	[System.Serializable]
	public class DefineFoodSet
	{
		public ItemSet set;
		public ItemColor color = ItemColor.None;
		public Sprite sprite;
	}

	/// <summary>
	/// 음식을 생성합니다.
	/// </summary>
	public class GenerateFood : MonoBehaviour
	{
		[Header("Data - 떡볶이")]
		[SerializeField]
		private Sprite tbk_base;
		[SerializeField]
		private List<DefineFoodSet> tbk_select1;
		[SerializeField]
		private List<DefineFoodSet> tbk_select2;

		[Header("Data - 약과")]
		[SerializeField]
		private Sprite yk_base;
		[SerializeField]
		private List<DefineFoodSet> yk_select1;
		[SerializeField]
		private List<DefineFoodSet> yk_select2;

		[Header("Data - 우럭매운탕")]
		[SerializeField]
		private Sprite mt_base;
		[SerializeField]
		private List<DefineFoodSet> mt_select1;
		[SerializeField]
		private List<DefineFoodSet> mt_select2;
		[SerializeField]
		private List<DefineFoodSet> mt_select3;

		[Header("Prefabs")]
		[SerializeField]
		private GameObject imagePrefab;

		[Header("GameObjects")]
		[SerializeField]
		private Transform parent;

		[Header("Classes")]
		[SerializeField]
		private RecipeManager recipeManager;
		[SerializeField]
		private RecipeStorage recipeStorage;

		private void Start()
		{
			RemoveAll();
		}

		public void RemoveAll()
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				Destroy(parent.GetChild(i).gameObject);
			}
		}

		/// <summary>
		/// 요리 생성을 시도합니다.
		/// </summary>
		/// <returns>완성된 요리의 성공 여부를 반환합니다.</returns>
		public bool TryGenerate()
		{
			AvailableMenus menu = CookingLoadParams.Instance.menu;
			
			List<Image> images = new List<Image>();
			List<int> userRecipe = recipeManager.SelectedItems;
			List<ItemProperty> data = new List<ItemProperty>();

			foreach (int itemID in userRecipe)
			{
				data.Add(ItemBuffer.Instance.items[itemID]);
			}

			for (int i = 0; i < (menu == AvailableMenus.우럭매운탕 ? 4 : 3); i++)
			{
				Image child = Instantiate(imagePrefab, Vector3.zero, Quaternion.identity, parent).GetComponent<Image>();
				images.Add(child);
			}

			switch (menu)
			{
				case AvailableMenus.떡볶이:
					images[0].sprite = tbk_base;
					images[1].sprite = tbk_select1.Find(target => target.set == data[0].set).sprite;
					images[2].sprite = tbk_select2.Find(target => target.color == data[1].color).sprite;
					break;
				case AvailableMenus.약과:
					images[0].sprite = yk_base;
					images[1].sprite = yk_select1.Find(target => target.set == data[0].set).sprite;
					images[2].sprite = yk_select2.Find(target => target.color == data[1].color).sprite;
					break;
				case AvailableMenus.우럭매운탕:
					images[0].sprite = mt_base;
					images[1].sprite = mt_select1.Find(target => target.set == data[0].set).sprite;
					images[2].sprite = mt_select2.Find(target => target.color == data[1].color).sprite;
					images[3].sprite = mt_select3.Find(target => target.set == data[2].set).sprite;
					break;
			}

			List<int> originalRecipe = recipeStorage.GetRecipe(menu).stepCorrectItems;

			return userRecipe.SequenceEqual(originalRecipe);
		}
	}
}
