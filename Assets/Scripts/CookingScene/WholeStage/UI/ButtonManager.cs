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

		/// <summary>
		/// 다음 버튼의 활성화 여부를 지정합니다.
		/// </summary>
		/// <param name="active">활성화 여부를 지정합니다.</param>
		public void SetButtonActive(bool active)
		{
			nextButton.interactable = active;
			nextButtonImageArea.sprite = buttonImages[active ? 1 : 0];
			nextButtonTextImageArea.color = buttonTextColors[active ? 1 : 0];
		}

		/// <summary>
		/// 버튼의 이미지를 지정합니다.
		/// </summary>
		/// <param name="index">버튼의 이미지 Index를 지정합니다.</param>
		public void SetButtonText(int index)
		{
			nextButtonTextImageArea.sprite = buttonTextImages[index];
		}
	}
}
