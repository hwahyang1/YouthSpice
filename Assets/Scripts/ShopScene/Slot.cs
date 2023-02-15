using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.ShopScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class Slot : MonoBehaviour
	{
		public ItemProperty item;
		public UnityEngine.UI.Image Image;
		public UnityEngine.UI.Image foodInfoImage;

		public Button button;

		public void SetItem(ItemProperty item, Action<Slot> buttonCallback = null)
		{
			this.item = item;
			if (item == null)
			{
				Image.enabled = false;

				gameObject.name = "Empty";
			}
			else
			{
				Image.enabled = true;
				gameObject.name = item.name;
				Image.sprite = item.sprite;
			}
			
			if (buttonCallback != null)
			{
				button.onClick.AddListener(() => { buttonCallback(this);});
			}
		}
	}
}
