using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Files;
using YouthSpice.StoryScene.Chapter;

namespace YouthSpice.StoryScene.UI
{
	/// <summary>
	/// 스탠딩 일러스트의 노출을 관리합니다.
	/// </summary>
	public class StandingIllusts : MonoBehaviour
	{
		[SerializeField]
		private Sprite blank;
		
		[SerializeField]
		private Image[] frontStandingIllustAreas;

		[SerializeField]
		private Image[] backStandingIllustAreas;

		[Header("Status")]
		// false -> Back / true -> Front
		[SerializeField, ReadOnly]
		private bool activePosition = false;

		[SerializeField, ReadOnly]
		private bool isRunning = false;

		[Header("Time")]
		[SerializeField, Tooltip("Dissolve 기준으로, Fade의 경우 지정된 시간의 2배를 사용합니다.")]
		private float totalAnimationTime = 1f;

		[SerializeField, Tooltip("값이 높을수록 느려집니다.")]
		private float animationDelay = 0.01f;

		private Coroutine activeCoroutine = null;

		private ChapterManager chapterManager;

		private void Start()
		{
			chapterManager = GetComponent<ChapterManager>();
		}

		public void OnKeyDown()
		{
			if (isRunning)
			{
				if (activeCoroutine != null) StopCoroutine(activeCoroutine);

				foreach (Image area in frontStandingIllustAreas)
				{
					if (area.sprite == null) area.color = new Color(1f, 1f, 1f, 0f);
					else area.color = new Color(1f, 1f, 1f, activePosition ? 1f : 0f);
				}

				foreach (Image area in backStandingIllustAreas)
				{
					if (area.sprite == null) area.color = new Color(1f, 1f, 1f, 0f);
					else area.color = new Color(1f, 1f, 1f, activePosition ? 0f : 1f);
				}

				activeCoroutine = null;
				isRunning = false;
				chapterManager.isStandingImageEnded = true;
				chapterManager.PlayNext();
			}
		}

		/// <summary>
		/// 스탠딩 일러스트를 변경합니다.
		/// </summary>
		public void ChangeIllust(Dictionary<string, string> data, bool forceDisableTransition = false)
		{
			isRunning = true;

			List<Sprite> images = new List<Sprite>();

			int imageIndex1 = int.Parse(data["CharacterSlot1"]);
			int imageIndex2 = int.Parse(data["CharacterSlot2"]);
			int imageIndex3 = int.Parse(data["CharacterSlot3"]);
			images.Add(imageIndex1 <= 0
				? blank
				: SourceFileManager.Instance.AvailableStandingIllusts[imageIndex1 - 1]);
			images.Add(imageIndex2 <= 0
				? blank
				: SourceFileManager.Instance.AvailableStandingIllusts[imageIndex2 - 1]);
			images.Add(imageIndex3 <= 0
				? blank
				: SourceFileManager.Instance.AvailableStandingIllusts[imageIndex3 - 1]);

			DefineImageTransitions transition = (DefineImageTransitions)int.Parse(data["Transition"]);
			activeCoroutine = StartCoroutine(ChangeIllustCoroutine(images.ToArray(), forceDisableTransition, transition));
		}

		private IEnumerator ChangeIllustCoroutine(Sprite[] image, bool forceDisableTransition, DefineImageTransitions transitionType)
		{
			if (forceDisableTransition) transitionType = DefineImageTransitions.None;
			
			WaitForSeconds timeout = new WaitForSeconds(animationDelay);

			Image[] currentAreas = activePosition ? frontStandingIllustAreas : backStandingIllustAreas;
			Image[] nextAreas = activePosition ? backStandingIllustAreas : frontStandingIllustAreas;

			for (int i = 0; i < nextAreas.Length; i++)
			{
				if (nextAreas[i].sprite == blank) nextAreas[i].color = new Color(1f, 1f, 1f, 0f);
				else nextAreas[i].color = new Color(1f, 1f, 1f, 0f);
				nextAreas[i].sprite = image[i];
			}

			//yield return null;

			activePosition = !activePosition;

			isRunning = true;

			float currentTime = 0f;

			switch (transitionType)
			{
				case DefineImageTransitions.None:
					foreach (Image area in currentAreas)
					{
						area.color = new Color(1f, 1f, 1f, 0f);
					}

					foreach (Image area in nextAreas)
					{
						if (area.sprite == blank) area.color = new Color(1f, 1f, 1f, 0f);
						else area.color = new Color(1f, 1f, 1f, 1f);
					}

					break;
				case DefineImageTransitions.Dissolve:
					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						foreach (Image area in currentAreas)
						{
							if (area.sprite == blank) area.color = new Color(1f, 1f, 1f, 0f);
							else area.color = new Color(1f, 1f, 1f, 1f - (currentTime / totalAnimationTime));
						}

						foreach (Image area in nextAreas)
						{
							if (area.sprite == blank) area.color = new Color(1f, 1f, 1f, 0f);
							else area.color = new Color(1f, 1f, 1f, currentTime / totalAnimationTime);
						}

						yield return timeout;
					}

					break;
				case DefineImageTransitions.Fade:
					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						foreach (Image area in currentAreas)
						{
							if (area.sprite == blank) area.color = new Color(1f, 1f, 1f, 0f);
							else area.color = new Color(1f, 1f, 1f, 1f - (currentTime / totalAnimationTime));
						}

						yield return timeout;
					}

					currentTime = 0f;

					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						foreach (Image area in nextAreas)
						{
							if (area.sprite == blank) area.color = new Color(1f, 1f, 1f, 0f);
							else area.color = new Color(1f, 1f, 1f, currentTime / totalAnimationTime);
						}

						yield return timeout;
					}

					break;
			}

			yield return new WaitForSeconds(0.25f);

			activeCoroutine = null;
			isRunning = false;
			chapterManager.isStandingImageEnded = true;
			chapterManager.PlayNext();
		}
	}
}
