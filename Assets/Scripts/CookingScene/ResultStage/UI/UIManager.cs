using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using YouthSpice.CookingScene.Extern;

namespace YouthSpice.CookingScene.ResultStage.UI
{
	/// <summary>
	/// 전반적인 UI를 관리합니다.
	/// </summary>
	public class UIManager : MonoBehaviour
	{
		[Header("Resources")]
		[Tooltip("AvailableMenus 순서대로 지정합니다.")]
		[SerializeField]
		private Sprite[] menuTitleImages;

		[SerializeField]
		[Tooltip("캐릭터 순서대로 지정합니다. 없는 경우, 비웁니다.")]
		private Sprite[] successSpeechTextImages;

		[SerializeField]
		[Tooltip("캐릭터 순서대로 지정합니다. 없는 경우, 비웁니다.")]
		private Sprite[] failSpeechTextImages;

		[Header("GameObjects")]
		[SerializeField]
		private Image menuTitleImageArea;

		[SerializeField]
		private Image speechTextImageArea;

		/// <summary>
		/// 음식명과 결과 텍스트를 설정합니다.
		/// </summary>
		/// <param name="success">음식의 성공 여부를 지정합니다.</param>
		public void Set(bool success)
		{
			menuTitleImageArea.sprite = menuTitleImages[(int)CookingLoadParams.Instance.menu];
			speechTextImageArea.sprite = success
				? successSpeechTextImages[CookingLoadParams.Instance.currentCharacter]
				: failSpeechTextImages[CookingLoadParams.Instance.currentCharacter];
		}
	}
}
