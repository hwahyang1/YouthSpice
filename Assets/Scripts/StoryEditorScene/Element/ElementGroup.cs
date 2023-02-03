using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Alert;

namespace YouthSpice.StoryEditorScene.Element
{
	/// <summary>
	/// 각 Element의 부모 Class 입니다.
	/// </summary>
	public abstract class ElementGroup : MonoBehaviour
	{
		private ElementManager elementManager;
		public ElementManager ElementManager => elementManager;

		// 분기점 안에 있는 항목인지
		[SerializeField, ReadOnly]
		private bool isSubElement = false;

		public bool IsSubElement => isSubElement;

		[SerializeField, ReadOnly]
		private ChapterElementType type;

		public ChapterElementType Type => type;

		public void Init(ElementManager elementManager, ChapterElementType type, Dictionary<string, string> data, bool subElement = false)
		{
			this.elementManager = elementManager;
			this.type = type;
			isSubElement = subElement;
			Init(data);
		}

		protected abstract void Init(Dictionary<string, string> data);
		public abstract Dictionary<string, string> GetData();

		public void OnGoUpClicked()
		{
			int currentIndex = transform.GetSiblingIndex();

			if (isSubElement) elementManager.MoveElementInSelection(transform.parent, currentIndex, currentIndex - 1);
			else elementManager.MoveElement(currentIndex, currentIndex - 1);
		}

		public void OnGoDownClicked()
		{
			int currentIndex = transform.GetSiblingIndex();

			if (isSubElement) elementManager.MoveElementInSelection(transform.parent, currentIndex, currentIndex + 1);
			else elementManager.MoveElement(currentIndex, currentIndex + 1);
		}

		public void OnDeleteClicked()
		{
			AlertManager.Instance.Show(AlertType.Double, "경고", "이 작업은 취소 할 수 없습니다.\n계속할까요?",
				new Dictionary<string, Action>()
				{
					{
						"예", () =>
						{
							if (isSubElement)
								elementManager.DeleteElementInSelection(transform.parent, transform.GetSiblingIndex());
							else elementManager.DeleteElement(transform.GetSiblingIndex());
						}
					},
					{ "아니요", null }
				});
		}
	}
}
