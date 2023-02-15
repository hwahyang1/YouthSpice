using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using YouthSpice.PreloadScene.Audio;

namespace YouthSpice.ConfigScene.UI
{
	/// <summary>
	/// 탭 이동을 관리합니다.
	/// </summary>
	public class TabManager : MonoBehaviour
	{
		[SerializeField]
		private AudioClip tabClip;
		
		[Header("ButtonImages")]
		[SerializeField]
		private Sprite[] buttonImages;
		
		[Header("Object")]
		[SerializeField]
		private Button[] tabButtons;

		[SerializeField]
		private GameObject[] tabPanels;

		private int currentSelection = 0;

		private void Awake()
		{
			if (tabButtons.Length != tabPanels.Length) Debug.LogWarning("WARN:: 'Tab Buttons' and 'Tab Panels' Count doesn't match. might cause error.");
			MoveTab(0);
		}

		public void MoveTab(int to)
		{
			AudioManager.Instance.PlayEffectAudio(tabClip);
			
			currentSelection = to;
			int buttonImageIndex = 1; // 0번은 Active에 사용됨
			for (int i = 0; i < tabButtons.Length; i++)
			{
				tabButtons[i].GetComponent<Image>().sprite = i == currentSelection ? buttonImages[0] : buttonImages[buttonImageIndex++];
				tabButtons[i].enabled = i != currentSelection;
				tabPanels[i].SetActive(i == currentSelection);
			}
		}
	}
}
