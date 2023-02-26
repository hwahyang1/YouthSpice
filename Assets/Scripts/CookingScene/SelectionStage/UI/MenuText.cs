using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using YouthSpice.CookingScene.Extern;

namespace YouthSpice.CookingScene.SelectionStage.UI
{
	/// <summary>
	/// 제작해야 하는 메뉴의 이름을 표시합니다.
	/// </summary>
	public class MenuText : MonoBehaviour
	{
		[SerializeField]
		private Text menuText;
		
		private void Start()
		{
			menuText.text = CookingLoadParams.Instance.menu.ToString();
			Destroy(this);
		}
	}
}
