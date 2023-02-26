using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Item;

namespace YouthSpice.CookingScene.WholeStage.UI
{
	/// <summary>
	/// 선택된 항목을 표출합니다
	/// </summary>
	public class ShowSelection : MonoBehaviour
	{
		[Header("Status")]
		[SerializeField, ReadOnly]
		private int currentItemNumber;

		public int CurrentItemNumber => currentItemNumber;

		[Header("Resources")]
		[SerializeField]
		private Sprite selectNoneSprite;

		[Header("GameObjects")]
		[SerializeField]
		private Image itemArea;

		[SerializeField]
		private Button selectButton;

		[SerializeField]
		private Button cancelButton;

		private Action cancelClickedCallback1 = null;
		private Action cancelClickedCallback2 = null;
		private Action selectClickedCallback = null;

		public void Init()
		{
			SetCancelActive(false);
		}

		/// <summary>
		/// 특정 슬롯에 항목을 추가합니다.
		/// </summary>
		/// <param name="id">추가할 아이템의 ID를 지정합니다.</param>
		/// <param name="cancelClickedCallback1">취소 시 실행할 callback을 지정합니다. 없을 경우, null로 지정합니다.</param>
		/// <param name="cancelClickedCallback2">취소 시 실행할 callback을 지정합니다. 없을 경우, null로 지정합니다.</param>
		/// <param name="selectClickedCallback">선택 시 실행할 callback을 지정합니다. 없을 경우, null로 지정합니다.</param>
		public void Set(
			int id,
			Action cancelClickedCallback1 = null,
			Action cancelClickedCallback2 = null,
			Action selectClickedCallback = null
		)
		{
			currentItemNumber = id;

			// -1이면 선택 포기
			// -2이면 선택 해제
			if (id == -2)
			{
				itemArea.sprite = null;
				itemArea.color = new Color(1f, 1f, 1f, 0f);
				SetCancelActive(false);
			}
			else if (id == -1)
			{
				itemArea.sprite = selectNoneSprite;
				itemArea.color = new Color(1f, 1f, 1f, 1f);
				SetCancelActive(true);
			}
			else
			{
				ItemProperty data = ItemBuffer.Instance.items[id];
				itemArea.sprite = data.sprite;
				itemArea.color = new Color(1f, 1f, 1f, 1f);
				SetCancelActive(true);
			}

			this.cancelClickedCallback1 = cancelClickedCallback1;
			this.cancelClickedCallback2 = cancelClickedCallback2;
			this.selectClickedCallback = selectClickedCallback;
		}

		/// <summary>
		/// 항목을 선택 가능하게 할 지 여부를 지정합니다.
		/// </summary>
		/// <param name="interactable">선택 가능 여부를 지정합니다.</param>
		public void SetSelectInteractable(bool interactable)
		{
			selectButton.interactable = interactable;
		}

		/// <summary>
		/// 취소 버튼의 활성화 여부를 지정합니다.
		/// </summary>
		/// <param name="active">활성화 버튼의 여부를 지정합니다.</param>
		public void SetCancelActive(bool active)
		{
			cancelButton.gameObject.SetActive(active);
		}

		/// <summary>
		/// 취소 버튼이 클릭되었을 때의 이벤트를 처리합니다.
		/// </summary>
		public void OnCancelButtonClicked()
		{
			cancelClickedCallback1?.Invoke();
			cancelClickedCallback2?.Invoke();
		}

		/// <summary>
		/// 슬롯이 선택되었을 때의 이벤트를 처리합니다.
		/// </summary>
		public void OnSelectButtonClicked()
		{
			selectClickedCallback?.Invoke();
		}
	}
}
