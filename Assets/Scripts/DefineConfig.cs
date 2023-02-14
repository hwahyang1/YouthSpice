using System.Collections;
using System.Collections.Generic;
using Serializable = System.SerializableAttribute;

using UnityEngine;

namespace YouthSpice
{
	/// <summary>
	/// 사용 가능한 설정값을 정의합니다.
	/// </summary>
	[Serializable]
	public class DefineConfig
	{
		public float backgroundVolume;
		public float effectVolume;

		public bool useTypingEffect;
		public float typingSpeed;
		public bool useResearchEffect;
		public bool useFullscreen;

		public DefineConfig()
		{
			backgroundVolume = 0.5f;
			effectVolume = 0.5f;

			useTypingEffect = true;
			typingSpeed = 0.9f;
			useResearchEffect = true;
			useFullscreen = true;
		}

		public DefineConfig(
			float backgroundVolume,
			float effectVolume,
			bool useTypingEffect,
			float typingSpeed,
			bool useResearchEffect,
			bool useFullscreen
		)
		{
			this.backgroundVolume = backgroundVolume;
			this.effectVolume = effectVolume;
			this.useTypingEffect = useTypingEffect;
			this.typingSpeed = typingSpeed;
			this.useResearchEffect = useResearchEffect;
			this.useFullscreen = useFullscreen;
		}
	}
}
