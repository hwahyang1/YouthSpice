using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using YouthSpice.PreloadScene.Item;

namespace YouthSpice.CookingScene.RecipeStage.UI
{
	/// <summary>
	/// 항목의 선택 및 표출을 관리합니다.
	/// </summary>
	public class ItemSelectManager : MonoBehaviour
	{
		[Header("GameObjects")]
		[SerializeField]
		private Text description;

		[SerializeField]
		private Image foodImage;

		[SerializeField]
		private Image foodImageUnknown;

		/// <summary>
		/// 설명을 변경합니다.
		/// </summary>
		public void ChangeDescription(string text)
		{
			description.text = text.Replace(@"\n", "\n");
		}

		/// <summary>
		/// 식재료 아이템 이미지를 변경합니다.
		/// </summary>
		/// <param name="itemID">변경할 ID를 지정합니다.</param>
		public void ChangeFoodImage(int itemID)
		{
			ChangeFoodImage(itemID == -1 ? null : ItemBuffer.Instance.items[itemID].sprite);
		}

		/// <summary>
		/// 식재료 아이템 이미지를 변경합니다.
		/// </summary>
		/// <param name="image">변경할 이미지를 지정합니다.</param>
		public void ChangeFoodImage(Sprite image = null)
		{
			if (image == null)
			{
				foodImage.sprite = null;
				foodImage.gameObject.SetActive(false);
				foodImageUnknown.gameObject.SetActive(true);
			}
			else
			{
				foodImage.sprite = image;
				foodImage.gameObject.SetActive(true);
				foodImageUnknown.gameObject.SetActive(false);
			}
		}
	}
}
