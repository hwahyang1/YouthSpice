using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Files;
using YouthSpice.StoryScene.Chapter;

namespace YouthSpice.StoryScene.UI
{
	/// <summary>
	/// 배경 이미지를 관리합니다.
	/// </summary>
	public class BackgroundImage : MonoBehaviour
	{
		[SerializeField]
		private Image[] backgroundImageAreas;

		[Header("Status")]
		// false -> Back(0) / true -> Front(1)
		[SerializeField, ReadOnly]
		private bool activePosition = false;
		
		[Header("Time")]
		[SerializeField, Tooltip("이 값은 Dissolve 기준으로, Fade의 경우 지정된 시간의 2배를 사용합니다.")]
		private float totalAnimationTime = 1.5f;

		[SerializeField]
		private float animationDelay = 0.01f;

		[Header("BlankTime")]
		[SerializeField]
		private float[] blankAnimationTime;

		private Coroutine activeCoroutine = null;

		private ChapterManager chapterManager;

		private Action callback = null;

		private void Start()
		{
			chapterManager = GetComponent<ChapterManager>();
		}

		public void OnKeyDown()
		{
			//
		}

		/// <summary>
		/// 스탠딩 일러스트를 변경합니다.
		/// </summary>
		public void ChangeImage(Dictionary<string, string> data, bool forceDisableTransition = false, Action callback = null)
		{
			this.callback = callback;

			Sprite image = data["Background"] == "0"
				? null
				: SourceFileManager.Instance.AvailableBackgroundImages[int.Parse(data["Background"]) - 1];

			activeCoroutine =
				StartCoroutine(ChangeImageCoroutine(image, forceDisableTransition, (DefineImageTransitions)int.Parse(data["Transition"])));
		}

		private IEnumerator ChangeImageCoroutine(Sprite image, bool forceDisableTransition, DefineImageTransitions transitionType)
		{
			if (forceDisableTransition) transitionType = DefineImageTransitions.None;
			
			WaitForSeconds timeout = new WaitForSeconds(animationDelay);

			Image currentArea = backgroundImageAreas[activePosition ? 1 : 0];
			Image nextArea = backgroundImageAreas[activePosition ? 0 : 1];

			nextArea.color = new Color(1f, 1f, 1f, 0f);
			nextArea.sprite = image;

			yield return null;

			activePosition = !activePosition;

			float currentTime = 0f;

			switch (transitionType)
			{
				case DefineImageTransitions.None:
					currentArea.color = new Color(1f, 1f, 1f, 0f);
					nextArea.color = new Color(1f, 1f, 1f, 1f);
					break;
				case DefineImageTransitions.Dissolve:
					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						currentArea.color = new Color(1f, 1f, 1f, 1f - (currentTime / totalAnimationTime));
						nextArea.color = new Color(1f, 1f, 1f, currentTime / totalAnimationTime);
						yield return timeout;
					}
					
					yield return new WaitForSeconds(totalAnimationTime / 2f);

					break;
				case DefineImageTransitions.Fade:
					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						currentArea.color = new Color(1f, 1f, 1f, 1f - (currentTime / totalAnimationTime));
						yield return timeout;
					}

					yield return new WaitForSeconds(totalAnimationTime / 2f);

					currentTime = 0f;

					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						nextArea.color = new Color(1f, 1f, 1f, currentTime / totalAnimationTime);
						yield return timeout;
					}
					
					yield return new WaitForSeconds(totalAnimationTime / 2f);

					break;
				case DefineImageTransitions.BlinkFade:
					yield return new WaitForSeconds(blankAnimationTime[1] / 2f);
					
					while (currentTime < blankAnimationTime[0])
					{
						currentTime += animationDelay;
						currentArea.color = new Color(1f, 1f, 1f, 1f - (currentTime / blankAnimationTime[0]));
						yield return timeout;
					}

					yield return new WaitForSeconds(blankAnimationTime[1] / 2f);

					currentTime = 0f;

					while (currentTime < blankAnimationTime[1])
					{
						currentTime += animationDelay;
						nextArea.color = new Color(1f, 1f, 1f, currentTime / blankAnimationTime[1]);
						yield return timeout;
					}
					
					yield return new WaitForSeconds(blankAnimationTime[1] / 2f);
					
					break;
			}

			yield return new WaitForSeconds(0.25f);

			callback?.Invoke();
			callback = null;
			activeCoroutine = null;
			chapterManager.isBackgroundImageEnded = true;
			chapterManager.PlayNext();
		}
	}
}
