using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class GameVFSFeatures
	{
		#region Handling
		// Check variables to read and display the value of the given key associated to the current game (or all keys if null or empty)
		public static void DisplayGameKey(string key)
		{
			// A VFSHandler instance should be attached to an active object of the scene to display the result
			if (!VFSHandler.HasInstance)
				Debug.LogError("[CotcSdkTemplate:GameVFSFeatures] No VFSHandler instance found >> Please attach a VFSHandler script on an active object of the scene");
			else
				GetValue(key, DisplayGameKey_OnSuccess, DisplayGameKey_OnError);
		}
		#endregion

		#region Features
		// Read the value of the given key associated to the current game (or all keys if null or empty)
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		private static void GetValue(string key, Action<Bundle> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud to proceed
			if (!CloudFeatures.IsCloudInitialized())
				return;
			
			// Call the API method which returns a Promise<Bundle> (promising a Bundle result)
			CloudFeatures.cloud.Game.GameVfs.Domain(domain).GetValue(key)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("GameVFSFeatures", "GetValue", exception);

					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (Bundle keyValue)
				{
					Debug.Log(string.Format("[CotcSdkTemplate:GameVFSFeatures] GetValue success >> Key Value: {0}", keyValue));

					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(keyValue);
				});
		}
		#endregion

		#region Delegate Callbacks
		// What to do if any DisplayGameKey request succeeded
		private static void DisplayGameKey_OnSuccess(Bundle keyValue)
		{
			string resultField = "result";

			if (keyValue.Has(resultField))
				VFSHandler.Instance.FillAndShowVFSPanel(keyValue[resultField].AsDictionary());
			else
				Debug.LogError(string.Format("[CotcSdkTemplate:GameVFSFeatures] No {0} field found in the key value result", resultField));
			
			// TODO: You may want to parse the result Bundle fields (e.g.: if (keyValue["result"]["TestString"].Type == Bundle.DataType.String) { string testString = keyValue["result"]["TestString"].AsString(); })
		}

		// What to do if any DisplayGameKey request failed
		private static void DisplayGameKey_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: the specified key doesn't exist yet
				case "KeyNotFound":
				VFSHandler.Instance.FillAndShowVFSPanel(null);
				break;

				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:GameVFSFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}
		#endregion
	}
}
