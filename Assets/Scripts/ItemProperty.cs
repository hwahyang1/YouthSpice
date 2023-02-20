using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice
{
	/// <summary>
	/// 아이템의 등급을 정의합니다.
	/// </summary>
	public enum ItemRank
	{
		Low,
		MediumLow,
		MediumHigh,
		High
	}

	/// <summary>
	/// 아이템의 색상을 정의합니다.
	/// </summary>
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

	/// <summary>
	/// 아이템의 출현 지역을 정의합니다.
	/// </summary>
	public enum ItemField
	{
		Mountain,
		Ground,
		Ocean,
		ShopOnly
	}

	/// <summary>
	/// 아이템의 대구분을 지정합니다.
	/// </summary>
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

	/// <summary>
	/// 개별 아이템을 정의합니다.
	/// </summary>
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
