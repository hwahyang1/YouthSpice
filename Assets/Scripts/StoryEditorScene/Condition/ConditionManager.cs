using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using NaughtyAttributes;

using YouthSpice.StoryEditorScene.UI;

namespace YouthSpice.StoryEditorScene.Condition
{
	/// <summary>
	/// 프로젝트의 조건을 관리합니다.
	/// </summary>
	public class ConditionManager : MonoBehaviour
	{
		[Header("Former Chapter")]
		[SerializeField]
		private Toggle formerChapterToggle;

		[SerializeField]
		private TMP_InputField formerChapterInput;

		[Header("Min Day")]
		[SerializeField]
		private Toggle minDayToggle;

		[SerializeField]
		private TMP_InputField minDayInput;

		[Header("Friendship 1")]
		[SerializeField]
		private Toggle friendship1Toggle;

		[SerializeField]
		private TMP_InputField friendship1Input;

		[Header("Friendship 2")]
		[SerializeField]
		private Toggle friendship2Toggle;

		[SerializeField]
		private TMP_InputField friendship2Input;

		[Header("Friendship 3")]
		[SerializeField]
		private Toggle friendship3Toggle;

		[SerializeField]
		private TMP_InputField friendship3Input;

		[ReadOnly]
		public bool reset = true;

		/// <summary>
		/// 실행 조건을 지정합니다.
		/// 사용하지 않는 데이터는 null로 지정합니다.
		/// </summary>
		public void ApplyCustom(
			string formerChapter,
			string minDay,
			string friendship1,
			string friendship2,
			string friendship3
		)
		{
			StartCoroutine(ApplyCustomCoroutine(formerChapter, minDay, friendship1, friendship2, friendship3));
		}

		private IEnumerator ApplyCustomCoroutine(
			string formerChapter,
			string minDay,
			string friendship1,
			string friendship2,
			string friendship3
		)
		{
			GetComponent<UIManager>().SetCustomAlertActive(true);

			yield return null;

			reset = false;

			if (formerChapter == null)
			{
				formerChapterToggle.isOn = false;
				formerChapterInput.interactable = false;
				formerChapterInput.text = "";
			}
			else
			{
				formerChapterToggle.isOn = true;
				formerChapterInput.interactable = true;
				formerChapterInput.text = formerChapter;
			}

			if (minDay == null)
			{
				minDayToggle.isOn = false;
				minDayInput.interactable = false;
				minDayInput.text = "";
			}
			else
			{
				minDayToggle.isOn = true;
				minDayInput.interactable = true;
				minDayInput.text = minDay;
			}

			if (friendship1 == null)
			{
				friendship1Toggle.isOn = false;
				friendship1Input.interactable = false;
				friendship1Input.text = "";
			}
			else
			{
				friendship1Toggle.isOn = true;
				friendship1Input.interactable = true;
				friendship1Input.text = friendship1;
			}

			if (friendship2 == null)
			{
				friendship2Toggle.isOn = false;
				friendship2Input.interactable = false;
				friendship2Input.text = "";
			}
			else
			{
				friendship2Toggle.isOn = true;
				friendship2Input.interactable = true;
				friendship2Input.text = friendship2;
			}

			if (friendship3 == null)
			{
				friendship3Toggle.isOn = false;
				friendship3Input.interactable = false;
				friendship3Input.text = "";
			}
			else
			{
				friendship3Toggle.isOn = true;
				friendship3Input.interactable = true;
				friendship3Input.text = friendship3;
			}

			yield return null;

			GetComponent<UIManager>().SetCustomAlertActive(false);
		}

		/// <summary>
		/// 현재 설정된 데이터를 반환합니다.
		/// </summary>
		/// <remarks>비활성화(선택되지 않은) 데이터는 반환하지 않습니다.</remarks>
		public Dictionary<ChapterCondition, string> GetData()
		{
			Dictionary<ChapterCondition, string> data = new Dictionary<ChapterCondition, string>();

			if (formerChapterToggle.isOn && formerChapterInput.text != "")
				data.Add(ChapterCondition.FormerChapter, formerChapterInput.text);

			if (minDayToggle.isOn && minDayInput.text != "") data.Add(ChapterCondition.MinDay, minDayInput.text);

			if (friendship1Toggle.isOn && friendship1Input.text != "")
				data.Add(ChapterCondition.MinPlayer1Friendship, friendship1Input.text);
			if (friendship2Toggle.isOn && friendship2Input.text != "")
				data.Add(ChapterCondition.MinPlayer2Friendship, friendship2Input.text);
			if (friendship3Toggle.isOn && friendship3Input.text != "")
				data.Add(ChapterCondition.MinPlayer3Friendship, friendship3Input.text);

			return data;
		}

		/// <summary>
		/// reset = true인 경우, 입력란을 초기화 합니다.
		/// </summary>
		public void ApplyReset()
		{
			if (!reset) return;

			formerChapterToggle.isOn = false;
			formerChapterInput.interactable = false;
			formerChapterInput.text = "";

			minDayToggle.isOn = false;
			minDayInput.interactable = false;
			minDayInput.text = "";

			friendship1Toggle.isOn = false;
			friendship1Input.interactable = false;
			friendship1Input.text = "";

			friendship2Toggle.isOn = false;
			friendship2Input.interactable = false;
			friendship2Input.text = "";

			friendship3Toggle.isOn = false;
			friendship3Input.interactable = false;
			friendship3Input.text = "";

			reset = false;
		}

		/* ===================================== ToggleButton Events ===================================== */
		
		public void ToggleFormerChapter() => formerChapterInput.interactable = formerChapterToggle.isOn;

		public void ToggleMinDay() => minDayInput.interactable = minDayToggle.isOn;

		public void ToggleFriendShip1() => friendship1Input.interactable = friendship1Toggle.isOn;
		public void ToggleFriendShip2() => friendship2Input.interactable = friendship2Toggle.isOn;
		public void ToggleFriendShip3() => friendship3Input.interactable = friendship3Toggle.isOn;
	}
}
