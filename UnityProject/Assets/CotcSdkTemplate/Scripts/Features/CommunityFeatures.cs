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
		/// Set the relationship between the current logged in gamer and the given other gamer.
		/// </summary>
		/// <param name="gamerID">Identifier of the gamer with who to change the relationship.</param>
		/// <param name="notificationJson">Message to send as notification if the targeted gamer is offline under {"languageCode1":"correspondingText1", "languageCode2":"correspondingText2", ...} format.</param>
		public static void Handling_SetGamerRelationship(string gamerID, string notificationJson, FriendRelationshipStatus relationship)
		{
			// The gamer ID should not be empty
			if (string.IsNullOrEmpty(gamerID))
				DebugLogs.LogError("[CotcSdkTemplate:CommunityFeatures] The gamer ID is empty ›› Please enter a valid gamer ID");
			else
			{
				PushNotification pushNotification = null;

				if (!string.IsNullOrEmpty(notificationJson))
				{
					// Get a Dictionary of all language/text pairs from the notification json string
					Dictionary<string, Bundle> notificationLanguagesTexts = Bundle.FromJson(notificationJson).AsDictionary();
					pushNotification = new PushNotification();

					// Add an entry in the pushNotification object for each language/text pair contained in the dictionary
					foreach (KeyValuePair<string, Bundle> notificationLanguageText in notificationLanguagesTexts)
						pushNotification.Message(notificationLanguageText.Key, notificationLanguageText.Value.AsString());
				}
				
				Backend_ChangeRelationshipStatus(gamerID, relationship, pushNotification, SetGamerRelationship_OnSuccess, SetGamerRelationship_OnError);
			}
		}
		#endregion

		#region Features
		/// <summary>
		/// Set the relationship between the current logged in gamer and the given other gamer.
		/// </summary>
		/// <param name="gamerID">Identifier of the gamer with who to change the relationship.</param>
		/// <param name="pushNotification">Message to send as notification if the targeted gamer is offline.</param>
		/// <param name="relationship">Type of relationship to set.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_ChangeRelationshipStatus(string gamerID, FriendRelationshipStatus relationship, PushNotification pushNotification, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Dictionary<string, AchievementDefinition> result
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
