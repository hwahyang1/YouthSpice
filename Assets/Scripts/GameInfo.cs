using System.Collections;
using System.Collections.Generic;
using Serializable = System.SerializableAttribute;

using UnityEngine;

namespace YouthSpice
{
	/// <summary>
	/// 게임 진행에 필요한 정보를 정의합니다.
	/// </summary>
	[Serializable]
	public class DefineGame
	{
		public int MajorChapter;
		public int MinorChapter;
		
		public string PlayerName;

		public int[] Inventory;

		public int[] Friendship;

		public DefineGame()
		{
			//
		}

		public DefineGame(int majorChapter, int minorChapter, string playerName, int[] inventory, int[] friendship)
		{
			MajorChapter = majorChapter;
			MinorChapter = minorChapter;
			PlayerName = playerName;
			Inventory = inventory;
			Friendship = friendship;
		}
	}

	/// <summary>
	/// 현재 게임 정보를 정의합니다.
	/// </summary>
	public class GameInfo : Singleton<GameInfo>
	{
		public int majorChapter;
		public int minorChapter;
		
		public string playerName;

		public List<int> inventory = new List<int>();

		public int[] friendship = new int[3];

		public DefineGame ConvertToDefineGame()
		{
			return new DefineGame(majorChapter, minorChapter, playerName, inventory.ToArray(), friendship);
		}

		public void ConvertFromDefineGame(DefineGame data)
		{
			majorChapter = data.MajorChapter;
			minorChapter = data.MinorChapter;

			playerName = data.PlayerName;

			inventory = new List<int>(data.Inventory);

			friendship = data.Friendship;
		}
	}
}
