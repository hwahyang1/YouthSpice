using System.Collections;
using System.Collections.Generic;
using Serializable = System.SerializableAttribute;

using UnityEngine;

namespace YouthSpice
{
	/// <summary>
	/// 해금된 CG 목록을 관리합니다.
	/// </summary>
	[Serializable]
	public class DefineUnlockedCGs
	{
		public List<int> illusts;
		public List<int> recipeFoods;
		public List<int> researchItems;

		public DefineUnlockedCGs()
		{
			illusts = new List<int>();
			recipeFoods = new List<int>();
			researchItems = new List<int>();
		}
	}
}
