using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace YouthSpice
{
	/// <summary>
	/// Instance 변수가 빠진 싱글톤 디자인 패턴 템플릿입니다.
	/// </summary>
	public abstract class SingleTonWithoutInstance<T> : MonoBehaviour where T : SingleTonWithoutInstance<T>
	{
		/// <summary>
		/// 자식 스크립트가 자동으로 Destroy 될 조건을 지정합니다.
		/// null(기본값)일 경우, Destroy를 하지 않습니다.
		/// </summary>
		protected System.Func<bool> destroyCondition = null;
		/// <summary>
		/// 자식 스크립트가 자동으로 Destroy 될 때 실행할 코드를 지정합니다.
		/// null(기본값)이거나, destroyCondition = null일 경우 실행되지 않습니다.
		/// </summary>
		protected System.Action destroyAction = null;

		/// <summary>
		/// 싱글톤 디자인 패턴을 적용하기 위해 Awake() 메소드 실행문 맨 앞에 base.Awake()가 무조건 호출되어야 합니다.
		/// </summary>
		protected virtual void Awake()
		{
			T[] duplicate = FindObjectsOfType<T>();

			if (duplicate.Length > 1)
			{
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);

			// Unity Editor에서 Scene별로 GameObject를 묶고 싶은 경우, 아래 코드를 사용합니다.
			// 이 때, Project 상에 'DontDestroyOnLoadRoot' 태그가 등록되어 있어야 합니다.
			#if UNITY_EDITOR
				string ActiveScene = SceneManager.GetActiveScene().name;

				GameObject Parent = new List<GameObject>(GameObject.FindGameObjectsWithTag("DontDestroyOnLoadRoot")).Find(target => target.name.StartsWith(ActiveScene));
				if (Parent == null)
				{
					Parent = new GameObject(ActiveScene);
					Parent.tag = "DontDestroyOnLoadRoot";
					DontDestroyOnLoad(Parent);
				}
				gameObject.transform.SetParent(Parent.transform);
			#endif
		}

		/// <summary>
		/// destroyCondition을 사용 할 경우, Update() 메소드 안에서 base.Update()가 무조건 호출되어야 합니다.
		/// </summary>
		protected virtual void Update()
		{
			if (destroyCondition == null) return;

			if (destroyCondition())
			{
				destroyAction?.Invoke();
				Destroy(gameObject);
				return;
			}
		}
	}
}
