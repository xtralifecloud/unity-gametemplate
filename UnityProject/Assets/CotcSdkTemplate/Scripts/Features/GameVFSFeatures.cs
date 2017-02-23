using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to use the CotcSdk's game VFS (Virtual File System) features.
	/// </summary>
	public static class GameVFSFeatures
	{
		#region Handling
		/// <summary>
		/// Get and display the value of the given key (or all keys if null or empty) associated to the current game.
		/// </summary>
		/// <param name="key">Name of the key to get.</param>
		public static void Handling_DisplayGameKey(string key)
		{
			// A VFSHandler instance should be attached to an active object of the scene to display the result
			if (!VFSHandler.HasInstance)
				Debug.LogError("[CotcSdkTemplate:GameVFSFeatures] No VFSHandler instance found >> Please attach a VFSHandler script on an active object of the scene");
			else
				Backend_GetValue(key, DisplayGameKey_OnSuccess, DisplayGameKey_OnError);
		}
		#endregion

		#region Features
		/// <summary>
		/// Get the value of the given key (or all keys if null or empty) associated to the current game.
		/// </summary>
		/// <param name="key">Name of the key to get.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_GetValue(string key, Action<Bundle> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud to proceed
			if (!CloudFeatures.IsCloudInitialized())
				return;
			
			// Call the API method which returns a Bundle result
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
				.Done(delegate (Bundle keysValues)
				{
					Debug.Log(string.Format("[CotcSdkTemplate:GameVFSFeatures] GetValue success >> Keys Values: {0}", keysValues));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(keysValues);
				});
		}
		#endregion

		#region Delegate Callbacks
		/// <summary>
		/// What to do if any DisplayGameKey request succeeded.
		/// </summary>
		/// <param name="keysValues">List of keys and their values under the Bundle format.</param>
		private static void DisplayGameKey_OnSuccess(Bundle keysValues)
		{
			string resultField = "result";

			if (keysValues.Has(resultField))
				VFSHandler.Instance.FillAndShowVFSPanel(keysValues[resultField].AsDictionary(), "Game VFS Keys");
			else
				Debug.LogError(string.Format("[CotcSdkTemplate:GameVFSFeatures] No {0} field found in the key value result", resultField));
			
			// TODO: You may want to parse the result Bundle fields (e.g.: if (keyValue["result"]["TestString"].Type == Bundle.DataType.String) { string testString = keyValue["result"]["TestString"].AsString(); })
		}

		/// <summary>
		/// What to do if any DisplayGameKey request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
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
