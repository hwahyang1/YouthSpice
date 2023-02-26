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

		public bool ViewedRecipe;
		public bool ViewedItem;
		public bool ViewedShop;
		public bool ViewedResearch;

		public DefineGame()
		{
			//
		}

		public DefineGame(
			string slotName,
			long dateTime,
			int majorChapter,
			int minorChapter,
			string playerName,
			int[] inventory,
			int money,
			int[] friendship,
			bool viewedRecipe,
			bool viewedItem,
			bool viewedShop,
			bool viewedResearch
		)
		{
			SlotName = slotName;
			DateTime = dateTime;
			MajorChapter = majorChapter;
			MinorChapter = minorChapter;
			PlayerName = playerName;
			Inventory = inventory;
			Money = money;
			Friendship = friendship;
			ViewedRecipe = viewedRecipe;
			ViewedItem = viewedItem;
			ViewedShop = viewedShop;
			ViewedResearch = viewedResearch;
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

		public bool viewedRecipe = false;
		public bool viewedItem = false;
		public bool viewedShop = false;
		public bool viewedResearch = false;

		public DefineGame ConvertToDefineGame()
		{
			return new DefineGame(slotName, dateTime, majorChapter, minorChapter, playerName, inventory.ToArray(),
				money, friendship, viewedRecipe, viewedItem, viewedShop, viewedResearch);
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

			viewedRecipe = data.ViewedRecipe;
			viewedItem = data.ViewedItem;
			viewedShop = data.ViewedShop;
			viewedResearch = data.ViewedResearch;
		}

		public void Exit()
		{
			Destroy(gameObject);
		}
	}
}
