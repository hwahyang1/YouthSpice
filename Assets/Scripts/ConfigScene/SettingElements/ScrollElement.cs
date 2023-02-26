using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

namespace YouthSpice.ConfigScene.SettingElements
{
	/// <summary>
	/// 스크롤 항목을 묶는 부모 Class 입니다.
	/// </summary>
	public abstract class ScrollElement : Elements
	{
		[Header("Values")]
		[SerializeField]
		protected float minValue = 0f;

		[SerializeField]
		protected float maxValue = 100f;

		[SerializeField, ReadOnly]
		protected float currentValue = 0f;

		[Header("GameObjects")]
		[SerializeField]
		private Button upButton;

		[SerializeField]
		private Button downButton;

		[SerializeField]
		private Slider elementSlider;

		[SerializeField]
		private Text elementValue;

		protected virtual void Awake()
		{
			elementSlider.onValueChanged.AddListener(OnValueChanged);
			upButton.onClick.AddListener(OnUpButtonClicked);
			downButton.onClick.AddListener(OnDownButtonClicked);
		}

		/// <summary>
		/// 해당 Method는 minValue, maxValue, currentValue 변수를 기준으로 UI를 업데이트 시킵니다.
		/// 모든 작업이 완료된 후 base.Start()를 호출합니다.
		/// </summary>
		protected virtual void Start()
		{
			elementSlider.GetComponent<Slider>().minValue = minValue;
			elementSlider.GetComponent<Slider>().maxValue = maxValue;
			elementSlider.GetComponent<Slider>().value = currentValue;
		}

		/// <summary>
		/// 선택 값이 바뀌었을 때 이벤트를 처리합니다.
		/// </summary>
		/// <param name="currentValue">바뀐 값을 지정합니다.</param>
		protected virtual void OnValueChanged(float currentValue)
		{
			this.currentValue = currentValue;
			elementValue.GetComponent<Text>().text =
				(int)((currentValue - minValue) / (maxValue - minValue) * 100) + "%";
		}

		/// <summary>
		/// 버튼의 이벤트를 처리합니다.
		/// </summary>
		protected virtual void OnUpButtonClicked()
		{
			elementSlider.value += 10;
		}

		/// <summary>
		/// 버튼의 이벤트를 처리합니다.
		/// </summary>
		protected virtual void OnDownButtonClicked()
		{
			elementSlider.value -= 10;
		}
	}
}
