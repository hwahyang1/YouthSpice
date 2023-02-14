using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice
{
	public enum ItemRank
	{
		Low,
		MediumLow,
		MediumHigh,
		High
	}

	public enum ItemColor
	{
		Red,
		White,
		Green,
		Yellow,
		Pink,
		Orange,
		Black,
		Brown,
		None
	}

	public enum ItemField
	{
		Mountain,
		Ground,
		Ocean,
		ShopOnly
	}

	public enum ItemSet
	{
		과일모둠,
		채소모둠,
		고기모둠,
		해산물모둠,
		혼합된소스모둠,
		떡,
		기본약과,
		없음
	}
    
	[System.Serializable]
	public class ItemProperty
	{
		public string name;
		public Sprite sprite;
		public int sellPrice;
		public ItemRank rank;
		public ItemColor color;
		public ItemField field;
		public ItemSet set;
	}
}