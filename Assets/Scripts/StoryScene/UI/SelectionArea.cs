using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Newtonsoft.Json;
using NaughtyAttributes;

using YouthSpice.StoryScene.Audio;
using YouthSpice.StoryScene.Chapter;

namespace YouthSpice.StoryScene.UI
{
	/// <summary>
	/// 선택지를 관리합니다.
	/// </summary>
	public class SelectionArea : MonoBehaviour
	{
		[SerializeField]
		private GameObject selectionBackground;

		[SerializeField]
		private Button[] selectionButtons;

		[SerializeField, ReadOnly]
		private bool ready = false;

		public bool Ready => ready;

		private int currentElementIndex = 0;
		private List<ChapterElement> currentElements;
		private List<ChapterElement> currentSelections;

		private AudioManager audioManager;
		private BackgroundImage backgroundImage;
		private StandingIllusts standingIllusts;
		private FrontTop frontTop;
		private SpeechArea speechArea;

		private ChapterManager chapterManager;

		private void Awake()
		{
			HideSelection();
		}

		private void Start()
		{
			audioManager = GetComponent<AudioManager>();
			backgroundImage = GetComponent<BackgroundImage>();
			standingIllusts = GetComponent<StandingIllusts>();
			frontTop = GetComponent<FrontTop>();
			speechArea = GetComponent<SpeechArea>();
			
			chapterManager = GetComponent<ChapterManager>();
		}

		public void OnKeyDown()
		{
			//
		}

		private void ShowSelection()
		{
			selectionBackground.SetActive(true);

			for (int i = 0; i < currentSelections.Count; i++)
			{
				selectionButtons[i].transform.GetChild(0).GetComponent<Text>().text = currentSelections[i].Data["SelectionName"];
				selectionButtons[i].gameObject.SetActive(true);
			}
		}

		public void OnButtonInteract(int order)
		{
			HideSelection();

			currentElementIndex = currentElements.FindIndex(target =>
				target.Type == ChapterElementType.SelectionName &&
				target.Data["SelectionName"] == currentSelections[order].Data["SelectionName"]);

			ready = true;
			
			PlayNext();
		}

		private void HideSelection()
		{
			selectionBackground.SetActive(false);

			foreach (Button currentButton in selectionButtons)
			{
				currentButton.gameObject.SetActive(false);
			}
		}

		public void RunRoutine(Dictionary<string, string> data)
		{
			currentElements = new List<ChapterElement>();
			currentSelections = new List<ChapterElement>();
			
			for (int i = 0;; i++)
			{
				if (!data.ContainsKey(i.ToString())) break;
				ChapterElement currentData = JsonConvert.DeserializeObject<ChapterElement>(data[i.ToString()]);
				currentElements.Add(currentData);
			}
			
			currentSelections = currentElements.FindAll(target => target.Type == ChapterElementType.SelectionName);

			ready = false;
			currentElementIndex = 0;

			ShowSelection();
		}

		public void PlayNext()
		{
			if (!ready) return;
			if (!chapterManager.isSpeechEnded || !chapterManager.isDayImageEnded ||
			    !chapterManager.isBackgroundImageEnded || !chapterManager.isBackgroundMusicEnded ||
			    !chapterManager.isDayImageEnded || !chapterManager.isEffectSoundEnded ||
			    !chapterManager.isStandingImageEnded || !chapterManager.isFriendshipEnded ||
			    !chapterManager.isGetPlayerNameEnded) return;

			ChapterElement currentElement;
			bool loop = true;

			while (loop)
			{
				currentElementIndex++;

				if (currentElements.Count == currentElementIndex)
				{
					// 분기점 종료
					loop = false;
					chapterManager.isSelectionEnded = true;
					chapterManager.PlayNext();
					break;
				}

				currentElement = currentElements[currentElementIndex];

				switch (currentElement.Type)
				{
					case ChapterElementType.Speech:
						loop = false;
						chapterManager.isSpeechEnded = false;
						speechArea.ShowSpeech(currentElement.Data);
						break;
					case ChapterElementType.DayImage:
						loop = false;
						chapterManager.isDayImageEnded = false;
						frontTop.ChangeImage(currentElement.Data);
						break;
					case ChapterElementType.BackgroundImage:
						loop = false;
						chapterManager.isBackgroundImageEnded = false;
						speechArea.SetBlank();
						speechArea.SetActive(false);
						backgroundImage.ChangeImage(currentElement.Data, false, () => { speechArea.SetActive(true); });
						break;
					case ChapterElementType.BackgroundMusic:
						chapterManager.isBackgroundMusicEnded = false;
						audioManager.PlayBackgroundAudio(currentElement.Data);
						break;
					case ChapterElementType.EffectSound:
						chapterManager.isEffectSoundEnded = false;
						audioManager.PlayEffectAudio(currentElement.Data);
						break;
					case ChapterElementType.StandingImage:
						loop = false;
						chapterManager.isStandingImageEnded = false;
						standingIllusts.ChangeIllust(currentElement.Data);
						break;
					case ChapterElementType.Friendship:
						// TODO
						//loop = false;
						//isFriendshipEnded = false;
						break;
					case ChapterElementType.GetPlayerName:
						// TODO
						//loop = false;
						//isGetPlayerNameEnded = false;
						break;
					case ChapterElementType.Selection:
						// 이럴 일 없음
						break;
					case ChapterElementType.SelectionName:
						// 분기점 종료
						loop = false;
						chapterManager.isSelectionEnded = true;
						chapterManager.PlayNext();
						break;
				}
			}
		}
	}
}
