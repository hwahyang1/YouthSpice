using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.ConfigScene.UI
{
	/// <summary>
	/// 현재 게임 버전을 표기합니다.
	/// </summary>
	public class GetVersion : MonoBehaviour
	{
		[SerializeField]
		private Text versionText;

		private void Start()
		{
			versionText.text = "Ver. " + Application.version;
			Destroy(this);
		}
	}
}
