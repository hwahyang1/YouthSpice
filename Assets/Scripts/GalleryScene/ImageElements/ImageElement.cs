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

		private void OnButtonClicked()
		{
			clickedCallback?.Invoke(sprite);
		}
	}
}
