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
		MinDate,
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
		public string Data;

		public ChapterElement(ChapterElementType type, string data)
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

		public ChapterCondition[] ConditionType;
		public int[] ConditionData;

		public ChapterElement[] Elements;

		public Chapter(string id, ChapterCondition[] conditionType, int[] conditionData, ChapterElement[] elements)
		{
			ID = id;

			ConditionType = conditionType;
			ConditionData = conditionData;

			Elements = elements;
		}
	}
}
