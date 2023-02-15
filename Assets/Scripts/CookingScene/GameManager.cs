using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;
using UnityEngine.SceneManagement;
using YouthSpice.CookingScene.Extern;
using YouthSpice.CookingScene.RecipeStage;
using YouthSpice.CookingScene.ResultStage.UI;
using YouthSpice.CookingScene.UI;
using YouthSpice.CookingScene.WholeStage;
using YouthSpice.CookingScene.WholeStage.UI;
using YouthSpice.InGameMenuScene;
using YouthSpice.PreloadScene.Audio;
using YouthSpice.PreloadScene.Files;
using YouthSpice.PreloadScene.Game;
using YouthSpice.PreloadScene.Scene;
using YouthSpice.StoryScene.Extern;

namespace YouthSpice.CookingScene
{
	/// <summary>
	/// 게임의 전반적인 실행을 관리합니다.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		[SerializeField]
		private AudioClip backgroundClip;
		[SerializeField]
		private AudioClip startClip;
		[SerializeField]
		private AudioClip nextClip;
		
		[Header("Status")]
		[SerializeField, ReadOnly]
		private CookingFlow currentChapter;
		public CookingFlow CurrentChapter => currentChapter;
		[SerializeField, ReadOnly]
		private bool isFirstRecipe = true;
		[SerializeField, ReadOnly]
		private bool isFirstResult = true;

		[ReadOnly]
		public bool ended = false;

		[Header("Classes")]
		[SerializeField]
		private SelectionManager selectionManager;
		[SerializeField]
		private ButtonManager buttonManager;
		[SerializeField]
		private RecipeManager recipeManager;
		[SerializeField]
		private GenerateFood generateFood;
		[SerializeField]
		private UIManager uiManager;
		[SerializeField]
		private UIAnimator uiAnimator;

		private StageManager stageManager;
		private bool runed = false;

		private void Start()
		{
			stageManager = GetComponent<StageManager>();
			
			AudioManager.Instance.PlayBackgroundAudio(backgroundClip);
			
			Set();
		}

		private void Update()
		{
			// 다른 창 열렸을 때 입력되는 현상 방지
			if (SceneManager.sceneCount == 1 && currentChapter != CookingFlow.Result)
			{
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

			// 나가기
			if (ended)
			{
				if (Input.anyKeyDown) Exit();
			}
		}

		public void GoNext()
		{
			if (currentChapter == CookingFlow.Recipe && !recipeManager.IsEnded)
			{
				AudioManager.Instance.PlayEffectAudio(nextClip);
				recipeManager.GoNext();
				return;
			}

			if (currentChapter == CookingFlow.Selection)
			{
				AudioManager.Instance.PlayEffectAudio(startClip);
			}
			
			currentChapter = (CookingFlow)((int)currentChapter + 1);
			
			Set();
		}

		private void Set()
		{
			buttonManager.SetButtonActive(false);
			buttonManager.SetButtonText((int)currentChapter);

			// 튜토리얼
			switch (currentChapter)
			{
				case CookingFlow.Selection:
					if (GameInfo.Instance.viewedItem) return;
					StorySceneLoadParams.Instance.isTutorialScene = true;
					StorySceneLoadParams.Instance.chapterID = GameProgressManager.Instance.itemTutorial;
					SceneChange.Instance.Add("StoryScene_Tutorial");
					GameInfo.Instance.viewedItem = true;
					break;
				case CookingFlow.Recipe:
					if (GameInfo.Instance.viewedRecipe) return;
					StorySceneLoadParams.Instance.isTutorialScene = true;
					StorySceneLoadParams.Instance.chapterID = GameProgressManager.Instance.recipeTutorial;
					SceneChange.Instance.Add("StoryScene_Tutorial");
					GameInfo.Instance.viewedRecipe = true;
					break;
				case CookingFlow.Result:
					break;
			}

			// 처리
			if (currentChapter == CookingFlow.Result)
			{
				if (isFirstResult)
				{
					AudioManager.Instance.StopBackgroundAudio();
					uiAnimator.First(Set);
					isFirstResult = false;
				}
				else
				{
					stageManager.GoNext();
					
					bool success = generateFood.TryGenerate();

					if (success)
					{
						DefineUnlockedCGs unlockedCGs = UnlockedCGsManager.Instance.GetAllData();
						unlockedCGs.recipeFoods.Add((int)CookingLoadParams.Instance.menu);
						UnlockedCGsManager.Instance.Save(unlockedCGs);
					}
					
					uiManager.Set(success);
					uiAnimator.Run();
				}
			}
			else
			{
				if (currentChapter == CookingFlow.Recipe && isFirstRecipe)
				{
					recipeManager.Set();
					isFirstRecipe = false;
				}
				stageManager.GoNext();
				selectionManager.GoNext();
			}
		}

		private void Exit()
		{
			if (runed) return;
			runed = true;
			CookingLoadParams.Instance.Exit();
			GameProgressManager.Instance.CountUp();
			GameProgressManager.Instance.RunThisChapter();
		}
	}
}
