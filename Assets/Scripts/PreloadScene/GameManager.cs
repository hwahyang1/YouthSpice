using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Scene;

namespace YouthSpice.PreloadScene
{
	/// <summary>
	/// Scene의 전반적인 실행을 관리합니다.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private List<GameObject> canvases = new List<GameObject>();

		[SerializeField]
		private SceneChange sceneChange;

		private void Start()
		{
			foreach (GameObject obj in canvases)
			{
				DontDestroyOnLoad(obj);
			}

			StartCoroutine(DelayedStart());
		}

		private IEnumerator DelayedStart()
		{
			yield return new WaitForSeconds(1f);

			sceneChange.ChangeScene("PreloadScene", false, true);
		}
	}
}
