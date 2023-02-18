using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using NaughtyAttributes;

using YouthSpice.InGameMenuScene;
using YouthSpice.PreloadScene.Audio;
using YouthSpice.PreloadScene.Files;
using YouthSpice.PreloadScene.Game;
using YouthSpice.PreloadScene.Item;
using YouthSpice.PreloadScene.Scene;

namespace YouthSpice.GameScene
{
	/// <summary>
	/// 탐색 게임을 관리합니다.
	/// </summary>
	public class Research : MonoBehaviour
	{
		[SerializeField]
		private BackGround backGround;
		[SerializeField]
		private Map map;

		// audio
		[SerializeField]
		private AudioClip researchClip;

		[SerializeField]
		private AudioClip lowRankClip;

		[SerializeField]
		private AudioClip midHighRankClip;

		[SerializeField]
		private AudioClip midHighRankBackgroundClip;

		//ui
		[SerializeField, ReadOnly]
		private int timer = 8; //왼쪽위 제한시간

		[SerializeField]
		private GameObject needle;

		[SerializeField]
		private Sprite[] researchImage; //탐색 상태 이미지

		[SerializeField]
		private Image researchImageRoot; //탐색 이미지 변화 위치 

		//탐색 기능
		private bool startTimerTime = false; //cooldown 지속 시간 (초)
		private float cooldownTimer; //cooldown 타이머

		[SerializeField]
		private bool onCooldown = false; //cooldown 중인지 여부

		//값 변경
		[Header("성공 확률 (%) , 미니게임성공확률과 성공 확률의 사이가")]
		[SerializeField]
		private int successPercentage; //성공 확률

		[Header("미니게임 성공 확률 (%) , 실패 확률 입니다")]
		[SerializeField]
		private int minigamePercentage; //미니게임성공 확률

		[Header("탐색 시간")]
		[SerializeField]
		private float cooldownDuration; //탐색 시간 

		//미니게임
		[Header("미니게임")]
		[SerializeField]
		private GameObject minigamePanel;

		[SerializeField]
		private float minigameCount = 50f;

		[SerializeField]
		private Slider clickSlider;

		[SerializeField]
		private int sliderForce;

		//시스템 제어
		[SerializeField]
		private bool isControl = true; //미니게임 할떄 다른 기능 사용 못하게 하는 전제 제어 불값

		[SerializeField]
		private bool isMiniGameControl = false;

		//아이템 불러오는 리스트 
		private List<ItemProperty> items;

		[SerializeField]
		private GameObject getItemPanel;

		[SerializeField]
		private Image getItemImage;

		[ReadOnly]
		public bool isOcean;
		[ReadOnly]
		public bool isGround;
		[ReadOnly]
		public bool isMountain;

		[SerializeField]
		private Sprite[] getResearchImage;

		private bool canMinigameGetItem = true;

		private bool runOnce = false;

		private void Start()
		{
			items = ItemBuffer.Instance.items;
		}

		private void Update()
		{
			// 다른 창 열렸을 때 입력되는 현상 방지
			if (SceneManager.sceneCount < 3 && !isMiniGameControl && isControl && !onCooldown && timer != -1)
			{
				if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.F1))
				{
					if (SceneManager.sceneCount == 3)
					{
						//SceneChange.Instance.Unload("InGameMenuScene");
						GameObject.FindObjectOfType<MenuManager>().Exit();
					}
					else
					{
						SceneChange.Instance.Add("InGameMenuScene");
					}
				}
			}

			if (isControl)
			{
				ResearchFunction();

				TimeLimit();
			}

			if (isMiniGameControl)
			{
				MiniGame();
			}

