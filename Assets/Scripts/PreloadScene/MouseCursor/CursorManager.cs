using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.PreloadScene.MouseCursor
{
	/// <summary>
	/// 커서를 관리합니다.
	/// </summary>
	public class CursorManager : Singleton<CursorManager>
	{
		[SerializeField]
		private Texture2D[] cursor;

		protected override void Awake()
		{
			base.Awake();

			Cursor.SetCursor(cursor[0], Vector3.zero, CursorMode.ForceSoftware);
		}

		public void SetCursorImage(int index) => Cursor.SetCursor(cursor[index], Vector3.zero, CursorMode.ForceSoftware);
	}
}
