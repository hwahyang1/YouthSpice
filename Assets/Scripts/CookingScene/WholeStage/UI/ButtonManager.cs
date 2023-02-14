using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.CookingScene.WholeStage.UI
{
	/// <summary>
	/// 버튼의 실행을 관리합니다.
	/// </summary>
	public class ButtonManager : MonoBehaviour
	{
		[Header("Resources")]
		[Tooltip("비활성화, 활성화 순으로 지정합니다.")]
		[SerializeField]
		private Sprite[] buttonImages;
		[SerializeField]
		private Sprite[] buttonTextImages;

		[Header("Colors")]
		[Tooltip("비활성화, 활성화 순으로 지정합니다.")]
		[SerializeField]
		private Color[] buttonTextColors;

		[Header("GameObjects")]
		[SerializeField]
		private Button nextButton;
		[SerializeField]
		private Image nextButtonImageArea;
		[SerializeField]
		private Image nextButtonTextImageArea;

		private void Start()
		{
			SetButtonActive(false);
			SetButtonText(0);
		}

		public void SetButtonActive(bool active)
		{
			nextButton.interactable = active;
			nextButtonImageArea.sprite = buttonImages[active ? 1 : 0];
			nextButtonTextImageArea.color = buttonTextColors[active ? 1 : 0];
		}

		public void SetButtonText(int index)
		{
			nextButtonTextImageArea.sprite = buttonTextImages[index];
		}
	}
}
