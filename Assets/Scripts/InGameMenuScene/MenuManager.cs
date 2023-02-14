using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using YouthSpice.CookingScene.Extern;
using YouthSpice.PreloadScene.Scene;
using YouthSpice.SaveLoadSlotScene.Extern;
using YouthSpice.StoryScene.Extern;

namespace YouthSpice.InGameMenuScene
{
	/// <summary>
	/// 메뉴 버튼 이벤트를 관리합니다.
	/// </summary>
	public class MenuManager : MonoBehaviour
	{
		public void OnSaveButtonClicked()
		{
			SaveLoadSlotLoadParams.Instance.mode = SaveLoadSlotMode.Save;
			SceneChange.Instance.Add("SaveLoadSlotScene");
		}
		
		public void OnLoadButtonClicked()
		{
			SaveLoadSlotLoadParams.Instance.mode = SaveLoadSlotMode.Load;
			SceneChange.Instance.Add("SaveLoadSlotScene");
		}

		public void OnConfigButtonClicked()
		{
			SceneChange.Instance.Add("ConfigScene");
		}

		public void OnExitButtonClicked()
		{
			GameInfo.Instance.Exit();
			CookingLoadParams.Instance.Exit();
			StorySceneLoadParams.Instance.Exit();
			SceneChange.Instance.ChangeScene("MenuScene");
		}
	}
}
