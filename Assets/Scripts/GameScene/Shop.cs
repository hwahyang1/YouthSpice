using System.Collections;
using System.Collections.Generic;

using System;
using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice
{
	/// <summary>
	/// Description
	/// </summary>
	public class Shop : MonoBehaviour
	{
		[SerializeField] private bool dealCondition = true; //true 일떄가 구매 false일떄가 판매 
		[SerializeField] private GameObject buyPanel;//구매하는 창 
		[SerializeField] private GameObject sellPanel;//판매하는창

		[SerializeField] private GameObject buyBtnColor;
		[SerializeField] private GameObject sellBtnColor;
		[SerializeField] private GameObject foodInfoPanel;
		private void Update()
		{
			
			buyPanel.SetActive(dealCondition);

			sellPanel.SetActive(!dealCondition);

			if (dealCondition)
			{
				buyBtnColor.SetActive(true);
				sellBtnColor.SetActive(false);
			}
			else
			{
				buyBtnColor.SetActive(false);
				sellBtnColor.SetActive(true);
			}
		}
		public void BuySelect()
		{
			dealCondition = true;
		}
		public void SellSelect()
		{
			dealCondition = false;
		}

		public void FoodInfoFalse()
		{
			foodInfoPanel.SetActive(false);
		}
	}
}
