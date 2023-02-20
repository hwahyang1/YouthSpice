using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using YouthSpice.PreloadScene.Audio;
using YouthSpice.PreloadScene.Scene;
using YouthSpice.SaveLoadSlotScene.Extern;

namespace YouthSpice.SaveLoadSlotScene.UI
{
	/// <summary>
	/// 전반적인 UI의 실행을 관리합니다.
	/// </summary>
	public class UIManager : MonoBehaviour
	{
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private AudioClip backClip;

		[SerializeField]
		[Tooltip("SaveLoadSlotMode 기준으로 이미지를 지정합니다.")]
		private Sprite[] titleTextImages;

		[SerializeField]
		private Image titleTextImageArea;

		private void Start()
		{
			animator.SetTrigger("On");
		}

		public void Exit()
		{
			animator.SetTrigger("Off");

			SaveLoadSlotLoadParams.Instance.Exit();
			AudioManager.Instance.PlayEffectAudio(backClip);

			StartCoroutine(DelayedExitCoroutine());
		}

		private IEnumerator DelayedExitCoroutine()
		{
			yield return new WaitForSeconds(1.05f);

			if (SceneManager.sceneCount != 1) SceneChange.Instance.Unload("SaveLoadSlotScene");
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
