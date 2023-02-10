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
		public Ingredient Ingredient;
		[SerializeField] private bool dealCondition = true; //true 일떄가 구매 false일떄가 판매 
		[SerializeField] private GameObject buyPanel;//구매하는 창 
		[SerializeField] private GameObject sellPanel;//판매하는창

		[SerializeField] private GameObject buyBtnColor;
		[SerializeField] private GameObject sellBtnColor;
		[SerializeField] private GameObject foodInfoPanel;
		
		[SerializeField] private Text foodText;
		[SerializeField] private Text costText;
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

		public void FoodInfoTrue(int buttonNumber)
		{
			foodInfoPanel.SetActive(true);
			switch (buttonNumber)
			{
				case 1:
					foodText.text = "첫번때 버튼";
					break;
				case 2:
					foodText.text = "두번쨰 버튼";
					break;
				case 3:
					foodText.text = "세번째 버튼";
					break;
				case 4:
					foodText.text = "네번째 버튼";
					break;
			}
		}
	}
}
