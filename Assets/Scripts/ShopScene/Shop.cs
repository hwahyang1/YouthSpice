using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using YouthSpice.InGameMenuScene;
using YouthSpice.PreloadScene.Game;
using YouthSpice.PreloadScene.Scene;
using YouthSpice.StoryScene.Extern;

namespace YouthSpice.ShopScene
{
	/// <summary>
	/// 상점 UI를 관리합니다.
	/// </summary>
	public class Shop : MonoBehaviour
	{
		[SerializeField]
		private bool dealCondition = true; //true 일떄가 구매 false일떄가 판매 

		[SerializeField]
		private GameObject buyPanel; //구매하는 창 

		[SerializeField]
		private GameObject sellPanel; //판매하는창

		[SerializeField]
		private GameObject buyBtnColor;

		[SerializeField]
		private GameObject sellBtnColor;

		[SerializeField]
		private Text foodText;

		[SerializeField]
		private Text costText;

		[SerializeField]
		private Text havingMoney;

		private void Start()
		{
			if (GameInfo.Instance.viewedShop) return;
			StorySceneLoadParams.Instance.isTutorialScene = true;
			StorySceneLoadParams.Instance.chapterID = GameProgressManager.Instance.shopTutorial;
			SceneChange.Instance.Add("StoryScene_Tutorial");
			GameInfo.Instance.viewedShop = true;
		}

		private void Update()
		{
			// 다른 창 열렸을 때 입력되는 현상 방지
			if (SceneManager.sceneCount == 1)
			{
				if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F1))
				{
					SceneChange.Instance.Add("InGameMenuScene");
				}
			}

			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F1))
			{
				if (SceneManager.sceneCount == 2)
				{
					//SceneChange.Instance.Unload("InGameMenuScene");
					GameObject.FindObjectOfType<MenuManager>()?.Exit();
				}
			}

			// 이 아래는 상점

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

			havingMoney.text = "" + GameInfo.Instance.money;
		}

		public void BuySelect()
		{
			dealCondition = true;
		}

		public void SellSelect()
		{
			dealCondition = false;
		}
	}
}
