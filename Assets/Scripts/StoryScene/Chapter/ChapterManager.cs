using System;
using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

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
		private AudioManager audioManager;
		private BackgroundImage backgroundImage;
		private StandingIllusts standingIllusts;
		private FrontTop frontTop;
		private SpeechArea speechArea;
		
		private int currentChapterIndex = -1;
		private DefineChapter currentChapter;

		private void Awake()
		{
			audioManager = GetComponent<AudioManager>();
			backgroundImage = GetComponent<BackgroundImage>();
			standingIllusts = GetComponent<StandingIllusts>();
			frontTop = GetComponent<FrontTop>();
			speechArea = GetComponent<SpeechArea>();
		}

		private void Start()
		{
			if (LoadParams.Instance.chapterID == null || !SourceFileManager.Instance.AvailableChapters.ContainsKey(LoadParams.Instance.chapterID))
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
				PlayNext();
			}
		}

		public void PlayNext()
		{
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
						
						break;
					case ChapterElementType.DayImage:
						
						break;
					case ChapterElementType.BackgroundImage:
						
						break;
					case ChapterElementType.BackgroundMusic:
						
						break;
					case ChapterElementType.EffectSound:
						
						break;
					case ChapterElementType.StandingImage:
						
						break;
					case ChapterElementType.Friendship:
						// TODO
						break;
					case ChapterElementType.GetPlayerName:
						
						break;
					case ChapterElementType.Selection:
						
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
