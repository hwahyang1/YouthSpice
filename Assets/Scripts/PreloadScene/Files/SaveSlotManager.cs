using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using DateTimeOffset = System.DateTimeOffset;
using Action = System.Action;

using UnityEngine;
using UnityEngine.Networking;

using YouthSpice.PreloadScene.Alert;
using YouthSpice.PreloadScene.Game;
using YouthSpice.PreloadScene.Scene;

namespace YouthSpice.PreloadScene.Files
{
	/// <summary>
	/// 저장 슬롯을 관리합니다
	/// </summary>
	public class SaveSlotManager : Singleton<SaveSlotManager>
	{
		[SerializeField]
		private Sprite screenshot;
		
		private DefineGame[] slots = new DefineGame[7];
		private string filePath;
		private string fileScreenshotFolderPath;
		
		private BinaryFormatter formatter = new BinaryFormatter();

		protected override void Awake()
		{
			base.Awake();

			filePath = Application.persistentDataPath + @"\YouthSpice.dat";
			fileScreenshotFolderPath = Application.persistentDataPath + @"\ScreenShots_Internal\";
			
			if (!Directory.Exists(fileScreenshotFolderPath)) Directory.CreateDirectory(fileScreenshotFolderPath);
			
			LoadFromFile();
		}

		public List<DefineGame> GetAllData()
		{
			return new List<DefineGame>(slots);
		}

		public DefineGame GetData(int index)
		{
			return slots[index];
		}

		public Sprite GetScreenShot(int slot)
		{
			if (GetData(slot) == null) return null;
			return screenshot;
			
			string path = fileScreenshotFolderPath + @$"S{slot}.png";
			
			Sprite sprite = null;
			if (new FileInfo(path).Length != 0L)
			{
				using (UnityWebRequest webRequest =
				       UnityWebRequestTexture.GetTexture(path))
				{
					webRequest.SendWebRequest();

					while (!webRequest.isDone) /*await*/ Task.Delay(5);

					if (webRequest.result == UnityWebRequest.Result.ConnectionError)
					{
						Debug.LogError($"Sprite Import Error!\n{path}");
						AlertManager.Instance.Show(AlertType.Single, "경고",
							$"Sprite Import Error!\n{path}",
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

			return sprite;
		}

		// 게임 정보는 GameInfo에서 가져옴
		public void Save(int slot)
		{
			GameInfo.Instance.dateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			slots[slot] = GameInfo.Instance.ConvertToDefineGame();

			//StartCoroutine(TakeScreenShot(slot));
			
			SaveToFile();
		}

		private IEnumerator TakeScreenShot(int slot)
		{
			GameObject[] objects = GameObject.FindGameObjectsWithTag("ScreenshotExcept");

			foreach (GameObject current in objects)
			{
				current.SetActive(false);
			}
			
			yield return null;
			
			ScreenCapture.CaptureScreenshot(fileScreenshotFolderPath + @$"S{slot}.png");

			foreach (GameObject current in objects)
			{
				current.SetActive(true);
			}
		}

		private void SaveToFile()
		{
			FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			formatter.Serialize(stream, slots);
			stream.Close();
			
			LoadFromFile();
		}

		private void LoadFromFile()
		{
			if (!File.Exists(filePath)) return;
			
			FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
			slots = formatter.Deserialize(stream) as DefineGame[];
			stream.Close();
		}

		public void Load(int slot)
		{
			GameInfo.Instance.ConvertFromDefineGame(slots[slot]);
			GameProgressManager.Instance.RunThisChapter();
		}
	}
}
