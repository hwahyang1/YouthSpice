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
		public string SlotName;
		public long DateTime;

		public int MajorChapter;
		public int MinorChapter;
		
		public string PlayerName;

		public int[] Inventory;
		public int Money;

		public int[] Friendship;

		public DefineGame()
		{
			//
		}

		public DefineGame(string slotName, long dateTime, int majorChapter, int minorChapter, string playerName, int[] inventory, int money, int[] friendship)
		{
			SlotName = slotName;
			DateTime = dateTime;
			MajorChapter = majorChapter;
			MinorChapter = minorChapter;
			PlayerName = playerName;
			Inventory = inventory;
			Money = money;
			Friendship = friendship;
		}
	}

	/// <summary>
	/// 현재 게임 정보를 정의합니다.
	/// </summary>
	public class GameInfo : Singleton<GameInfo>
	{
		public string slotName;
		public long dateTime;
		
		public int majorChapter;
		public int minorChapter;
		
		public string playerName;

		public List<int> inventory = new List<int>();
		public int money;

		public int[] friendship = new int[3];

		public DefineGame ConvertToDefineGame()
		{
			return new DefineGame(slotName, dateTime, majorChapter, minorChapter, playerName, inventory.ToArray(), money, friendship);
		}

		public void ConvertFromDefineGame(DefineGame data)
		{
			slotName = data.SlotName;
			dateTime = data.DateTime;
			
			majorChapter = data.MajorChapter;
			minorChapter = data.MinorChapter;

			playerName = data.PlayerName;

			inventory = new List<int>(data.Inventory);
			money = data.Money;

			friendship = data.Friendship;
		}

		public void Exit()
		{
			Destroy(gameObject);
		}
	}
}
