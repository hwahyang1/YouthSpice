using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;
using YouthSpice.PreloadScene.Alert;
using YouthSpice.PreloadScene.Files;
using YouthSpice.PreloadScene.Scene;
using YouthSpice.StoryEditorScene.Condition;
using YouthSpice.StoryEditorScene.Element;
using YouthSpice.StoryEditorScene.Files;
using YouthSpice.StoryEditorScene.UI;
using YouthSpice.StoryScene.Extern;
using Random = UnityEngine.Random;

namespace YouthSpice.StoryEditorScene
{
	/// <summary>
	/// 챕터 프로젝트를 관리합니다.
	/// </summary>
	public class ChapterManager : MonoBehaviour
	{
		private UIManager uiManager;
		private ConditionManager conditionManager;
		private ElementManager elementManager;
		private ElementConverter elementConverter;
		private ChapterFileManager chapterFileManager;

		[SerializeField, ReadOnly]
		private string chapterID;

		private void Start()
		{
			uiManager = GetComponent<UIManager>();
			conditionManager = GetComponent<ConditionManager>();
			elementManager = GetComponent<ElementManager>();
			elementConverter = GetComponent<ElementConverter>();
			chapterFileManager = GetComponent<ChapterFileManager>();
		}

		/// <summary>
		/// 새로운 데이터를 생성합니다.
		/// </summary>
		public void NewData()
		{
			ulong timestamp = (ulong)System.DateTimeOffset.Now.ToUnixTimeSeconds();
			float random = Random.Range(0.01f, 1.2f);
			chapterID = ((ulong)(timestamp * random)).ToString();

			elementManager.RemoveAll();
			uiManager.SetChapterName("");
			uiManager.SetChapterID(chapterID + "");
			conditionManager.reset = true;
		}

		/// <summary>
		/// 기존 챕터 데이터를 불러옵니다.
		/// </summary>
		/// <param name="chapter">불러올 데이터를 지정합니다.</param>
		public void LoadData(DefineChapter chapter)
		{
			conditionManager.reset = true;
			elementManager.RemoveAll();

			chapterID = chapter.ID;

			uiManager.SetChapterID(chapterID + "");
			uiManager.SetChapterName(chapter.Name);

			string formerChapter = null;
			string minDay = null;
			string friendship1 = null;
			string friendship2 = null;
			string friendship3 = null;
			for (int i = 0; i < chapter.ConditionType.Length; i++)
			{
				switch (chapter.ConditionType[i])
				{
					case ChapterCondition.FormerChapter:
						formerChapter = chapter.ConditionData[i];
						break;
					case ChapterCondition.MinDay:
						minDay = chapter.ConditionData[i];
						break;
					case ChapterCondition.MinPlayer1Friendship:
						friendship1 = chapter.ConditionData[i];
						break;
					case ChapterCondition.MinPlayer2Friendship:
						friendship2 = chapter.ConditionData[i];
						break;
					case ChapterCondition.MinPlayer3Friendship:
						friendship3 = chapter.ConditionData[i];
						break;
				}
			}

			conditionManager.ApplyCustom(formerChapter, minDay, friendship1, friendship2, friendship3);

			elementConverter.ConvertClassToElement(chapter.Elements);
		}

		/// <summary>
		/// 작성된 챕터 데이터를 저장용 클래스로 변환합니다.
		/// </summary>
		/// <returns>변환한 데이터가 반환됩니다.</returns>
		public DefineChapter GetData()
		{
			DefineChapter data = new DefineChapter
			{
				ID = chapterID,
				Name = uiManager.GetChapterName()
			};

			List<ChapterCondition> conditionType = new List<ChapterCondition>();
			List<string> conditionData = new List<string>();
			foreach (KeyValuePair<ChapterCondition, string> currentCondition in conditionManager.GetData())
			{
				conditionType.Add(currentCondition.Key);
				conditionData.Add(currentCondition.Value);
			}

			data.ConditionType = conditionType.ToArray();
			data.ConditionData = conditionData.ToArray();

			data.Elements = elementConverter.ConvertElementToClass();

			return data;
		}

		/// <remarks>
		/// 실제 메소드 실행은 UIManager를 통해야 합니다.
		/// </remarks>
		public async void StartChapterPreview()
		{
			GameInfo.Instance.playerName = "(플레이어 이름)";
			
			StorySceneLoadParams.Instance.chapterCustomPath = chapterFileManager.PastFilePath;
			StorySceneLoadParams.Instance.resourceCustomPath = Application.dataPath + @"/.StoryEditor_Data";
			StorySceneLoadParams.Instance.chapterID = chapterID;

			await SourceFileManager.Instance.RefreshAll();

			SceneChange.Instance.Add("StoryScene");
		}
	}
}
