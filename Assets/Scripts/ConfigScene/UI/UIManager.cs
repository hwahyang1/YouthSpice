using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

using YouthSpice.PreloadScene.Scene;

namespace YouthSpice.ConfigScene.UI
{
	/// <summary>
	/// 전반적인 UI의 실행을 관리합니다.
	/// </summary>
	public class UIManager : MonoBehaviour
	{
		public void Exit()
		{
			if (SceneManager.sceneCount != 1) SceneChange.Instance.Unload("ConfigScene");
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
