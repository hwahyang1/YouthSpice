using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.PreloadScene.Audio;
using YouthSpice.PreloadScene.Game;
using YouthSpice.PreloadScene.Scene;
using YouthSpice.SaveLoadSlotScene.Extern;

namespace YouthSpice.MenuScene.UI
{
	/// <summary>
	/// UI의 상호작용을 관리합니다.
	/// </summary>
	public class UIInteraction : MonoBehaviour
	{
		[SerializeField]
		private AudioClip effectSound;
		
		public void OnNewGameButtonClicked()
		{
			AudioManager.Instance.PlayEffectAudio(effectSound);
			AudioManager.Instance.StopBackgroundAudio();
			
			GameInfo.Instance.majorChapter = 0;
			GameInfo.Instance.minorChapter = 0;
			GameInfo.Instance.slotName = "프롤로그";

			GameProgressManager.Instance.RunThisChapter();
		}

		public void OnLoadGameButtonClicked()
		{
			AudioManager.Instance.PlayEffectAudio(effectSound);
			
			SaveLoadSlotLoadParams.Instance.mode = SaveLoadSlotMode.Load;
			SceneChange.Instance.Add("SaveLoadSlotScene");
		}

		public void OnGalleryButtonClicked()
		{
			AudioManager.Instance.PlayEffectAudio(effectSound);

			SceneChange.Instance.Add("GalleryScene");
		}

		public void OnConfigButtonClicked()
		{
			AudioManager.Instance.PlayEffectAudio(effectSound);

			//SceneChange.Instance.Add("ConfigScene");
			SceneChange.Instance.ChangeScene("GameScene");
		}
	}
}
