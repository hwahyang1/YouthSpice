using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.GameScene
{
	/// <summary>
	/// 알림창의 각 항목을 관리합니다.
	/// </summary>
	public class AlertElement : MonoBehaviour
	{
		[SerializeField]
		private Image itemImageArea;

		[SerializeField]
		private Text itemNameArea;

		[SerializeField]
		private Text itemCountArea;

		public void Init(Sprite sprite, string name, int count)
		{
			itemImageArea.sprite = sprite;
			itemNameArea.text = name;
			itemCountArea.text = $"x  {count.ToString("00")}";
		}
	}
}
