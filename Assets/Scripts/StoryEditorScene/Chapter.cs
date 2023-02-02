using System;
using System.Collections;
using System.Collections.Generic;

namespace YouthSpice.StoryEditorScene
{
	/// <summary>
	/// 챕터 조건을 정의합니다.
	/// </summary>
	[System.Serializable]
	public enum ChapterCondition
	{
		None,
		FormerChapter,
		MinDay,
		MinPlayer1Friendship,
		MinPlayer2Friendship,
		MinPlayer3Friendship
	}

	/// <summary>
	/// 챕터 항목을 정의합니다.
	/// </summary>
	[System.Serializable]
	public enum ChapterElementType
	{
		Speech,
		DayImage,
		BackgroundImage,
		BackgroundMusic,
		EffectSound,
		StandingImage,
		Friendship,
		Condition,
		ConditionTitle
	}

	/// <summary>
	/// 챕터 항목을 정의합니다.
	/// </summary>
	[System.Serializable]
	public class ChapterElement
	{
		public ChapterElementType Type;
		public Dictionary<string, string> Data;

		public ChapterElement(ChapterElementType type, Dictionary<string, string> data)
		{
			Type = type;
			Data = data;
		}
	}

	/// <summary>
	/// 챕터 프로젝트 파일의 정보를 담습니다.
	/// </summary>
	[System.Serializable]
	public class Chapter
	{
		public string ID;
		public string Name;

		public ChapterCondition[] ConditionType;
		public string[] ConditionData;

		public ChapterElement[] Elements;

		public Chapter()
		{
			//
		}

		public Chapter(
			string id,
			string name,
			ChapterCondition[] conditionType,
			string[] conditionData,
			ChapterElement[] elements
		)
		{
			ID = id;
			Name = name;

			ConditionType = conditionType;
			ConditionData = conditionData;

			Elements = elements;
		}
	}
}
