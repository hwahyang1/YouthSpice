using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using YouthSpice.PreloadScene.Audio;
using YouthSpice.PreloadScene.Game;
using YouthSpice.PreloadScene.Item;

namespace YouthSpice.GameScene
{
	/// <summary>
	/// 알림창을 관리합니다.
	/// </summary>
	public class AlertBox : MonoBehaviour
	{
		[SerializeField]
		private AudioClip confirmAudioClip;
		
		[SerializeField]
		private GameObject alertPanelParent;
		
		[SerializeField]
		private GameObject elementPrefab;

		[SerializeField]
		private Transform elementParent;

		private bool runed = false;

		private void Awake()
		{
			alertPanelParent.SetActive(false);
		}

		public void Init(List<int> items)
		{
			runed = false;
			
			for (int i = 0; i < elementParent.transform.childCount; i++)
			{
				Destroy(elementParent.GetChild(i).gameObject);
			}

			int[] uniqueItems = items.Distinct().ToArray();
			foreach (int currentItem in uniqueItems)
			{
				AlertElement child = Instantiate(elementPrefab, elementParent).GetComponent<AlertElement>();

				ItemProperty itemInfo = ItemBuffer.Instance.items[currentItem];
				int duplicateCount = items.FindAll(target => target == currentItem).Count;
				
				child.Init(itemInfo.sprite, itemInfo.name, duplicateCount);
			}
		}
		
		public void Show()
		{
			alertPanelParent.SetActive(true);
		}

		public void Exit()
		{
			if (runed) return;
			runed = true;
			AudioManager.Instance.PlayEffectAudio(confirmAudioClip);
			GameProgressManager.Instance.CountUp();
			GameProgressManager.Instance.RunThisChapter();
		}
	}
}
