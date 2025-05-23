using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice
{
	/// <summary>
	/// Slider의 버튼을 제어합니다.
	/// </summary>
	public class ScrollButtonController : MonoBehaviour
	{
		[Header("Configs")]
		[SerializeField]
		private float changeValue = 50f;

		[Header("GameObjects")]
		[SerializeField]
		private RectTransform[] target;

		[SerializeField]
		private Button upButton;

		[SerializeField]
		private Button downButton;

		private void Start()
		{
			upButton.onClick.AddListener(OnUpButtonClicked);
			downButton.onClick.AddListener(OnDownButtonClicked);
		}

		/// <summary>
		/// 위 버튼이 클릭되었을 때의 이벤트를 처리합니다.
		/// </summary>
		private void OnUpButtonClicked()
		{
			foreach (RectTransform current in target)
			{
				Vector2 anchoredPosition = current.anchoredPosition;
				anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y - changeValue);
				current.anchoredPosition = anchoredPosition;
			}
		}

		/// <summary>
		/// 아래 버튼이 클릭되었을 때의 이벤트를 처리합니다.
		/// </summary>
		private void OnDownButtonClicked()
		{
			foreach (RectTransform current in target)
			{
				Vector2 anchoredPosition = current.anchoredPosition;
				anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + changeValue);
				current.anchoredPosition = anchoredPosition;
			}
		}
	}
}
