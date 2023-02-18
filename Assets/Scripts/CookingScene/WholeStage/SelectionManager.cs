using System.Collections;
using System.Collections.Generic;
using Array = System.Array;
using Action = System.Action;

using UnityEngine;

using YouthSpice.CookingScene.RecipeStage;
using YouthSpice.CookingScene.WholeStage.UI;
using YouthSpice.PreloadScene.Audio;

namespace YouthSpice.CookingScene.WholeStage
{
	/// <summary>
	/// 선택된 항목들을 관리합니다.
	/// </summary>
	public class SelectionManager : MonoBehaviour
	{
		[SerializeField]
		private AudioClip clip;

		[Header("Config")]
		[SerializeField]
		private int selectionCount = 5;

		[Header("Status")]
		[SerializeField]
		private int[] items;

		[Header("Prefabs")]
		[SerializeField]
		private GameObject elementPrefab;

		[Header("GameObjects")]
		[SerializeField]
		private Transform elementParent;

		[Header("Classes")]
		[SerializeField]
		private GameManager gameManager;

		[SerializeField]
		private RecipeManager recipeManager;

		private ButtonManager buttonManager;

		private void Awake()
		{
			for (int i = 0; i < elementParent.childCount; i++)
			{
				Destroy(elementParent.GetChild(i).gameObject);
			}

			buttonManager = GetComponent<ButtonManager>();
		}

		private void Start()
		{
			items = new int[selectionCount];
			Array.Fill(items, -2);
			for (int i = 0; i < selectionCount; i++)
			{
				ShowSelection child = Instantiate(elementPrefab, Vector3.zero, Quaternion.identity, elementParent)
					.GetComponent<ShowSelection>();
				child.Init();
				child.Set(-2);
			}
		}

		/// <summary>
		/// 현재 Flow에 맞춰 초기 함수를 실행합니다.
		/// </summary>
		public void GoNext()
		{
			CookingFlow current = gameManager.CurrentChapter;

			switch (current)
			{
				case CookingFlow.Selection:
					break;
				case CookingFlow.Recipe:
					SetCloseButton(false);
					break;
				case CookingFlow.Result:
					break;
			}
		}

		/* ====================================== Selection Stage ====================================== */

		/// <summary>
		/// 빈 Element를 찾아 반환합니다.
		/// </summary>
		/// <returns>
		/// 발견된 빈 Element의 Index가 반환됩니다.
		/// -1인 경우, 빈 ELement가 없는 경우입니다.
		/// </returns>
		public int GetEmptyElement()
		{
			for (int i = 0; i < selectionCount; i++)
			{
				ShowSelection child = elementParent.GetChild(i).GetComponent<ShowSelection>();
				if (child.CurrentItemNumber == -2) return i;
			}

			return -1;
		}

		/// <summary>
		/// 슬롯에 아이템을 추가합니다.
		/// </summary>
		/// <param name="id">추가할 아이템의 ID를 지정합니다.</param>
		/// <param name="cancelClickedCallback">취소 시의 callback을 지정합니다. 없을 경우, null을 지정합니다.</param>
		public void AddItem(int id, Action cancelClickedCallback = null)
		{
			int elementIndex = GetEmptyElement();
			if (elementIndex == -1) return;

			AudioManager.Instance.PlayEffectAudio(clip);

			items[elementIndex] = id;

			ShowSelection child = elementParent.GetChild(elementIndex).GetComponent<ShowSelection>();
			child.Set(id, cancelClickedCallback, () => { RemoveItem(elementIndex); },
				() => { SelectItem(elementIndex, id); });

			CanGoNext();
		}

		/// <summary>
		/// 다음 항목으로 넘어가는 버튼의 활성화 여부를 지정합니다.
		/// </summary>
		private void CanGoNext()
		{
			int elementIndex = GetEmptyElement();
			buttonManager.SetButtonActive(elementIndex == -1);
		}

		/// <summary>
		/// 특정 슬롯의 선택을 취소합니다.
		/// </summary>
		/// <param name="index">취소할 슬롯의 Index를 지정합니다.</param>
		private void RemoveItem(int index)
		{
			AudioManager.Instance.PlayEffectAudio(clip);

			items[index] = -2;

			ShowSelection child = elementParent.GetChild(index).GetComponent<ShowSelection>();
			child.Set(-2);

			CanGoNext();
		}

		/* ====================================== Recipe Stage ====================================== */

		/// <summary>
		/// 취소 버튼의 표출을 관리합니다.
		/// </summary>
		/// <param name="set">표출 여부를 지정합니다.</param>
		private void SetCloseButton(bool set)
		{
			for (int i = 0; i < selectionCount; i++)
			{
				ShowSelection child = elementParent.GetChild(i).GetComponent<ShowSelection>();
				child.SetCancelActive(false);
			}
		}

		/// <summary>
		/// 슬롯을 선택했을 때의 이벤트를 처리합니다.
		/// </summary>
		/// <param name="index">선택한 슬롯의 Index를 지정합니다.</param>
		/// <param name="itemID">선택할 슬롯의 아이템 ID를 지정합니다.</param>
		private void SelectItem(int index, int itemID)
		{
			if (gameManager.CurrentChapter != CookingFlow.Recipe) return;

			AudioManager.Instance.PlayEffectAudio(clip);

			recipeManager.SelectItem(index, itemID);
		}

		/// <summary>
		/// 아이템을 선택 가능하게 설정합니다.
		/// </summary>
		/// <param name="index">설정할 슬롯의 Index를 지정합니다.</param>
		public void EnableItem(int index)
		{
			if (gameManager.CurrentChapter != CookingFlow.Recipe) return;

			ShowSelection child = elementParent.GetChild(index).GetComponent<ShowSelection>();
			child.SetSelectInteractable(true);
		}

		/// <summary>
		/// 아이템을 선택 불가능하게 지정합니다.
		/// </summary>
		/// <param name="index">설정할 슬롯의 Index를 지정합니다.</param>
		public void DisableItem(int index)
		{
			if (gameManager.CurrentChapter != CookingFlow.Recipe) return;

			ShowSelection child = elementParent.GetChild(index).GetComponent<ShowSelection>();
			child.SetSelectInteractable(false);
		}

		/* ====================================== Result Stage ====================================== */
	}
}
