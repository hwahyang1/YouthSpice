using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Item;

namespace YouthSpice.ShopScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class Store : MonoBehaviour
	{
		public ItemBuffer ItemBuffer;
		[SerializeField] private Transform buySlotRoot;
		private List<Slot> buySlots;
		
		[SerializeField] private Transform sellSlotRoot;
		private List<Slot> sellSlots;

		private void Start()
		{
			buySlots = new List<Slot>();

			for (int i = 0; i < buySlotRoot.childCount; i++)
			{
				var slot = buySlotRoot.GetChild(i).GetComponent<Slot>();

				if (i < ItemBuffer.items.Count)
				{
					slot.SetItem(ItemBuffer.items[i]);
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

				if (i < ItemBuffer.items.Count)
				{
					slot.SetItem(ItemBuffer.items[i]);
				}
				else
				{
					slot.GetComponent<UnityEngine.UI.Button>().interactable = false;
				}
				sellSlots.Add(slot);
			}
		}

		public void OnClickSlot(Slot slot)
		{
			Debug.Log(slot.name);
			//GameInfo.Instance.money -= 
		}
	}
}
