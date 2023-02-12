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
		[SerializeField] private Text havingMoney;
		
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

			havingMoney.text = "" + DataManager.Instance.coin;
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
					foodText.text = "오렌지";
					DataManager.Instance.selectNumber = 1;
					break;
				case 2:
					foodText.text = "라임";
					DataManager.Instance.selectNumber = 1;
					break;
				case 3:
					foodText.text = "레몬";
					DataManager.Instance.selectNumber = 1;
					break;
				case 4:
					foodText.text = "키위";
					DataManager.Instance.selectNumber = 1;
					break;
				case 5:
					foodText.text = "바나나";
					DataManager.Instance.selectNumber = 1;
					break;
				case 6:
					foodText.text = "완두콩";
					DataManager.Instance.selectNumber = 1;
					break;
				case 7:
					foodText.text = "파";
					DataManager.Instance.selectNumber = 1;
					break;
				case 8:
					foodText.text = "양파";
					DataManager.Instance.selectNumber = 1;
					break;
				case 9:
					foodText.text = "마늘";
					DataManager.Instance.selectNumber = 1;
					break;
				case 10:
					foodText.text = "고추";
					DataManager.Instance.selectNumber = 1;
					break;
				case 11:
					foodText.text = "미역";
					DataManager.Instance.selectNumber = 1;
					break;
				case 12:
					foodText.text = "가쓰오부시";
					DataManager.Instance.selectNumber = 1;
					break;
				case 13:
					foodText.text = "꼬막";
					DataManager.Instance.selectNumber = 1;
					break;
				case 14:
					foodText.text = "홍합";
					DataManager.Instance.selectNumber = 1;
					break;
				case 15:
					foodText.text = "새우";
					DataManager.Instance.selectNumber = 1;
					break;
				
				case 16:
					foodText.text = "딸기";
					DataManager.Instance.selectNumber = 2;
					break;
				case 17:
					foodText.text = "사과";
					DataManager.Instance.selectNumber = 2;
					break;
				case 18:
					foodText.text = "코코넛";
					DataManager.Instance.selectNumber = 2;
					break;
				case 19:
					foodText.text = "망고";
					DataManager.Instance.selectNumber = 2;
					break;
				case 20:
					foodText.text = "체리";
					DataManager.Instance.selectNumber = 2;
					break;
				case 21:
					foodText.text = "배";
					DataManager.Instance.selectNumber = 2;
					break;
				case 22:
					foodText.text = "연어";
					DataManager.Instance.selectNumber = 2;
					break;
				case 23:
					foodText.text = "문어";
					DataManager.Instance.selectNumber = 2;
					break;
				case 24:
					foodText.text = "오징어";
					DataManager.Instance.selectNumber = 2;
					break;
				case 25:
					foodText.text = "가리비";
					DataManager.Instance.selectNumber = 2;
					break;
				case 26:
					foodText.text = "우럭";
					DataManager.Instance.selectNumber = 2;
					break;
				case 27:
					foodText.text = "해삼";
					DataManager.Instance.selectNumber = 2;
					break;
				case 28:
					foodText.text = "시금치";
					DataManager.Instance.selectNumber = 2;
					break;
				case 29:
					foodText.text = "애호박";
					DataManager.Instance.selectNumber = 2;
					break;
				case 30:
					foodText.text = "가지";
					DataManager.Instance.selectNumber = 2;
					break;
				case 31:
					foodText.text = "당근";
					DataManager.Instance.selectNumber = 2;
					break;
				case 32:
					foodText.text = "콩나물";
					DataManager.Instance.selectNumber = 2;
					break;
				
				case 33:
					foodText.text = "돼지고기";
					DataManager.Instance.selectNumber = 3;
					break;
				case 34:
					foodText.text = "오리";
					DataManager.Instance.selectNumber = 3;
					break;
				case 35:
					foodText.text = "메론";
					DataManager.Instance.selectNumber = 3;
					break;
				case 36:
					foodText.text = "브로콜리";
					DataManager.Instance.selectNumber = 3;
					break;
				case 37:
					foodText.text = "토마토";
					DataManager.Instance.selectNumber = 3;
					break;
				case 38:
					foodText.text = "미나리";
					DataManager.Instance.selectNumber = 3;
					break;
				case 39:
					foodText.text = "양배추";
					DataManager.Instance.selectNumber = 3;
					break;
				case 40:
					foodText.text = "참치";
					DataManager.Instance.selectNumber = 3;
					break;
				case 41:
					foodText.text = "꽃게";
					DataManager.Instance.selectNumber = 3;
					break;
				case 42:
					foodText.text = "전복";
					DataManager.Instance.selectNumber = 3;
					break;
				case 43:
					foodText.text = "복어";
					DataManager.Instance.selectNumber = 3;
					break;
				case 44:
					foodText.text = "햄";
					DataManager.Instance.selectNumber = 3;
					break;
				case 45:
					foodText.text = "베이컨";
					DataManager.Instance.selectNumber = 3;
					break;
				case 46:
					foodText.text = "밀가루";
					DataManager.Instance.selectNumber = 3;
					break;
				case 47:
					foodText.text = "치즈";
					DataManager.Instance.selectNumber = 3;
					break;
				case 48:
					foodText.text = "떡";
					DataManager.Instance.selectNumber = 3;
					break;
				//오렌지 라임 레몬 키위 바나나 완두콩 파 양파 마늘 고추 미역(8%) 가쓰오부시(8%) 꼬막(8%) 홍합(8%) 새우(8%)
				//딸기 사과 코코넛 망고 체리 배 연어 문어 오징어 가리비 우럭 해삼 시금치 애호박 가지 당근 콩나물 무
				//돼지고기 닭 오리 메론 브로콜리 토마토 미나리 양배추 참치 꽃게 전복 복어 햄 베이컨 밀가루 치즈 떡 

			}
		}

		public void BuyButton()
		{
			if (DataManager.Instance.selectNumber == 1)
			{
				DataManager.Instance.coin -= 1000;
			}
			if (DataManager.Instance.selectNumber == 2)
			{
				DataManager.Instance.coin -= 2000;
			}
		} 
	}
}
