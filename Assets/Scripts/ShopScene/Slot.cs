using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.ShopScene
{
	/// <summary>
	/// 상점의 각 항목을 정의합니다.
	/// </summary>
	public class Slot : MonoBehaviour
	{
		[SerializeField]
		private ItemProperty item;

		[SerializeField]
		private Image itemImage;

		[SerializeField]
		private Text itemName;

		[SerializeField]
		private Text itemPrice;

		[SerializeField]
		private Button button;

		/// <summary>
		/// 아이템의 정보를 지정합니다.
		/// </summary>
		/// <param name="item">아이템의 정보를 지정합니다.</param>
		/// <param name="isSellArea">판매 여부를 지정합니다.</param>
		/// <param name="buttonCallback">버튼 클릭시 callback을 지정합니다. 없을 경우, null을 지정합니다.</param>
		public void SetItem(ItemProperty item, bool isSellArea = false, System.Action<Slot> buttonCallback = null)
		{
			this.item = item;
			if (item == null)
			{
				itemImage.enabled = false;
				itemName.text = "";
				itemPrice.text = "0";

				gameObject.name = "Empty";
			}
			else
			{
				itemImage.enabled = true;
				itemImage.sprite = item.sprite;
				itemName.text = item.name;
				itemPrice.text = (isSellArea ? Mathf.Round(item.sellPrice * 0.5f) : item.sellPrice).ToString();

				gameObject.name = item.name;
			}

			if (buttonCallback != null)
			{
				button.onClick.AddListener(() => { buttonCallback(this); });
			}
		}
	}
}
