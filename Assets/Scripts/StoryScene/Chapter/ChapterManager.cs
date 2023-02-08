using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Action = System.Action;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Scene;
using YouthSpice.PreloadScene.Alert;
using YouthSpice.PreloadScene.Files;
using YouthSpice.StoryScene.Audio;
using YouthSpice.StoryScene.Extern;
using YouthSpice.StoryScene.UI;

namespace YouthSpice.StoryScene.Chapter
{
	/// <summary>
	/// 챕터 진행을 관리합니다.
	/// </summary>
	public class ChapterManager : MonoBehaviour
	{
		[ReadOnly]
		public bool isSpeechEnded = true;

		[ReadOnly]
		public bool isDayImageEnded = true;

		[ReadOnly]
		public bool isBackgroundImageEnded = true;

		[ReadOnly]
		public bool isBackgroundMusicEnded = true;

		[ReadOnly]
		public bool isEffectSoundEnded = true;

		[ReadOnly]
		public bool isStandingImageEnded = true;

		[ReadOnly]
		public bool isFriendshipEnded = true;

		[ReadOnly]
		public bool isGetPlayerNameEnded = true;

		[ReadOnly]
		public bool isSelectionEnded = true;

		[ReadOnly]
		public bool isMouseOverButton = false;

		private AudioManager audioManager;
		private BackgroundImage backgroundImage;
		private StandingIllusts standingIllusts;
		private FrontTop frontTop;
		private SpeechArea speechArea;
		private SelectionArea selectionArea;

		private int currentChapterIndex = -1;
		private DefineChapter currentChapter;

		private void Awake()
		{
			audioManager = GetComponent<AudioManager>();
			backgroundImage = GetComponent<BackgroundImage>();
			standingIllusts = GetComponent<StandingIllusts>();
			frontTop = GetComponent<FrontTop>();
			speechArea = GetComponent<SpeechArea>();
			selectionArea = GetComponent<SelectionArea>();
		}

		private void Start()
		{
			if (LoadParams.Instance.chapterID == null ||
			    !SourceFileManager.Instance.AvailableChapters.ContainsKey(LoadParams.Instance.chapterID))
			{
				AlertManager.Instance.Show(AlertType.Single, "경고", "유효하지 않은 챕터 ID 입니다.",
					new Dictionary<string, Action>()
					{
						{
							"종료", Exit
						}
					});
			}
			else
			{
				currentChapter = SourceFileManager.Instance.AvailableChapters[LoadParams.Instance.chapterID];
				StartCoroutine(nameof(LateStartCoroutine));
			}
		}

		private IEnumerator LateStartCoroutine()
		{
			yield return new WaitForSeconds(0.1f);
			PlayNext();
		}

		private void Update()
		{
			if (AlertManager.Instance.IsRunning) return;

			if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) ||
			    Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				ExecuteAllOnKeyDown();
			}

