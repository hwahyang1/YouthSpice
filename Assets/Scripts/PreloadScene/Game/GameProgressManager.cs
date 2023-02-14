using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.CookingScene;
using YouthSpice.CookingScene.Extern;
using YouthSpice.PreloadScene.Scene;
using YouthSpice.StoryScene.Extern;

namespace YouthSpice.PreloadScene.Game
{
	/// <summary>
	/// 게임의 진행 상황을 관리합니다.
	/// </summary>
	public class GameProgressManager : Singleton<GameProgressManager>
	{
		[Tooltip("MajorChapter 순서대로 지정합니다.")]
		[SerializeField]
		private List<string> minorChapter1StoryIds;
		[Tooltip("MajorChapter 순서대로 지정합니다.")]
		[SerializeField]
		private List<string> minorChapter2StoryIds;
		[Tooltip("MajorChapter 순서대로 지정합니다.")]
		[SerializeField]
		private List<string> minorChapter3StoryIds;

		[Tooltip("MajorChapter 순서대로 지정합니다.")]
		[SerializeField]
		private List<int> cookingGameCharacterIds;
		
		public void CountUp()
		{
			// 대챕터 0 -> 바로 넘김
			if (GameInfo.Instance.majorChapter == 0)
			{
				GameInfo.Instance.majorChapter = 1;
				GameInfo.Instance.minorChapter = 0;
			}
			// 대챕터 4 -> 메인으로 돌아감
			else if (GameInfo.Instance.majorChapter == 4)
			{
				SceneChange.Instance.ChangeScene("MenuScene");
			}
			else
			{
				GameInfo.Instance.minorChapter++;
				
				// 대챕터 끝난 경우 -> 돈/인벤 리셋하고 대챕터 넘김
				if (GameInfo.Instance.minorChapter == 8)
				{
					GameInfo.Instance.inventory.Clear();
					GameInfo.Instance.money = 0;
					
					GameInfo.Instance.majorChapter++;
					GameInfo.Instance.minorChapter = 0;
				}
			}
		}
		
		public void RunThisChapter()
		{
			int majorChapter = GameInfo.Instance.majorChapter;
			int minorChapter = GameInfo.Instance.minorChapter;

			switch (minorChapter)
			{
				case 0:
					SceneChange.Instance.Add("StoryScene");
					StorySceneLoadParams.Instance.chapterID = minorChapter1StoryIds[majorChapter];
					break;
				case 1:
					SceneChange.Instance.ChangeScene("GameScene");
					break;
				case 2:
					SceneChange.Instance.Add("StoryScene");
					StorySceneLoadParams.Instance.chapterID = minorChapter2StoryIds[majorChapter];
					break;
				case 3:
					SceneChange.Instance.ChangeScene("GameScene");
					break;
				case 4:
					SceneChange.Instance.Add("StoryScene");
					StorySceneLoadParams.Instance.chapterID = minorChapter3StoryIds[majorChapter];
					break;
				case 5:
					SceneChange.Instance.ChangeScene("GameScene");
					break;
				case 6:
					SceneChange.Instance.ChangeScene("ShopScene");
					break;
				case 7: // 8~9 포함
					CookingLoadParams.Instance.menu = (AvailableMenus)majorChapter;
					CookingLoadParams.Instance.currentCharacter = cookingGameCharacterIds[majorChapter];
					SceneChange.Instance.ChangeScene("CookingScene");
					break;
			}
		}
	}
}
