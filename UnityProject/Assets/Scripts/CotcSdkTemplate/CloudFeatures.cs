using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class CloudFeatures
	{
		#region Initialization
		// The cloud allows to make generic operations (non user related)
		public static Cloud cloud = null;
		// The gamer is the base to perform most operations. A gamer object is obtained after successfully signing in.
		public static Gamer gamer = null;

		// Initialize the CotcSdk's Cloud
		public static void InitializeCloud()
		{
			// Find the CotcGameObject instance in the scene
			CotcGameObject cotcGameObject = GameObject.FindObjectOfType<CotcGameObject>();

			if (cotcGameObject == null)
			{
				Debug.LogError("[CotcSdkTemplate] Please attach a CotcGameObject script on an object of your scene! CotcSdk features are not available otherwise.");
				return;
			}

			// Register to the UnhandledException event
			Promise.UnhandledException += LogUnhandledException;

			// Get the main Cloud object's reference
			cotcGameObject.GetCloud().Done(cloudReference =>
				{
					cloud = cloudReference;
					Debug.Log("[CotcSdkTemplate] Cloud initialized");

					// Register to the HttpRequestFailedHandler event
					cloud.HttpRequestFailedHandler = RetryFailedRequestOnce;

					// Call the CloudInitialized event if any callback registered to it
					if (CloudInitialized != null)
						CloudInitialized(cloud);
				});
		}
		#endregion

		#region Events Callbacks
		// Allow the registration of callbacks for when the Cloud is initialized
		public static event Action<Cloud> CloudInitialized = null;

		// Time to wait before a failed HTPP request retry
		private const int httpRequestRetryDelay = 1000;

		// Log unhandled exceptions (.Done block without .Catch -- Not called if there is any .Then)
		private static void LogUnhandledException(object sender, ExceptionEventArgs exceptionEventArgs)
		{
			Debug.LogError("[CotcSdkTemplate] Unhandled exception: " + exceptionEventArgs.Exception);
		}

		// Retry failed HTTP requests once
		private static void RetryFailedRequestOnce(HttpRequestFailedEventArgs httpRequestFailedEventArgs)
		{
			if (httpRequestFailedEventArgs.UserData == null)
			{
				Debug.LogWarning(string.Format("[CotcSdkTemplate] HTTP request failed >> Retry in {0}ms ({1})", httpRequestRetryDelay, httpRequestFailedEventArgs.Url));
				httpRequestFailedEventArgs.UserData = new object();
				httpRequestFailedEventArgs.RetryIn(httpRequestRetryDelay);
			}
			else
			{
				Debug.LogError(string.Format("[CotcSdkTemplate] HTTP request failed >> Abort ({0})", httpRequestFailedEventArgs.Url));
				httpRequestFailedEventArgs.Abort();
			}
		}
		#endregion
	}
}
