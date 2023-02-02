using System;
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

		public void ConvertClassToElement(ChapterElement[] data)
		{
			foreach (ChapterElement element in data)
			{
				elementManager.NewElement((int)element.Type, element.Data);
			}
		}

		public ChapterElement[] ConvertElementToClass()
		{
			List<ChapterElement> data = new List<ChapterElement>();

			for (int i = 0; i < elementManager.ElementContentParent.childCount; i++)
			{
				GameObject targetObject = elementManager.ElementContentParent.GetChild(i).gameObject;
				ElementGroup target = null;
				int type = 0;

				if (targetObject.TryGetComponent(out SpeechElementGroup comp0))
				{
					type = 0;
					target = comp0;
				}
				else if (targetObject.TryGetComponent(out DayImageElementGroup comp1))
				{
					type = 1;
					target = comp1;
				}
				else if (targetObject.TryGetComponent(out BackgroundImageElementGroup comp2))
				{
					type = 2;
					target = comp2;
				}
				else if (targetObject.TryGetComponent(out BackgroundMusicElementGroup comp3))
				{
					type = 3;
					target = comp3;
				}
				else if (targetObject.TryGetComponent(out EffectMusicElementGroup comp4))
				{
					type = 4;
					target = comp4;
				}
				else if (targetObject.TryGetComponent(out CharacterElementGroup comp5))
				{
					type = 5;
					target = comp5;
				}
				else if (targetObject.TryGetComponent(out FriendshipElementGroup comp6))
				{
					type = 6;
					target = comp6;
				}
				else if (targetObject.TryGetComponent(out SelectionElementGroup comp7))
				{
					type = 7;
					target = comp7;
				}
				else if (targetObject.TryGetComponent(out SelectionNameSubElementGroup comp8))
				{
					type = 8;
					target = comp8;
				}

				data.Add(new ChapterElement((ChapterElementType)type, target?.GetData()));
			}

			return data.ToArray();
		}
	}
}
