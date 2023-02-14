using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using YouthSpice.GameScene;
using YouthSpice.PreloadScene.Item;

namespace YouthSpice
{
	/// <summary>
	/// Description
	/// </summary>
	public class Map : MonoBehaviour
	{
		public Research research;
		[SerializeField] private GameObject mapCanvas;
		[SerializeField] private Sprite[] backgroundSprite; //탐색 상태 이미지

		[SerializeField] private Image backgroundImage;
		public Button mapSelectBtn;
		public void OceanMap()
		{ 
			research.GetComponent<Research>().isOcean = true;
			research.GetComponent<Research>().isGround = false;
			research.GetComponent<Research>().isMountain = false;
			backgroundImage.sprite = backgroundSprite[0];
			mapCanvas.SetActive(false);
		}

		public void GroundMap()
		{
			research.GetComponent<Research>().isOcean = false;
			research.GetComponent<Research>().isGround = true;
			research.GetComponent<Research>().isMountain = false;
			backgroundImage.sprite = backgroundSprite[1];
			mapCanvas.SetActive(false);
		}
		public void MountainMap()
		{
			research.GetComponent<Research>().isOcean = false;
			research.GetComponent<Research>().isGround = false;
			research.GetComponent<Research>().isMountain = true;
			backgroundImage.sprite = backgroundSprite[2];
			mapCanvas.SetActive(false);
		}

		public void MapSelect()
		{
			mapCanvas.SetActive(true);
		} 
	}
}
