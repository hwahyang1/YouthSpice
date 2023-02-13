using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.SaveLoadSlotScene.Extern
{
	public enum SaveLoadSlotMode
	{
		Save,
		Load
	}
	
	/// <summary>
	/// 실행 값을 지정합니다.
	/// </summary>
	public class SaveLoadSlotLoadParams : Singleton<SaveLoadSlotLoadParams>
	{
		public SaveLoadSlotMode mode;

		public void Exit()
		{
			Destroy(gameObject);
		}
	}
}
