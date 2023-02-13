using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YouthSpice.PreloadScene.Item;

namespace YouthSpice.ShopScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class Store : MonoBehaviour
	{
		[SerializeField] private Transform buySlotRoot;
		private List<Slot> buySlots;
		
		[SerializeField] private Transform sellSlotRoot;
		private List<Slot> sellSlots;

		[SerializeField] private GameObject buyFoodInfo;
		[SerializeField] private GameObject sellFoodInfo;

		[SerializeField] private UnityEngine.UI.Button bnt;

		private void Start()
		{
			buySlots = new List<Slot>();

			for (int i = 0; i < buySlotRoot.childCount; i++)
			{
				var slot = buySlotRoot.GetChild(i).GetComponent<Slot>();

				if (i < ItemBuffer.Instance.items.Count)
				{
					slot.SetItem(ItemBuffer.Instance.items[i]);
				}
				else
				{
					slot.GetComponent<UnityEngine.UI.Button>().interactable = false;
				}
				buySlots.Add(slot);
			}

			sellSlots = new List<Slot>();

			for (int i = 0; i < sellSlotRoot.childCount; i++)
			{
				var slot = sellSlotRoot.GetChild(i).GetComponent<Slot>();

				if (i < ItemBuffer.Instance.items.Count)
				{
					slot.SetItem(ItemBuffer.Instance.items[i]);
				}
				else
				{
					slot.GetComponent<UnityEngine.UI.Button>().interactable = false;
				}
				sellSlots.Add(slot);
			}
		}

		public void BuyFoodInfo()
		{ 
			buyFoodInfo.SetActive(true);
		}
		public void BuyFoodInfoFalse()
		{
			buyFoodInfo.SetActive(false);
		}
		public void SellFoodInfo()
		{ 
			sellFoodInfo.SetActive(true);
		}
		public void SellFoodInfoFalse()
		{ 
			sellFoodInfo.SetActive(true);
		}
		public void OnClickBuySlot(Slot slot)
		{
			BuyFoodInfo();

			bnt.onClick = new Button.ButtonClickedEvent();
			bnt.onClick.AddListener(() => { BuyItem(slot);});
		}

		public void BuyItem(Slot slot)
		{
			int index = ItemBuffer.Instance.GetIndex(slot.name);
			GameInfo.Instance.money -= ItemBuffer.Instance.items[index].sellPrice;
			BuyFoodInfoFalse();
		}
		
		public void OnClickSellSlot(Slot slot)
		{
			int index = ItemBuffer.Instance.GetIndex(slot.name);
			GameInfo.Instance.money += Mathf.RoundToInt(ItemBuffer.Instance.items[index].sellPrice * 0.5f);
			BuyFoodInfoFalse();
		}
	}
}
