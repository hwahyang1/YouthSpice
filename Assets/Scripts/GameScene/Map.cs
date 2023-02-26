using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using YouthSpice.PreloadScene.Audio;
using YouthSpice.PreloadScene.Game;
using YouthSpice.PreloadScene.Scene;
using YouthSpice.StoryScene.Extern;

namespace YouthSpice.GameScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class Map : MonoBehaviour
	{
		// audio
		[SerializeField]
		private AudioClip clickClip;

		[SerializeField]
		private AudioClip mapButtonClip;

		[SerializeField]
		private Research research;

		[SerializeField]
		private GameObject mapCanvas;

		[SerializeField]
		private Sprite[] backgroundSprite; //탐색 상태 이미지

		[SerializeField]
		private Image backgroundImage;
		
		public Button mapSelectBtn;

		private void Start()
		{
			if (GameInfo.Instance.viewedResearch) return;
			StorySceneLoadParams.Instance.isTutorialScene = true;
			StorySceneLoadParams.Instance.chapterID = GameProgressManager.Instance.researchTutorial;
			SceneChange.Instance.Add("StoryScene_Tutorial");
			GameInfo.Instance.viewedResearch = true;
		}

		/// <summary>
		/// 바다가 선택되었을 때의 이벤트를 처리합니다.
		/// </summary>
		public void OceanMap()
		{
			AudioManager.Instance.PlayEffectAudio(clickClip);
			research.isOcean = true;
			research.isGround = false;
			research.isMountain = false;
			backgroundImage.sprite = backgroundSprite[0];
			mapCanvas.SetActive(false);
		}

		/// <summary>
		/// 논/밭이 선택되었을 때의 이벤트를 처리합니다.
		/// </summary>
		public void GroundMap()
		{
			AudioManager.Instance.PlayEffectAudio(clickClip);
			research.isOcean = false;
			research.isGround = true;
			research.isMountain = false;
			backgroundImage.sprite = backgroundSprite[1];
			mapCanvas.SetActive(false);
		}

		/// <summary>
		/// 산이 선택되었을 때의 이벤트를 처리합니다.
		/// </summary>
		public void MountainMap()
		{
			AudioManager.Instance.PlayEffectAudio(clickClip);
			research.isOcean = false;
			research.isGround = false;
			research.isMountain = true;
			backgroundImage.sprite = backgroundSprite[2];
			mapCanvas.SetActive(false);
		}

		/// <summary>
		/// 맵 선택창을 표시합니다.
		/// </summary>
		public void MapSelect()
		{
			AudioManager.Instance.PlayEffectAudio(mapButtonClip);
			mapCanvas.SetActive(true);
		}
	}
}
