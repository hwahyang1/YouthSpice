using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;

using NaughtyAttributes;

namespace YouthSpice.StoryScene.Chapter
{
	/// <summary>
	/// 호감도를 관리합니다.
	/// </summary>
	public class Friendship : MonoBehaviour
	{
		[Header("류한나, 소여울, 태은호")]
		[SerializeField, ReadOnly]
		private int[] friendshipAdjustValue = new int[3];

		public void AdjustFromElement(Dictionary<string, string> data, Action callback = null)
		{
			int character = int.Parse(data.ContainsKey("Character") ? data["Character"] : "0");
			int adjust = int.Parse(data.ContainsKey("Friendship") ? data["Friendship"] : "0");
			Adjust(character, adjust);

			callback?.Invoke();
		}

		public void Adjust(int character, int value)
		{
			character -= 2; // 이름없음, ???, 플레이어
			if (character < 0 || character >= friendshipAdjustValue.Length) return;
			friendshipAdjustValue[character] += value;
		}

		public void Apply()
		{
			GameInfo.Instance.friendship[0] += friendshipAdjustValue[0];
			GameInfo.Instance.friendship[1] += friendshipAdjustValue[1];
			GameInfo.Instance.friendship[2] += friendshipAdjustValue[2];
		}
	}
}
