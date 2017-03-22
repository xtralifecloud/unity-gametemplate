using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to use the CotcSdk's account features.
	/// </summary>
	public static class AccountFeatures
	{
		#region Handling
		/// <summary>
		/// Convert a logged in gamer's anonymous account to an email one with the given credentials.
		/// </summary>
		/// <param name="email">Identifier of the gamer's email account.</param>
		/// <param name="password">Password of the gamer's email account.</param>
		public static void Handling_ConvertAnonymousToEmail(string email, string password)
		{
			// The email should not be empty
			if (string.IsNullOrEmpty(email))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] The email is empty ›› Please enter a valid email");
			// The password should not be empty
			else if (string.IsNullOrEmpty(password))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] The password is empty ›› Please enter a valid password");
			// The logged in account type should be anonymous
			else if ((CloudFeatures.gamer == null) || (CloudFeatures.gamer["network"].AsString() != "anonymous"))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] Wrong account type ›› Please try to convert only anonymous type account");
			else
			{
				string network = LoginNetwork.Email.ToString().ToLower();
				Backend_Convert(network, email, password, Convert_OnSuccess, Convert_OnError);
			}
		}
		#endregion

		#region Backend
		/// <summary>
		/// Convert a logged in gamer's anonymous account to an email one with the given credentials.
		/// </summary>
		/// <param name="network">Name of the network to use (lowercase from the LoginNetwork enum).</param>
		/// <param name="accountID">Identifier (email, ID, ...) of the gamer's account.</param>
		/// <param name="accountSecret">Secret (password, token, ...) of the gamer's account.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_Convert(string network, string accountID, string accountSecret, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(ErrorCode.NotLoggedIn), "NotLoggedIn"));
				return;
			}

			// Call the API method which returns a Done result
			CloudFeatures.gamer.Account.Convert(network, accountID, accountSecret)
				// Result if everything went well
				.Done(delegate (Done convertDone)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:AccountFeatures] Convert success ›› Successful: {0}", convertDone.Successful));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(convertDone);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("AccountFeatures", "Convert", exception);
				});
		}
		#endregion

		#region Delegate Callbacks
		/// <summary>
		/// What to do if any Convert request succeeded.
		/// </summary>
		/// <param name="convertDone">Request result details.</param>
		private static void Convert_OnSuccess(Done convertDone)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any Convert request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void Convert_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case "NotLoggedIn":
				// Do whatever...
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "AccountFeatures", exceptionError));
				break;
			}
		}
		#endregion
	}
}
