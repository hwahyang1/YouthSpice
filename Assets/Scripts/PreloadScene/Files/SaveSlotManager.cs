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

		/// <summary>
		/// 모든 슬롯의 데이터를 반환합니다.
		/// </summary>
		public List<DefineGame> GetAllData()
		{
			return new List<DefineGame>(slots);
		}

		/// <summary>
		/// 특정 슬롯의 데이터를 반환합니다.
		/// </summary>
		/// <param name="index">조회할 슬롯을 지정합니다.</param>
		/// <returns>해당 슬롯의 데이터가 반환됩니다.</returns>
		public DefineGame GetData(int index)
		{
			return slots[index];
		}

		/// <summary>
		/// 스크린샷을 촬영합니다.
		/// </summary>
		/// <param name="slot">저장할 슬롯을 지정합니다.</param>
		/// <returns>촬영된 이미지가 반환됩니다.</returns>
		/// <remarks>슬롯이 비어있는 경우, 촬영하지 않습니다.</remarks>
		/// <remarks>최적화 및 기타 문제로 현재는 기본 이미지가 반환됩니다.</remarks>
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

		/// <summary>
		/// 현재 진행 정보를 저장합니다.
		/// </summary>
		/// <param name="slot">저장할 슬롯을 지정합니다.</param>
		/// <remarks>이 Method는 저장할 데이터를 GameInfo에서 가져옵니다.</remarks>
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

		/// <summary>
		/// 메모리 상에 있는 슬롯 데이터를 파일로 저장합니다.
		/// </summary>
		private void SaveToFile()
		{
			FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			formatter.Serialize(stream, slots);
			stream.Close();

			LoadFromFile();
		}

		/// <summary>
		/// 파일에 있는 슬롯 데이터를 메모리로 읽어옵니다.
		/// </summary>
		private void LoadFromFile()
		{
			if (!File.Exists(filePath)) return;

			FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
			slots = formatter.Deserialize(stream) as DefineGame[];
			stream.Close();
		}

		/// <summary>
		/// 특정 슬롯의 데이터를 불러옵니다.
		/// </summary>
		/// <param name="slot">불러올 슬롯을 지정합니다.</param>
		public void Load(int slot)
		{
			GameInfo.Instance.ConvertFromDefineGame(slots[slot]);
			GameProgressManager.Instance.RunThisChapter();
		}
	}
}
