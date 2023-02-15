using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Action = System.Action;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YouthSpice.StoryScene.UI
{
	/// <summary>
	/// 이름 획득 UI를 관리합니다.
	/// </summary>
	public class GetNameArea : MonoBehaviour
	{
		[SerializeField]
		private AudioClip selectClip;
		
		[SerializeField]
		private Image background;

		[SerializeField]
		private Transform parent;

		[SerializeField]
		private TMP_InputField nameInput;

		[SerializeField]
		private Button confirmButton;
		
		private readonly Regex nameRegex = new Regex(@"^[ㄱ-ㅎ|가-힣|a-z|A-Z|0-9]+$");

		private Action callback = null;

		private void Awake()
		{
			RemoveAll();
			SetActive(false);
		}

		public void Show(Action callback)
		{
			SetActive(true);
			RemoveAll();
			this.callback = callback;
		}

		private void RemoveAll()
		{
			callback = null;
			nameInput.text = "";
			confirmButton.interactable = false;
		}

		private void SetActive(bool active)
		{
			background.gameObject.SetActive(active);
			parent.gameObject.SetActive(active);
		}

		private bool CheckValidate(string text)
		{
			string textWithoutBlank = text.Trim();
			if (textWithoutBlank.Length <= 0 || text.Length > 8)
			{
				return false;
			}

			return nameRegex.IsMatch(text);
		}

		public void OnInputEdited()
		{
			confirmButton.interactable = CheckValidate(nameInput.text);
		}

		public void OnConfirmButtonClicked()
		{
			PreloadScene.Audio.AudioManager.Instance.PlayEffectAudio(selectClip);
			
			string name = nameInput.text;
			bool isValid = CheckValidate(name);
			if (!isValid) return;

			GameInfo.Instance.playerName = name;
			
			SetActive(false);
			
			callback?.Invoke();
			callback = null;
		}
	}
}
