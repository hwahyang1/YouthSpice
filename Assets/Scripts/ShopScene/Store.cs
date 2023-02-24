using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;
using UnityEngine.UI;

using YouthSpice.PreloadScene.Alert;
using YouthSpice.PreloadScene.Audio;
using YouthSpice.PreloadScene.Files;
using YouthSpice.PreloadScene.Game;
using YouthSpice.PreloadScene.Item;

namespace YouthSpice.ShopScene
{
	/// <summary>
	/// 상점을 관리합니다.
	/// </summary>
	public class Store : MonoBehaviour
	{
		[SerializeField]
		private AudioClip slotClickClip;

		[SerializeField]
		private AudioClip buttonClickClip;

		[SerializeField]
		private Transform buySlotRoot;

		[SerializeField]
		private GameObject sellElementPrefab;

		[SerializeField]
		private Transform sellSlotRoot;

		[SerializeField]
		private GameObject buyFoodInfo;

		[SerializeField]
		private GameObject sellFoodInfo;

		[SerializeField]
		private Button buyBnt;

		[SerializeField]
		private Button sellBnt;

		[SerializeField]
		private Image buyInfoImage;

		[SerializeField]
		private Text buyInfoNameText;

		[SerializeField]
		private Text buyInfoPriceText;

		[SerializeField]
		private Image sellInfoImage;

		[SerializeField]
		private Text sellInfoNameText;

		[SerializeField]
		private Text sellInfoPriceText;

		private void Start()
		{
			for (int i = 0; i < buySlotRoot.childCount; i++)
			{
				Slot slot = buySlotRoot.GetChild(i).GetComponent<Slot>();

				if (i < ItemBuffer.Instance.items.Count)
				{
					slot.SetItem(ItemBuffer.Instance.items[i], false, OnClickBuySlot);
				}
			}

			RefreshSellSlots();
		}

		/// <summary>
		/// 판매 창의 슬롯을 다시 생성합니다.
		/// </summary>
		private void RefreshSellSlots()
		{
			for (int i = 0; i < sellSlotRoot.childCount; i++)
			{
				Destroy(sellSlotRoot.GetChild(i).gameObject);
			}

			List<int> inventory = GameInfo.Instance.inventory;

			foreach (int currentItemNumber in inventory)
			{
				ItemProperty currentItem = ItemBuffer.Instance.items[currentItemNumber];
				Slot child = Instantiate(sellElementPrefab, sellSlotRoot).GetComponent<Slot>();
				child.SetItem(currentItem, true, OnClickSellSlot);
			}
		}

		/// <summary>
		/// 구매창의 활성화 여부를 지정합니다.
		/// </summary>
		public void BuyFoodInfoSetActive(bool active)
		{
			buyFoodInfo.SetActive(active);
		}

		/// <summary>
		/// 구매 확인창을 띄웁니다.
		/// </summary>
		/// <param name="slot">구매할 아이템의 슬롯 정보를 지정합니다.</param>
		public void OnClickBuySlot(Slot slot)
		{
			int index = ItemBuffer.Instance.GetIndex(slot.name);
			if (GameInfo.Instance.money < ItemBuffer.Instance.items[index].sellPrice)
			{
				AlertManager.Instance.Show(AlertType.Single, "알림",
					$"돈이 부족합니다.\n선택한 아이템의 가격은 {ItemBuffer.Instance.items[index].sellPrice.ToString()}G 입니다.",
					new Dictionary<string, Action>()
						{ { "확인", () => { AudioManager.Instance.PlayEffectAudio(buttonClickClip); } } });
				return;
			}

			AudioManager.Instance.PlayEffectAudio(slotClickClip);

			BuyFoodInfoSetActive(true);
			buyBnt.onClick = new Button.ButtonClickedEvent();
			buyBnt.onClick.AddListener(() => { BuyItem(slot); });
			ItemProperty item = ItemBuffer.Instance.items[index];
			buyInfoImage.sprite = item.sprite;
			buyInfoNameText.text = item.name;
			buyInfoPriceText.text = item.sellPrice + "G";
		}

		/// <summary>
		/// 구매를 진행합니다.
		/// </summary>
		/// <param name="slot">구매할 아이템의 슬롯 정보를 지정합니다.</param>
		private void BuyItem(Slot slot)
		{
			int index = ItemBuffer.Instance.GetIndex(slot.name);
			if (GameInfo.Instance.money < ItemBuffer.Instance.items[index].sellPrice)
			{
				AlertManager.Instance.Show(AlertType.Single, "알림",
					$"돈이 부족합니다.\n선택한 아이템의 가격은 {ItemBuffer.Instance.items[index].sellPrice.ToString()}G 입니다.",
					new Dictionary<string, Action>()
						{ { "확인", () => { AudioManager.Instance.PlayEffectAudio(buttonClickClip); } } });
				return;
			}

			AudioManager.Instance.PlayEffectAudio(buttonClickClip);

			GameInfo.Instance.money -= ItemBuffer.Instance.items[index].sellPrice;
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			BuyFoodInfoSetActive(false);

			RefreshSellSlots();
		}

		/// <summary>
		/// 판매창의 활성화 여부를 지정합니다.
		/// </summary>
		public void SellFoodInfoSetActive(bool active)
		{
			sellFoodInfo.SetActive(active);
		}

		/// <summary>
		/// 판매 확인창을 띄웁니다.
		/// </summary>
		/// <param name="slot">판매할 아이템의 슬롯 정보를 지정합니다.</param>
		private void OnClickSellSlot(Slot slot)
		{
			AudioManager.Instance.PlayEffectAudio(slotClickClip);

			SellFoodInfoSetActive(true);
			sellBnt.onClick = new Button.ButtonClickedEvent();
			sellBnt.onClick.AddListener(() => { SellItem(slot); });
			int index = ItemBuffer.Instance.GetIndex(slot.name);
			ItemProperty item = ItemBuffer.Instance.items[index];
			sellInfoImage.sprite = item.sprite;
			sellInfoNameText.text = item.name;
			sellInfoPriceText.text = Mathf.RoundToInt(item.sellPrice) * 0.5f + "G";
		}

		/// <summary>
		/// 판매를 진행합니다.
		/// </summary>
		/// <param name="slot">판매할 아이템의 슬롯 정보를 지정합니다.</param>
		public void SellItem(Slot slot)
		{
			AudioManager.Instance.PlayEffectAudio(buttonClickClip);

			int index = ItemBuffer.Instance.GetIndex(slot.name);
			GameInfo.Instance.money += Mathf.RoundToInt(ItemBuffer.Instance.items[index].sellPrice * 0.5f);
			GameInfo.Instance.inventory.Remove(index);
			SellFoodInfoSetActive(false);

			RefreshSellSlots();
		}

		/// <summary>
		/// 상점을 나가고 다음 챕터로 넘어갑니다.
		/// </summary>
		public void Exit()
		{
			AudioManager.Instance.PlayEffectAudio(buttonClickClip);
			GameProgressManager.Instance.CountUp();
			GameProgressManager.Instance.RunThisChapter();
		}
	}
}
