using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

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
		[SerializeField]
		private Animator animator;

		private void Start()
		{
			animator.SetTrigger("On");
		}
		
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
			Exit();
			SceneChange.Instance.ChangeScene("MenuScene", true, true);
		}

		public void Exit()
		{
			animator.SetTrigger("Off");

			StartCoroutine(DelayedExitCoroutine());
		}

		private IEnumerator DelayedExitCoroutine()
		{
			yield return new WaitForSeconds(1.1f);
			
			if (SceneManager.sceneCount != 1) SceneChange.Instance.Unload("InGameMenuScene");
			else
			{
				#if UNITY_EDITOR
				EditorApplication.ExecuteMenuItem("Edit/Play");
				#else
				Application.Quit();
				#endif
			}
		}
	}
}
