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
	/// <summary>
	/// 레시피 아이템의 정보를 정의합니다.
	/// </summary>
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

		/// <summary>
		/// 결과 이미지를 제거합니다.
		/// </summary>
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
				// -2 == 그냥 빈 칸
				if (itemID == -2)
				{
					data.Add(new ItemProperty());
				}
				// -1 == 등록 포기
				else if (itemID == -1)
				{
					data.Add(new ItemProperty() { name = "(선택 안 함)", color = ItemColor.None, set = ItemSet.없음 });
				}
				else
				{
					data.Add(ItemBuffer.Instance.items[itemID]);
				}
			}

			// 고추-고추장 복수정답 -> 고추 선택 시, 내부적으로 아이템 코드를 고추장으로 변환함.
			int index = userRecipe.FindIndex(target => target == 9);
			if (index != -1) userRecipe[index] = 56;

			for (int i = 0; i < (menu == AvailableMenus.우럭매운탕 ? 4 : 3); i++)
			{
				Image child = Instantiate(imagePrefab, Vector3.zero, Quaternion.identity, parent).GetComponent<Image>();
				child.sprite = null;

				RectTransform childRect = child.GetComponent<RectTransform>();
				childRect.offsetMin = Vector2.zero;
				childRect.offsetMax = Vector2.zero;

				images.Add(child);
			}

			switch (menu)
			{
				case AvailableMenus.떡볶이:
					images[0].sprite = tbk_base;
					if (data[0].name != "(선택 안 함)")
						images[1].sprite = tbk_select1.Find(target => target.set == data[0].set).sprite;
					if (data[1].name != "(선택 안 함)")
						images[2].sprite = tbk_select2.Find(target => target.color == data[1].color).sprite;
					break;
				case AvailableMenus.약과:
					images[0].sprite = yk_base;
					if (data[0].name != "(선택 안 함)")
					{
						if (data[0].name == "밀가루")
						{
							images[1].sprite = yk_select1.Find(target => target.set == ItemSet.기본약과).sprite;
						}
						else
						{
							images[1].sprite = yk_select1.Find(target => target.set == data[0].set).sprite;
						}
					}

					if (data[1].name != "(선택 안 함)")
						images[2].sprite = yk_select2.Find(target => target.color == data[1].color).sprite;
					break;
				case AvailableMenus.우럭매운탕:
					images[0].sprite = mt_base;
					if (data[2].name != "(선택 안 함)")
						images[2].sprite = mt_select3.Find(target => target.set == data[2].set).sprite;
					if (data[3].name != "(선택 안 함)")
						images[1].sprite = mt_select2.Find(target => target.color == data[3].color).sprite;
					if (data[5].name != "(선택 안 함)")
						images[3].sprite = mt_select1.Find(target => target.set == data[5].set).sprite;
					break;
			}

			foreach (Image image in images)
			{
				if (image.sprite == null) image.gameObject.SetActive(false);
				else image.color = new Color(1f, 1f, 1f, 1f);
			}

			List<int> originalRecipe = recipeStorage.GetRecipe(menu).stepCorrectItems;

			return userRecipe.SequenceEqual(originalRecipe);
		}
	}
}
