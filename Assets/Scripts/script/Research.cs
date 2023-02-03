using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;


namespace YouthSpice
{
	/// <summary>
	/// Description
	/// </summary>
	public class Research : MonoBehaviour
	{
		//ui
		private float timer = 60; //왼쪽위 제한시간
		[SerializeField] private Text timerText; //왼쪽위 타이머 텍스트
		[SerializeField] private Text researchText;//탐색 상태 텍스트
		//탐색 기능
		private bool startTimerTime = false; //cooldown 지속 시간 (초)
		private float cooldownTimer;   //cooldown 타이머
		private bool onCooldown = false;   //cooldown 중인지 여부
		
		//값 변경
		[Header("성공 확률 (%)")]
		[SerializeField] private int Percentage; //성공 확률
		[Header("탐색 시간")]
		[SerializeField] private float cooldownDuration; //탐색 시간 

		private void Update()
		{
			Researchfunction();
			
			TimeLimit();
		}

		void Researchfunction()
		{
			// spacebar가 눌렸고, cooldown이 아직 안되어있으면
			if (Input.GetKeyDown(KeyCode.Space) && !onCooldown)
			{
				researchText.text = "탐색중"; //탐색중 택스트
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

		void Randomfunction()
		{
			float successChance = UnityEngine.Random.Range(0f, 101f);
			if (successChance <= Percentage)
			{
				Success();
			}
			else
			{
				Fail();
			}
		}
		void Success()
		{
			researchText.text = "탐색 성공";
			Debug.Log("성공");
		}
		void Fail()
		{
			researchText.text = "탐색 실패";
			
			Debug.Log("실패");
		}

		void TimeLimit()
		{
			timerText.text = "시간" + timer;
			if (startTimerTime)
			{
				timer -= Time.deltaTime;
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				startTimerTime = true;
			}
		}
	}
}