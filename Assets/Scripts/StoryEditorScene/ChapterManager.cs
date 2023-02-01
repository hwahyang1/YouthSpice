using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

using YouthSpice.StoryEditorScene.Condition;
using YouthSpice.StoryEditorScene.Element;
using YouthSpice.StoryEditorScene.UI;

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

		[SerializeField, ReadOnly]
		private string chapterID;
		
		private void Start()
		{
			uiManager = GetComponent<UIManager>();
			conditionManager = GetComponent<ConditionManager>();
			elementManager = GetComponent<ElementManager>();
		}

		/// <summary>
		/// 새로운 데이터를 생성합니다.
		/// </summary>
		public void NewData()
		{
			ulong timestamp = (ulong)System.DateTimeOffset.Now.ToUnixTimeSeconds();
			float random = Random.Range(0.01f, 1.2f);
			ulong chapterId = (ulong)(timestamp * random);
			
			elementManager.RemoveAll();
			uiManager.SetChapterID(chapterId + "");
			conditionManager.Reset = true;
		}

		/// <summary>
		/// 기존 챕터 데이터를 불러옵니다.
		/// </summary>
		/// <param name="chapter">불러올 데이터를 지정합니다.</param>
		public void LoadData(Chapter chapter)
		{
			
		}
	}
}