			if (!isMouseOverButton && Input.GetMouseButtonDown(0))
			{
				if (!isSelectionEnded)
				{
					if (!selectionArea.Ready)
					{
						ExecuteAllOnKeyDown();
					}
				}
				else
				{
					ExecuteAllOnKeyDown();
				}
			}
		}

		private void ExecuteAllOnKeyDown()
		{
			PlayNext();
			audioManager.OnKeyDown();
			backgroundImage.OnKeyDown();
			standingIllusts.OnKeyDown();
			frontTop.OnKeyDown();
			speechArea.OnKeyDown();
			selectionArea.OnKeyDown();
		}

		public void SkipCurrent()
		{
			// 분기점 안에서는 처리 안함
			if (!isSelectionEnded) return;
			
			// 남은 구간만 List로 가져오기
			List<ChapterElement> list = new List<ChapterElement>(currentChapter.Elements.Skip(currentChapterIndex).Take(currentChapter.Elements.Length - currentChapterIndex));
			int index = list.FindIndex(target => target.Type == ChapterElementType.Selection || target.Type == ChapterElementType.GetPlayerName);
			print(index);

			ChapterElement currentElement;
			
			// 앞에 분기점 없음
			if (index == -1)
			{
				print("asdf");
				for (; currentChapterIndex < currentChapter.Elements.Length; currentChapterIndex++)
				{
					currentElement = currentChapter.Elements[currentChapterIndex];

					switch (currentElement.Type)
					{
						case ChapterElementType.Speech:
							break;
						case ChapterElementType.DayImage:
							frontTop.ChangeImage(currentElement.Data);
							break;
						case ChapterElementType.BackgroundImage:
							backgroundImage.ChangeImage(currentElement.Data, true, () => { speechArea.SetActive(true); });
							break;
						case ChapterElementType.BackgroundMusic:
							audioManager.PlayBackgroundAudio(currentElement.Data, true);
							break;
						case ChapterElementType.EffectSound:
							break;
						case ChapterElementType.StandingImage:
							standingIllusts.ChangeIllust(currentElement.Data, true);
							break;
						case ChapterElementType.Friendship:
							// TODO
							break;
						case ChapterElementType.GetPlayerName:
							// 앞에서 걸러짐
							break;
						case ChapterElementType.Selection:
							// 얘로 올 일이 없음
							break;
						case ChapterElementType.SelectionName:
							// ChapterElementType.Selection에서 처리됨
							break;
					}
				}
				Exit();
			}
			// 앞에 분기점 있으면 -> 직전 항목으로 이동시킴
			else
			{
				for (; currentChapterIndex < index; currentChapterIndex++)
				{
					print(currentChapterIndex);
					
					currentElement = currentChapter.Elements[currentChapterIndex];

					switch (currentElement.Type)
					{
						case ChapterElementType.Speech:
							break;
						case ChapterElementType.DayImage:
							frontTop.ChangeImage(currentElement.Data);
							break;
						case ChapterElementType.BackgroundImage:
							backgroundImage.ChangeImage(currentElement.Data, true, () => { speechArea.SetActive(true); });
							break;
						case ChapterElementType.BackgroundMusic:
							audioManager.PlayBackgroundAudio(currentElement.Data, true);
							break;
						case ChapterElementType.EffectSound:
							break;
						case ChapterElementType.StandingImage:
							standingIllusts.ChangeIllust(currentElement.Data, true);
							break;
						case ChapterElementType.Friendship:
							// TODO
							break;
						case ChapterElementType.GetPlayerName:
							// 앞에서 걸러짐
							break;
						case ChapterElementType.Selection:
							// 얘로 올 일이 없음
							break;
						case ChapterElementType.SelectionName:
							// ChapterElementType.Selection에서 처리됨
							break;
					}
				}

				currentChapterIndex--;
				ExecuteAllOnKeyDown();
			}
		}

		public void PlayNext()
		{
			if (!isSelectionEnded)
			{
				selectionArea.PlayNext();
				return;
			}

			if (!isSpeechEnded || !isDayImageEnded || !isBackgroundImageEnded || !isBackgroundMusicEnded ||
			    !isDayImageEnded || !isEffectSoundEnded || !isStandingImageEnded || !isFriendshipEnded ||
			    !isGetPlayerNameEnded) return;

			ChapterElement currentElement;
			bool loop = true;

			while (loop)
			{
				currentChapterIndex++;

				if (currentChapterIndex >= currentChapter.Elements.Length)
				{
					Exit();
					loop = false;
					break;
				}

				currentElement = currentChapter.Elements[currentChapterIndex];

				switch (currentElement.Type)
				{
					case ChapterElementType.Speech:
						loop = false;
						isSpeechEnded = false;
						speechArea.ShowSpeech(currentElement.Data);
						break;
					case ChapterElementType.DayImage:
						loop = false;
						isDayImageEnded = false;
						frontTop.ChangeImage(currentElement.Data);
						break;
					case ChapterElementType.BackgroundImage:
						loop = false;
						isBackgroundImageEnded = false;
						speechArea.SetBlank();
						speechArea.SetActive(false);
						backgroundImage.ChangeImage(currentElement.Data, false, () => { speechArea.SetActive(true); });
						break;
					case ChapterElementType.BackgroundMusic:
						isBackgroundMusicEnded = false;
						audioManager.PlayBackgroundAudio(currentElement.Data);
						break;
					case ChapterElementType.EffectSound:
						isEffectSoundEnded = false;
						audioManager.PlayEffectAudio(currentElement.Data);
						break;
					case ChapterElementType.StandingImage:
						loop = false;
						isStandingImageEnded = false;
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
						loop = false;
						isSelectionEnded = false;
						selectionArea.RunRoutine(currentElement.Data);
						break;
					case ChapterElementType.SelectionName:
						// ChapterElementType.Selection에서 처리됨
						break;
				}
			}
		}

		/// <summary>
		/// 챕터 연출을 종료합니다.
		/// </summary>
		public void Exit()
		{
			Destroy(LoadParams.Instance.gameObject);
			if (SceneManager.sceneCount != 1) SceneChange.Instance.Unload("StoryScene");
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
