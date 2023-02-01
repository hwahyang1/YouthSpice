using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using YouthSpice.StoryEditorScene;

namespace YouthSpice.PreloadScene.Alert
{
	/// <summary>
	/// 알림 종류를 지정합니다.
	/// </summary>
	public enum AlertType
	{
		None,
		Single,
		Double,
		Triple
	}

	/// <summary>
	/// 알림창의 노출을 관리합니다.
	/// </summary>
	/// <remarks>
	/// 알림창 버튼의 순서는 왼쪽부터 0번입니다.
	/// </remarks>
	public class AlertManager : Singleton<AlertManager>
	{
		[SerializeField]
		private Transform parentCanvas;

		[SerializeField]
		private GameObject prefab;

		private Stack<AlertBox> currentAlert = new Stack<AlertBox>();
		public bool IsRunning { get { return currentAlert.Count > 0; } }

		protected override void Update()
		{
			return;
			if (IsRunning)
			{
				if (Input.GetKeyDown(KeyCode.Escape)) // Esc 입력 시 -> 맨 우측 버튼 클릭과 동일하게 취급
				{
					AlertBox current = currentAlert.Peek();
					current.OnButtonClicked((int)current.AlertType - 1);
				}
				// Space 키는 경우에 따라 사용, Enter키(NumPad 포함) 입력 시 -> 0번째 버튼 클릭과 동일하게 취급
				else if (/*Input.GetKeyDown(KeyCode.Space) || */Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
				{
					AlertBox current = currentAlert.Peek();
					current.OnButtonClicked(0);
				}
			}
		}

		/// <summary>
		/// 알림을 표시합니다.
		/// </summary>
		/// <param name="alertType">알림창의 버튼 개수를 지정합니다.</param>
		/// <param name="title">알림창의 제목을 지정합니다.</param>
		/// <param name="description">알림창의 설명을 지정합니다.</param>
		/// <param name="buttons">각 버튼에 대한 Callback을 지정합니다. 왼쪽 버튼부터 차례로 지정합니다.</param>
		public void Show(AlertType alertType, string title, string description, Dictionary<string, System.Action> buttons)
		{
			if ((int)alertType != buttons.Count)
			{
				Debug.LogWarning("The value of 'AlertType' and the number of 'buttons(Dictionary)' do not match each other. The notification window may not work as intended.");
			}
			EventSystem.current?.SetSelectedGameObject(null);
			GameObject parent = Instantiate(prefab, parentCanvas);
			parent.SetActive(true);
			AlertBox alertBox = parent.transform.GetChild(1).GetComponent<AlertBox>();
			alertBox.Init(alertType, title, description, buttons, ChildDestroy);
			alertBox.Show();
			currentAlert.Push(alertBox);
		}

		public void Pop()
		{
			ChildDestroy();
		}

		private void ChildDestroy()
		{
			AlertBox current = currentAlert.Pop();
			Destroy(current.transform.parent.gameObject);
		}
	}
}
