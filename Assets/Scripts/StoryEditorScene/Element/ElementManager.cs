using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using YouthSpice.StoryEditorScene.Element.Child;

namespace YouthSpice.StoryEditorScene.Element
{
	/// <summary>
	/// 각 Element의 생성 및 수정, 삭제를 관리합니다.
	/// </summary>
	public class ElementManager : MonoBehaviour
	{
		[SerializeField]
		private Transform elementContentParent;
		
		[SerializeField, Tooltip("ChapterElementType와 같은 순서대로 입력합니다.")]
		private GameObject[] elementPrefabs;

		private void Start()
		{
			RemoveAll();
		}

		public void RemoveAll()
		{
			for (int i = 0; i < elementContentParent.childCount; i++)
			{
				Destroy(elementContentParent.GetChild(i).gameObject);
			}
		}

		public void NewElement(int type, string data)
		{
			GameObject child = Instantiate(elementPrefabs[type], elementContentParent);
			switch (type)
			{
				case 0:
					child.GetComponent<SpeechElementGroup>().Init(this, data);
					break;
				case 1:
					child.GetComponent<BackgroundImageElementGroup>().Init(this, data);
					break;
				case 2:
					child.GetComponent<BackgroundMusicElementGroup>().Init(this, data);
					break;
				case 3:
					child.GetComponent<EffectMusicElementGroup>().Init(this, data);
					break;
				case 4:
					child.GetComponent<CharacterElementGroup>().Init(this, data);
					break;
				case 5:
					child.GetComponent<FriendshipElementGroup>().Init(this, data);
					break;
				case 6:
					child.GetComponent<SelectionElementGroup>().Init(this, data);
					break;
				case 7:
					child.GetComponent<SelectionNameSubElementGroup>().Init(this, data);
					break;
			}
		}

		public void MoveElement(int fromIndex, int toIndex)
		{
			if (toIndex < 0) return;
			if (toIndex == elementContentParent.childCount) return;
			EventSystem.current?.SetSelectedGameObject(null);
			elementContentParent.GetChild(fromIndex).SetSiblingIndex(toIndex);
		}
		
		// 대상이 분기점 안에 있을 경우
		public void MoveElementInSelection(Transform selectionParent, int fromIndex, int toIndex)
		{
			
		}

		public void DeleteElement(int index)
		{
			GameObject target = elementContentParent.GetChild(index).gameObject;
			Destroy(target);
		}

		// 대상이 분기점 안에 있을 경우
		public void DeleteElementInSelection(Transform selectionParent, int index)
		{
			
		}
	}
}
