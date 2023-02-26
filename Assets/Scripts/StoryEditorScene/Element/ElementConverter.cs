using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using YouthSpice.StoryEditorScene.Element.Child;

namespace YouthSpice.StoryEditorScene.Element
{
	/// <summary>
	/// Element를 변환합니다.
	/// </summary>
	public class ElementConverter : MonoBehaviour
	{
		private ElementManager elementManager;

		private void Start()
		{
			elementManager = GetComponent<ElementManager>();
		}

		/// <summary>
		/// 클래스를 Element로 변환합니다.
		/// </summary>
		/// <param name="data">변환할 데이터를 지정합니다.</param>
		public void ConvertClassToElement(ChapterElement[] data)
		{
			foreach (ChapterElement element in data)
			{
				elementManager.NewElement(element.Type, element.Data);
			}
		}

		/// <summary>
		/// Element를 클래스로 변환합니다.
		/// </summary>
		/// <returns>변환된 데이터가 반환됩니다.</returns>
		public ChapterElement[] ConvertElementToClass()
		{
			List<ChapterElement> data = new List<ChapterElement>();

			for (int i = 0; i < elementManager.ElementContentParent.childCount; i++)
			{
				GameObject targetObject = elementManager.ElementContentParent.GetChild(i).gameObject;
				ElementGroup target = null;
				ChapterElementType type = 0;

				if (targetObject.TryGetComponent(out SpeechElementGroup comp0))
				{
					type = ChapterElementType.Speech;
					target = comp0;
				}
				else if (targetObject.TryGetComponent(out DayImageElementGroup comp1))
				{
					type = ChapterElementType.DayImage;
					target = comp1;
				}
				else if (targetObject.TryGetComponent(out BackgroundImageElementGroup comp2))
				{
					type = ChapterElementType.BackgroundImage;
					target = comp2;
				}
				else if (targetObject.TryGetComponent(out BackgroundMusicElementGroup comp3))
				{
					type = ChapterElementType.BackgroundMusic;
					target = comp3;
				}
				else if (targetObject.TryGetComponent(out EffectMusicElementGroup comp4))
				{
					type = ChapterElementType.EffectSound;
					target = comp4;
				}
				else if (targetObject.TryGetComponent(out CharacterElementGroup comp5))
				{
					type = ChapterElementType.StandingImage;
					target = comp5;
				}
				else if (targetObject.TryGetComponent(out FriendshipElementGroup comp6))
				{
					type = ChapterElementType.Friendship;
					target = comp6;
				}
				else if (targetObject.TryGetComponent(out GetPlayerNameElementGroup comp7))
				{
					type = ChapterElementType.GetPlayerName;
					target = comp7;
				}
				else if (targetObject.TryGetComponent(out SelectionElementGroup comp8))
				{
					type = ChapterElementType.Selection;
					target = comp8;
				}
				else if (targetObject.TryGetComponent(out SelectionNameSubElementGroup comp9))
				{
					type = ChapterElementType.SelectionName;
					target = comp9;
				}

				data.Add(new ChapterElement(type, target?.GetData()));
			}

			return data.ToArray();
		}

		/// <summary>
		/// ElementGroup을 하위 클래스로 변환하여 반환합니다.
		/// </summary>
		public Dictionary<string, string> GetData(ElementGroup from)
		{
			ElementGroup target = null;

			if (from.TryGetComponent(out SpeechElementGroup comp0)) target = comp0;
			else if (from.TryGetComponent(out DayImageElementGroup comp1)) target = comp1;
			else if (from.TryGetComponent(out BackgroundImageElementGroup comp2)) target = comp2;
			else if (from.TryGetComponent(out BackgroundMusicElementGroup comp3)) target = comp3;
			else if (from.TryGetComponent(out EffectMusicElementGroup comp4)) target = comp4;
			else if (from.TryGetComponent(out CharacterElementGroup comp5)) target = comp5;
			else if (from.TryGetComponent(out FriendshipElementGroup comp6)) target = comp6;
			else if (from.TryGetComponent(out GetPlayerNameElementGroup comp7)) target = comp7;
			else if (from.TryGetComponent(out SelectionElementGroup comp8)) target = comp8;
			else if (from.TryGetComponent(out SelectionNameSubElementGroup comp9)) target = comp9;

			return target?.GetData();
		}
	}
}
