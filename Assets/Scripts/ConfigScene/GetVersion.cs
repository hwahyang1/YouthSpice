using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace YouthSpice.ConfigScene
{
	/// <summary>
	/// Description
	/// </summary>
	public class GetVersion : MonoBehaviour
	{
		[SerializeField]
		private Text versionText;
		
		private void Start()
		{
			versionText.text = "Ver. " + Application.version;
		}
	}
}
