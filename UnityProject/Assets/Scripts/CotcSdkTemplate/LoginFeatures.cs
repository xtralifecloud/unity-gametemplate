using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class LoginFeatures
	{
		#region Handling
		// The PlayerPref keys to store the last used account's gamerID and gamerSecret
		private const string gamerIDPrefKey = "GamerID";
		private const string gamerSecretPrefKey = "GamerSecret";

		// Login with the last used account if any exist or login anonymously
		public static void AutoLogin()
		{
			// Retrieve the last stored credentials if any
			string storedGamerID = PlayerPrefs.GetString(gamerIDPrefKey);
			string storedGamerSecret = PlayerPrefs.GetString(gamerSecretPrefKey);

			// If credentials are found, use them to login to the last used account
			if (!string.IsNullOrEmpty(storedGamerID) && !string.IsNullOrEmpty(storedGamerSecret))
				ResumeSession(storedGamerID, storedGamerSecret);
			// Else, create a new anonymous account
			else
				LoginAnonymously();
		}
		#endregion

		#region Features
		// Login with an anonymous account
		public static void LoginAnonymously()
		{
			// Need an initialized Cloud to proceed
			if (!CloudFeatures.IsCloudInitialized())
				return;

			// Call the API method which returns a Promise<Gamer> (promising a Gamer result)
			CloudFeatures.cloud.LoginAnonymously()
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						CotcException cotcException = exception as CotcException;

						if (cotcException != null)
							Debug.LogError(string.Format("[CotcSdkTemplate:LoginFeatures] LoginAnonymously failed >> ({0}) {1}: {2} >> {3}", cotcException.HttpStatusCode, cotcException.ErrorCode, cotcException.ErrorInformation, cotcException.ServerData));
						else
							Debug.LogError(string.Format("[CotcSdkTemplate:LoginFeatures] LoginAnonymously failed >> {0}", exception));
					})
				// The result if everything went well
				.Done(delegate (Gamer loggedInGamer)
					{
						// Keep the Gamer's reference
						CloudFeatures.gamer = loggedInGamer;
						Debug.Log(string.Format("[CotcSdkTemplate:LoginFeatures] LoginAnonymously success >> {0}", loggedInGamer));

						// Call the GamerLoggedIn event if any callback registered to it
						if (GamerLoggedIn != null)
							GamerLoggedIn(CloudFeatures.gamer);
					});
		}

		// Login with the last used account
		public static void ResumeSession(string gamerID, string gamerSecret)
		{
			// Need an initialized Cloud to proceed
			if (!CloudFeatures.IsCloudInitialized())
				return;

			// Call the API method which returns a Promise<Gamer> (promising a Gamer result)
			CloudFeatures.cloud.ResumeSession(gamerID, gamerSecret)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						CotcException cotcException = exception as CotcException;

						if (cotcException != null)
							Debug.LogError(string.Format("[CotcSdkTemplate:LoginFeatures] ResumeSession failed >> ({0}) {1}: {2} >> {3}", cotcException.HttpStatusCode, cotcException.ErrorCode, cotcException.ErrorInformation, cotcException.ServerData));
						else
							Debug.LogError(string.Format("[CotcSdkTemplate:LoginFeatures] ResumeSession failed >> {0}", exception));
					})
				// The result if everything went well
				.Done(delegate (Gamer loggedInGamer)
					{
						// Keep the Gamer's reference
						CloudFeatures.gamer = loggedInGamer;
						Debug.Log(string.Format("[CotcSdkTemplate:LoginFeatures] ResumeSession success >> {0}", loggedInGamer));

						// Call the GamerLoggedIn event if any callback registered to it
						if (GamerLoggedIn != null)
							GamerLoggedIn(CloudFeatures.gamer);
					});
		}
		#endregion

		#region Events Callbacks
		// Allow the registration of callbacks for when a gamer has logged in
		public static event Action<Gamer> GamerLoggedIn = OnGamerLoggedIn;

		// What to do once a gamer has logged in
		private static void OnGamerLoggedIn(Gamer gamer)
		{
			// Keep the gamerID and gamerSecret credentials in PlayerPrefs to allow to use them later to login with this same account again
			// TODO: You may want to encrypt those credentials for obvious security reasons!
			PlayerPrefs.SetString(gamerIDPrefKey, gamer.GamerId);
			PlayerPrefs.SetString(gamerSecretPrefKey, gamer.GamerSecret);
		}
		#endregion
	}
}
