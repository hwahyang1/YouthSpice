using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.CookingScene;
using YouthSpice.CookingScene.Extern;
using YouthSpice.PreloadScene.Files;
using YouthSpice.PreloadScene.Scene;
using YouthSpice.StoryScene.Extern;

namespace YouthSpice.PreloadScene.Game
{
	/// <summary>
	/// 게임의 진행 상황을 관리합니다.
	/// </summary>
	public class GameProgressManager : Singleton<GameProgressManager>
	{
		[Header("Chapter IDs")]
		[Tooltip("MajorChapter 순서대로 지정합니다. `|`을 구분자로 사용하며, 왼쪽부터 실행을 시도합니다.")]
		[SerializeField]
		private List<string> minorChapter1StoryIds;

		[Tooltip("MajorChapter 순서대로 지정합니다. `|`을 구분자로 사용하며, 왼쪽부터 실행을 시도합니다.")]
		[SerializeField]
		private List<string> minorChapter2StoryIds;

		[Tooltip("MajorChapter 순서대로 지정합니다. `|`을 구분자로 사용하며, 왼쪽부터 실행을 시도합니다.")]
		[SerializeField]
		private List<string> minorChapter3StoryIds;

		[Tooltip("MajorChapter 순서대로 지정합니다. `|`을 구분자로 사용하며, 왼쪽부터 실행을 시도합니다.")]
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

		/// <summary>
		/// 다음 챕터로 넘어갑니다. (변수만 갱신합니다.)
		/// </summary>
		public void CountUp()
		{
			// 대챕터 0 -> 바로 넘김
			if (GameInfo.Instance.majorChapter == 0)
			{
				GameInfo.Instance.majorChapter = 1;
				GameInfo.Instance.minorChapter = 0;
			}
			else
			{
				GameInfo.Instance.minorChapter++;

				if (GameInfo.Instance.majorChapter == 3 && GameInfo.Instance.minorChapter == 4)
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

		/// <summary>
		/// 현재 챕터를 실행합니다.
		/// </summary>
		public void RunThisChapter()
		{
			int majorChapter = GameInfo.Instance.majorChapter;
			int minorChapter = GameInfo.Instance.minorChapter;

			if (!re && minorChapter == 0 && majorChapter != 0)
			{
				StartCoroutine(nameof(Toggle));
				return;
			}

			// 대챕터 4 -> 메인으로 돌아감
			if (majorChapter == 4 && minorChapter == 1)
			{
				SceneChange.Instance.ChangeScene("MenuScene", callback: () =>
				{
					StorySceneLoadParams.Instance.Exit();
					GameInfo.Instance.Exit();
				});
				return;
			}

			re = false;

			switch (minorChapter)
			{
				case 0:
					GameInfo.Instance.slotName = $"스토리 {GameInfo.Instance.majorChapter}-1";
					SceneChange.Instance.ChangeScene("BlankScene", true, true,
						() => { SceneChange.Instance.Add("StoryScene"); });
					StorySceneLoadParams.Instance.chapterID = GetChapterID();
					break;
				case 1:
					GameInfo.Instance.slotName = $"탐색 {GameInfo.Instance.majorChapter}-1";
					SceneChange.Instance.ChangeScene("GameScene");
					break;
				case 2:
					GameInfo.Instance.slotName = $"스토리 {GameInfo.Instance.majorChapter}-2";
					SceneChange.Instance.ChangeScene("BlankScene", true, true,
						() => { SceneChange.Instance.Add("StoryScene"); });
					StorySceneLoadParams.Instance.chapterID = GetChapterID();
					break;
				case 3:
					GameInfo.Instance.slotName = $"탐색 {GameInfo.Instance.majorChapter}-2";
					SceneChange.Instance.ChangeScene("GameScene");
					break;
				case 4:
					GameInfo.Instance.slotName = $"스토리 {GameInfo.Instance.majorChapter}-3";
					SceneChange.Instance.ChangeScene("BlankScene", true, true,
						() => { SceneChange.Instance.Add("StoryScene"); });
					StorySceneLoadParams.Instance.chapterID = GetChapterID();
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

		/// <summary>
		/// majorChapter, minorChapter와 호감도를 기반으로 현재 실행해야 하는 챕터 ID를 반환합니다.
		/// </summary>
		/// <returns>
		/// 현재 실행해야 하는 챕터 ID를 반환합니다.
		/// 없을 경우, 빈 string이 반환됩니다.
		/// </returns>
		public string GetChapterID()
		{
			int majorChapter = GameInfo.Instance.majorChapter;
			int minorChapter = GameInfo.Instance.minorChapter;

			string returnChapterID = "";
			string targetChapterID = "";
			
			switch (minorChapter)
			{
				case 0:
					targetChapterID = minorChapter1StoryIds[majorChapter];
					break;
				case 2:
					targetChapterID = minorChapter2StoryIds[majorChapter];
					break;
				case 4:
					targetChapterID = minorChapter3StoryIds[majorChapter];
					break;
			}

			// 값이 두개 이상 주어진 경우
			if (targetChapterID.Contains("|"))
			{
				Dictionary<string, DefineChapter> availableChapterIDs = SourceFileManager.Instance.AvailableChapters;
				string[] chapterIDs = targetChapterID.Split("|");
				foreach (string chapterID in chapterIDs)
				{
					if (!availableChapterIDs.ContainsKey(chapterID)) continue;
					
					DefineChapter chapterInfo = availableChapterIDs[chapterID];
					
					// 조건 없는 경우 -> 현재 챕터로 확정
					if (chapterInfo.ConditionType.Length == 0)
					{
						returnChapterID = chapterID;
						break;
					}
					
					// 조건 있는 경우 -> 검사 후 충족하면 현재 챕터로 확정
					for (int i = 0; i < chapterInfo.ConditionType.Length; i++)
					{
						ChapterCondition conditionKey = chapterInfo.ConditionType[i];
						string conditionValue = chapterInfo.ConditionData[i];
						
						// 호감도를 제외한 나머지 값은 무시함.
						switch (conditionKey)
						{
							case ChapterCondition.MinPlayer1Friendship:
								if (int.Parse(conditionValue) <= GameInfo.Instance.friendship[0])
								{
									return chapterID;
								}
								break;
							case ChapterCondition.MinPlayer2Friendship:
								if (int.Parse(conditionValue) <= GameInfo.Instance.friendship[1])
								{
									return chapterID;
								}
								break;
							case ChapterCondition.MinPlayer3Friendship:
								if (int.Parse(conditionValue) <= GameInfo.Instance.friendship[2])
								{
									return chapterID;
								}
								break;
						}
					}
				}
			}
			else
			{
				// 값이 하나만 주어진 경우 -> 현재 챕터로 확정
				returnChapterID = targetChapterID;
			}
			
			return returnChapterID;
		}
	}
}
