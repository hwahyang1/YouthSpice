using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

using YouthSpice.PreloadScene.Scene;

namespace YouthSpice.GalleryScene.UI
{
	/// <summary>
	/// 전반적인 UI를 관리합니다.
	/// </summary>
	public class UIManager : MonoBehaviour
	{
		[SerializeField]
		private Animator animator;

		private void Start()
		{
			animator.SetTrigger("On");
		}
		
		public void Exit()
		{
			animator.SetTrigger("Off");

			StartCoroutine(DelayedExitCoroutine());
		}

		private IEnumerator DelayedExitCoroutine()
		{
			yield return new WaitForSeconds(0.55f);
			
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
