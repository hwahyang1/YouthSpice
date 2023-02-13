using System.Collections;
using System.Collections.Generic;
using Action = System.Action;
using DateTime = System.DateTime;
using TimeZoneInfo = System.TimeZoneInfo;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;
using UnityEngine.EventSystems;

namespace YouthSpice.SaveLoadSlotScene.UI
{
	/// <summary>
	/// 각 슬롯의 항목을 관리합니다.
	/// </summary>
	public class SlotElement : MonoBehaviour
	{
		[Header("Sources")]
		[Tooltip("기본 이미지, 선택 되었을 때의 이미지 순서로 지정합니다.")]
		[SerializeField]
		private Sprite[] slotBackgroundImages;
		
		[Header("GameObjects")]
		[SerializeField]
		private Image slotBackgroundImageArea;
		
		[SerializeField]
		private Text slotIndexText;

		[SerializeField]
		private Image slotPreviewImageArea;

		[SerializeField]
		private Text slotTitleText;

		[SerializeField]
		private Text slotDateTimeText;

		/// <summary>
		/// 항목을 초기화 합니다.
		/// </summary>
		/// <param name="index">항목의 순서를 지정합니다.</param>
		/// <param name="previewImage">항목의 미리보기 이미지를 지정합니다.</param>
		/// <param name="title">항목의 챕터 소제목을 지정합니다.</param>
		/// <param name="dateTime">항목의 마지막 저장일을 지정합니다. UTC 기준으로 지정합니다.</param>
		/// <param name="callback">항목이 선택되었을 때의 callback을 지정합니다.</param>
		public void Init(int index, Sprite previewImage, string title, int dateTime, Action callback = null)
		{
			EventTrigger eventTrigger = GetComponent<EventTrigger>();

			EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
			entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
			entry_PointerEnter.callback.AddListener((data) => { OnMouseEnterCallback((PointerEventData)data); });
			eventTrigger.triggers.Add(entry_PointerEnter);

			EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
			entry_PointerExit.eventID = EventTriggerType.PointerExit;
			entry_PointerExit.callback.AddListener((data) => { OnMouseExitCallback((PointerEventData)data); });
			eventTrigger.triggers.Add(entry_PointerExit);

			EventTrigger.Entry entry_PointerClick = new EventTrigger.Entry();
			entry_PointerClick.eventID = EventTriggerType.PointerClick;
			entry_PointerClick.callback.AddListener((data) => { OnClicked((PointerEventData)data); });
			eventTrigger.triggers.Add(entry_PointerClick);
			
			int offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).Hours;
			DateTime savedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(dateTime).AddHours(offset);

			slotIndexText.text = $"SLOT {index.ToString("00")}";
			slotPreviewImageArea.sprite = previewImage;
			slotTitleText.text = title;
			slotDateTimeText.text = savedTime.ToString("yyyy/MM/dd HH:mm:ss");
		}

		private void OnMouseEnterCallback(PointerEventData data)
		{
			
		}

		private void OnMouseExitCallback(PointerEventData data)
		{
			
		}

		private void OnClicked(PointerEventData data)
		{
			
		}
	}
}
