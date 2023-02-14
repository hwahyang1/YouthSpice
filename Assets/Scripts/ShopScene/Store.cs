using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
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

		[SerializeField] private GameObject sellElementPrefab;
		[SerializeField] private Transform sellSlotRoot;
		private List<Slot> sellSlots;

		[SerializeField] private GameObject buyFoodInfo;
		[SerializeField] private GameObject sellFoodInfo;

		[SerializeField] private UnityEngine.UI.Button buyBnt;
		[SerializeField] private UnityEngine.UI.Button sellBnt;

		[SerializeField] private Image buyInfoImage;
		[SerializeField] private Text buyInfoNameText;
		[SerializeField] private Text buyInfoPriceText;
		
		[SerializeField] private Image sellInfoImage;
		[SerializeField] private Text sellInfoNameText;
		[SerializeField] private Text sellInfoPriceText;

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
				buySlots.Add(slot);
			}

			RefreshSellSlots();
		}

		private void Update()
		{
			
		}

		public void RefreshSellSlots()
		{
			/*
			sellSlots = new List<Slot>();

			for (int i = 0; i < sellSlotRoot.childCount; i++)
			{
				var slot = sellSlotRoot.GetChild(i).GetComponent<Slot>();

				if (i < ItemBuffer.Instance.items.Count)
				{
					slot.SetItem(ItemBuffer.Instance.items[i]);
				}
				sellSlots.Add(slot);
				
				List<int> inventory = GameInfo.Instance.inventory;
				for (int i = 0; i < inventory.Count; i++)
				{
					int currentItemNumber = inventory[i];
					ItemProperty currentItem = ItemBuffer.Instance.items[currentItemNumber];
				}
			}*/

			for (int i = 0; i < sellSlotRoot.childCount; i++)
			{
				Destroy(sellSlotRoot.GetChild(i).gameObject);
			}
			
			List<int> inventory = GameInfo.Instance.inventory;
			sellSlots = new List<Slot>();
			
			for (int i = 0; i < inventory.Count; i++)
			{
				int currentItemNumber = inventory[i];
				ItemProperty currentItem = ItemBuffer.Instance.items[currentItemNumber];
				Slot child = Instantiate(sellElementPrefab, sellSlotRoot).GetComponent<Slot>();
				child.SetItem(currentItem, OnClickSellSlot);
				sellSlots.Add(child);
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
		// 구매 확인창 띄우는 부분
		public void OnClickBuySlot(Slot slot)
		{
			BuyFoodInfo();
			buyBnt.onClick = new Button.ButtonClickedEvent();
			buyBnt.onClick.AddListener(() => { BuyItem(slot);});
			int index = ItemBuffer.Instance.GetIndex(slot.name);
			ItemProperty item = ItemBuffer.Instance.items[index];
			buyInfoImage.sprite = item.sprite;
			buyInfoNameText.text = item.name;
			buyInfoPriceText.text = "구매 가격:" + item.sellPrice;
		}
		// 구매 진행
		public void BuyItem(Slot slot)
		{
			int index = ItemBuffer.Instance.GetIndex(slot.name);
			GameInfo.Instance.money -= ItemBuffer.Instance.items[index].sellPrice;
			GameInfo.Instance.inventory.Add(index);
			BuyFoodInfoFalse();

			RefreshSellSlots();
		}
		public void SellFoodInfo()
		{ 
			sellFoodInfo.SetActive(true);
		}

		public void SellFoodInfoFalse()
		{
			sellFoodInfo.SetActive(false);
		}
		public void OnClickSellSlot(Slot slot)
		{
			SellFoodInfo();
			sellBnt.onClick = new Button.ButtonClickedEvent();
			sellBnt.onClick.AddListener(() => { SellItem(slot);});
			int index = ItemBuffer.Instance.GetIndex(slot.name);
			ItemProperty item = ItemBuffer.Instance.items[index];
			sellInfoImage.sprite = item.sprite;
			sellInfoNameText.text = item.name;
			sellInfoPriceText.text = "판매 가격:" + Mathf.RoundToInt(item.sellPrice) * 0.5f;
		}
		public void SellItem(Slot slot)
		{
			Debug.Log("팔림");
			int index = ItemBuffer.Instance.GetIndex(slot.name);
			GameInfo.Instance.money += Mathf.RoundToInt(ItemBuffer.Instance.items[index].sellPrice * 0.5f);
			GameInfo.Instance.inventory.Remove(index);
			SellFoodInfoFalse();

			RefreshSellSlots();
		}
	}
}


