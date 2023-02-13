using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DateTimeOffset = System.DateTimeOffset;

using UnityEngine;

namespace YouthSpice.PreloadScene.Files
{
	/// <summary>
	/// 저장 슬롯을 관리합니다
	/// </summary>
	public class SaveSlotManager : Singleton<SaveSlotManager>
	{
		private DefineGame[] slots = new DefineGame[7];
		private string filePath;
		
		private BinaryFormatter formatter = new BinaryFormatter();

		protected override void Awake()
		{
			base.Awake();

			filePath = Application.persistentDataPath + @"\YouthSpice.dat";
			
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

		// 게임 정보는 GameInfo에서 가져옴
		public void Save(int slot, string title)
		{
			GameInfo.Instance.slotName = title;
			GameInfo.Instance.dateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			slots[slot] = GameInfo.Instance.ConvertToDefineGame();
			
			SaveToFile();
		}

		private void SaveToFile()
		{
			FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			formatter.Serialize(stream, slots);
			stream.Close();
		}

		private void LoadFromFile()
		{
			if (!Directory.Exists(filePath)) return;
			
			FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
			slots = formatter.Deserialize(stream) as DefineGame[];
			stream.Close();
		}
	}
}
