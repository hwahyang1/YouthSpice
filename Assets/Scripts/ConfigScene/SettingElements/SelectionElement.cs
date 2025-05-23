using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Audio;

namespace YouthSpice.ConfigScene.SettingElements
{
	/// <summary>
	/// 선택형 항목을 묶는 부모 Class 입니다.
	/// </summary>
	public abstract class SelectionElement : Elements
	{
		[SerializeField]
		private AudioClip selectClip;

		[Header("Images")]
		[Tooltip("선택되지 않은 경우, 선택된 경우 순으로 지정합니다.")]
		[SerializeField]
		protected Sprite[] buttonImages;

		[Header("Values")]
		[SerializeField, ReadOnly]
		protected bool currentSelection = true;

		[Header("GameObjects")]
		[SerializeField]
		private Button onButton;

		[SerializeField]
		private Button offButton;

		protected virtual void Awake()
		{
			onButton.onClick.AddListener(OnOnButtonClicked);
			offButton.onClick.AddListener(OnOffButtonClicked);
		}

		/// <summary>
		/// 해당 Method는 currentSelection 변수를 기준으로 UI를 업데이트 시킵니다.
		/// 모든 작업이 완료된 후 base.Start()를 호출합니다.
		/// </summary>
		protected virtual void Start()
		{
			UpdateUI();
		}

		/// <summary>
		/// UI를 갱신합니다.
		/// </summary>
		protected virtual void UpdateUI()
		{
			onButton.interactable = !currentSelection;
			onButton.GetComponent<Image>().sprite = buttonImages[currentSelection ? 1 : 0];

			offButton.interactable = currentSelection;
			offButton.GetComponent<Image>().sprite = buttonImages[currentSelection ? 0 : 1];
		}

		/// <summary>
		/// 값이 변경되었을 때의 이벤트를 처리합니다.
		/// </summary>
		protected abstract void OnValueChanged();

		/// <summary>
		/// On 버튼이 클릭되었을 때의 이벤트를 처리합니다.
		/// </summary>
		protected virtual void OnOnButtonClicked()
		{
			AudioManager.Instance.PlayEffectAudio(selectClip);

			currentSelection = true;
			OnValueChanged();
			UpdateUI();
		}

		/// <summary>
		/// Off 버튼이 클릭되었을 때의 이벤트를 처리합니다.
		/// </summary>
		protected virtual void OnOffButtonClicked()
		{
			AudioManager.Instance.PlayEffectAudio(selectClip);

			currentSelection = false;
			OnValueChanged();
			UpdateUI();
		}
	}
}
