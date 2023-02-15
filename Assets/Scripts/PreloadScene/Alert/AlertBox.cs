using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;
using YouthSpice.PreloadScene.Audio;

namespace YouthSpice.PreloadScene.Alert
{
	/// <summary>
	/// 생성된 알림창을 관리합니다.
	/// </summary>
	public class AlertBox : MonoBehaviour
	{
		[SerializeField]
		private AudioClip clickClip;
		
		[Header("GameObject")]
		[SerializeField]
		private GameObject screenCover;

		[SerializeField]
		private Text titleText;

		[SerializeField]
		private Text descriptionText;

		[SerializeField]
		private Transform buttonsParent;

		[SerializeField]
		private Button closeButton;

		[Header("Info")]
		[SerializeField, ReadOnly]
		private AlertType alertType;

		public AlertType AlertType => alertType;

		private Action[] callback;
		private Action onDestroy;

		private void Awake()
		{
			for (int i = 0; i < buttonsParent.childCount; i++)
			{
				buttonsParent.GetChild(i).gameObject.SetActive(false);
			}

			screenCover.SetActive(false);
			gameObject.SetActive(false);
		}

		public void Init(
			AlertType alertType,
			string title,
			string description,
			Dictionary<string, Action> buttons,
			bool useCloseButton,
			Action onDestroy
		)
		{
			this.alertType = alertType;
			titleText.text = title;
			descriptionText.text = description;
			callback = new Action[(int)alertType];
			closeButton.gameObject.SetActive(useCloseButton);
			this.onDestroy = onDestroy;

			int i = 0;
			GameObject targetButtonGroup = buttonsParent.GetChild((int)alertType).gameObject;
			foreach (KeyValuePair<string, Action> data in buttons)
			{
				callback[i] = data.Value;
				GameObject targetButton = targetButtonGroup.transform.GetChild(i).gameObject;
				targetButton.transform.GetChild(1).GetComponent<Text>().text = data.Key;
				i++;
			}

			targetButtonGroup.SetActive(true);
		}

		public void Show()
		{
			gameObject.SetActive(true);
			screenCover.SetActive(true);
		}

		public void OnButtonClicked(int index)
		{
			AudioManager.Instance.PlayEffectAudio(clickClip);
			
			if (alertType == AlertType.None) return;
			if ((int)alertType < index)
			{
				Debug.LogError("Not Allowed Callback");
				return;
			}

			callback[index]?.Invoke();
			onDestroy();
		}

		public void OnCloseButtonClicked()
		{
			onDestroy();
		}
	}
}
