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
		[SerializeField]
		private DefineUnlockedCGs data;

		private string filePath;

		private readonly BinaryFormatter formatter = new BinaryFormatter();

		protected override void Awake()
		{
			base.Awake();

			filePath = Application.persistentDataPath + @"\YouthSpice.ulk";

			LoadFromFile();
		}

		/// <summary>
		/// 모든 해금된 CG 목록을 가져옵니다.
		/// </summary>
		public DefineUnlockedCGs GetAllData()
		{
			return data;
		}

		/// <summary>
		/// 해금된 CG 목록을 갱신합니다.
		/// </summary>
		/// <param name="newData">덮어씌울 새 데이터를 지정합니다.</param>
		public void Save(DefineUnlockedCGs newData)
		{
			data = newData;
			SaveToFile();
		}

		/// <summary>
		/// 메모리 상에 있는 해금된 CG 데이터를 파일로 저장합니다.
		/// </summary>
		private void SaveToFile()
		{
			FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
			formatter.Serialize(stream, data);
			stream.Close();
		}

		/// <summary>
		/// 파일에 있는 해금된 CG 데이터를 메모리로 읽어옵니다.
		/// </summary>
		private void LoadFromFile()
		{
			if (!File.Exists(filePath))
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
