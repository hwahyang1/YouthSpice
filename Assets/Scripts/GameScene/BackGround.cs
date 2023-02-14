using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice
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
			if (isGrow)
			{
				width += Time.deltaTime * growthSpeed * 2f;
				height += Time.deltaTime * growthSpeed;
			}
			
			rectTransform = GetComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(1920 + width, 1080 + height);
		}
	}
}
