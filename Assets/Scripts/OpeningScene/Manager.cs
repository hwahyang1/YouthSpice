using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Scene;

namespace YouthSpice.OpeningScene
{
	/// <summary>
	/// Description
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
			yield return new WaitForSeconds(9.75f);
			SceneChange.Instance.ChangeScene("MenuScene", false, false);
		}
	}
}
