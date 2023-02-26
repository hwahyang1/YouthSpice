using System.Collections;
using System.Collections.Generic;
using ActionInt = System.Action<int>;
using DateTime = System.DateTime;
using TimeZoneInfo = System.TimeZoneInfo;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.SaveLoadSlotScene.UI
{
	/// <summary>
	/// 각 슬롯의 항목을 관리합니다.
	/// </summary>
	public class SlotElement : MonoBehaviour
	{
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

		[Header("Status")]
		private int index;

		private ActionInt callback = null;

		/// <summary>
		/// 항목을 초기화 합니다.
		/// </summary>
		/// <param name="index">항목의 순서를 지정합니다.</param>
		/// <param name="previewImage">항목의 미리보기 이미지를 지정합니다.</param>
		/// <param name="title">항목의 챕터 소제목을 지정합니다.</param>
		/// <param name="dateTime">항목의 마지막 저장일을 지정합니다. UTC 기준으로 지정합니다.</param>
		/// <param name="callback">항목이 선택되었을 때의 callback을 지정합니다.</param>
		public void Init(int index, Sprite previewImage, string title, long dateTime, ActionInt callback = null)
		{
			this.index = index;

			slotIndexText.text = $"SLOT {(index + 1).ToString("00")}";
			slotPreviewImageArea.sprite = previewImage;
			slotTitleText.text = title;

			if (dateTime == 0)
			{
				slotDateTimeText.text = "----/--/-- --:--:--";
			}
			else
			{
				int offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).Hours;
				DateTime savedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(dateTime).AddHours(offset);
				slotDateTimeText.text = savedTime.ToString("yyyy/MM/dd HH:mm:ss");
			}

			this.callback = callback;
		}

		/// <summary>
		/// 슬롯이 클릭되었을 때의 이벤트를 처리합니다.
		/// </summary>
		public void OnClicked()
		{
			callback?.Invoke(index);
		}
	}
}
