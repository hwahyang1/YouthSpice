using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Alert;
using YouthSpice.PreloadScene.Files;
using YouthSpice.StoryScene.Chapter;

namespace YouthSpice.StoryScene.UI
{
	/// <summary>
	/// 날짜 이미지와 스킵 버튼을 관리합니다.
	/// </summary>
	public class FrontTop : MonoBehaviour
	{
		[SerializeField]
		private Sprite blank;

		[SerializeField]
		private Image[] dayImageAreas;

		[Header("Status")]
		// false -> Back(0) / true -> Front(1)
		[SerializeField, ReadOnly]
		private bool activePosition = false;

		[Header("Time")]
		[SerializeField, Tooltip("Dissolve 기준으로, Fade의 경우 지정된 시간의 2배를 사용합니다.")]
		private float totalAnimationTime = 1f;

		[SerializeField, Tooltip("값이 높을수록 느려집니다.")]
		private float animationDelay = 0.01f;

		private Coroutine activeCoroutine = null;

		[Header("Buttons")]
		[SerializeField]
		private Button skipButton;

		private ChapterManager chapterManager;

		private void Start()
		{
			chapterManager = GetComponent<ChapterManager>();

			EventTrigger eventTrigger = skipButton.GetComponent<EventTrigger>();

			EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
			entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
			entry_PointerEnter.callback.AddListener((data) =>
			{
				OnSkipButtonMouseEnterCallback((PointerEventData)data);
			});
			eventTrigger.triggers.Add(entry_PointerEnter);

			EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
			entry_PointerExit.eventID = EventTriggerType.PointerExit;
			entry_PointerExit.callback.AddListener((data) =>
			{
				OnSkipButtonMouseExitCallback((PointerEventData)data);
			});
			eventTrigger.triggers.Add(entry_PointerExit);
		}

		public void OnKeyDown()
		{
			//
		}

		public void SetSkipButtonActive(bool active)
		{
			skipButton.gameObject.SetActive(active);
		}

		private void OnSkipButtonMouseEnterCallback(PointerEventData data)
		{
			chapterManager.isMouseOverButton = true;
		}

		private void OnSkipButtonMouseExitCallback(PointerEventData data)
		{
			chapterManager.isMouseOverButton = false;
		}

		public void OnSkipButtonClicked()
		{
			AlertManager.Instance.Show(AlertType.Double, "알림", "현재 구간을 스킵하시겠습니까?",
				new Dictionary<string, Action>() { { "예", chapterManager.SkipCurrent }, { "아니요", null } });
		}

		/// <summary>
		/// 날짜 이미지를 변경합니다.
		/// </summary>
		public void ChangeImage(Dictionary<string, string> data)
		{
			int imageIndex = int.Parse(data["Day"]);
			Sprite image = imageIndex <= 0
				? blank
				: SourceFileManager.Instance.AvailableDayImages[imageIndex - 1];

			DefineImageTransitions transition = (DefineImageTransitions)int.Parse(data["Transition"]);
			activeCoroutine = StartCoroutine(ChangeImageCoroutine(image, transition));
		}

		private IEnumerator ChangeImageCoroutine(Sprite image, DefineImageTransitions transitionType)
		{
			WaitForSeconds timeout = new WaitForSeconds(animationDelay);

			Image currentArea = dayImageAreas[activePosition ? 1 : 0];
			Image nextArea = dayImageAreas[activePosition ? 1 : 0];

			nextArea.color = new Color(1f, 1f, 1f, 0f);
			nextArea.sprite = image;

			//yield return null;

			activePosition = !activePosition;

			float currentTime = 0f;

			switch (transitionType)
			{
				case DefineImageTransitions.None:
					currentArea.color = new Color(1f, 1f, 1f, 0f);
					if (nextArea.sprite == blank) nextArea.color = new Color(1f, 1f, 1f, 0f);
					else nextArea.color = new Color(1f, 1f, 1f, 1f);
					break;
				case DefineImageTransitions.Dissolve:
					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						if (currentArea.sprite == blank) currentArea.color = new Color(1f, 1f, 1f, 0f);
						else currentArea.color = new Color(1f, 1f, 1f, 1f - (currentTime / totalAnimationTime));
						if (nextArea.sprite == blank) nextArea.color = new Color(1f, 1f, 1f, 0f);
						else nextArea.color = new Color(1f, 1f, 1f, currentTime / totalAnimationTime);
						yield return timeout;
					}

					yield return new WaitForSeconds(totalAnimationTime / 2f);

					break;
				case DefineImageTransitions.Fade:
					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						if (currentArea.sprite == blank) currentArea.color = new Color(1f, 1f, 1f, 0f);
						else currentArea.color = new Color(1f, 1f, 1f, 1f - (currentTime / totalAnimationTime));
						yield return timeout;
					}

					yield return new WaitForSeconds(totalAnimationTime / 2f);

					currentTime = 0f;

					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						if (nextArea.sprite == blank) nextArea.color = new Color(1f, 1f, 1f, 0f);
						else nextArea.color = new Color(1f, 1f, 1f, currentTime / totalAnimationTime);
						yield return timeout;
					}

					yield return new WaitForSeconds(totalAnimationTime / 2f);

					break;
			}

			yield return new WaitForSeconds(0.25f);

			activeCoroutine = null;
			chapterManager.isDayImageEnded = true;
			chapterManager.PlayNext();
		}
	}
}
