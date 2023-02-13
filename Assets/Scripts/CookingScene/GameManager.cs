using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

using YouthSpice.CookingScene.RecipeStage;
using YouthSpice.CookingScene.ResultStage.UI;
using YouthSpice.CookingScene.UI;
using YouthSpice.CookingScene.WholeStage;
using YouthSpice.CookingScene.WholeStage.UI;

namespace YouthSpice.CookingScene
{
	/// <summary>
	/// 게임의 전반적인 실행을 관리합니다.
	/// </summary>
	public class GameManager : MonoBehaviour
	{
		[Header("Status")]
		[SerializeField, ReadOnly]
		private CookingFlow currentChapter;
		public CookingFlow CurrentChapter => currentChapter;

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

		private void Start()
		{
			stageManager = GetComponent<StageManager>();
			
			Set();
		}

		public void GoNext()
		{
			if (currentChapter == CookingFlow.Recipe && !recipeManager.IsEnded)
			{
				recipeManager.GoNext();
				return;
			}
			
			currentChapter = (CookingFlow)((int)currentChapter + 1);
			
			Set();
		}

		private void Set()
		{
			buttonManager.SetButtonActive(false);
			buttonManager.SetButtonText((int)currentChapter);

			if (currentChapter == CookingFlow.Result)
			{
				bool success = generateFood.TryGenerate();
				uiManager.Set(success);
				uiAnimator.Run();
			}
			else
			{
				stageManager.GoNext();
				selectionManager.GoNext();
			}
		}
	}
}
