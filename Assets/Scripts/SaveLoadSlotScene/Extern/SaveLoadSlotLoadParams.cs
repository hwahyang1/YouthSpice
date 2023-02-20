using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.SaveLoadSlotScene.Extern
{
	/// <summary>
	/// Scene의 실행 모드를 지정합니다.
	/// </summary>
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
