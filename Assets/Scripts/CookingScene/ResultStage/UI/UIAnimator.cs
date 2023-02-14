using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.CookingScene.ResultStage.UI
{
	/// <summary>
	/// 애니메이션을 관리합니다.
	/// </summary>
	public class UIAnimator : MonoBehaviour
	{
		[Header("Configs")]
		[SerializeField]
		private float animationDelay = 0.01f;
		[SerializeField]
		private float fadeTime = 1.2f;
		[SerializeField]
		private float fadeWaitTime = 0.5f;
		[SerializeField]
		private float cameraEffectOutTime = 1.5f;

		[Header("GameObjects")]
		[SerializeField]
		private Image blackCover;
		[SerializeField]
		private Image whiteCover;
		[SerializeField]
		private Animator titleSpeechAnimator;
		
		[Header("Classes")]
		[SerializeField]
		private GameManager gameManager;

		private WaitForSeconds waitAnimationDelay;

		private void Start()
		{
			waitAnimationDelay = new WaitForSeconds(animationDelay);

			blackCover.color = new Color(0f, 0f, 0f, 0f);
			blackCover.gameObject.SetActive(false);
			whiteCover.color = new Color(1f, 1f, 1f, 0f);
			whiteCover.gameObject.SetActive(false);
		}

		public void First(Action callback=null)
		{
			StartCoroutine(FirstCoroutine(callback));
		}

		private IEnumerator FirstCoroutine(Action callback=null)
		{
			blackCover.color = new Color(0f, 0f, 0f, 0f);
			blackCover.gameObject.SetActive(true);
			
			float currentTime = 0f;
			while (currentTime < fadeTime)
			{
				currentTime += animationDelay;
				blackCover.color = new Color(0f, 0f, 0f, currentTime / fadeTime);
				yield return waitAnimationDelay;
			}
			
			yield return new WaitForSeconds(fadeWaitTime);
			
			callback?.Invoke();
		}
		
		public void Run()
		{
			StartCoroutine(RunCoroutine());
		}

		private IEnumerator RunCoroutine()
		{
			float currentTime = 0f;
			while (currentTime < fadeTime)
			{
				currentTime += animationDelay;
				blackCover.color = new Color(0f, 0f, 0f, 1f - (currentTime / fadeTime));
				yield return waitAnimationDelay;
			}
			
			blackCover.color = new Color(0f, 0f, 0f, 0f);
			blackCover.gameObject.SetActive(false);
			
			yield return new WaitForSeconds(1f);
			
			currentTime = 0f;
			
			whiteCover.color = new Color(1f, 1f, 1f, 1f);
			whiteCover.gameObject.SetActive(true);
			
			while (currentTime < fadeTime)
			{
				currentTime += animationDelay;
				whiteCover.color = new Color(1f, 1f, 1f, 1f - (currentTime / fadeTime));
				yield return waitAnimationDelay;
			}
			
			blackCover.color = new Color(0f, 0f, 0f, 0f);
			blackCover.gameObject.SetActive(false);
			
			yield return new WaitForSeconds(0.5f);
			
			titleSpeechAnimator.SetTrigger("MenuTitle");
			
			yield return new WaitForSeconds(2.25f);
			
			titleSpeechAnimator.SetTrigger("SpeechText");
			
			yield return new WaitForSeconds(1f);

			gameManager.ended = true;
		}
	}
}
