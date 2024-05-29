using UnityEngine;

namespace Core
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T _instance;

		public static T Instance
		{
			get
			{
				if (_instance)
					return _instance;
				
				_instance = (T)FindObjectOfType(typeof(T));
				if (_instance)
					return _instance;
				
				GameObject obj = new GameObject(typeof(T).Name, typeof(T));
				_instance = obj.GetComponent<T>();
				return _instance;
			}
		}
		
		public void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Destroy(gameObject);
				return;
			}
			DontDestroyOnLoad(gameObject);
			AwakeInit();
		}

		protected virtual void AwakeInit()
		{
		}
	}
}