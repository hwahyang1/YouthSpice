using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Config;
using YouthSpice.PreloadScene.Files;
using YouthSpice.StoryScene.Chapter;

namespace YouthSpice.StoryScene.Audio
{
	/// <summary>
	/// 오디오 재생을 관리합니다.
	/// </summary>
	public class AudioManager : MonoBehaviour
	{
		[Header("Background")]
		[SerializeField]
		private AudioSource[] backgroundAudios;
		
		[Header("Status")]
		// false -> Back(0) / true -> Front(1)
		[SerializeField, ReadOnly]
		private bool activePosition = false;

		[SerializeField, ReadOnly]
		private float backgroundAudioMax;
		[SerializeField, ReadOnly]
		private float effectAudioMax;

		[Header("Time")]
		[SerializeField, Tooltip("이 값은 Dissolve 기준으로, Fade의 경우 지정된 시간의 2배를 사용합니다.")]
		private float totalAnimationTime = 1.5f;

		[SerializeField]
		private float animationDelay = 0.01f;

		private Coroutine activeCoroutine = null;

		[Header("Effect")]
		[SerializeField]
		private AudioSource effectAudio;

		private ChapterManager chapterManager;

		private void Start()
		{
			chapterManager = GetComponent<ChapterManager>();
		}
		
		public void OnKeyDown()
		{
			//
		}

		private void FixedUpdate()
		{
			DefineConfig config = ConfigManager.Instance.GetConfig();
			backgroundAudioMax = config.backgroundVolume;
			effectAudioMax = config.effectVolume;
		}

		/// <summary>
		/// 효과음을 재생합니다.
		/// </summary>
		public void PlayEffectAudio(Dictionary<string, string> data)
		{
			effectAudio.volume = effectAudioMax;
			effectAudio.PlayOneShot(SourceFileManager.Instance.AvailableAudios[int.Parse(data["Effect"]) - 1], effectAudioMax);
			chapterManager.isEffectSoundEnded = true;
		}

		/// <summary>
		/// 배경음을 재생합니다.
		/// </summary>
		public void PlayBackgroundAudio(Dictionary<string, string> data, bool forceDisableTransition = false)
		{
			AudioClip clip = data["Background"] == "0"
				? null
				: SourceFileManager.Instance.AvailableAudios[int.Parse(data["Background"]) - 1];
			activeCoroutine =
				StartCoroutine(PlayBackgroundAudioCoroutine(clip, forceDisableTransition,
					(DefineAudioTransitions)int.Parse(data["Transition"])));
		}

		private IEnumerator PlayBackgroundAudioCoroutine(AudioClip clip, bool forceDisableTransition, DefineAudioTransitions transitionType)
		{
			if (forceDisableTransition) transitionType = DefineAudioTransitions.None;
			
			WaitForSeconds timeout = new WaitForSeconds(animationDelay);

			AudioSource currentSource = backgroundAudios[activePosition ? 1 : 0];
			AudioSource nextSource = backgroundAudios[activePosition ? 0 : 1];

			nextSource.volume = 0f;
			nextSource.clip = clip;

			yield return null;

			activePosition = !activePosition;

			float currentTime = 0f;

			switch (transitionType)
			{
				case DefineAudioTransitions.None:
					if (currentSource.clip != null)
					{
						currentSource.Stop();
						currentSource.volume = 0f;
					}

					if (nextSource.clip != null)
					{
						nextSource.volume = backgroundAudioMax;
						nextSource.Play();
					}
					break;
				case DefineAudioTransitions.Dissolve:
					if (nextSource.clip != null) nextSource.Play();
					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						if (currentSource.clip != null) currentSource.volume = backgroundAudioMax - (currentTime / totalAnimationTime);
						if (nextSource.clip != null) nextSource.volume = currentTime / totalAnimationTime * backgroundAudioMax;
						yield return timeout;
					}
					if (currentSource.clip != null) currentSource.Stop();
					
					yield return new WaitForSeconds(totalAnimationTime / 2f);

					break;
				case DefineAudioTransitions.Fade:
					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						if (currentSource.clip != null) currentSource.volume = backgroundAudioMax - (currentTime / totalAnimationTime);
						yield return timeout;
					}
					if (currentSource.clip != null) currentSource.Stop();

					yield return new WaitForSeconds(totalAnimationTime / 2f);

					currentTime = 0f;

					if (nextSource.clip != null) nextSource.Play();
					while (currentTime < totalAnimationTime)
					{
						currentTime += animationDelay;
						if (nextSource.clip != null) nextSource.volume = currentTime / totalAnimationTime * backgroundAudioMax;
						yield return timeout;
					}
					
					yield return new WaitForSeconds(totalAnimationTime / 2f);

					break;
			}

			yield return new WaitForSeconds(0.25f);

			activeCoroutine = null;
			chapterManager.isBackgroundMusicEnded = true;
		}
	}
}
