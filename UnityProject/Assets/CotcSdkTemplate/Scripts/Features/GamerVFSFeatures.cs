using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class GamerVFSFeatures
	{
		#region Handling
		// Check variables to create / update the value of the given key associated to the current logged in gamer
		public static void SetGamerKey(string valueType, string key, string value)
		{
			// The key name should not be empty
			if (string.IsNullOrEmpty(key))
				Debug.LogError("[CotcSdkTemplate:GamerVFSFeatures] The key name is empty >> Please enter a valid key name");
			else
			{
				Bundle setValue;

				// Create a Bundle from the given value string (a Bundle is an Json-like object to represent
				switch (valueType)
				{
					case "Json":
					setValue = Bundle.FromJson(value);
					break;
					
					case "String":
					setValue = new Bundle(value);
					break;

					case "Float":
					setValue = new Bundle(float.Parse(value));
					break;

					case "Int":
					setValue = new Bundle(int.Parse(value));
					break;

					case "Bool":
					setValue = new Bundle(bool.Parse(value));
					break;

					// TODO: You may want to add more types handled by Bundle (like double, List, Dictionary, ...)
					default:
					Debug.LogError(string.Format("[CotcSdkTemplate:GamerVFSFeatures] The {0} type is unhandled >> Please handle it or use a handled type", valueType));
					return;
				}

				SetValue(key, setValue, SetGamerKey_OnSuccess, SetGamerKey_OnError);
			}
		}
		#endregion

		#region Features
		// Create / update the value of the given key associated to the current logged in gamer
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		public static void SetValue(string key, Bundle value, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;

			// Call the API method which returns a Promise<Done> (promising a Done result)
			CloudFeatures.gamer.GamerVfs.Domain(domain).SetValue(key, value)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						ExceptionTools.LogCotcException("GamerVFSFeatures", "SetValue", exception);

						// Call the OnError action if any callback registered to it
						if (OnError != null)
							OnError(ExceptionTools.GetExceptionError(exception));
					})
				// The result if everything went well
				.Done(delegate (Done setDone)
					{
						Debug.Log(string.Format("[CotcSdkTemplate:GamerVFSFeatures] SetValue success >> Successful: {0}", setDone.Successful));

						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(setDone);
					});
		}
		#endregion

		#region Delegate Callbacks
		// What to do if any SetGamerKey request succeeded
		private static void SetGamerKey_OnSuccess(Done setDone)
		{
			// Do whatever...
		}

		// What to do if any SetGamerKey request failed
		private static void SetGamerKey_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:GamerVFSFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}
		#endregion
	}
}
