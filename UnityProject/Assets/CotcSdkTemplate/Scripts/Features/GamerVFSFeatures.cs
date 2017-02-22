using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class GamerVFSFeatures
	{
		#region Handling
		// Check variables to read and display the value of the given key associated to the current logged in gamer (or all keys if null or empty)
		public static void DisplayGamerKey(string key)
		{
			// A VFSHandler instance should be attached to an active object of the scene to display the result
			if (!VFSHandler.HasInstance)
				Debug.LogError("[CotcSdkTemplate:GamerVFSFeatures] No VFSHandler instance found >> Please attach a VFSHandler script on an active object of the scene");
			else
				GetValue(key, DisplayGamerKey_OnSuccess, DisplayGamerKey_OnError);
		}

		// Check variables to create / update the value of the given key associated to the current logged in gamer
		public static void SetGamerKey(Bundle.DataType valueType, string key, string value)
		{
			// The key name should not be empty
			if (string.IsNullOrEmpty(key))
				Debug.LogError("[CotcSdkTemplate:GamerVFSFeatures] The key name is empty >> Please enter a valid key name");
			else
			{
				Bundle setValue;

				// Create a Bundle from the given value string (a Bundle is an Json-like object to represent fields data)
				switch (valueType)
				{
					case Bundle.DataType.Object:
					setValue = Bundle.FromJson(value);
					break;
					
					case Bundle.DataType.String:
					setValue = new Bundle(value);
					break;

					case Bundle.DataType.Double:
					setValue = new Bundle(double.Parse(value));
					break;

					case Bundle.DataType.Integer:
					setValue = new Bundle(int.Parse(value));
					break;

					case Bundle.DataType.Boolean:
					setValue = new Bundle(bool.Parse(value));
					break;

					// TODO: You may want to add more types handled by Bundle
					default:
					Debug.LogError(string.Format("[CotcSdkTemplate:GamerVFSFeatures] The {0} type is unhandled >> Please handle it or use a handled type", valueType));
					return;
				}

				SetValue(key, setValue, SetGamerKey_OnSuccess, SetGamerKey_OnError);
			}
		}

		// Delete the given key associated to the current logged in gamer (or all keys if null or empty)
		public static void DeleteGamerKey(string key)
		{
			DeleteValue(key, DeleteGamerKey_OnSuccess, DeleteGamerKey_OnError);
		}
		#endregion

		#region Features
		// Read the value of the given key associated to the current logged in gamer (or all keys if null or empty)
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		private static void GetValue(string key, Action<Bundle> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Promise<Bundle> (promising a Bundle result)
			CloudFeatures.gamer.GamerVfs.Domain(domain).GetValue(key)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("GamerVFSFeatures", "GetValue", exception);

					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (Bundle keyValue)
				{
					Debug.Log(string.Format("[CotcSdkTemplate:GamerVFSFeatures] GetValue success >> Key Value: {0}", keyValue));

					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(keyValue);
				});
		}

		// Create / update the value of the given key associated to the current logged in gamer
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		private static void SetValue(string key, Bundle value, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
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

		// Delete the given key associated to the current logged in gamer (or all keys if null or empty)
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		private static void DeleteValue(string key, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Promise<Done> (promising a Done result)
			CloudFeatures.gamer.GamerVfs.Domain(domain).DeleteValue(key)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("GamerVFSFeatures", "DeleteValue", exception);

					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (Done deleteDone)
				{
					Debug.Log(string.Format("[CotcSdkTemplate:GamerVFSFeatures] DeleteValue success >> Successful: {0}", deleteDone.Successful));

					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(deleteDone);
				});
		}
		#endregion

		#region Delegate Callbacks
		// What to do if any DisplayGamerKey request succeeded
		private static void DisplayGamerKey_OnSuccess(Bundle keyValue)
		{
			string resultField = "result";

			// TODO: You may want to parse the result Bundle fields (e.g.: if (keyValue["result"]["TestString"].Type == Bundle.DataType.String) { string testString = keyValue["result"]["TestString"].AsString(); })
			if (keyValue.Has(resultField))
				VFSHandler.Instance.FillAndShowVFSPanel(keyValue[resultField].AsDictionary(), "Gamer VFS Keys");
			else
				Debug.LogError(string.Format("[CotcSdkTemplate:GamerVFSFeatures] No {0} field found in the key value result", resultField));
		}

		// What to do if any DisplayGamerKey request failed
		private static void DisplayGamerKey_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: the specified key doesn't exist yet
				case "KeyNotFound":
				VFSHandler.Instance.FillAndShowVFSPanel(null);
				break;

				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:GamerVFSFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}

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

		// What to do if any DeleteGamerKey request succeeded
		private static void DeleteGamerKey_OnSuccess(Done deleteDone)
		{
			// Do whatever...
		}

		// What to do if any DeleteGamerKey request failed
		private static void DeleteGamerKey_OnError(ExceptionError exceptionError)
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
