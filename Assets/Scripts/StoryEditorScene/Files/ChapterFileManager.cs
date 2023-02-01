using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;

using SimpleFileBrowser;

using YouthSpice.PreloadScene.Alert;
using YouthSpice.StoryEditorScene.Element;
using YouthSpice.StoryEditorScene.UI;

namespace YouthSpice.StoryEditorScene.Files
{
	/// <summary>
	/// 챕터 프로젝트 파일의 IO를 관리합니다.
	/// </summary>
	public class ChapterFileManager : MonoBehaviour
	{
		private ChapterManager chapterManager;
		private UIManager uiManager;
		
		private void Start()
		{
			chapterManager= GetComponent<ChapterManager>();
			uiManager = GetComponent<UIManager>();
			AlertManager.Instance.Show(AlertType.Double, "알림", "기존 파일을 불러올까요, 새로운 파일을 생성할까요?",
				new Dictionary<string, Action>() { { "새로 만들기", CreateNewFile }, { "불러오기", LoadExistFile } });
		}

		public void CreateNewFileAlert() => AlertManager.Instance.Show(AlertType.Double, "경고",
			"이 작업은 모든 진행상황을 초기화 합니다.\n계속할까요?", new Dictionary<string, Action>()
			{
				{ "계속", CreateNewFile },
				{ "취소", null }
			});
		private void CreateNewFile() => chapterManager.NewData();

		/// <summary>
		/// 파일을 저장합니다.
		/// </summary>
		public void SaveFile()
		{
			
		}

		public  void LoadExistFileAlert() => AlertManager.Instance.Show(AlertType.Double, "경고",
			"이 작업은 모든 진행상황을 초기화 합니다.\n계속할까요?", new Dictionary<string, Action>()
			{
				{ "계속", LoadExistFile },
				{ "취소", null }
			});
		private void LoadExistFile() => StartCoroutine(LoadExistFileCoroutine());

		private IEnumerator LoadExistFileCoroutine()
		{
			FileBrowser.SetFilters(true, new FileBrowser.Filter("Chapter File", ".cpt"));
			FileBrowser.SetDefaultFilter(".cpt");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			while (true)
			{
				uiManager.SetCustomCoverActive(true);
				yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, @"C:\", null,
					"Select Chapter File...", "Load");

				if (FileBrowser.Success)
				{
					uiManager.SetCustomCoverActive(false);
					AlertManager.Instance.Show(AlertType.None, "알림", $"파일을 불러오는 중 입니다...: {FileBrowser.Result[0]}",
						new Dictionary<string, Action>() { });
					Chapter data = JsonUtility.FromJson<Chapter>(FileBrowser.Result[0]);
					chapterManager.LoadData(data);
					AlertManager.Instance.Pop();
					break;
				}
			}
		}
	}
}
