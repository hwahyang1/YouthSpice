using System.Collections;
using System.Collections.Generic;
using ActionSprite = System.Action<UnityEngine.Sprite>;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

namespace YouthSpice.GalleryScene.ImageElements
{
	/// <summary>
	/// 개별 이미지 항목을 정의합니다.
	/// </summary>
	public class ImageElement : MonoBehaviour
	{
		[SerializeField]
		private Button[] buttons;

		[SerializeField]
		private Image image;

		private Sprite sprite;

		[SerializeField, ReadOnly]
		private bool isUnlocked;

		private ActionSprite clickedCallback;

		private void Start()
		{
			foreach (Button button in buttons)
			{
				button.onClick.AddListener(OnButtonClicked);
			}
		}

		/// <summary>
		/// 이미지를 초기화 합니다.
		/// </summary>
		/// <param name="sprite">이미지를 지정합니다.</param>
		/// <param name="isUnlocked">이미지의 해금 여부를 지정합니다.</param>
		/// <param name="callback">이미지가 클릭 되었을 때 callback을 지정합니다. 없을 경우, null을 지정합니다.</param>
		public void Init(Sprite sprite, bool isUnlocked, ActionSprite callback = null)
		{
			this.sprite = sprite;
			foreach (Button button in buttons)
			{
				button.interactable = isUnlocked;
			}

			image.gameObject.SetActive(isUnlocked);
			if (isUnlocked)
			{
				image.sprite = sprite;
				image.preserveAspect = true;
			}

			clickedCallback = callback;
		}

		/// <summary>
		/// 이미지가 클릭 되었을 때의 이벤트를 처리합니다.
		/// </summary>
		private void OnButtonClicked()
		{
			clickedCallback?.Invoke(sprite);
		}
	}
}
