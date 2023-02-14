using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using UnityEngine;

namespace YouthSpice.PreloadScene.Files
{
	/// <summary>
	/// 해금된 CG 목록을 관리합니다.
	/// </summary>
	public class UnlockedCGsManager : Singleton<UnlockedCGsManager>
	{
		private DefineUnlockedCGs data;
		private string filePath;
		
		private BinaryFormatter formatter = new BinaryFormatter();

		protected override void Awake()
		{
			base.Awake();

			filePath = Application.persistentDataPath + @"\YouthSpice.ulk";
			
			LoadFromFile();
		}

		public DefineUnlockedCGs GetAllData()
		{
			return data;
		}

		// 게임 정보는 GameInfo에서 가져옴
		public void Save(DefineUnlockedCGs newData)
		{
			data = newData;
			SaveToFile();
		}

		private void SaveToFile()
		{
			FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			formatter.Serialize(stream, data);
			stream.Close();
		}

		private void LoadFromFile()
		{
			if (!Directory.Exists(filePath))
			{
				data = new DefineUnlockedCGs();
				return;
			}
			
			FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
			data = formatter.Deserialize(stream) as DefineUnlockedCGs;
			stream.Close();
		}
	}
}
