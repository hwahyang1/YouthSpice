using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.StoryEditorScene.Element.Child
{
	/// <summary>
	/// 플레이어 이름 획득을 정의하는 자식 스크립트입니다.
	/// </summary>
	public class GetPlayerNameElementGroup : ElementGroup
	{
		protected override void Init(Dictionary<string, string> data)
		{
			//
		}

		public override Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>();
		}
	}
}
