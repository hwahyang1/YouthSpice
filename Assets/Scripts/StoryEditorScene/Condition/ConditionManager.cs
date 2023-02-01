using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

using NaughtyAttributes;

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
		public bool Reset = true;
		
		public void ApplyCurrent()
		{
			if (!Reset) return;
			
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

			Reset = false;
		}

		public void ToggleFormerChapter() => formerChapterInput.interactable = formerChapterToggle.isOn;
		
		public void ToggleMinDay() => minDayInput.interactable = minDayToggle.isOn;

		public void ToggleFriendShip1() =>friendship1Input.interactable = friendship1Toggle.isOn;
		public void ToggleFriendShip2() =>friendship2Input.interactable = friendship2Toggle.isOn;
		public void ToggleFriendShip3() =>friendship3Input.interactable = friendship3Toggle.isOn;
	}
}
