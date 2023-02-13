using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.CookingScene.Extern
{
	public enum AvailableMenus
	{
		떡볶이,
		약과,
		우럭매운탕
	}
	
	/// <summary>
	/// 실행에 필요한 정보를 지정합니다.
	/// </summary>
	public class CookingLoadParams : Singleton<CookingLoadParams>
	{
		public AvailableMenus menu;

		public void Exit()
		{
			Destroy(gameObject);
		}
	}
}
