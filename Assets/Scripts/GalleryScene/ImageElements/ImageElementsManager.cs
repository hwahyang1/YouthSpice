using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Files;
using YouthSpice.PreloadScene.Item;

namespace YouthSpice.GalleryScene.ImageElements
{
	/// <summary>
	/// 전체 이미지 항목을 관리합니다.
	/// </summary>
	public class ImageElementsManager : MonoBehaviour
	{
		[Header("Resources")]
		[SerializeField]
		private Sprite[] recipeFoodSprites;

		[Header("GameObjects")]
		[SerializeField]
		private Transform illustsParent;

		[SerializeField]
		private Transform recipeFoodsParent;

		[SerializeField]
		private Transform researchItemsParent;

		private List<ImageElement> illustsChilds;
		private List<ImageElement> recipeFoodsChilds;
		private List<ImageElement> researchItemsChilds;

		private ImageFullScreen imageFullScreen;

		private void Start()
		{
			imageFullScreen = GetComponent<ImageFullScreen>();
			RefreshAllChildLists();
			InitAllChildLists();
		}

		/// <summary>
		/// 이미지의 목록을 갱신합니다.
		/// </summary>
		/// <remarks>
		/// 해당 Method는 GameObject를 Destroy하지 않고 목록만 갱신합니다.
		/// </remarks>
		private void RefreshAllChildLists()
		{
			illustsChilds = new List<ImageElement>();
			for (int i = 1; i < illustsParent.childCount; i++)
			{
				Transform parent = illustsParent.GetChild(i);
				for (int j = 0; j < parent.childCount; j++)
				{
					ImageElement child = parent.GetChild(j).GetComponent<ImageElement>();
					illustsChilds.Add(child);
				}
			}

			recipeFoodsChilds = new List<ImageElement>();
			for (int i = 1; i < recipeFoodsParent.childCount; i++)
			{
				Transform parent = recipeFoodsParent.GetChild(i);
				for (int j = 0; j < parent.childCount; j++)
				{
					ImageElement child = parent.GetChild(j).GetComponent<ImageElement>();
					recipeFoodsChilds.Add(child);
				}
			}

			researchItemsChilds = new List<ImageElement>();
			for (int i = 1; i < researchItemsParent.childCount; i++)
			{
				Transform parent = researchItemsParent.GetChild(i);
				for (int j = 0; j < parent.childCount; j++)
				{
					ImageElement child = parent.GetChild(j).GetComponent<ImageElement>();
					researchItemsChilds.Add(child);
				}
			}
		}

		/// <summary>
		/// 이미지를 재설정 합니다.
		/// </summary>
		/// <remarks>
		/// 해당 Method는 GameObject를 Destroy하지 않고 항목만 갱신합니다.
		/// </remarks>
		private void InitAllChildLists()
		{
			DefineUnlockedCGs unlocked = UnlockedCGsManager.Instance.GetAllData();

			int imagesCount = SourceFileManager.Instance.AvailableBackgroundImages.Count;
			int j = 0;
			for (int i = imagesCount - 4; i < imagesCount; i++)
			{
				bool isUnlocked = unlocked.illusts.Exists(target => target == i);
				Sprite sprite =
					SourceFileManager.Instance.AvailableBackgroundImages[i];
				illustsChilds[j].Init(sprite, isUnlocked, imageFullScreen.Show);
				j++;
			}

			for (int i = 0; i < recipeFoodsChilds.Count; i++)
			{
				bool isUnlocked = unlocked.recipeFoods.Exists(target => target == i);
				recipeFoodsChilds[i].Init(recipeFoodSprites[i], isUnlocked, imageFullScreen.Show);
			}

			List<ItemProperty> foods = ItemBuffer.Instance.items;
			for (int i = 0; i < researchItemsChilds.Count; i++)
			{
				bool isUnlocked =
					unlocked.researchItems.Exists(target => target == ItemBuffer.Instance.GetIndex(foods[i].name));
				researchItemsChilds[i].Init(foods[i].sprite, isUnlocked, imageFullScreen.Show);
			}
		}
	}
}
