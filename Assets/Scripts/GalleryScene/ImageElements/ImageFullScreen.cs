using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

namespace YouthSpice.GalleryScene.ImageElements
{
	/// <summary>
	/// 이미지 전체화면 보기를 관리합니다
	/// </summary>
	public class ImageFullScreen : MonoBehaviour
	{
		[Header("Statuses")]
		[SerializeField, ReadOnly]
		private bool isRunning = false;
		
		[Header("Configs")]
		[SerializeField]
		private Color activeColor = new Color(1f, 1f, 1f, 1f);
		[SerializeField]
		private Color inactiveColor = new Color(1f, 1f, 1f, 0f);
		
		[Header("GameObjects")]
		[SerializeField]
		private Image imageObject;
		[SerializeField]
		private Image backgroundObject;

		private void Awake()
		{
			Hide();
		}

		private void Update()
		{
			if (isRunning && Input.anyKeyDown)
			{
				Hide();
			}
		}

		public void Show(Sprite sprite)
		{
			imageObject.color = activeColor;
			imageObject.sprite = sprite;
			imageObject.preserveAspect = true;
			imageObject.gameObject.SetActive(true);
			backgroundObject.gameObject.SetActive(true);
			isRunning = true;
		}

		private void Hide()
		{
			imageObject.color = inactiveColor;
			imageObject.sprite = null;
			imageObject.gameObject.SetActive(false);
			backgroundObject.gameObject.SetActive(false);
			isRunning = false;
		}
	}
}
