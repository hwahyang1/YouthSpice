using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Array = System.Array;
using Action = System.Action;

using UnityEngine;
using UnityEngine.Networking;

using Newtonsoft.Json;
using NaughtyAttributes;

using YouthSpice.PreloadScene.Alert;
using YouthSpice.StoryScene.Extern;

namespace YouthSpice.PreloadScene.Files
{
	/// <summary>
	/// 소스 파일을 관리합니다.
	/// </summary>
	public class SourceFileManager : Singleton<SourceFileManager>
	{
		private readonly string[] sourceFolders = new string[]
			{ "Audios", "BackgroundImages", "DayImages", "StandingIllusts" };

		[Header("Paths")]
		[SerializeField, ReadOnly]
		private bool isCustomResourceFolder;

		[SerializeField, ReadOnly]
		private bool isCustomChapterFolder;

		[Header("Files")]
		[SerializeField, ReadOnly]
		private List<AudioClip> availableAudios = new List<AudioClip>();

		public List<AudioClip> AvailableAudios => availableAudios;

		[SerializeField, ReadOnly]
		private List<Sprite> availableBackgroundImages = new List<Sprite>();

		public List<Sprite> AvailableBackgroundImages => availableBackgroundImages;

		[SerializeField, ReadOnly]
		private List<Sprite> availableDayImages = new List<Sprite>();

		public List<Sprite> AvailableDayImages => availableDayImages;

		[SerializeField, ReadOnly]
		private List<Sprite> availableStandingIllusts = new List<Sprite>();

		public List<Sprite> AvailableStandingIllusts => availableStandingIllusts;

		private Dictionary<string, DefineChapter> availableChapters = new Dictionary<string, DefineChapter>();
		public Dictionary<string, DefineChapter> AvailableChapters => availableChapters;

		protected override async void Awake()
		{
			base.Awake();

			await RefreshAll();
		}

		/// <summary>
		/// 모든 리소스를 다시 불러옵니다.
		/// </summary>
		/// <remarks>
		///	이 작업은 사양에 따라 수 분 이상 걸리는 작업입니다.
		/// </remarks>
		public async Task RefreshAll()
		{
			availableAudios.Clear();
			availableBackgroundImages.Clear();
			availableDayImages.Clear();
			availableStandingIllusts.Clear();
			availableChapters.Clear();

			isCustomResourceFolder = StorySceneLoadParams.Instance.resourceCustomPath != null;
			isCustomChapterFolder = StorySceneLoadParams.Instance.chapterCustomPath != null;

			if (isCustomResourceFolder)
			{
				string[] rawAvailableAudios =
					Directory.GetFiles($"{StorySceneLoadParams.Instance.resourceCustomPath}/{sourceFolders[0]}");
				rawAvailableAudios =
					Array.FindAll(rawAvailableAudios, target => !target.EndsWith(".gitkeep")).ToArray();
				foreach (string currentAudioPath in rawAvailableAudios)
				{
					AudioClip clip = null;
					using (UnityWebRequest webRequest =
					       UnityWebRequestMultimedia.GetAudioClip(currentAudioPath, AudioType.MPEG))
					{
						webRequest.SendWebRequest();

						while (!webRequest.isDone) await Task.Delay(5);

						if (webRequest.result == UnityWebRequest.Result.ConnectionError)
						{
							Debug.LogError($"AudioClip Import Error!\n{currentAudioPath}");
							AlertManager.Instance.Show(AlertType.Single, "경고",
								$"AudioClip Import Error!\n{currentAudioPath}",
								new Dictionary<string, Action>() { { "닫기", null } });
						}
						else
						{
							clip = DownloadHandlerAudioClip.GetContent(webRequest);
						}
					}

					availableAudios.Add(clip);
				}

				string[] rawAvailableBackgroundImages =
					Directory.GetFiles($"{StorySceneLoadParams.Instance.resourceCustomPath}/{sourceFolders[1]}");
				rawAvailableBackgroundImages = Array
				                               .FindAll(rawAvailableBackgroundImages,
					                               target => !target.EndsWith(".gitkeep")).ToArray();
				foreach (string currentBackgroundImagePath in rawAvailableBackgroundImages)
				{
					Sprite sprite = null;
					if (new FileInfo(currentBackgroundImagePath).Length != 0L)
					{
						using (UnityWebRequest webRequest =
						       UnityWebRequestTexture.GetTexture(currentBackgroundImagePath))
						{
							webRequest.SendWebRequest();

							while (!webRequest.isDone) await Task.Delay(5);

							if (webRequest.result == UnityWebRequest.Result.ConnectionError)
							{
								Debug.LogError($"Sprite Import Error!\n{currentBackgroundImagePath}");
								AlertManager.Instance.Show(AlertType.Single, "경고",
									$"Sprite Import Error!\n{currentBackgroundImagePath}",
									new Dictionary<string, Action>() { { "닫기", null } });
							}
							else
							{
								Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
								sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
									new Vector2(0.5f, 0.5f));
							}
						}
					}

					availableBackgroundImages.Add(sprite);
				}

				string[] rawAvailableDayImages =
					Directory.GetFiles($"{StorySceneLoadParams.Instance.resourceCustomPath}/{sourceFolders[2]}");
				rawAvailableDayImages = Array.FindAll(rawAvailableDayImages, target => !target.EndsWith(".gitkeep"))
				                             .ToArray();
				foreach (string currentDayImagePath in rawAvailableDayImages)
				{
					Sprite sprite = null;
					if (new FileInfo(currentDayImagePath).Length != 0L)
					{
						using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(currentDayImagePath))
						{
							webRequest.SendWebRequest();

							while (!webRequest.isDone) await Task.Delay(5);

							if (webRequest.result == UnityWebRequest.Result.ConnectionError)
							{
								Debug.LogError($"Sprite Import Error!\n{currentDayImagePath}");
								AlertManager.Instance.Show(AlertType.Single, "경고",
									$"Sprite Import Error!\n{currentDayImagePath}",
									new Dictionary<string, Action>() { { "닫기", null } });
							}
							else
							{
								Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
								sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
									new Vector2(0.5f, 0.5f));
							}
						}
					}

