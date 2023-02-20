using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.StoryScene.Extern
{
	/// <summary>
	/// 로드할 챕터 정보를 지정합니다.
	/// </summary>
	public class StorySceneLoadParams : Singleton<StorySceneLoadParams>
	{
		/// <summary>
		/// 지정되지 않은 경우, 'Resources/Resources'에서 가져옵니다.
		/// </summary>
		public string resourceCustomPath = null;

		/// <summary>
		/// 지정되지 않은 경우, 'Resources/Chapters'에서 가져옵니다.
		/// </summary>
		public string chapterCustomPath = null;

		public string chapterID;

		public bool isTutorialScene = false;

		public void Exit()
		{
			Destroy(gameObject);
		}
	}
}
