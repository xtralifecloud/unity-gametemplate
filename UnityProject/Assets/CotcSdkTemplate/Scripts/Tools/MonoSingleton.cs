using UnityEngine;

namespace CotcSdkTemplate
{
	public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		#region Instance Handling
		private static MonoSingleton<T> instance = null;

		private void Awake()
		{
			if (instance == null)
				instance = this;
			else
			{
				Debug.LogError("[MonoSingleton:Awake] Found more than one instance of " + this.GetType().Name + " >> Destroying the last one");
				Destroy(this);
			}
		}

		public static T Instance
		{
			get
			{
				if (instance != null)
					return instance as T;
				else
				{
					Debug.LogError("[MonoSingleton:InstanceGet] Not found any instance of " + typeof(T).Name + " >> Returning null");
					return null;
				}
			}
		}

		public static bool HasInstance
		{
			get
			{
				return instance != null;
			}
		}
		#endregion
	}
}
