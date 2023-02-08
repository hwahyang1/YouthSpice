using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice.StoryScene
{
	/// <summary>
	/// 사용 가능한 이미지 트랜지션 효과를 정의합니다.
	/// </summary>
	public enum DefineImageTransitions
	{
		None,
		Dissolve,
		Fade,
		BlinkFade
	}

	/// <summary>
	/// 사용 가능한 오디오 트랜지션 효과를 정의합니다.
	/// </summary>
	public enum DefineAudioTransitions
	{
		None,
		Dissolve,
		Fade
	}
}
