using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Action = System.Action;

using UnityEngine;
using UnityEngine.SceneManagement;

using NaughtyAttributes;

using YouthSpice.InGameMenuScene;
using YouthSpice.PreloadScene.Scene;
using YouthSpice.PreloadScene.Alert;
using YouthSpice.PreloadScene.Files;
using YouthSpice.PreloadScene.Game;
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
		[SerializeField, ReadOnly]
		private string currentChapterName = "";
		
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

		private bool calledExit = false;

		private AudioManager audioManager;
		private BackgroundImage backgroundImage;
		private StandingIllusts standingIllusts;
		private FrontTop frontTop;
		private SpeechArea speechArea;
		private SelectionArea selectionArea;
		private GetNameArea getNameArea;
		private Friendship friendship;

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
			getNameArea = GetComponent<GetNameArea>();
			friendship = GetComponent<Friendship>();
		}

		private void Start()
		{
			if (!StorySceneLoadParams.Instance.isTutorialScene) frontTop.SetSkipButtonActive(false);

			if (StorySceneLoadParams.Instance.chapterID == null ||
			    !SourceFileManager.Instance.AvailableChapters.ContainsKey(StorySceneLoadParams.Instance.chapterID))
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
				currentChapter = SourceFileManager.Instance.AvailableChapters[StorySceneLoadParams.Instance.chapterID];
				//if (!StorySceneLoadParams.Instance.isTutorialScene) GameInfo.Instance.slotName = currentChapter.Name;
				currentChapterName = currentChapter.Name;
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

			// 다른 창 열렸을 때 입력되는 현상 방지
			if ((!StorySceneLoadParams.Instance.isTutorialScene && SceneManager.sceneCount <= 2) ||
			    (StorySceneLoadParams.Instance.isTutorialScene && SceneManager.sceneCount == 2))
			{
				if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) ||
				    Input.GetKeyDown(KeyCode.KeypadEnter))
				{
					ExecuteAllOnKeyDown();
				}

				if (!isMouseOverButton && Input.GetMouseButtonDown(0))
				{
					if (!isSelectionEnded)
					{
						if (selectionArea.Ready)
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

			if (StorySceneLoadParams.Instance.isTutorialScene) return; //여기는 각각 Scene들이 처리 해 줄 거임
			if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F1))
			{
				if (SceneManager.sceneCount == 3)
				{
					//SceneChange.Instance.Unload("InGameMenuScene");
					GameObject.FindObjectOfType<MenuManager>().Exit();
				}
				else
				{
					SceneChange.Instance.Add("InGameMenuScene");
				}
			}
		}

		private void ExecuteAllOnKeyDown()
		{
			PlayNext();
			speechArea.OnKeyDown();

			if (!StorySceneLoadParams.Instance.isTutorialScene)
			{
				audioManager.OnKeyDown();
				backgroundImage.OnKeyDown();
				standingIllusts.OnKeyDown();
				frontTop.OnKeyDown();
				selectionArea.OnKeyDown();
			}
		}

		public void SkipCurrent()
		{
			// 분기점 안에서는 처리 넘김
			if (!isSelectionEnded)
			{
				selectionArea.SkipCurrent();
				return;
			}

			// 남은 구간만 List로 가져오기
			List<ChapterElement> list = new List<ChapterElement>(currentChapter.Elements.Skip(currentChapterIndex)
			                                                                   .Take(currentChapter.Elements.Length -
				                                                                   currentChapterIndex));
			int index = list.FindIndex(target =>
				target.Type == ChapterElementType.Selection || target.Type == ChapterElementType.GetPlayerName);

			ChapterElement currentElement;

			// 앞에 분기점 & 이름 획득 없음
			if (index == -1)
			{
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
							backgroundImage.ChangeImage(currentElement.Data, true,
								() => { speechArea.SetActive(true); });
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
							friendship.AdjustFromElement(currentElement.Data);
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
				// 위 작업으로 빠진 앞부분 Index를 다시 채워넣음
				index += currentChapterIndex - 3;
				
				for (; currentChapterIndex < index; currentChapterIndex++)
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
							backgroundImage.ChangeImage(currentElement.Data, true,
								() => { speechArea.SetActive(true); });
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
							friendship.AdjustFromElement(currentElement.Data);
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
					if (!calledExit) Exit();
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
						frontTop.SetSkipButtonActive(false);
						backgroundImage.ChangeImage(currentElement.Data, false, () =>
						{
							speechArea.SetActive(true);
							frontTop.SetSkipButtonActive(true);
						});
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
						loop = false;
						isFriendshipEnded = false;
						friendship.AdjustFromElement(currentElement.Data, () =>
						{
							isFriendshipEnded = true;
							PlayNext();
						});
						break;
					case ChapterElementType.GetPlayerName:
						loop = false;
						isGetPlayerNameEnded = false;
						getNameArea.Show(() =>
						{
							isGetPlayerNameEnded = true;
							PlayNext();
						});
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
			calledExit = true;

			if (StorySceneLoadParams.Instance.isTutorialScene)
			{
				StorySceneLoadParams.Instance.Exit();
				SceneChange.Instance.Unload("StoryScene_Tutorial");
			}
			else
			{
				StorySceneLoadParams.Instance.Exit();

				// GameInfo에 변경사항 반영
				friendship.Apply();

				GameProgressManager.Instance.CountUp();
				GameProgressManager.Instance.RunThisChapter();
				if (SceneManager.sceneCount != 1) SceneChange.Instance.Unload("StoryScene");
			}
		}
	}
}
