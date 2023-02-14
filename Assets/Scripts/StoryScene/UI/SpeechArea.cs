using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;
using YouthSpice.PreloadScene.Config;
using YouthSpice.StoryScene.Chapter;

namespace YouthSpice.StoryScene.UI
{
	/// <summary>
	/// 대사 구역을 관리합니다.
	/// </summary>
	public class SpeechArea : MonoBehaviour
	{
		[Header("Sprites")]
		[SerializeField]
		private Sprite[] nameTagImages;

		[SerializeField]
		private Sprite[] scriptEndedImages;

		[Header("GameObjects")]
		[SerializeField]
		private GameObject parent;

		[SerializeField]
		private Image nameArea;

		[SerializeField]
		private Text speechArea;

		[SerializeField]
		private Image scriptEndedArea;

		[Header("Status")]
		[SerializeField, ReadOnly]
		private bool isPrinting = false;

		[SerializeField, ReadOnly]
		private bool isRunning = false;

		[SerializeField, ReadOnly]
		private string fullScript = "";

		private ChapterManager chapterManager;

		private Coroutine activeCoroutine = null;

		private void Start()
		{
			chapterManager = GetComponent<ChapterManager>();
			nameArea.sprite = null;
			nameArea.color = new Color(1f, 1f, 1f, 0f);
		}

		public void SetBlank()
		{
			nameArea.sprite = null;
			nameArea.color = new Color(1f, 1f, 1f, 0f);
			scriptEndedArea.sprite = null;
			scriptEndedArea.color = new Color(1f, 1f, 1f, 0f);
			speechArea.text = "";
		}

		public void SetActive(bool active)
		{
			parent.SetActive(active);
		}

		public void OnKeyDown()
		{
			if (isRunning)
			{
				if (isPrinting)
				{
					if (activeCoroutine != null)
					{
						StopCoroutine(activeCoroutine);
						activeCoroutine = null;
					}
					speechArea.text = fullScript;
					scriptEndedArea.color = new Color(1f, 1f, 1f, 1f);
					isPrinting = false;
				}
				else
				{
					isRunning = false;
					chapterManager.isSpeechEnded = true;
					chapterManager.PlayNext();
				}
			}
		}

		public void ShowSpeech(Dictionary<string, string> data)
		{
			Sprite characterName = nameTagImages[int.Parse(data["Character"])];
			scriptEndedArea.sprite = scriptEndedImages[int.Parse(data["Character"])];
			/*switch (data["Character"])
			{
				case "0":
					characterName = "";
					break;
				case "1":
					characterName = "???";
					break;
				case "2":
					characterName = GameInfo.Instance.playerName;
					break;
				case "3":
					characterName = "류한나";
					break;
				case "4":
					characterName = "소여울";
					break;
				case "5":
					characterName = "태은호";
					break;
				case "6":
					characterName = "";
					break;
			}*/

			fullScript = data["Script"].Replace("{Player}", GameInfo.Instance.playerName);

			activeCoroutine = StartCoroutine(ShowSpeechCoroutine(characterName));
		}

		private IEnumerator ShowSpeechCoroutine(Sprite characterName)
		{
			WaitForSeconds timeout = new WaitForSeconds(1f - ConfigManager.Instance.GetConfig().typingSpeed);

			nameArea.sprite = characterName;
			if (characterName != null)
			{
				nameArea.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				nameArea.color = new Color(1f, 1f, 1f, 0f);
			}
			speechArea.text = "";
			scriptEndedArea.color = new Color(1f, 1f, 1f, 0f);
			
			yield return null;

			isRunning = true;
			isPrinting = true;

			if (ConfigManager.Instance.GetConfig().useTypingEffect)
			{
				foreach (char currentCharacter in fullScript)
				{
					if (currentCharacter == ' ') // 공백은 한번에 한해 제함
					{
						speechArea.text += currentCharacter;
						continue;
					}

					speechArea.text += currentCharacter;

					yield return timeout;
				}
			}

			speechArea.text = fullScript;

			scriptEndedArea.color = new Color(1f, 1f, 1f, 1f);

			isPrinting = false;
			isRunning = false;
			chapterManager.isSpeechEnded = true;
		}
	}
}
