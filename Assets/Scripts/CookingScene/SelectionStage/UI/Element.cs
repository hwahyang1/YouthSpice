using System.Collections;
using System.Collections.Generic;
using ActionIntInt = System.Action<int, int>;

using UnityEngine;
using UnityEngine.UI;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Item;

namespace YouthSpice.CookingScene.SelectionStage.UI
{
	/// <summary>
	/// 각 항목의 정보를 정의합니다.
	/// </summary>
	public class Element : MonoBehaviour
	{
		[Header("Status")]
		[SerializeField, ReadOnly]
		private int majorIndex;

		public int MajorIndex => majorIndex;

		[SerializeField, ReadOnly]
		private int minorIndex;

		public int MinorIndex => minorIndex;

		[SerializeField, ReadOnly]
		private int itemNumber;

		public int ItemNumber => itemNumber;

		[Header("GameObjects")]
		[SerializeField]
		private Image itemImage;

		[SerializeField]
		private Text itemName;

		[SerializeField]
		private Button selectButton;

		private ActionIntInt callback = null;

		/// <summary>
		/// 항목을 초기화 합니다.
		/// </summary>
		/// <param name="majorIndex">항목의 행을 지정합니다.</param>
		/// <param name="minorIndex">항목의 열을 지정합니다.</param>
		/// <param name="id">항목이 가질 식재료의 Id를 지정합니다.</param>
		/// <param name="callback">항목이 클릭되었을 떄의 callback을 지정합니다. 없을 경우, null로 지정합니다.</param>
		public void Init(int majorIndex, int minorIndex, int id, ActionIntInt callback = null)
		{
			this.majorIndex = majorIndex;
			this.minorIndex = minorIndex;

			itemNumber = id;

			// -1 == 선택 안함
			if (id != -1)
			{
				ItemProperty data = ItemBuffer.Instance.items[id];

				itemImage.sprite = data.sprite;
				itemName.text = data.name;
			}

			this.callback = callback;
		}

		public void SetInteractable(bool interactable)
		{
			selectButton.interactable = interactable;
		}

		public void OnClick()
		{
			callback?.Invoke(majorIndex, minorIndex);
		}
	}
}
