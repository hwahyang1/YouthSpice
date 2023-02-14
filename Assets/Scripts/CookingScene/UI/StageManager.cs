using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.CookingScene.UI
{
	/// <summary>
	/// 화면 전환을 관리합니다.
	/// </summary>
	public class StageManager : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] stages;

		private GameManager gameManager;

		private void Awake()
		{
			gameManager = GetComponent<GameManager>();
		}

		public void GoNext()
		{
			CookingFlow current = gameManager.CurrentChapter;

			for (int i = 0; i < stages.Length; i++)
			{
				stages[i].SetActive((int)current == i);
			}
		}
	}
}
