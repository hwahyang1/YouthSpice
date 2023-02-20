using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using YouthSpice.PreloadScene.Alert;
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

		public GameObject[] ElementPrefabs => elementPrefabs;

		private EditorDataManager editorDataManager;
		private ElementConverter elementConverter;
		public ElementConverter ElementConverter => elementConverter;

		private void Start()
		{
			editorDataManager = GetComponent<EditorDataManager>();
			elementConverter = GetComponent<ElementConverter>();
			RemoveAll();
		}

		public void RemoveAll()
		{
			for (int i = 0; i < elementContentParent.childCount; i++)
			{
				Destroy(elementContentParent.GetChild(i).gameObject);
			}
		}

		public void NewElement(int type, Dictionary<string, string> data, Transform parent = null, int order = -1) =>
			NewElement((ChapterElementType)type, data, parent, order);

		public void NewElement(
			ChapterElementType type,
			Dictionary<string, string> data,
			Transform parent = null,
			int order = -1
		)
		{
			if (data == null) data = new Dictionary<string, string>();
			if (parent == null) parent = elementContentParent;
			if (type == ChapterElementType.Selection && parent != elementContentParent) return;

			GameObject child = Instantiate(elementPrefabs[(int)type], parent);
			if (order != -1) child.transform.SetSiblingIndex(order);
			switch (type)
			{
				case ChapterElementType.Speech:
					data.Add("AvailableNames", string.Join(" | ", editorDataManager.AvailableCharacters));
					child.GetComponent<SpeechElementGroup>().Init(this, type, data, parent != elementContentParent);
					break;
				case ChapterElementType.DayImage:
					data.Add("AvailableImages", string.Join(" | ", editorDataManager.AvailableDayImages));
					data.Add("AvailableTransitions", string.Join(" | ", editorDataManager.AvailableImageTransitions));
					child.GetComponent<DayImageElementGroup>().Init(this, type, data, parent != elementContentParent);
					break;
				case ChapterElementType.BackgroundImage:
					data.Add("AvailableImages", string.Join(" | ", editorDataManager.AvailableBackgroundImages));
					data.Add("AvailableTransitions", string.Join(" | ", editorDataManager.AvailableImageTransitions));
					child.GetComponent<BackgroundImageElementGroup>()
					     .Init(this, type, data, parent != elementContentParent);
					break;
				case ChapterElementType.BackgroundMusic:
					data.Add("AvailableAudios", string.Join(" | ", editorDataManager.AvailableAudios));
					data.Add("AvailableTransitions", string.Join(" | ", editorDataManager.AvailableAudioTransitions));
					child.GetComponent<BackgroundMusicElementGroup>()
					     .Init(this, type, data, parent != elementContentParent);
					break;
				case ChapterElementType.EffectSound:
					data.Add("AvailableAudios", string.Join(" | ", editorDataManager.AvailableAudios));
					child.GetComponent<EffectMusicElementGroup>()
					     .Init(this, type, data, parent != elementContentParent);
					break;
				case ChapterElementType.StandingImage:
					data.Add("AvailableCharacters", string.Join(" | ", editorDataManager.AvailableStandingIllusts));
					data.Add("AvailableTransitions", string.Join(" | ", editorDataManager.AvailableImageTransitions));
					child.GetComponent<CharacterElementGroup>().Init(this, type, data, parent != elementContentParent);
					break;
				case ChapterElementType.Friendship:
					data.Add("AvailableNames", string.Join(" | ", editorDataManager.AvailableCharacters));
					child.GetComponent<FriendshipElementGroup>().Init(this, type, data, parent != elementContentParent);
					break;
				case ChapterElementType.GetPlayerName:
					child.GetComponent<GetPlayerNameElementGroup>()
					     .Init(this, type, data, parent != elementContentParent);
					break;
				case ChapterElementType.Selection:
					child.GetComponent<SelectionElementGroup>().Init(this, type, data);
					break;
				case ChapterElementType.SelectionName:
					child.GetComponent<SelectionNameSubElementGroup>()
					     .Init(this, type, data, parent != elementContentParent);
					break;
			}

			if (parent != elementContentParent)
				LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
		}

		public void MoveElement(int fromIndex, int toIndex)
		{
			// 맨 위쪽인 경우 무시
			if (toIndex < 0) return;
			// 맨 아래인 경우 무시
			if (toIndex == elementContentParent.childCount) return;
			// 키 눌렀을때 이동 안되도록 선택 해제
			EventSystem.current?.SetSelectedGameObject(null);

			Transform from = elementContentParent.GetChild(fromIndex);
			ElementGroup fromElement = from.GetComponent<ElementGroup>();

			// 이동하려는 위치에 있는 GameObject가 분기점일 경우 -> 분기점 안으로 넣을건지 확인 (분기점 Element 제외)
			if (fromElement.Type != ChapterElementType.Selection &&
			    elementContentParent.GetChild(toIndex).TryGetComponent(out SelectionElementGroup to))
			{
				AlertManager.Instance.Show(AlertType.Double, "확인", "이동하려는 위치에 분기점이 있습니다.\n항목을 분기점 안으로 넣으시겠습니까?",
					new Dictionary<string, Action>()
					{
						{
							"예\n(분기점 안으로 넣기)", () =>
							{
								// 기존 GameObject 제거 후 분기점 안에 새로운 GameObject 생성
								NewElement(fromElement.Type, elementConverter.GetData(fromElement), to.SelectionParent,
									toIndex - fromIndex);
								DeleteElement(fromIndex);
							}
						},
						{ "아니요", () => { from.SetSiblingIndex(toIndex); } }
					});
			}
			else
			{
				from.SetSiblingIndex(toIndex);
			}
		}

		// 대상이 분기점 안에 있을 경우
		public void MoveElementInSelection(Transform selectionParent, int fromIndex, int toIndex)
		{
			Transform from = selectionParent.GetChild(fromIndex);
			ElementGroup fromElement = from.GetComponent<ElementGroup>();

			// (분기점 이름 제외) 맨 위 또는 아래에 있는 경우 분기점 밖으로 뺄 건지 확인
			if (toIndex < 1 || toIndex == selectionParent.childCount)
			{
				AlertManager.Instance.Show(AlertType.Double, "확인", "이동하려는 위치는 분기점 밖입니다.\n항목을 분기점 밖으로 꺼내시겠습니까?",
					new Dictionary<string, Action>()
					{
						{
							"예\n(분기점 밖으로 꺼내기)", () =>
							{
								int newIndex = selectionParent.parent.GetSiblingIndex() + (toIndex - fromIndex);
								if (newIndex < 0) newIndex = 0;
								NewElement(fromElement.Type, elementConverter.GetData(fromElement), null, newIndex);
								DeleteElementInSelection(selectionParent, fromIndex);
							}
						},
						{ "아니요", null }
					});
			}
			else
			{
				from.SetSiblingIndex(toIndex);
			}
		}

		public void DeleteElement(int index)
		{
			GameObject target = elementContentParent.GetChild(index).gameObject;
			Destroy(target);
		}

		// 대상이 분기점 안에 있을 경우
		public void DeleteElementInSelection(Transform selectionParent, int index)
		{
			GameObject target = selectionParent.GetChild(index).gameObject;
			Destroy(target);
		}
	}
}
