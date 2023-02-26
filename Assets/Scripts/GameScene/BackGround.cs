using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Config;

namespace YouthSpice.GameScene
{
	/// <summary>
	/// 배경 애니메이션을 처리합니다.
	/// </summary>
	public class BackGround : MonoBehaviour
	{
		private RectTransform rectTransform;

		[ReadOnly]
		public float width;

		[ReadOnly]
		public float height;

		[ReadOnly]
		public bool isGrow;

		[SerializeField]
		private float growthSpeed;

		private void Update()
		{
			if (isGrow && ConfigManager.Instance.GetConfig().useResearchEffect)
			{
				width += Time.deltaTime * growthSpeed * 2f;
				height += Time.deltaTime * growthSpeed;
			}

			rectTransform = GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(1920 + width, 1080 + height);
		}
	}
}
