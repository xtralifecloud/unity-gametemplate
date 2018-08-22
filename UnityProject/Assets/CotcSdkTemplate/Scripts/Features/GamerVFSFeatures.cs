using System;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to use the CotcSdk's gamer VFS (Virtual File System) features.
	/// </summary>
	public static class GamerVFSFeatures
	{
		#region Handling
		/// <summary>
		/// Get and display the value of the given key (or all keys if null or empty) associated to the current logged in gamer.
		/// </summary>
		/// <param name="key">Name of the key to get.</param>
		public static void Handling_DisplayGamerKey(string key)
		{
			// A VFSHandler instance should be attached to an active object of the scene to display the result
			if (!VFSHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "GamerVFSFeatures", "VFSHandler"));
			else
			{
				VFSHandler.Instance.ShowVFSPanel("Gamer VFS Keys");
				Backend_GetValue(key, DisplayGamerKey_OnSuccess, DisplayGamerKey_OnError);
			}
		}

		/// <summary>
		/// Create / update the value of the given key associated to the current logged in gamer.
		/// </summary>
		/// <param name="valueType">Type of the value to set.</param>
		/// <param name="key">Name of the key to set.</param>
		/// <param name="value">Value of the key to set.</param>
		public static void Handling_SetGamerKey(Bundle.DataType valueType, string key, string value)
		{
			// The key name should not be empty
			if (string.IsNullOrEmpty(key))
				DebugLogs.LogError("[CotcSdkTemplate:GamerVFSFeatures] The key name is empty ›› Please enter a valid key name");
			else
			{
				Bundle setValue;

				// Create a Bundle from the given value string (a Bundle is an json-like object to represent fields data) according to expected value type
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

					// TODO: You may want to add the Array (list) type handling
					default:
					DebugLogs.LogError(string.Format("[CotcSdkTemplate:GamerVFSFeatures] The {0} type is unhandled ›› Please handle it or use an handled type", valueType));
					return;
				}

				Backend_SetValue(key, setValue, SetGamerKey_OnSuccess, SetGamerKey_OnError);
			}
		}

		/// <summary>
		/// Delete the given key associated to the current logged in gamer.
		/// </summary>
		/// <param name="key">Name of the key to delete.</param>
		public static void Handling_DeleteGamerKey(string key)
		{
			// The key name should not be empty
			if (string.IsNullOrEmpty(key))
				DebugLogs.LogError("[CotcSdkTemplate:GamerVFSFeatures] The key name is empty ›› Please enter a valid key name");
			else
				Backend_DeleteValue(key, DeleteGamerKey_OnSuccess, DeleteGamerKey_OnError);
		}
		#endregion

		#region Backend
		/// <summary>
		/// Get the value of the given key (or all keys if null or empty) associated to the current logged in gamer.
		/// </summary>
		/// <param name="key">Name of the key to get.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_GetValue(string key, Action<Bundle> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}

			// Call the API method which returns a Bundle result
			LoginFeatures.gamer.GamerVfs.Domain(domain).GetValue(key)
				// Result if everything went well
				.Done(delegate (Bundle keyValues)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:GamerVFSFeatures] GetValue success ›› Keys Values: {0}", keyValues));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(keyValues);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("GamerVFSFeatures", "GetValue", exception);
				});
		}

		/// <summary>
		/// Create / update the value of the given key associated to the current logged in gamer.
		/// </summary>
		/// <param name="key">Name of the key to set.</param>
		/// <param name="value">Value of the key to set under the Bundle format.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_SetValue(string key, Bundle value, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}

			// Call the API method which returns a Done result
			LoginFeatures.gamer.GamerVfs.Domain(domain).SetValue(key, value)
				// Result if everything went well
				.Done(delegate (Done setDone)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:GamerVFSFeatures] SetValue success ›› Successful: {0}", setDone.Successful));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(setDone);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("GamerVFSFeatures", "SetValue", exception);
				});
		}

		/// <summary>
		/// Delete the given key (or all keys if null or empty) associated to the current logged in gamer.
		/// </summary>
		/// <param name="key">Name of the key to delete.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_DeleteValue(string key, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}

			// Call the API method which returns a Done result
			LoginFeatures.gamer.GamerVfs.Domain(domain).DeleteValue(key)
				// Result if everything went well
				.Done(delegate (Done deleteDone)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:GamerVFSFeatures] DeleteValue success ›› Successful: {0}", deleteDone.Successful));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(deleteDone);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("GamerVFSFeatures", "DeleteValue", exception);
				});
		}
		#endregion

		#region Delegate Callbacks
		/// <summary>
		/// What to do if any DisplayGamerKey request succeeded.
		/// </summary>
		/// <param name="keysValues">List of keys and their values under the Bundle format.</param>
		private static void DisplayGamerKey_OnSuccess(Bundle keysValues)
		{
			string resultField = "result";

			// TODO: You may want to parse the result Bundle fields (e.g.: if (keyValue["result"]["TestString"].Type == Bundle.DataType.String) { string testString = keyValue["result"]["TestString"].AsString(); })
			if (!keysValues.Has(resultField))
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:GamerVFSFeatures] No {0} field found in the key value result", resultField));
			else
				VFSHandler.Instance.FillVFSPanel(keysValues[resultField].AsDictionary());
		}

		/// <summary>
		/// What to do if any DisplayGamerKey request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void DisplayGamerKey_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: the specified key doesn't exist yet
				case ExceptionTools.keyNotFoundErrorType:
				VFSHandler.Instance.FillVFSPanel(null);
				break;

				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				VFSHandler.Instance.ShowError(ExceptionTools.notLoggedInMessage);
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "GamerVFSFeatures", exceptionError));
				VFSHandler.Instance.ShowError(ExceptionTools.unhandledErrorMessage);
				break;
			}
		}

		/// <summary>
		/// What to do if any SetGamerKey request succeeded.
		/// </summary>
		/// <param name="setDone">Request result details.</param>
		private static void SetGamerKey_OnSuccess(Done setDone)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any SetGamerKey request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void SetGamerKey_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				// Do whatever...
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "GamerVFSFeatures", exceptionError));
				break;
			}
		}

		/// <summary>
		/// What to do if any DeleteGamerKey request succeeded.
		/// </summary>
		/// <param name="deleteDone">Request result details.</param>
		private static void DeleteGamerKey_OnSuccess(Done deleteDone)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any DeleteGamerKey request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void DeleteGamerKey_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				// Do whatever...
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "GamerVFSFeatures", exceptionError));
				break;
			}
		}
		#endregion
	}
}
