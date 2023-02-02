using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.StoryEditorScene.Element.Child
{
	/// <summary>
	/// 효과음을 정의하는 자식 스크립트입니다.
	/// </summary>
	public class EffectMusicElementGroup : ElementGroup
	{
		[SerializeField]
		private Dropdown effectMusicDropdown;
		
		protected override void Init(Dictionary<string, string> data)
		{
			if (data.ContainsKey("AvailableAudios") && data["AvailableAudios"] != "")
			{
				foreach (string musicName in data["AvailableAudios"].Split(" | "))
				{
					effectMusicDropdown.options.Add(new Dropdown.OptionData(musicName));
				}
			}
			
			if (data.ContainsKey("Effect")) effectMusicDropdown.value = int.Parse(data["Effect"]);
		}
		
		public override Dictionary<string, string> GetData()
		{
			return new Dictionary<string, string>(){{"Effect", effectMusicDropdown.value.ToString()}};
		}
	}
}
