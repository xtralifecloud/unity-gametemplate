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

		// Login with a new anonymous account
		public static void LoginAsAnonymous()
		{
			LoginAnonymously();
		}

		// Login with a previously created account
		public static void LoginWithCredentials(string gamerID, string gamerSecret)
		{
			// The gamer ID should not be empty
			if (string.IsNullOrEmpty(gamerID))
				Debug.LogError("[CotcSdkTemplate:LoginFeatures] The gamer ID is empty >> Please enter a valid gamer ID");
			// The gamer secret should not be empty
			else if (string.IsNullOrEmpty(gamerSecret))
				Debug.LogError("[CotcSdkTemplate:LoginFeatures] The gamer secret is empty >> Please enter a valid gamer secret");
			else
				ResumeSession(gamerID, gamerSecret);
		}

		// Logout the current logged in gamer
		public static void LogoutGamer()
		{
			Logout();
		}
		#endregion

		#region Features
		// Login with an anonymous account
		private static void LoginAnonymously()
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
						ExceptionTools.LogCotcException("LoginFeatures", "LoginAnonymously", exception);
					})
				// The result if everything went well
				.Done(delegate (Gamer loggedInGamer)
					{
						Debug.Log(string.Format("[CotcSdkTemplate:LoginFeatures] LoginAnonymously success >> {0}", loggedInGamer));

						// Keep the Gamer's reference
						CloudFeatures.gamer = loggedInGamer;

						// Call the GamerLoggedIn event if any callback registered to it
						if (GamerLoggedIn != null)
							GamerLoggedIn(CloudFeatures.gamer);
					});
		}

		// Login with the last used account
		private static void ResumeSession(string gamerID, string gamerSecret)
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
						ExceptionTools.LogCotcException("LoginFeatures", "ResumeSession", exception);
					})
				// The result if everything went well
				.Done(delegate (Gamer loggedInGamer)
					{
						Debug.Log(string.Format("[CotcSdkTemplate:LoginFeatures] ResumeSession success >> {0}", loggedInGamer));

						// Keep the Gamer's reference
						CloudFeatures.gamer = loggedInGamer;

						// Call the GamerLoggedIn event if any callback registered to it
						if (GamerLoggedIn != null)
							GamerLoggedIn(CloudFeatures.gamer);
					});
		}

		// Logout the current logged in gamer
		private static void Logout()
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;

			// Call the API method which returns a Promise<Done> (promising a Done result)
			CloudFeatures.cloud.Logout()
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						ExceptionTools.LogCotcException("LoginFeatures", "Logout", exception);
					})
				// The result if everything went well
				.Done(delegate (Done logoutDone)
					{
						Debug.Log(string.Format("[CotcSdkTemplate:LoginFeatures] Logout success >> {0}", logoutDone));

						// Discard the Gamer's reference
						CloudFeatures.gamer = null;

						// Call the GamerLoggedOut event if any callback registered to it
						if (GamerLoggedOut != null)
							GamerLoggedOut();
					});
		}
		#endregion

		#region Events Callbacks
		// Allow the registration of callbacks for when a gamer has logged in
		public static event Action<Gamer> GamerLoggedIn = OnGamerLoggedIn;

		// Allow the registration of callbacks for when a gamer has logged out
		public static event Action GamerLoggedOut = OnGamerLoggedOut;

		// What to do once a gamer has logged in
		private static void OnGamerLoggedIn(Gamer gamer)
		{
			// Keep the gamerID and gamerSecret credentials in PlayerPrefs to allow to use them later to login with this same account again
			// TODO: You may want to encrypt those credentials for obvious security reasons!
			PlayerPrefs.SetString(gamerIDPrefKey, gamer.GamerId);
			PlayerPrefs.SetString(gamerSecretPrefKey, gamer.GamerSecret);

			// Update the login status text with the newly connected gamer infos
			if (LoginHandler.HasInstance)
				LoginHandler.Instance.UpdateText_LoginStatus(gamer);
		}

		// What to do once a gamer has logged out
		private static void OnGamerLoggedOut()
		{
			// Discard the gamerID and gamerSecret credentials in PlayerPrefs to prevent to use them later to login with this same account again
			PlayerPrefs.DeleteKey(gamerIDPrefKey);
			PlayerPrefs.DeleteKey(gamerSecretPrefKey);

			// Update the login status text with the newly connected gamer infos
			if (LoginHandler.HasInstance)
				LoginHandler.Instance.UpdateText_LoginStatus();
		}
		#endregion
	}
}
