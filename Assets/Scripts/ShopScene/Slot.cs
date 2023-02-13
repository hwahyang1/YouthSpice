using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.ShopScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class Slot : MonoBehaviour
	{
		public ItemProperty item;
		public UnityEngine.UI.Image Image;

		public void SetItem(ItemProperty item)
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
		}
	}
}
