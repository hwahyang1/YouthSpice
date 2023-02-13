using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Scene;

namespace YouthSpice.InGameMenuScene
{
	/// <summary>
	/// 메뉴 버튼 이벤트를 관리합니다.
	/// </summary>
	public class MenuManager : MonoBehaviour
	{
		public void OnSaveButtonClicked()
		{
			// TODO
		}
		
		public void OnLoadButtonClicked()
		{
			// TODO
		}

		public void OnConfigButtonClicked()
		{
			SceneChange.Instance.Add("ConfigScene");
		}

		public void OnExitButtonClicked()
		{
			// TODO
		}
	}
}
