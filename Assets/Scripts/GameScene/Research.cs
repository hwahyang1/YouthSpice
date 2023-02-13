using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.GameScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class Research : MonoBehaviour
	{
		//ui
		private int timer = 8; //왼쪽위 제한시간
		[SerializeField] private GameObject needle;
		[SerializeField] private Text timerText; //왼쪽위 타이머 텍스트
		[SerializeField] private Text researchText;//탐색 상태 텍스트
		[SerializeField] private Sprite[] researchImage;//탐색 상태 텍스트
		[SerializeField] private Image researchImageRoot;//탐색 상태 텍스트
		//탐색 기능
		private bool startTimerTime = false; //cooldown 지속 시간 (초)
		private float cooldownTimer;   //cooldown 타이머
		private bool onCooldown = false;   //cooldown 중인지 여부
		
		//값 변경
		[Header("성공 확률 (%) , 미니게임성공확률과 성공 확률의 사이가")]
		[SerializeField] private int successPercentage; //성공 확률
                                                  
		[Header("미니게임 성공 확률 (%) , 실패 확률 입니다")]
		[SerializeField] private int minigamePercentage; //미니게임성공 확률
		[Header("탐색 시간")]
		[SerializeField] private float cooldownDuration; //탐색 시간 
		
		//미니게임
		[Header("미니게임")]
		[SerializeField] private GameObject minigamePanel;
		[SerializeField] private float minigameCount = 50f;
		[SerializeField] private Slider clickSlider;
		[SerializeField] private Text sucessText;
		[SerializeField] private int sliderForce;
		
		//배경
		private RectTransform rectTransform;
		[SerializeField] private GameObject background;
		//시스템 제어
		[SerializeField] private bool isControl = true; //미니게임 할떄 다른 기능 사용 못하게 하는 전제 제어 불값
		[SerializeField] private bool isMiniGameControl = false;
		

		private void Update()
		{
			if (isControl)
			{
				Researchfunction();

				TimeLimit();
			}

			if (isMiniGameControl)
			{
				MiniGame();
			}
		}

		private void Researchfunction()
		{
			if (timer >= 0)
			{
				// spacebar가 눌렸고, cooldown이 아직 안되어있으면
				if (Input.GetKeyDown(KeyCode.Space) && !onCooldown)
				{
					researchImageRoot.gameObject.SetActive(false);
					researchText.text = "탐색중"; //탐색중 택스트
					timer--;//남은 탐색횟수 감소;
					onCooldown = true; // cooldown 중
					cooldownTimer = cooldownDuration; // cooldown 타이머 초기화
				}

				// cooldown 중이면
				if (onCooldown)
				{
					cooldownTimer -= Time.deltaTime; // 타이머 감소
					// 타이머가 0 이하면
					if (cooldownTimer <= 0f)
					{
						Randomfunction(); //랜덤 함수 실행
						onCooldown = false; // cooldown 완료
					}
				}
			}
			else
			{
				//researchText.text = "탐색 종료"; //탐색중 택스트
				researchImageRoot.sprite = researchImage[1];
			}
		}

		private void Randomfunction()
		{
			float successChance = UnityEngine.Random.Range(0f, 101f);
			if (successChance <= successPercentage)
			{
				Success();
			}
			else if(successChance > successPercentage && successChance < 100f- minigamePercentage)
			{
				Fail();
			}
			else if ( successChance >= 100f - minigamePercentage)
			{
				//researchText.text = "미니게임!";
				researchImageRoot.gameObject.SetActive(true);
				researchImageRoot.sprite = researchImage[2];
				isControl = false;
				Invoke("MiniGamePanel",3f);
			}
		}
		private void Success()
		{
			researchText.text = "탐색 성공";
			Debug.Log("성공");
		}
		private void Fail()
		{
			//researchText.text = "탐색 실패";
			researchImageRoot.gameObject.SetActive(true);
			researchImageRoot.sprite = researchImage[0];
			Debug.Log("실패");
		}

		private void TimeLimit()
		{
			if (timer >= 0)
			{
				timerText.text = "남은 탐색 횟수:" + timer;
			}
			else
			{
				timerText.text = "";
			}

			switch (timer)
			{
				case 8 :
					needle.transform.eulerAngles = new Vector3(0, 0, 71f);
					break;
				case 7 :
					needle.transform.eulerAngles = new Vector3(0, 0, 27f);
					break;
				case 6 :
					needle.transform.eulerAngles = new Vector3(0, 0, -18f);
					break;
				case 5 :
					needle.transform.eulerAngles = new Vector3(0, 0, -68f);
					break; 
				case 4 :
					needle.transform.eulerAngles = new Vector3(0, 0, -103f);
					break;
				case 3 :
					needle.transform.eulerAngles = new Vector3(0, 0, -150f);
					break;
				case 2 :
					needle.transform.eulerAngles = new Vector3(0, 0, -195f);
					break;
				case 1 :
					needle.transform.eulerAngles = new Vector3(0, 0, -245f);
					break; 
				case 0 :
					needle.transform.eulerAngles = new Vector3(0, 0, 71f);
					break;
			}
		}

		/// <summary>
		/// 미니게임 판을 true로 바꾸는 함수 
		/// </summary>
		private void MiniGamePanel()
		{
			Debug.Log("미니게임창 켜짐");
			isMiniGameControl = true;
			minigamePanel.SetActive(true);
		}
		/// <summary>
		/// 미니게임 함수
		/// </summary>
		private void MiniGame()
		{
			if (minigameCount > 100)
			{
				sucessText.text = "성공";
				minigameCount = 100;
				Invoke("MiniGamePanelFalse" , 3f);
			}
			else if(0 < minigameCount && minigameCount < 100)
			{
				minigameCount -= Time.deltaTime * sliderForce;
				clickSlider.value = minigameCount / 100;
			}
			else if(minigameCount < 0)
			{
				sucessText.text = "실패";
				Invoke("MiniGamePanelFalse" , 3f);
			}

			if (Input.anyKeyDown)
			{
				minigameCount++;
			}
		}

		private void MiniGamePanelFalse()
		{
			isControl = true;
			isMiniGameControl = false;
			minigameCount = 50;
			sucessText.text = "";
			minigamePanel.SetActive(false);
			Debug.Log("미니게임창 꺼짐");
		}
	}
}
