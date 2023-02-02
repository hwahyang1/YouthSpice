using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;

using YouthSpice.PreloadScene.Alert;

namespace YouthSpice.StoryEditorScene.Element
{
	/// <summary>
	/// 각 Element의 부모 Class 입니다.
	/// </summary>
	public abstract class ElementGroup : MonoBehaviour
	{
		private ElementManager elementManager;

		public void Init(ElementManager elementManager, Dictionary<string, string> data)
		{
			this.elementManager = elementManager;
			Init(data);
		}

		protected abstract void Init(Dictionary<string, string> data);
		public abstract Dictionary<string, string> GetData();

		public void OnGoUpClicked()
		{
			int currentIndex = transform.GetSiblingIndex();
			elementManager.MoveElement(currentIndex, currentIndex - 1);
		}

		public void OnGoDownClicked()
		{
			int currentIndex = transform.GetSiblingIndex();
			elementManager.MoveElement(currentIndex, currentIndex + 1);
		}

		public void OnDeleteClicked()
		{
			AlertManager.Instance.Show(AlertType.Double, "경고", "이 작업은 취소 할 수 없습니다.\n계속할까요?",
				new Dictionary<string, Action>()
					{ { "예", () => { elementManager.DeleteElement(transform.GetSiblingIndex()); } }, { "아니요", null } });
		}

		protected ElementManager GetElementManager() => elementManager;
	}
}
