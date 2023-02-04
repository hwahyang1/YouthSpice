using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Action = System.Action;
using Array = System.Array;

using UnityEngine;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Alert;
using YouthSpice.StoryEditorScene.UI;

namespace YouthSpice.StoryEditorScene.Files
{
	/// <summary>
	/// 에디터 파일의 IO를 관리합니다.
	/// </summary>
	public class EditorDataManager : MonoBehaviour
	{
		private string rootPath;

		private readonly string[] rootPathFolders = new string[]
			{ "Audios", "BackgroundImages", "DayImages", "StandingIllusts" };

		private readonly string[] rootPathFiles = new string[]
			{ "AvailableAudioTransitions", "AvailableCharacters", "AvailableImageTransitions", "Info" };

		[Header("Version")]
		[SerializeField]
		private int editorVersion;

		[SerializeField, ReadOnly]
		private int fileVersion;

		[Header("Files")]
		[SerializeField, ReadOnly]
		private string[] availableAudios;

		public string[] AvailableAudios => availableAudios;

		[SerializeField, ReadOnly]
		private string[] availableBackgroundImages;

		public string[] AvailableBackgroundImages => availableBackgroundImages;

		[SerializeField, ReadOnly]
		private string[] availableDayImages;

		public string[] AvailableDayImages => availableDayImages;

		[SerializeField, ReadOnly]
		private string[] availableStandingIllusts;

		public string[] AvailableStandingIllusts => availableStandingIllusts;

		[SerializeField, ReadOnly]
		private string[] availableAudioTransitions;

		public string[] AvailableAudioTransitions => availableAudioTransitions;

		[SerializeField, ReadOnly]
		private string[] availableCharacters;

		public string[] AvailableCharacters => availableCharacters;

		[SerializeField, ReadOnly]
		private string[] availableImageTransitions;

		public string[] AvailableImageTransitions => availableImageTransitions;

		private void Awake()
		{
			rootPath = Application.dataPath + @"/.StoryEditor_Data";
			GetComponent<UIManager>().UpdateDataVersion(editorVersion, 0);
		}

		private void Start()
		{
			for (int i = 0; i < rootPathFolders.Length; i++)
			{
				string currentPath = $"{rootPath}/{rootPathFolders[i]}";

				if (!Directory.Exists(currentPath))
				{
					ShowNotFound(currentPath);
					return;
				}
			}

			for (int i = 0; i < rootPathFiles.Length; i++)
			{
				string currentPath = $"{rootPath}/{rootPathFiles[i]}";

				if (!File.Exists(currentPath))
				{
					ShowNotFound(currentPath);
					return;
				}
			}

			try
			{
				fileVersion = int.Parse(File.ReadAllText($"{rootPath}/{rootPathFiles[3]}"));
				availableImageTransitions = File.ReadAllText($"{rootPath}/{rootPathFiles[2]}").Split("\n");
				availableCharacters = File.ReadAllText($"{rootPath}/{rootPathFiles[1]}").Split("\n");
				availableAudioTransitions = File.ReadAllText($"{rootPath}/{rootPathFiles[2]}").Split("\n");

				availableAudios = Directory.GetFiles($"{rootPath}/{rootPathFolders[0]}");
				availableAudios = Array.FindAll(availableAudios, target => !target.EndsWith(".gitkeep")).ToArray();
				for (int i = 0; i < availableAudios.Length; i++)
					availableAudios[i] = availableAudios[i].Replace($@"{rootPath}/{rootPathFolders[0]}\", "");

				availableBackgroundImages = Directory.GetFiles($"{rootPath}/{rootPathFolders[1]}");
				availableBackgroundImages = Array.FindAll(availableBackgroundImages, target => !target.EndsWith(".gitkeep")).ToArray();
				for (int i = 0; i < availableBackgroundImages.Length; i++)
					availableBackgroundImages[i] =
						availableBackgroundImages[i].Replace($@"{rootPath}/{rootPathFolders[1]}\", "");

				availableDayImages = Directory.GetFiles($"{rootPath}/{rootPathFolders[2]}");
				availableDayImages = Array.FindAll(availableDayImages, target => !target.EndsWith(".gitkeep")).ToArray();
				for (int i = 0; i < availableDayImages.Length; i++)
					availableDayImages[i] = availableDayImages[i].Replace($@"{rootPath}/{rootPathFolders[2]}\", "");

				availableStandingIllusts = Directory.GetFiles($"{rootPath}/{rootPathFolders[3]}");
				availableStandingIllusts = Array.FindAll(availableStandingIllusts, target => !target.EndsWith(".gitkeep")).ToArray();
				for (int i = 0; i < availableStandingIllusts.Length; i++)
					availableStandingIllusts[i] =
						availableStandingIllusts[i].Replace($@"{rootPath}/{rootPathFolders[3]}\", "");
			}
			catch (System.Exception e)
			{
				ShowNotFound(e.Message);
				return;
			}

			GetComponent<UIManager>().UpdateDataVersion(editorVersion, fileVersion);
			GetComponent<ChapterFileManager>().ShowFirstScreen();

			if (availableAudios.Length == 0) ShowWarning($"{rootPath}/{rootPathFolders[0]}");
			if (availableDayImages.Length == 0) ShowWarning($"{rootPath}/{rootPathFolders[1]}");
			if (availableBackgroundImages.Length == 0) ShowWarning($"{rootPath}/{rootPathFolders[2]}");
			if (availableStandingIllusts.Length == 0) ShowWarning($"{rootPath}/{rootPathFolders[3]}");
		}

		private void ShowNotFound(string path) => AlertManager.Instance.Show(AlertType.None, "경고",
			$"필수 데이터에 문제가 있어 에디터를 실행 할 수 없습니다.\n\nInfo: {path}", new Dictionary<string, Action>());

		private void ShowWarning(string path) => AlertManager.Instance.Show(AlertType.Single, "경고",
			$"데이터 폴더에 파일이 없습니다.\n에디터 사용이 가능하나, 일부 기능에 제약이 있을 수 있습니다.\n\nPath: {path}",
			new Dictionary<string, Action>() { { "닫기", null } });
	}
}
