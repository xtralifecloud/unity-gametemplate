using System.Collections.Generic;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to convert Cotc objects.
	/// </summary>
	public static class CotcConverter
	{
		#region Json Handling
		/// <summary>
		/// Build a PushNotification object from its equivalent json string.
		/// </summary>
		/// <param name="notificationJson">Notification data under {"languageCode1":"text1", "languageCode2":"text2", ...} format.</param>
		public static PushNotification GetPushNotificationFromJson(string notificationJson)
		{
			PushNotification pushNotification = null;

			if (!string.IsNullOrEmpty(notificationJson))
			{
				// Get a Dictionary of all languageCode/text pairs from the notification json string
				Dictionary<string, Bundle> notificationLanguagesTexts = Bundle.FromJson(notificationJson).AsDictionary();
				pushNotification = new PushNotification();

				// Add an entry in the pushNotification object for each language/text pair contained in the dictionary
				foreach (KeyValuePair<string, Bundle> notificationLanguageText in notificationLanguagesTexts)
					pushNotification.Message(notificationLanguageText.Key, notificationLanguageText.Value.AsString());
			}

			return pushNotification;
		}
		#endregion
	}
}
