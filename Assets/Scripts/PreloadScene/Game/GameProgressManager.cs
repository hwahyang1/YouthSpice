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

		[SerializeField]
		public string recipeTutorial;

		[SerializeField]
		public string itemTutorial;

		[SerializeField]
		public string shopTutorial;

		[SerializeField]
		public string researchTutorial;

		private bool re = false;

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

				if (GameInfo.Instance.majorChapter == 3 && GameInfo.Instance.minorChapter == 2)
				{
					GameInfo.Instance.minorChapter += 2;
				}

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

		private IEnumerator Toggle()
		{
			yield return new WaitForSeconds(1f);
			re = true;
			RunThisChapter();
		}

		public void RunThisChapter()
		{
			int majorChapter = GameInfo.Instance.majorChapter;
			int minorChapter = GameInfo.Instance.minorChapter;

			if (!re && minorChapter == 0 && majorChapter != 0)
			{
				StartCoroutine(nameof(Toggle));
				return;
			}

			re = false;

			switch (minorChapter)
			{
				case 0:
					SceneChange.Instance.ChangeScene("BlankScene", true, true,
						() => { SceneChange.Instance.Add("StoryScene"); });
					StorySceneLoadParams.Instance.chapterID = minorChapter1StoryIds[majorChapter];
					break;
				case 1:
					GameInfo.Instance.slotName = $"탐색 {GameInfo.Instance.majorChapter}-1";
					SceneChange.Instance.ChangeScene("GameScene");
					break;
				case 2:
					SceneChange.Instance.ChangeScene("BlankScene", true, true,
						() => { SceneChange.Instance.Add("StoryScene"); });
					StorySceneLoadParams.Instance.chapterID = minorChapter2StoryIds[majorChapter];
					break;
				case 3:
					GameInfo.Instance.slotName = $"탐색 {GameInfo.Instance.majorChapter}-2";
					SceneChange.Instance.ChangeScene("GameScene");
					break;
				case 4:
					SceneChange.Instance.ChangeScene("BlankScene", true, true,
						() => { SceneChange.Instance.Add("StoryScene"); });
					StorySceneLoadParams.Instance.chapterID = minorChapter3StoryIds[majorChapter];
					break;
				case 5:
					GameInfo.Instance.slotName = $"탐색 {GameInfo.Instance.majorChapter}-3";
					SceneChange.Instance.ChangeScene("GameScene");
					break;
				case 6:
					GameInfo.Instance.slotName = $"상점 {GameInfo.Instance.majorChapter}";
					SceneChange.Instance.ChangeScene("ShopScene");
					break;
				case 7: // 8~9 포함
					GameInfo.Instance.slotName = $"레시피북 {GameInfo.Instance.majorChapter}";
					CookingLoadParams.Instance.menu = (AvailableMenus)majorChapter - 1;
					CookingLoadParams.Instance.currentCharacter = cookingGameCharacterIds[majorChapter];
					SceneChange.Instance.ChangeScene("CookingScene");
					break;
			}
		}
	}
}
