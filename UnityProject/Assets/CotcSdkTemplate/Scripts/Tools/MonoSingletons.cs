using System.Collections.Generic;
using UnityEngine;

namespace CotcSdkTemplate
{
	public static class MonoSingletons
	{
		#region Instances Handling
		// Keep the registered MonoBehaviour instances in a Dictionary with instance's class name as key
		private static Dictionary<string, MonoBehaviour> monoInstances = new Dictionary<string, MonoBehaviour>();

		// Return if there is a registered instance for the asked MonoBehaviour inherited class' type
		public static bool HasInstance<T>() where T : MonoBehaviour
		{
			return monoInstances.ContainsKey(typeof(T).Name);
		}

		// Get a previously registered MonoBehaviour inherited class' instance
		public static T Instance<T>() where T : MonoBehaviour
		{
			// If there is a registered instance for the asked MonoBehaviour inherited type, return it
			if (monoInstances.ContainsKey(typeof(T).Name))
				return monoInstances[typeof(T).Name] as T;
			// Else, return null
			else
			{
				Debug.LogError("[CotcSdkTemplate:MonoSingletons] " + typeof(T).Name + " class instance doesn't exist >> Null returned");
				return null;
			}
		}

		// Register a MonoBehaviour inherited class' instance
		public static void Register<T>(T instance) where T : MonoBehaviour
		{
			// If there is no registered instance for the asked MonoBehaviour inherited type, register the new instance
			if (!monoInstances.ContainsKey(typeof(T).Name))
				monoInstances.Add(typeof(T).Name, instance);
			// Else, destroy the new instance
			else
			{
				Debug.LogError("[CotcSdkTemplate:MonoSingletons] " + typeof(T).Name + " class instance already exists >> New instance dismissed");
				Object.Destroy(instance);
			}
		}
		#endregion
	}
}
