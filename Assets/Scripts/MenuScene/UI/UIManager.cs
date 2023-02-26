using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Audio;

namespace YouthSpice.MenuScene.UI
{
	/// <summary>
	/// 전반적인 UI를 관리합니다.
	/// </summary>
	public class UIManager : MonoBehaviour
	{
		[SerializeField]
		private AudioClip backgroundAudioClip;

		private void Start()
		{
			AudioManager.Instance.PlayBackgroundAudio(backgroundAudioClip);
		}
	}
}
