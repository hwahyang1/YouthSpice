using System;
using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace YouthSpice.PreloadScene.Scene
{
	/// <summary>
	/// Scene 변경과 트랜지션을 관리합니다.
	/// </summary>
	public class SceneChange : Singleton<SceneChange>
	{
		[Header("GameObject (Canvas)")]
		[SerializeField]
		private GameObject canvas;

		[SerializeField]
		private Image loadingCover;

		[SerializeField]
		private Text loadingPercent;

		[Header("Speed")]
		[SerializeField]
		private float transitionTime = 1f;

		private Action addSceneCallback = null;
		private Action loadSceneCallback = null;

		/// <summary>
		/// 직전 Scene의 이름을 담습니다.
		/// SceneChange.Change()를 사용하여 Scene을 변경해야 반영됩니다.
		/// </summary>
		public string PreviousScene { get; private set; } = "";

		/// <summary>
		/// Scene을 변경합니다.
		/// </summary>
		/// <param name="sceneName">변경할 Scene의 이름을 입력합니다.</param>
		/// <param name="fadeIn">Scene 변경 시 페이드 인 트랜지션을 적용 할 지 결정합니다.</param>
		/// <param name="fadeOut">Scene 변경 시 페이드 아웃 트랜지션을 적용 할 지 결정합니다.</param>
		/// <param name="callback">Scene 변경 이후 Callback을 받으려면 지정합니다.</param>
		public void ChangeScene(string sceneName, bool fadeIn = true, bool fadeOut = true, Action callback = null)
		{
			loadSceneCallback = callback;
			StartCoroutine(ChangeSceneCoroutine(sceneName, fadeIn, fadeOut));
		}

		private IEnumerator ChangeSceneCoroutine(string sceneName, bool fadeIn, bool fadeOut)
		{
			loadingPercent.gameObject.SetActive(false);

			if (fadeIn || fadeOut)
			{
				loadingCover.color = new Color(1f, 1f, 1f, 0f);
				loadingCover.gameObject.SetActive(true);
				canvas.SetActive(true);
			}

			if (fadeIn)
			{
				float accumulateTime = 0f;
				while (loadingCover.color.a < 1f)
				{
					accumulateTime += Time.deltaTime;
					Color color = new Color(1f, 1f, 1f, accumulateTime / transitionTime);
					loadingCover.color = color;
					yield return null;
				}

				yield return new WaitForSeconds(0.25f);
			}

			if (fadeOut)
			{
				loadingPercent.gameObject.SetActive(true);
				loadingPercent.text = "0%";
				loadingCover.color = new Color(1f, 1f, 1f, 1f);
				yield return new WaitForSeconds(0.25f);
			}

			AsyncOperation sceneChange = SceneManager.LoadSceneAsync(sceneName);
			while (!sceneChange.isDone)
			{
				loadingPercent.text = string.Format("{0}%", Mathf.Round(sceneChange.progress * 1000) * 0.1f);
				yield return null;
			}

			loadingPercent.text = "100%";
			yield return new WaitForSeconds(0.2f);

			loadingPercent.gameObject.SetActive(false);
			loadingPercent.text = "0%";
			yield return new WaitForSeconds(0.05f);

			if (fadeOut)
			{
				float accumulateTime = 0f;
				while (loadingCover.color.a > 0f)
				{
					accumulateTime += Time.deltaTime;
					Color color = new Color(1f, 1f, 1f, 1f - (accumulateTime / transitionTime));
					loadingCover.color = color;
					yield return null;
				}
			}

			if (fadeIn || fadeOut)
			{
				loadingCover.gameObject.SetActive(false);
				canvas.SetActive(false);
			}

			loadSceneCallback?.Invoke();
			loadSceneCallback = null;
		}

		/// <summary>
		/// Scene을 추가합니다.
		/// 이 경우, PreviousScene이 변경되지 않습니다.
		/// </summary>
		/// <param name="sceneName">변경할 Scene의 이름을 입력합니다.</param>
		/// <param name="callback">Scene 변경 이후 Callback을 받으려면 지정합니다.</param>
		public void Add(string sceneName, Action callback = null)
		{
			#pragma warning disable CS0618 // Type or member is obsolete
			foreach (var scene in SceneManager.GetAllScenes())
				if (scene.name == sceneName)
					return;
			#pragma warning restore CS0618 // Type or member is obsolete

			addSceneCallback = callback;
			StartCoroutine(nameof(AddSceneCoroutine), sceneName);
		}

		private IEnumerator AddSceneCoroutine(string sceneName)
		{
			AsyncOperation sceneChange = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			while (!sceneChange.isDone)
			{
				yield return null;
			}

			addSceneCallback?.Invoke();
			addSceneCallback = null;
		}

		/// <summary>
		/// Scene을 Unload 시킵니다.
		/// Active된 Scene이 한개 일 때만 PreviousScene이 변경됩니다.
		/// </summary>
		/// <param name="sceneName">Unload할 Scene의 이름을 입력합니다.</param>
		public void Unload(string sceneName)
		{
			bool found = false;
			#pragma warning disable CS0618 // Type or member is obsolete
			foreach (var scene in SceneManager.GetAllScenes())
			{
				if (scene.name == sceneName)
				{
					found = true;
					break;
				}
			}
			#pragma warning restore CS0618 // Type or member is obsolete
			if (!found) return;

			if (SceneManager.sceneCount == 1) PreviousScene = SceneManager.GetActiveScene().name;
			SceneManager.UnloadSceneAsync(sceneName);
		}
	}
}
