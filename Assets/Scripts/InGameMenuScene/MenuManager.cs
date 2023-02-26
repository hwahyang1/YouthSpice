using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

using YouthSpice.CookingScene.Extern;
using YouthSpice.PreloadScene.Alert;
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

		/// <summary>
		/// 저장 버튼이 클릭되었을 때의 이벤트를 처리합니다.
		/// </summary>
		public void OnSaveButtonClicked()
		{
			SaveLoadSlotLoadParams.Instance.mode = SaveLoadSlotMode.Save;
			SceneChange.Instance.Add("SaveLoadSlotScene");
		}

		/// <summary>
		/// 불러오기 버튼이 클릭되었을 때의 이벤트를 처리합니다.
		/// </summary>
		public void OnLoadButtonClicked()
		{
			SaveLoadSlotLoadParams.Instance.mode = SaveLoadSlotMode.Load;
			SceneChange.Instance.Add("SaveLoadSlotScene");
		}

		/// <summary>
		/// 설정 버튼이 클릭되었을 때의 이벤트를 처리합니다.
		/// </summary>
		public void OnConfigButtonClicked()
		{
			SceneChange.Instance.Add("ConfigScene");
		}

		/// <summary>
		/// 창을 닫습니다.
		/// </summary>
		public void OnExitButtonClicked()
		{
			AlertManager.Instance.Show(AlertType.Double, "경고", "메인 화면으로 돌아갈 경우 저장되지 않은 진행 상황이 초기화됩니다.\n계속하시겠습니까?",
				new Dictionary<string, Action>()
				{
					{
						"예",
						() =>
						{
							GameInfo.Instance.Exit();
							CookingLoadParams.Instance.Exit();
							StorySceneLoadParams.Instance.Exit();
							Exit();
							SceneChange.Instance.ChangeScene("MenuScene");
						}
					},
					{ "아니오", null }
				});
		}

		/// <summary>
		/// 저장 버튼이 클릭되었을 때의 이벤트를 처리합니다.
		/// </summary>
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
