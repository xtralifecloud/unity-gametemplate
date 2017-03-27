using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to initiliaze the CotcSdk and handle the Cloud and Gamer instances.
	/// </summary>
	public static class CloudFeatures
	{
		#region Handling
		// The cloud allows to make generic operations (non user related)
		public static Cloud cloud = null;

		/// <summary>
		/// Check if the CotcSdk's Cloud instance is initialized.
		/// </summary>
		/// <param name="verbose">If the check should log in case of error.</param>
		public static bool IsCloudInitialized(bool verbose = true)
		{
			if (cloud == null)
			{
				if (verbose)
					DebugLogs.LogError("[CotcSdkTemplate:CloudFeatures] Cloud is not initialized ›› Please call CloudFeatures.InitializeCloud() first (CotcSdk features are not available otherwise)");

				return false;
			}

			return true;
		}

		/// <summary>
		/// Initialize the CotcSdk's Cloud instance.
		/// </summary>
		public static void Handling_InitializeCloud()
		{
			// Find the CotcGameObject instance in the scene
			CotcGameObject cotcGameObject = GameObject.FindObjectOfType<CotcGameObject>();

			if (cotcGameObject == null)
			{
				DebugLogs.LogError("[CotcSdkTemplate:CloudFeatures] Please attach a CotcGameObject script on an active object of your scene! (CotcSdk features are not available otherwise)");
				return;
			}

			// Register to the UnhandledException event
			Promise.UnhandledException += LogUnhandledException;

			// Get and keep the Cloud instance reference
			Backend_GetCloud(cotcGameObject, InitializeCloud_OnSuccess, InitializeCloud_OnError);
		}
		#endregion

		#region Backend
		/// <summary>
		/// Get the CotcSdk's Cloud instance.
		/// </summary>
		/// <param name="cotcGameObject">CotcSdk's CotcGameObject instance.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_GetCloud(CotcGameObject cotcGameObject, Action<Cloud> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			// Call the API method which returns a Cloud result
			cotcGameObject.GetCloud()
				// Result if everything went well
				.Done(delegate (Cloud cloudInstance)
				{
					DebugLogs.LogVerbose("[CotcSdkTemplate:CloudFeatures] GetCloud success");
					
					// Keep the Cloud instance reference
					cloud = cloudInstance;
					
					// Register to the HttpRequestFailedHandler event
					cloud.HttpRequestFailedHandler = RetryFailedRequestOnce;
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(cloudInstance);
					
					// Call the CloudInitialized event if any callback registered to it
					if (Event_CloudInitialized != null)
						Event_CloudInitialized(cloud);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("CloudFeatures", "GetCloud", exception);
				});
		}
		#endregion

		#region Delegate Callbacks
		/// <summary>
		/// What to do if any InitializeCloud request succeeded.
		/// </summary>
		/// <param name="cloudInstance">The initiliazed Cloud instance.</param>
		private static void InitializeCloud_OnSuccess(Cloud cloudInstance)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any InitializeCloud request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void InitializeCloud_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "CloudFeatures", exceptionError));
				break;
			}
		}
		#endregion

		#region Events Callbacks
		// Allow the registration of callbacks for when the Cloud is initialized
		public static event Action<Cloud> Event_CloudInitialized = null;

		// Time to wait before a failed HTPP request retry
		private const int httpRequestRetryDelay = 1000;

		/// <summary>
		/// Log unhandled exceptions (when backend requests errors occur without any .Catch or .Then block set)
		/// </summary>
		/// <param name="sender">The exception sender object reference.</param>
		/// <param name="exceptionEventArgs">The exception event details.</param>
		private static void LogUnhandledException(object sender, ExceptionEventArgs exceptionEventArgs)
		{
			// The exception should always be of the CotcException type
			ExceptionTools.LogCotcException("CloudFeatures", "Unhandled", exceptionEventArgs.Exception);
		}

		/// <summary>
		/// Retry failed HTTP requests once.
		/// </summary>
		/// <param name="httpRequestFailedEventArgs">The exception event details.</param>
		private static void RetryFailedRequestOnce(HttpRequestFailedEventArgs httpRequestFailedEventArgs)
		{
			if (httpRequestFailedEventArgs.UserData == null)
			{
				DebugLogs.LogWarning(string.Format("[CotcSdkTemplate:CloudFeatures] HTTP request failed ›› Retry in {0}ms ({1})", httpRequestRetryDelay, httpRequestFailedEventArgs.Url));
				httpRequestFailedEventArgs.UserData = new object();
				httpRequestFailedEventArgs.RetryIn(httpRequestRetryDelay);
			}
			else
			{
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:CloudFeatures] HTTP request failed ›› Abort ({0})", httpRequestFailedEventArgs.Url));
				httpRequestFailedEventArgs.Abort();
			}
		}
		#endregion
	}
}
