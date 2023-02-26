using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

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

		private Stack<AlertBox> currentAlerts = new Stack<AlertBox>();

		public bool IsRunning => currentAlerts.Count > 0;

		/// <summary>
		/// 알림을 표시합니다.
		/// </summary>
		/// <param name="alertType">알림창의 버튼 개수를 지정합니다.</param>
		/// <param name="title">알림창의 제목을 지정합니다.</param>
		/// <param name="description">알림창의 설명을 지정합니다.</param>
		/// <param name="buttons">각 버튼에 대한 Callback을 지정합니다. 왼쪽 버튼부터 차례로 지정합니다.</param>
		public void Show(
			AlertType alertType,
			string title,
			string description,
			Dictionary<string, System.Action> buttons,
			bool useCloseButton = false
		)
		{
			if ((int)alertType != buttons.Count)
			{
				Debug.LogWarning(
					"The value of 'AlertType' and the number of 'buttons(Dictionary)' do not match each other. The notification window may not work as intended.");
			}

			EventSystem.current?.SetSelectedGameObject(null);
			GameObject parent = Instantiate(prefab, parentCanvas);
			parent.SetActive(true);
			AlertBox alertBox = parent.transform.GetChild(1).GetComponent<AlertBox>();
			alertBox.Init(alertType, title, description, buttons, useCloseButton, ChildDestroy);
			alertBox.Show();
			currentAlerts.Push(alertBox);
		}

		/// <summary>
		/// 알림을 제거합니다.
		/// </summary>
		public void Pop()
		{
			ChildDestroy();
		}

		/// <summary>
		/// 알림창을 제거합니다.
		/// </summary>
		private void ChildDestroy()
		{
			AlertBox current = currentAlerts.Pop();
			Destroy(current.transform.parent.gameObject);
		}
	}
}
