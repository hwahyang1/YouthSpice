using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.IO;
using UnityEngine.Windows;


public class Select : MonoBehaviour
	{
		[SerializeField] private GameObject nameEnterPanel;//플레이어 닉네임 입력 ui 판
		[SerializeField] private Text newPlayerName;//새로 입력된 플레이어의 닉네임
		[SerializeField] private GameObject restartPanel;//다시 시작하겠나는 질문 판
		private bool saveFile;

		void Start()
		{
			//저장된 데이터가 존제하는지 판단
			if (File.Exists(DataManager.Instance.path))
			{
				
				saveFile = true;
				DataManager.Instance.LoadData();
			}
		}
		public void Slot()
		{
			if (!saveFile)
			{
				//저장된 데이터가 없을때 -> 데이터를 만드는 창을 생성
				NamePanelCondition();
			}
			else
			{
				//저장된 데이터가 있으면 다시 계정을 만들거냐고 물어봄 
				restartPanel.SetActive(true);
			}
			
		}

		/// <summary>
		/// 저장된 데이터가 없을때 이름을 입력하는 창을 활성화 시키는 함수
		/// </summary>
		public void NamePanelCondition()
		{
			nameEnterPanel.SetActive(true);
		}
		/// <summary>
		/// 게임 씬으로 넘어가는 함수 
		/// </summary>
		public void EnterGame()
		{
			//만약 저장된 데이터가 있다면
			if (!saveFile)
			{
				//방금 입력한 정보를 덮어씌어라
				DataManager.Instance.NowPlayer.Name = newPlayerName.text;
				DataManager.Instance.SaveData(); // 저장
			}
			//게임 씬으로 진입
			SceneManager.LoadScene("Game");
		}
		/// <summary>
		/// 전에 플레이하던 정보가있을때 삭제하고 게임 다시 시작하는 함수 
		/// </summary>
		public void ReStartGame()
		{
			DataManager.Instance.DeleteData();
			saveFile = false;
			NamePanelCondition();
		}
		/// <summary>
		/// 이어하기 버튼 함수 
		/// </summary>
		public void RelayGame()
		{
			if(saveFile)
			{
				EnterGame();
			}
			else
			{
				//캐릭터를 생성해주세요 정보 띄움
			}
		}
		public void ConditionFalse()
		{
			nameEnterPanel.SetActive(false);
			restartPanel.SetActive(false);
		}
	}
