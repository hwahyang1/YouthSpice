using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace YouthSpice
{
	/// <summary>
	/// 싱글톤 디자인 패턴 템플릿입니다.
	/// </summary>
	public abstract class Singleton<T> : SingleTonWithoutInstance<T> where T : Singleton<T>
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<T>();

					if (instance == null)
					{
						instance = new GameObject("Singleton_" + typeof(T).Name).AddComponent<T>();
					}
				}

				return instance;
			}
		}
	}
}
