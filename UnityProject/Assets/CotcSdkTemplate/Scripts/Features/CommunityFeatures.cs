using System;
using System.Collections.Generic;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to use the CotcSdk's community features.
	/// </summary>
	public static class CommunityFeatures
	{
		#region Handling
		/// <summary>
		/// Send an event from the current logged in gamer to the given other gamer.
		/// </summary>
		/// <param name="gamerID">Identifier of the gamer with who to change the relationship.</param>
		/// <param name="eventData">Details of the event to send.</param>
		/// <param name="notificationJson">Message to send as notification if the target gamer is offline under {"languageCode1":"text1", "languageCode2":"text2", ...} format. (optional)</param>
		public static void Handling_SendEventToGamer(string gamerID, string eventData, string notificationJson = null)
		{
			// The gamer ID should not be empty
			if (string.IsNullOrEmpty(gamerID))
				DebugLogs.LogError("[CotcSdkTemplate:CommunityFeatures] The gamer ID is empty ›› Please enter a valid gamer ID");
			// The event data should not be empty
			else if (string.IsNullOrEmpty(eventData))
				DebugLogs.LogError("[CotcSdkTemplate:CommunityFeatures] The event data is empty ›› Please enter some event data");
			else
			{
				Bundle eventBundle = new Bundle(eventData);
				PushNotification pushNotification = CotcConverter.GetPushNotificationFromJson(notificationJson);
				Backend_SendEvent(gamerID, eventBundle, SendEvent_OnSuccess, SendEvent_OnError, pushNotification);
			}
		}

		/// <summary>
		/// Set the relationship between the current logged in gamer and the given other gamer.
		/// </summary>
		/// <param name="gamerID">Identifier of the gamer with who to change the relationship.</param>
		/// <param name="relationship">Type of relationship to set.</param>
		/// <param name="notificationJson">Message to send as notification if the target gamer is offline under {"languageCode1":"text1", "languageCode2":"text2", ...} format. (optional)</param>
		public static void Handling_SetGamerRelationship(string gamerID, FriendRelationshipStatus relationship, string notificationJson = null)
		{
			// The gamer ID should not be empty
			if (string.IsNullOrEmpty(gamerID))
				DebugLogs.LogError("[CotcSdkTemplate:CommunityFeatures] The gamer ID is empty ›› Please enter a valid gamer ID");
			else
			{
				PushNotification pushNotification = CotcConverter.GetPushNotificationFromJson(notificationJson);
				Backend_ChangeRelationshipStatus(gamerID, relationship, SetGamerRelationship_OnSuccess, SetGamerRelationship_OnError, pushNotification);
			}
		}
		#endregion

		#region Features
		/// <summary>
		/// Send an event from the current logged in gamer to the given other gamer.
		/// </summary>
		/// <param name="gamerID">Identifier of the gamer with who to change the relationship.</param>
		/// <param name="eventData">Details of the event to send under Bundle format.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="pushNotification">Message to send as notification if the target gamer is offline. (optional)</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_SendEvent(string gamerID, Bundle eventData, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null, PushNotification pushNotification = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Done result
			CloudFeatures.gamer.Community.Domain(domain).SendEvent(gamerID, eventData, pushNotification)
				// Result if everything went well
				.Done(delegate (Done sentDone)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:CommunityFeatures] SendEvent success ›› Successful: {0}", sentDone.Successful));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(sentDone);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("CommunityFeatures", "SendEvent", exception);
				});
		}

		/// <summary>
		/// Set the relationship between the current logged in gamer and the given other gamer.
		/// </summary>
		/// <param name="gamerID">Identifier of the gamer with who to change the relationship.</param>
		/// <param name="relationship">Type of relationship to set.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="pushNotification">Message to send as notification if the target gamer is offline. (optional)</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_ChangeRelationshipStatus(string gamerID, FriendRelationshipStatus relationship, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null, PushNotification pushNotification = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;

			// Call the API method which returns a Done result
			CloudFeatures.gamer.Community.Domain(domain).ChangeRelationshipStatus(gamerID, relationship, pushNotification)
				// Result if everything went well
				.Done(delegate (Done changeDone)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:CommunityFeatures] ChangeRelationshipStatus success ›› Successful: {0}", changeDone.Successful));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(changeDone);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("CommunityFeatures", "ChangeRelationshipStatus", exception);
				});
		}
		#endregion

		#region Delegate Callbacks
		/// <summary>
		/// What to do if any SendEvent request succeeded.
		/// </summary>
		/// <param name="sentDone">Request result details.</param>
		private static void SendEvent_OnSuccess(Done sentDone)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any SendEvent request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void SendEvent_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:CommunityFeatures] An unhandled error occured ›› {0}", exceptionError));
				break;
			}
		}

		/// <summary>
		/// What to do if any SetGamerRelationship request succeeded.
		/// </summary>
		/// <param name="changeDone">Request result details.</param>
		private static void SetGamerRelationship_OnSuccess(Done changeDone)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any SetGamerRelationship request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void SetGamerRelationship_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:CommunityFeatures] An unhandled error occured ›› {0}", exceptionError));
				break;
			}
		}
		#endregion
	}
}
