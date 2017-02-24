using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to use the CotcSdk's login features.
	/// </summary>
	public static class LoginFeatures
	{
		#region Handling
		// The PlayerPref keys to store the last used account's gamerID and gamerSecret
		private const string gamerIDPrefKey = "GamerID";
		private const string gamerSecretPrefKey = "GamerSecret";

		/// <summary>
		/// Login the gamer with the last used account if any exist, or login him anonymously.
		/// </summary>
		public static void Handling_AutoLogin()
		{
			// Retrieve the last stored credentials if any
			string storedGamerID = PlayerPrefs.GetString(gamerIDPrefKey);
			string storedGamerSecret = PlayerPrefs.GetString(gamerSecretPrefKey);

			// If credentials are found, use them to login to the last used account
			if (!string.IsNullOrEmpty(storedGamerID) && !string.IsNullOrEmpty(storedGamerSecret))
				Backend_ResumeSession(storedGamerID, storedGamerSecret, Login_OnSuccess, Login_OnError);
			// Else, create a new anonymous account
			else
				Backend_LoginAnonymously(Login_OnSuccess, Login_OnError);
		}

		/// <summary>
		/// Login the gamer with a new anonymous account.
		/// </summary>
		public static void Handling_LoginAsAnonymous()
		{
			Backend_LoginAnonymously(Login_OnSuccess, Login_OnError);
		}

		/// <summary>
		/// Login the gamer with a previously created account.
		/// </summary>
		/// <param name="gamerID">Identifier of the gamer's account.</param>
		/// <param name="gamerSecret">Secret of the gamer's account.</param>
		public static void Handling_LoginWithCredentials(string gamerID, string gamerSecret)
		{
			// The gamer ID should not be empty
			if (string.IsNullOrEmpty(gamerID))
				DebugLogs.LogError("[CotcSdkTemplate:LoginFeatures] The gamer ID is empty ›› Please enter a valid gamer ID");
			// The gamer secret should not be empty
			else if (string.IsNullOrEmpty(gamerSecret))
				DebugLogs.LogError("[CotcSdkTemplate:LoginFeatures] The gamer secret is empty ›› Please enter a valid gamer secret");
			else
				Backend_ResumeSession(gamerID, gamerSecret, Login_OnSuccess, Login_OnError);
		}

		/// <summary>
		/// Logout the current logged in gamer.
		/// </summary>
		public static void Handling_LogoutGamer()
		{
			Backend_Logout(Logout_OnSuccess, Logout_OnError);
		}
		#endregion

		#region Features
		/// <summary>
		/// Login the gamer with a new anonymous account.
		/// </summary>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_LoginAnonymously(Action<Gamer> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			// Need an initialized Cloud to proceed
			if (!CloudFeatures.IsCloudInitialized())
				return;
			
			// Call the API method which returns a Gamer result
			CloudFeatures.cloud.LoginAnonymously()
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("LoginFeatures", "LoginAnonymously", exception);
					
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (Gamer loggedInGamer)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:LoginFeatures] LoginAnonymously success ›› Logged In Gamer: {0}", loggedInGamer));
					
					// Keep the Gamer's reference
					CloudFeatures.gamer = loggedInGamer;
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(loggedInGamer);
					
					// Call the GamerLoggedIn event if any callback registered to it
					if (Event_GamerLoggedIn != null)
						Event_GamerLoggedIn(CloudFeatures.gamer);
				});
		}

		/// <summary>
		/// Login the gamer with a previously created account.
		/// </summary>
		/// <param name="gamerID">Identifier of the gamer's account.</param>
		/// <param name="gamerSecret">Secret of the gamer's account.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_ResumeSession(string gamerID, string gamerSecret, Action<Gamer> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			// Need an initialized Cloud to proceed
			if (!CloudFeatures.IsCloudInitialized())
				return;
			
			// Call the API method which returns a Gamer result
			CloudFeatures.cloud.ResumeSession(gamerID, gamerSecret)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("LoginFeatures", "ResumeSession", exception);
					
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (Gamer loggedInGamer)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:LoginFeatures] ResumeSession success ›› Logged In Gamer: {0}", loggedInGamer));
					
					// Keep the Gamer's reference
					CloudFeatures.gamer = loggedInGamer;
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(loggedInGamer);
					
					// Call the GamerLoggedIn event if any callback registered to it
					if (Event_GamerLoggedIn != null)
						Event_GamerLoggedIn(CloudFeatures.gamer);
				});
		}

		/// <summary>
		/// Logout the current logged in gamer.
		/// </summary>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_Logout(Action OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Done result
			CloudFeatures.cloud.Logout()
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("LoginFeatures", "Logout", exception);
					
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (Done logoutDone)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:LoginFeatures] Logout success ›› Successful: {0}", logoutDone.Successful));
					
					// Discard the Gamer's reference
					CloudFeatures.gamer = null;
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess();
					
					// Call the GamerLoggedOut event if any callback registered to it
					if (Event_GamerLoggedOut != null)
						Event_GamerLoggedOut();
				});
		}
		#endregion

		#region Delegate Callbacks
		/// <summary>
		/// What to do if any Login request succeeded.
		/// </summary>
		/// <param name="loggedInGamer">The logged in Gamer instance.</param>
		private static void Login_OnSuccess(Gamer loggedInGamer)
		{
			// Update the login status text with the newly connected gamer infos
			if (LoginHandler.HasInstance)
				LoginHandler.Instance.UpdateText_LoginStatus(loggedInGamer);
		}

		/// <summary>
		/// What to do if any Login request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void Login_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:LoginFeatures] An unhandled error occured ›› {0}", exceptionError));
				break;
			}
		}

		/// <summary>
		/// What to do if any Logout request succeeded.
		/// </summary>
		private static void Logout_OnSuccess()
		{
			// Update the login status text with no gamer infos
			if (LoginHandler.HasInstance)
				LoginHandler.Instance.UpdateText_LoginStatus();
		}

		/// <summary>
		/// What to do if any Logout request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void Logout_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:LoginFeatures] An unhandled error occured ›› {0}", exceptionError));
				break;
			}
		}
		#endregion

		#region Events Callbacks
		// Allow the registration of callbacks for when a gamer has logged in
		public static event Action<Gamer> Event_GamerLoggedIn = OnGamerLoggedIn;

		// Allow the registration of callbacks for when a gamer has logged out
		public static event Action Event_GamerLoggedOut = OnGamerLoggedOut;

		/// <summary>
		/// Once a gamer has logged in, store his account ID and secret to PlayerPrefs.
		/// </summary>
		/// <param name="loggedInGamer">The logged in Gamer instance.</param>
		private static void OnGamerLoggedIn(Gamer loggedInGamer)
		{
			// Keep the gamerID and gamerSecret credentials in PlayerPrefs to allow to use them later to login with this same account again
			// TODO: You may want to encrypt those credentials for obvious security reasons!
			PlayerPrefs.SetString(gamerIDPrefKey, loggedInGamer.GamerId);
			PlayerPrefs.SetString(gamerSecretPrefKey, loggedInGamer.GamerSecret);
		}

		/// <summary>
		/// Once a gamer has logged out, discard his account ID and secret from PlayerPrefs.
		/// </summary>
		private static void OnGamerLoggedOut()
		{
			// Discard the gamerID and gamerSecret credentials in PlayerPrefs to prevent to use them later to login with this same account again
			PlayerPrefs.DeleteKey(gamerIDPrefKey);
			PlayerPrefs.DeleteKey(gamerSecretPrefKey);
		}
		#endregion
	}
}
