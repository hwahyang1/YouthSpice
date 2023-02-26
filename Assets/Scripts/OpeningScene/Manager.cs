using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Scene;

namespace YouthSpice.OpeningScene
{
	/// <summary>
	/// 오프닝의 전반적인 실행을 관리합니다.
	/// </summary>
	public class Manager : MonoBehaviour
	{
		private void Start()
		{
			StartCoroutine(nameof(WaitCoroutine));
		}

		private void Update()
		{
			if (Input.anyKeyDown)
			{
				StopCoroutine(nameof(WaitCoroutine));
				SceneChange.Instance.ChangeScene("MenuScene", false, false);
			}
		}

		private IEnumerator WaitCoroutine()
		{
			yield return new WaitForSeconds(10.25f);
			SceneChange.Instance.ChangeScene("MenuScene", false, false);
		}
	}
}
