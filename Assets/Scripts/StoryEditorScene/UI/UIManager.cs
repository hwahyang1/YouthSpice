using System.Collections;
using System.Collections.Generic;
using Action = System.Action;
using DateTime = System.DateTime;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using YouthSpice.PreloadScene.Alert;
using YouthSpice.StoryEditorScene.Condition;
using YouthSpice.StoryEditorScene.Element;
using YouthSpice.StoryEditorScene.Files;

namespace YouthSpice.StoryEditorScene.UI
{
	/// <summary>
	/// UI를 관리합니다.
	/// </summary>
	public class UIManager : MonoBehaviour
	{
		[Header("Basic Info")]
		[SerializeField]
		private Text dataVersionText;

		[Header("Chapter Info")]
		[SerializeField]
		private TMP_InputField chapterNameInputField;

		[SerializeField]
		private Text chapterIDText;

		[Header("Custom Alert")]
		[SerializeField]
		private GameObject customAlertCanvas;

		[SerializeField]
		private GameObject customAlertParent;

		[Header("Element")]
		[SerializeField]
		private Dropdown elementDropdown;

		[SerializeField, Tooltip("ChapterElementType와 같은 순서대로 입력합니다.")]
		private string[] dropdownElements;

		private ElementManager elementManager;
		private ConditionManager conditionManager;
		private ChapterManager chapterManager;
		private ChapterFileManager chapterFileManager;

		private void Start()
		{
			conditionManager = GetComponent<ConditionManager>();
			elementManager = GetComponent<ElementManager>();
			chapterManager = GetComponent<ChapterManager>();
			chapterFileManager = GetComponent<ChapterFileManager>();

			foreach (string elementName in dropdownElements)
			{
				elementDropdown.options.Add(new Dropdown.OptionData(elementName));
			}
		}

		/// <summary>
		/// 챕터 ID 텍스트를 수정합니다.
		/// </summary>
		/// <param name="id">바뀔 ID를 지정합니다.</param>
		public void SetChapterID(string id) => chapterIDText.text = "챕터 고유 ID: " + id;

		/// <summary>
		/// 챕터명 텍스트를 수정합니다.
		/// </summary>
		/// <param name="name">바꿀 이름을 지정합니다.</param>
		public void SetChapterName(string name) => chapterNameInputField.text = name;

		/// <summary>
		/// 챕터명을 반환합니다.
		/// </summary>
		/// <returns>현재 작성된 챕터명을 반환합니다.</returns>
		public string GetChapterName() => chapterNameInputField.text;

		public void UpdateDataVersion(int editor, int data)
		{
			DateTime editorTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(editor).AddHours(9);
			DateTime dataTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data).AddHours(9);

			dataVersionText.text =
				$"에디터 최근 수정: {editorTime.ToString("yyyy/MM/dd HH:mm:ss")} (KST)\n데이터 파일 최근 수정: {dataTime.ToString("yyyy/MM/dd HH:mm:ss")} (KST)";
		}

		public void SetCustomCoverActive(bool active)
		{
			customAlertCanvas.SetActive(active);
			customAlertParent.SetActive(false);
		}

		public void SetCustomAlertActive(bool active)
		{
			customAlertCanvas.SetActive(active);
			customAlertParent.SetActive(active);
			if (active) conditionManager.ApplyReset();
		}

		public void OnNewElementDropdownChanged()
		{
			if (elementDropdown.value == 0) return;
			elementManager.NewElement(elementDropdown.value - 1, null);
			elementDropdown.value = 0;
		}

		public void OpenChapterPreview()
		{
			//TODO
			if (chapterFileManager.IsNewFile)
			{
				AlertManager.Instance.Show(AlertType.Double, "알림",
					"챕터 테스트를 진행하기 전, 파일을 저장해야 합니다.",
					new Dictionary<string, Action>()
					{
						{ "확인", null }
					});
			}
			else
			{
				AlertManager.Instance.Show(AlertType.Double, "알림",
					$"챕터 테스트를 진행하기 전, 파일을 저장해야 합니다.\n\n기존 파일이 존재합니다: {chapterFileManager.PastFilePath}/{chapterFileManager.PastFileName}\n기존 파일에 덮어씌울까요?",
					new Dictionary<string, Action>()
					{
						{
							"아니요\n(다른 파일에 저장)",
							() =>
							{
								StartCoroutine(
									chapterFileManager.SaveNewFileCoroutine(chapterManager.StartChapterPreview));
							}
						},
						{
							"예\n(기존 파일 덮어쓰기)",
							() =>
							{
								StartCoroutine(
									chapterFileManager.OverwriteFileCoroutine(chapterManager.StartChapterPreview));
							}
						}
					});
			}
		}
	}
}
