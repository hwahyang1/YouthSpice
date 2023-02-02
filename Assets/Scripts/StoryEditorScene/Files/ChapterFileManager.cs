using System.Collections;
using System.Collections.Generic;
using System.IO;
using Action = System.Action;

using UnityEngine;

using Newtonsoft.Json;
using SimpleFileBrowser;

using YouthSpice.PreloadScene.Alert;
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

		private string pastFilePath = "";
		private string pastFileName = "";

		private void Start()
		{
			chapterManager = GetComponent<ChapterManager>();
			uiManager = GetComponent<UIManager>();
		}

		public void ShowFirstScreen() => AlertManager.Instance.Show(AlertType.Double, "알림",
			"기존 파일을 불러올까요, 새로운 파일을 생성할까요?",
			new Dictionary<string, Action>() { { "새로 만들기", CreateNewFile }, { "불러오기", LoadExistFile } });

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
			if (pastFilePath != "" && pastFileName != "")
			{
				AlertManager.Instance.Show(AlertType.Double, "알림",
					$"기존 파일이 존재합니다: {pastFilePath}/{pastFileName}\n기존 파일에 덮어씌울까요?",
					new Dictionary<string, Action>()
					{
						{ "아니요(다른 파일에 저장)", () => { StartCoroutine(SaveNewFileCoroutine()); } },
						{ "예(기존 파일 덮어쓰기)", () => { StartCoroutine(OverwriteFileCoroutine()); } }
					});
			}
			else
			{
				StartCoroutine(SaveNewFileCoroutine());
			}
		}

		private IEnumerator OverwriteFileCoroutine()
		{
			yield return null;

			string path = pastFilePath + pastFileName;
			AlertManager.Instance.Show(AlertType.None, "알림", $"파일을 저장하는 중 입니다...: {path}",
				new Dictionary<string, Action>() { });

			yield return null;

			//File.WriteAllText(path, JsonUtility.ToJson(chapterManager.GetData()));
			File.WriteAllText(path, JsonConvert.SerializeObject(chapterManager.GetData()));

			AlertManager.Instance.Pop();

			yield return null;

			AlertManager.Instance.Show(AlertType.Single, "알림", $"파일을 성공적으로 저장했습니다.: {path}",
				new Dictionary<string, Action>() { { "확인", null } });
		}

		private IEnumerator SaveNewFileCoroutine()
		{
			FileBrowser.SetFilters(true, new FileBrowser.Filter("Chapter File", ".cpt"));
			FileBrowser.SetDefaultFilter(".cpt");
			FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

			uiManager.SetCustomCoverActive(true);
			yield return FileBrowser.WaitForSaveDialog(FileBrowser.PickMode.Files, false, @"C:\", "Chapter.cpt",
				"Select Chapter File...", "Save");

			if (FileBrowser.Success)
			{
				uiManager.SetCustomCoverActive(false);
				AlertManager.Instance.Show(AlertType.None, "알림", $"파일을 저장하는 중 입니다...: {FileBrowser.Result[0]}",
					new Dictionary<string, Action>() { });

				yield return null;

				//File.WriteAllText(FileBrowser.Result[0], JsonUtility.ToJson(chapterManager.GetData()));
				File.WriteAllText(FileBrowser.Result[0], JsonConvert.SerializeObject(chapterManager.GetData()));

				pastFilePath = Path.GetDirectoryName(FileBrowser.Result[0]);
				pastFileName = Path.GetFileName(FileBrowser.Result[0]);

				AlertManager.Instance.Pop();

				yield return null;

				AlertManager.Instance.Show(AlertType.Single, "알림", $"파일을 성공적으로 저장했습니다.: {FileBrowser.Result[0]}",
					new Dictionary<string, Action>() { { "확인", null } });
			}
		}

		public void LoadExistFileAlert() => AlertManager.Instance.Show(AlertType.Double, "경고",
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

					yield return new WaitForSeconds(0.1f);

					//Chapter data = JsonUtility.FromJson<Chapter>(File.ReadAllText(FileBrowser.Result[0]));
					Chapter data = JsonConvert.DeserializeObject<Chapter>(File.ReadAllText(FileBrowser.Result[0]));
					chapterManager.LoadData(data);

					pastFilePath = Path.GetDirectoryName(FileBrowser.Result[0]);
					pastFileName = Path.GetFileName(FileBrowser.Result[0]);

					AlertManager.Instance.Pop();
					break;
				}
			}
		}
	}
}
