using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.ShopScene
{
	/// <summary>
	/// 각 항목을 정의합니다.
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

		public void SetItem(ItemProperty item, bool isSellArea = false, System.Action<Slot> buttonCallback = null)
		{
			this.item = item;
			if (item == null)
			{
				itemImage.enabled = false;
				itemName.text = "";
				itemPrice.text = "0";

				#if UNITY_EDITOR
				gameObject.name = "Empty";
				#endif
			}
			else
			{
				itemImage.enabled = true;
				itemImage.sprite = item.sprite;
				itemName.text = item.name;
				itemPrice.text = (isSellArea ? Mathf.Round(item.sellPrice * 0.5f) : item.sellPrice).ToString();
				
				#if UNITY_EDITOR
				gameObject.name = item.name;
				#endif
			}
			
			if (buttonCallback != null)
			{
				button.onClick.AddListener(() => { buttonCallback(this);});
			}
		}
	}
}
