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
    
	[System.Serializable]
	public class ItemProperty
	{
		public string name;
		public Sprite sprite;
		public int sellPrice;
		public ItemRank rank;
		public ItemColor color;
		public ItemField field;
	}
}