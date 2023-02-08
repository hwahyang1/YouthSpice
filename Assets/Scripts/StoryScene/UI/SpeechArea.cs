using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

using YouthSpice.StoryScene.Chapter;

namespace YouthSpice.StoryScene.UI
{
	/// <summary>
	/// 대사 구역을 관리합니다.
	/// </summary>
	public class SpeechArea : MonoBehaviour
	{
		[SerializeField]
		private GameObject parent;
		
		[SerializeField]
		private Text nameArea;

		[SerializeField]
		private Text speechArea;

		[Header("Status")]
		[SerializeField, ReadOnly]
		private bool isPrinting = false;

		[SerializeField, ReadOnly]
		private bool isRunning = false;

		[SerializeField, ReadOnly]
		private string fullScript = "";

		[Header("Time")]
		[SerializeField, Range(0.01f, 1f), Tooltip("값이 높을수록 빨라집니다.")]
		private float speechSpeed = 0.95f;

		private ChapterManager chapterManager;

		private void Start()
		{
			chapterManager = GetComponent<ChapterManager>();
		}

		public void SetBlank()
		{
			nameArea.text = "";
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
					StopCoroutine(nameof(ShowSpeechCoroutine));
					speechArea.text = fullScript;
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
			string characterName = "";
			switch (data["Character"])
			{
				case "0":
					characterName = "";
					break;
				case "1":
					characterName = "???";
					break;
				case "2":
					characterName = "(플레이어 이름)";
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
			}

			//TODO
			fullScript = data["Script"].Replace("{Player}", "(플레이어 이름)");
			
			StartCoroutine(nameof(ShowSpeechCoroutine), characterName);
		}

		private IEnumerator ShowSpeechCoroutine(string characterName)
		{
			WaitForSeconds timeout = new WaitForSeconds(1f - speechSpeed);
			
			nameArea.text = characterName;
			speechArea.text = "";
			
			yield return null;
			
			isRunning = true;
			isPrinting = true;

			for (int i = 0; i < fullScript.Length; i++)
			{
				if (fullScript[i] == ' ') // 공백은 한번에 한해 제함
				{
					speechArea.text += fullScript[i];
					continue;
				}
				speechArea.text += fullScript[i];
				
				yield return timeout;
			}

			isPrinting = false;
			isRunning = false;
			chapterManager.isSpeechEnded = true;
		}
	}
}
