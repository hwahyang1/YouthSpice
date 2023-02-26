using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

using YouthSpice.PreloadScene.Scene;
using YouthSpice.PreloadScene.Audio;

namespace YouthSpice.GalleryScene.UI
{
	/// <summary>
	/// 전반적인 UI를 관리합니다.
	/// </summary>
	public class UIManager : MonoBehaviour
	{
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private AudioClip backClip;

		private void Start()
		{
			animator.SetTrigger("On");
		}

		public void Exit()
		{
			animator.SetTrigger("Off");

			AudioManager.Instance.PlayEffectAudio(backClip);

			StartCoroutine(DelayedExitCoroutine());
		}

		private IEnumerator DelayedExitCoroutine()
		{
			yield return new WaitForSeconds(1.05f);

			if (SceneManager.sceneCount != 1) SceneChange.Instance.Unload("GalleryScene");
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
