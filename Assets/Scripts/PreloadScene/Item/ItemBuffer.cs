using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.PreloadScene.Item
{
	/// <summary>
	/// Description
	/// </summary>
	public class ItemBuffer : Singleton<ItemBuffer>
	{
		public List<ItemProperty> items;

		/// <summary>
		/// 아이템의 이름을 기준으로 아이템 번호를 반환합니다
		/// </summary>
		/// <param name="name">찾을 아이템의 이름을 지정합니다.</param>
		/// <returns></returns>
		public int GetIndex(string name)
		{
			return items.FindIndex(target => target.name == name);
		}
	}
}
