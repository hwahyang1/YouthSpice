using System.Collections;
using System.Collections.Generic;
using Action = System.Action;

using UnityEngine;

using NaughtyAttributes;

using YouthSpice.PreloadScene.Alert;
using YouthSpice.PreloadScene.Audio;
using YouthSpice.PreloadScene.Files;
using YouthSpice.SaveLoadSlotScene.Extern;
using YouthSpice.SaveLoadSlotScene.UI;

namespace YouthSpice.SaveLoadSlotScene.Slots
{
	/// <summary>
	/// 전체 슬롯을 관리합니다.
	/// </summary>
	public class SlotsManager : MonoBehaviour
	{
		[SerializeField]
		private AudioClip clickClip;
		
		[SerializeField]
		private Transform parent;

		[SerializeField]
		private GameObject prefab;

		[SerializeField, ReadOnly]
		private SaveLoadSlotMode mode;
		
		private void Awake()
		{
			RebuildSlots();
		}

		private void RebuildSlots()
		{
			for (int i = 0; i < parent.childCount; i++)
			{
				Destroy(parent.GetChild(i).gameObject);
			}

			mode = SaveLoadSlotLoadParams.Instance.mode;
			
			List<DefineGame> slots = SaveSlotManager.Instance.GetAllData();

			for (int i = 0; i < slots.Count; i++)
			{
				DefineGame slot = slots[i];
				SlotElement child = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent).GetComponent<SlotElement>();

				if (slot == null)
				{
					child.Init(i, null, $"빈 슬롯 {i + 1}", 0, InteractionSlot);
				}
				else
				{
					child.Init(i, SaveSlotManager.Instance.GetScreenShot(i), slot.SlotName, slot.DateTime, InteractionSlot);
				}
			}
		}

		private void InteractionSlot(int index)
		{
			AudioManager.Instance.PlayEffectAudio(clickClip);
			
			DefineGame data = SaveSlotManager.Instance.GetData(index);
			
			if (mode == SaveLoadSlotMode.Save)
			{
				if (data == null)
				{
					SaveSlotManager.Instance.Save(index);
					RebuildSlots();
					return;
				}
				
				AlertManager.Instance.Show(AlertType.Double, "경고", "기존의 데이터를 덮어쓰시겠습니까?", new Dictionary<string, Action>(){{"저장하기",
					()=>
					{
						SaveSlotManager.Instance.Save(index);
						RebuildSlots();
					}}, {"취소", null}});
			}
			else
			{
				if (data == null)
				{
					AlertManager.Instance.Show(AlertType.Single, "알림", "비어있는 슬롯은 불러올 수 없습니다.", new Dictionary<string, Action>(){{"닫기", null}});
					return;
				}
				
				AlertManager.Instance.Show(AlertType.Double, "경고", "정말로 불러오시겠습니까?\n저장되지 않은 데이터는 사라집니다.", new Dictionary<string, Action>(){{"불러오기",
					() =>
					{
						AudioManager.Instance.StopBackgroundAudio();
						SaveSlotManager.Instance.Load(index);
					}}, {"취소", null}});
			}
		}
	}
}
