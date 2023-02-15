using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Config;

namespace YouthSpice.GameScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class BackGround : MonoBehaviour
	{
		//배경
		private RectTransform rectTransform;
		public float width;
		public float height;
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
