using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Audio;

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

		[SerializeField]
		private AudioClip clickClip;

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

		/// <summary>
		/// 이미지를 표출합니다.
		/// </summary>
		/// <param name="sprite">표출할 이미지를 지정합니다.</param>
		public void Show(Sprite sprite)
		{
			AudioManager.Instance.PlayEffectAudio(clickClip);

			imageObject.color = activeColor;
			imageObject.sprite = sprite;
			imageObject.preserveAspect = true;
			imageObject.gameObject.SetActive(true);
			backgroundObject.gameObject.SetActive(true);
			isRunning = true;
		}

		/// <summary>
		/// 이미지를 숨깁니다.
		/// </summary>
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
