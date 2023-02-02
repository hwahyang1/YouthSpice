using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

using YouthSpice.StoryEditorScene.Element.Child;
using YouthSpice.StoryEditorScene.Files;

namespace YouthSpice.StoryEditorScene.Element
{
	/// <summary>
	/// 각 Element의 생성 및 수정, 삭제를 관리합니다.
	/// </summary>
	public class ElementManager : MonoBehaviour
	{
		[SerializeField]
		private Transform elementContentParent;

		public Transform ElementContentParent => elementContentParent;

		[SerializeField, Tooltip("ChapterElementType와 같은 순서대로 입력합니다.")]
		private GameObject[] elementPrefabs;

		private EditorDataManager editorDataManager;

		private void Start()
		{
			editorDataManager = GetComponent<EditorDataManager>();
			RemoveAll();
		}

		public void RemoveAll()
		{
			for (int i = 0; i < elementContentParent.childCount; i++)
			{
				Destroy(elementContentParent.GetChild(i).gameObject);
			}
		}

		public void NewElement(int type, Dictionary<string, string> data)
		{
			if (data == null) data = new Dictionary<string, string>();

			GameObject child = Instantiate(elementPrefabs[type], elementContentParent);
			switch (type)
			{
				case 0:
					data.Add("AvailableNames", string.Join(" | ", editorDataManager.AvailableCharacters));
					child.GetComponent<SpeechElementGroup>().Init(this, data);
					break;
				case 1:
					data.Add("AvailableImages", string.Join(" | ", editorDataManager.AvailableDayImages));
					child.GetComponent<DayImageElementGroup>().Init(this, data);
					break;
				case 2:
					data.Add("AvailableImages", string.Join(" | ", editorDataManager.AvailableBackgroundImages));
					child.GetComponent<BackgroundImageElementGroup>().Init(this, data);
					break;
				case 3:
					data.Add("AvailableAudios", string.Join(" | ", editorDataManager.AvailableAudios));
					child.GetComponent<BackgroundMusicElementGroup>().Init(this, data);
					break;
				case 4:
					data.Add("AvailableAudios", string.Join(" | ", editorDataManager.AvailableAudios));
					child.GetComponent<EffectMusicElementGroup>().Init(this, data);
					break;
				case 5:
					data.Add("AvailableCharacters", string.Join(" | ", editorDataManager.AvailableStandingIllusts));
					child.GetComponent<CharacterElementGroup>().Init(this, data);
					break;
				case 6:
					data.Add("AvailableNames", string.Join(" | ", editorDataManager.AvailableCharacters));
					child.GetComponent<FriendshipElementGroup>().Init(this, data);
					break;
				case 7:
					child.GetComponent<SelectionElementGroup>().Init(this, data);
					break;
				case 8:
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
