using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.StoryScene.Audio
{
	/// <summary>
	/// 오디오 재생을 관리합니다.
	/// </summary>
	public class AudioManager : MonoBehaviour
	{
		[SerializeField]
		private AudioSource[] backgroundAudios;

		[SerializeField]
		private AudioSource effectAudios;
	}
}
