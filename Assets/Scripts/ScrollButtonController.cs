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
		private RectTransform target;
		[SerializeField]
		private Button upButton;
		[SerializeField]
		private Button downButton;

		private void Start()
		{
			upButton.onClick.AddListener(OnUpButtonClicked);
			downButton.onClick.AddListener(OnDownButtonClicked);
		}

		private void OnUpButtonClicked()
		{
			Vector2 anchoredPosition = target.anchoredPosition;
			anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y - changeValue);
			target.anchoredPosition = anchoredPosition;
		}

		private void OnDownButtonClicked()
		{
			Vector2 anchoredPosition = target.anchoredPosition;
			anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y + changeValue);
			target.anchoredPosition = anchoredPosition;
		}
	}
}
