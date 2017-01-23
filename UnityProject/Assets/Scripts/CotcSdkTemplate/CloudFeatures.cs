﻿using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class CloudFeatures
	{
		#region Cloud Initialization
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
				Debug.LogError("[CotcSdkTemplate:CloudFeatures] Please attach a CotcGameObject script on an active object of your scene! (CotcSdk features are not available otherwise)");
				return;
			}

			// Register to the UnhandledException event
			Promise.UnhandledException += LogUnhandledException;

			// Get the main Cloud object's reference
			cotcGameObject.GetCloud()
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						CotcException cotcException = exception as CotcException;

						if (cotcException != null)
							Debug.LogError(string.Format("[CotcSdkTemplate:CloudFeatures] InitializeCloud failed >> ({0}) {1}: {2} >> {3}", cotcException.HttpStatusCode, cotcException.ErrorCode, cotcException.ErrorInformation, cotcException.ServerData));
						else
							Debug.LogError(string.Format("[CotcSdkTemplate:CloudFeatures] InitializeCloud failed >> {0}", exception));
					})
				// The result if everything went well
				.Done(delegate (Cloud cloudReference)
					{
						// Keep the Cloud's reference
						cloud = cloudReference;
						Debug.Log("[CotcSdkTemplate:CloudFeatures] InitializeCloud success");

						// Register to the HttpRequestFailedHandler event
						cloud.HttpRequestFailedHandler = RetryFailedRequestOnce;

						// Call the CloudInitialized event if any callback registered to it
						if (CloudInitialized != null)
							CloudInitialized(cloud);
					});
		}

		// Check if the CotcSdk's Cloud is initialized
		public static bool IsCloudInitialized(bool verbose = true)
		{
			if (cloud == null)
			{
				if (verbose)
					Debug.LogError("[CotcSdkTemplate:CloudFeatures] Cloud is not initialized >> Please call CloudFeatures.InitializeCloud() first (CotcSdk features are not available otherwise)");
				
				return false;
			}

			return true;
		}

		// Check if the CotcSdk's Cloud is initialized and a Gamer is logged in
		public static bool IsGamerLoggedIn(bool verbose = true)
		{
			if (!IsCloudInitialized())
				return false;

			if (gamer == null)
			{
				if (verbose)
					Debug.LogError("[CotcSdkTemplate:CloudFeatures] No Gamer is logged in >> Please call a login method first (some of the CotcSdk features are not available otherwise)");
				
				return false;
			}

			return true;
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
			// The exception should always be of the CotcException type
			CotcException cotcException = exceptionEventArgs.Exception as CotcException;

			if (cotcException != null)
				Debug.LogError(string.Format("[CotcSdkTemplate:CloudFeatures] Unhandled exception >> ({0}) {1}: {2} >> {3}", cotcException.HttpStatusCode, cotcException.ErrorCode, cotcException.ErrorInformation, cotcException.ServerData));
			else
				Debug.LogError(string.Format("[CotcSdkTemplate:CloudFeatures] Unhandled exception >> {0}", exceptionEventArgs.Exception));
		}

		// Retry failed HTTP requests once
		private static void RetryFailedRequestOnce(HttpRequestFailedEventArgs httpRequestFailedEventArgs)
		{
			if (httpRequestFailedEventArgs.UserData == null)
			{
				Debug.LogWarning(string.Format("[CotcSdkTemplate:CloudFeatures] HTTP request failed >> Retry in {0}ms ({1})", httpRequestRetryDelay, httpRequestFailedEventArgs.Url));
				httpRequestFailedEventArgs.UserData = new object();
				httpRequestFailedEventArgs.RetryIn(httpRequestRetryDelay);
			}
			else
			{
				Debug.LogError(string.Format("[CotcSdkTemplate:CloudFeatures] HTTP request failed >> Abort ({0})", httpRequestFailedEventArgs.Url));
				httpRequestFailedEventArgs.Abort();
			}
		}
		#endregion
	}
}