					availableDayImages.Add(sprite);
				}

				string[] rawAvailableStandingIllusts =
					Directory.GetFiles($"{StorySceneLoadParams.Instance.resourceCustomPath}/{sourceFolders[3]}");
				rawAvailableStandingIllusts = Array
				                              .FindAll(rawAvailableStandingIllusts,
					                              target => !target.EndsWith(".gitkeep")).ToArray();
				foreach (string currentStandingIllustsPath in rawAvailableStandingIllusts)
				{
					Sprite sprite = null;
					if (new FileInfo(currentStandingIllustsPath).Length != 0L)
					{
						using (UnityWebRequest webRequest =
						       UnityWebRequestTexture.GetTexture(currentStandingIllustsPath))
						{
							webRequest.SendWebRequest();

							while (!webRequest.isDone) await Task.Delay(5);

							if (webRequest.result == UnityWebRequest.Result.ConnectionError)
							{
								Debug.LogError($"Sprite Import Error!\n{currentStandingIllustsPath}");
								AlertManager.Instance.Show(AlertType.Single, "경고",
									$"Sprite Import Error!\n{currentStandingIllustsPath}",
									new Dictionary<string, Action>() { { "닫기", null } });
							}
							else
							{
								Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
								sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
									new Vector2(0.5f, 0.5f));
							}
						}
					}

					availableStandingIllusts.Add(sprite);
				}
			}
			else
			{
				availableAudios = new List<AudioClip>(Resources.LoadAll<AudioClip>($"Resources/{sourceFolders[0]}"));
				availableBackgroundImages =
					new List<Sprite>(Resources.LoadAll<Sprite>($"Resources/{sourceFolders[1]}"));
				availableDayImages = new List<Sprite>(Resources.LoadAll<Sprite>($"Resources/{sourceFolders[2]}"));
				availableStandingIllusts = new List<Sprite>(Resources.LoadAll<Sprite>($"Resources/{sourceFolders[3]}"));
			}

			availableChapters = new Dictionary<string, DefineChapter>();
			if (isCustomChapterFolder)
			{
				string[] rawChapters = Directory.GetFiles(StorySceneLoadParams.Instance.chapterCustomPath);
				rawChapters = Array
				              .FindAll(rawChapters, target => (!target.EndsWith(".gitkeep") && target.EndsWith(".cpt")))
				              .ToArray();
				foreach (string currentChapter in rawChapters)
				{
					DefineChapter data =
						JsonConvert.DeserializeObject<DefineChapter>(await File.ReadAllTextAsync(currentChapter));
					availableChapters.Add(data.ID, data);
				}
			}
			else
			{
				TextAsset[] rawChapters = Resources.LoadAll<TextAsset>("Chapters");
				foreach (TextAsset chapter in rawChapters)
				{
					DefineChapter data = JsonConvert.DeserializeObject<DefineChapter>(chapter.text);
					availableChapters.Add(data.ID, data);
				}
			}
		}
	}
}