			if (timer == -1)
			{
				researchImageRoot.sprite = researchImage[1];
				researchImageRoot.gameObject.SetActive(true);
				backGround.isGrow = false;
				if (!runOnce)
				{
					StartCoroutine(ExitCoroutine());
					runOnce = true;
				}
			}
		}

		private IEnumerator ExitCoroutine()
		{
			yield return new WaitForSeconds(1.5f);
			GameProgressManager.Instance.CountUp();
			GameProgressManager.Instance.RunThisChapter();
		}

		/// <summary>
		/// 탐색을 시작합니다.
		/// </summary>
		private void ResearchFunction()
		{
			// 창 열려 있으면 무시
			if (SceneManager.sceneCount != 1) return;

			if (timer >= 0)
			{
				// spacebar가 눌렸고, cooldown이 아직 안되어있으면
				if (Input.GetKeyDown(KeyCode.Space) && !onCooldown)
				{
					AudioManager.Instance.PlayEffectAudio(researchClip);

					map.mapSelectBtn.gameObject.SetActive(false);
					getItemPanel.gameObject.SetActive(false);
					backGround.isGrow = true;
					researchImageRoot.gameObject.SetActive(false);
					timer--; //남은 탐색횟수 감소;
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
						RandomFunction(); //랜덤 함수 실행
						onCooldown = false; // cooldown 완료
					}
				}
			}
		}

		/// <summary>
		/// 무작위로 성공/미니게임(희귀아이템)/실패를 실행합니다.
		/// </summary>
		private void RandomFunction()
		{
			float successChance = Random.Range(0f, 101f);
			if (successChance <= successPercentage)
			{
				//성공
				backGround.width = 0;
				backGround.height = 0;
				backGround.isGrow = false;
				map.mapSelectBtn.gameObject.SetActive(true);
				Success();
			}
			else if (successChance > successPercentage && successChance < 100f - minigamePercentage)
			{
				//실패
				backGround.width = 0;
				backGround.height = 0;
				backGround.isGrow = false;
				map.mapSelectBtn.gameObject.SetActive(true);
				Fail();
			}
			else if (successChance >= 100f - minigamePercentage)
			{
				AudioManager.Instance.PlayEffectAudio(midHighRankClip);

				//미니게임
				map.mapSelectBtn.gameObject.SetActive(false);
				backGround.width = 0;
				backGround.height = 0;
				backGround.isGrow = false;
				researchImageRoot.gameObject.SetActive(true);
				researchImageRoot.sprite = researchImage[2];
				isControl = false;
				Invoke(nameof(MiniGamePanel), 3f);
			}
		}

		/// <summary>
		/// 성공 시의 아이템 등급을 지정합니다. (저가/중저가)
		/// </summary>
		private void Success()
		{
			getItemPanel.gameObject.SetActive(true);
			float rand = Random.value;
			if (isOcean)
			{
				if (rand <= 0.6f)
				{
					OceanLowRandom();
				}
				else
				{
					OceanMediumLowRandom();
				}
			}
			else if (isGround)
			{
				if (rand <= 0.6f)
				{
					GroundLowRandom();
				}
				else
				{
					GroundMediumLowRandom();
				}
			}
			else if (isMountain)
			{
				if (rand <= 0.6f)
				{
					MountainLowRandom();
				}
				else
				{
					MountainMediumLowRandom();
				}
			}
		}

		/// <summary>
		/// 실패시의 이벤트를 처리합니다.
		/// </summary>
		private void Fail()
		{
			researchImageRoot.gameObject.SetActive(true);
			researchImageRoot.sprite = researchImage[0];
			Debug.Log("실패");
		}

		/// <summary>
		/// 시계를 갱신합니다.
		/// </summary>
		private void TimeLimit()
		{
			switch (timer)
			{
				case 8:
					needle.transform.eulerAngles = new Vector3(0, 0, 71f);
					break;
				case 7:
					needle.transform.eulerAngles = new Vector3(0, 0, 27f);
					break;
				case 6:
					needle.transform.eulerAngles = new Vector3(0, 0, -18f);
					break;
				case 5:
					needle.transform.eulerAngles = new Vector3(0, 0, -68f);
					break;
				case 4:
					needle.transform.eulerAngles = new Vector3(0, 0, -103f);
					break;
				case 3:
					needle.transform.eulerAngles = new Vector3(0, 0, -150f);
					break;
				case 2:
					needle.transform.eulerAngles = new Vector3(0, 0, -195f);
					break;
				case 1:
					needle.transform.eulerAngles = new Vector3(0, 0, -245f);
					break;
				case 0:
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

			AudioManager.Instance.PlayBackgroundAudio(midHighRankBackgroundClip);

			isMiniGameControl = true;
			minigamePanel.SetActive(true);
		}


		/// <summary>
		/// 미니게임 함수
		/// </summary>
		private void MiniGame()
		{
			// 창 열려 있으면 무시
			if (SceneManager.sceneCount != 1) return;

			if (minigameCount > 100)
			{
				getItemPanel.gameObject.SetActive(true);
				float rand = Random.value;
				minigameCount = 100;
				if (canMinigameGetItem)
				{
					canMinigameGetItem = false;
					if (isOcean)
					{
						if (rand <= 0.8)
						{
							OceanMediumHighRandom();
						}
						else
						{
							OceanHighRandom();
						}
					}
					else if (isGround)
					{
						if (rand <= 0.8f)
						{
							GroundMediumHighRandom();
						}
						else
						{
							GroundHighRandom();
						}
					}
					else if (isMountain)
					{
						if (rand <= 0.8f)
						{
							MountainMediumHighRandom();
						}
						else
						{
							MountainHighRandom();
						}
					}
				}

				MiniGamePanelFalse();
			}
			else if (0 < minigameCount && minigameCount < 100)
			{
				minigameCount -= Time.deltaTime * sliderForce; //미니게임 바 내려가는 속도 
				clickSlider.value = minigameCount / 100;
			}
			else if (minigameCount < 0)
			{
				//sucessText.text = "실패";
				researchImageRoot.sprite = researchImage[3];
				MiniGamePanelFalse();
				researchImageRoot.gameObject.SetActive(true);
			}

			if (Input.anyKeyDown)
			{
				minigameCount += 3;
			}
		}

		/// <summary>
		/// 미니게임 창을 종료합니다.
		/// </summary>
		private void MiniGamePanelFalse()
		{
			AudioManager.Instance.StopBackgroundAudio();

			map.mapSelectBtn.gameObject.SetActive(true);
			researchImageRoot.gameObject.SetActive(false);
			canMinigameGetItem = true;
			isControl = true;
			isMiniGameControl = false;
			minigameCount = 50;
			minigamePanel.SetActive(false);
			backGround.width = 0;
			backGround.height = 0;
			Debug.Log("미니게임창 꺼짐");
		}

		/// <summary>
		/// 바다 지역의 저가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void OceanLowRandom()
		{
			//바다에서 나오는 저가 상품 
			List<ItemProperty> oceanItems =
				items.FindAll(target => target.field == ItemField.Ocean && target.rank == ItemRank.Low);
			//랜덤으로 식재료를 뽑음 

			int randomInt = Random.Range(0, oceanItems.Count);
			ItemProperty item = oceanItems[randomInt];
			print("인벤 저장");
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 바다 지역의 중저가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void OceanMediumLowRandom()
		{
			//바다에서 나오는 중저가 상품 
			List<ItemProperty> oceanItems = items.FindAll(target =>
				target.field == ItemField.Ocean && target.rank == ItemRank.MediumLow);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, oceanItems.Count);
			ItemProperty item = oceanItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 바다 지역의 중고가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void OceanMediumHighRandom()
		{
			//바다에서 나오는 중고가 상품 
			List<ItemProperty> oceanItems = items.FindAll(target =>
				target.field == ItemField.Ocean && target.rank == ItemRank.MediumHigh);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, oceanItems.Count);
			ItemProperty item = oceanItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 바다 지역의 고가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void OceanHighRandom()
		{
			//바다에서 나오는 고가 상품 
			List<ItemProperty> oceanItems =
				items.FindAll(target => target.field == ItemField.Ocean && target.rank == ItemRank.High);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, oceanItems.Count);
			ItemProperty item = oceanItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 논/밭 지역의 저가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void GroundLowRandom()
		{
			//밭/논에서 나오는 저가 상품 
			List<ItemProperty> groundItems =
				items.FindAll(target => target.field == ItemField.Ground && target.rank == ItemRank.Low);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, groundItems.Count);
			ItemProperty item = groundItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 논/밭 지역의 중저가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void GroundMediumLowRandom()
		{
			//밭/논에서 나오는 중저가 상품 
			List<ItemProperty> groundItems = items.FindAll(target =>
				target.field == ItemField.Ground && target.rank == ItemRank.MediumLow);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, groundItems.Count);
			ItemProperty item = groundItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 논/밭 지역의 중고가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void GroundMediumHighRandom()
		{
			//밭/논에서 나오는 중고가 상품 
			List<ItemProperty> groundItems = items.FindAll(target =>
				target.field == ItemField.Ground && target.rank == ItemRank.MediumHigh);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, groundItems.Count);
			ItemProperty item = groundItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 논/밭 지역의 고가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void GroundHighRandom()
		{
			//밭/논에서 나오는 중고가 상품 
			List<ItemProperty> groundItems =
				items.FindAll(target => target.field == ItemField.Ground && target.rank == ItemRank.High);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, groundItems.Count);
			ItemProperty item = groundItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 산 지역의 저가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void MountainLowRandom()
		{
			//산에서 나오는 저가 상품 
			List<ItemProperty> mountainItems =
				items.FindAll(target => target.field == ItemField.Mountain && target.rank == ItemRank.Low);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, mountainItems.Count);
			ItemProperty item = mountainItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 산 지역의 중저가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void MountainMediumLowRandom()
		{
			//산에서 나오는 중저가 상품 
			List<ItemProperty> mountainItems = items.FindAll(target =>
				target.field == ItemField.Mountain && target.rank == ItemRank.MediumLow);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, mountainItems.Count);
			ItemProperty item = mountainItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 산 지역의 중고가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void MountainMediumHighRandom()
		{
			//산에서 나오는 중고가 상품 
			List<ItemProperty> mountainItems = items.FindAll(target =>
				target.field == ItemField.Mountain && target.rank == ItemRank.MediumHigh);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, mountainItems.Count);
			ItemProperty item = mountainItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 산 지역의 고가 아이템을 하나 뽑고, UI를 갱신합니다.
		/// </summary>
		private void MountainHighRandom()
		{
			//산에서 나오는 고가 상품 
			List<ItemProperty> mountainItems =
				items.FindAll(target => target.field == ItemField.Mountain && target.rank == ItemRank.High);
			//랜덤으로 식재료를 뽑음 
			int randomInt = Random.Range(0, mountainItems.Count);
			ItemProperty item = mountainItems[randomInt];
			//인벤에 저장 
			int index = ItemBuffer.Instance.GetIndex(item.name);
			GameInfo.Instance.inventory.Add(index);

			if (!UnlockedCGsManager.Instance.GetAllData().researchItems.Exists(target => target == index))
			{
				DefineUnlockedCGs data = UnlockedCGsManager.Instance.GetAllData();
				data.researchItems.Add(index);
				UnlockedCGsManager.Instance.Save(data);
			}

			getItemImage.sprite = getResearchImage[index];

			AudioManager.Instance.PlayEffectAudio(lowRankClip);
		}

		/// <summary>
		/// 아이템 창을 종료합니다.
		/// </summary>
		public void GetItemPanelFalse()
		{
			getItemPanel.gameObject.SetActive(false);
		}
	}
}
