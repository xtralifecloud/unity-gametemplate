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
			else if ((LoginFeatures.gamer == null) || (LoginFeatures.gamer["network"].AsString() != LoginNetwork.Anonymous.Describe()))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] Wrong account type ›› Only anonymous type accounts can use this feature");
			else
			{
				string network = LoginNetwork.Email.Describe();
				Backend_Convert(network, email, password, Convert_OnSuccess, Convert_OnError);
			}
		}

		/// <summary>
		/// Change a logged in gamer's email account's email address.
		/// </summary>
		/// <param name="newEmailAddress">New identifier of the gamer's email account.</param>
		public static void Handling_ChangeEmailAddress(string newEmailAddress)
		{
			// The new email address should not be empty
			if (string.IsNullOrEmpty(newEmailAddress))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] The new email address is empty ›› Please enter a valid new email address");
			// The logged in account type should be email
			else if ((LoginFeatures.gamer == null) || (LoginFeatures.gamer["network"].AsString() != LoginNetwork.Email.Describe()))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] Wrong account type ›› Only email type accounts can use this feature");
			else
				Backend_ChangeEmailAddress(newEmailAddress, ChangeEmailAddress_OnSuccess, ChangeEmailAddress_OnError);
		}

		/// <summary>
		/// Change a logged in gamer's email account's password.
		/// </summary>
		/// <param name="newPassword">New password of the gamer's email account.</param>
		public static void Handling_ChangeEmailPassword(string newPassword)
		{
			// The new password should not be empty
			if (string.IsNullOrEmpty(newPassword))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] The new password is empty ›› Please enter a valid new password");
			// The logged in account type should be email
			else if ((LoginFeatures.gamer == null) || (LoginFeatures.gamer["network"].AsString() != LoginNetwork.Email.Describe()))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] Wrong account type ›› Only email type accounts can use this feature");
			else
				Backend_ChangePassword(newPassword, ChangeEmailPassword_OnSuccess, ChangeEmailPassword_OnError);
		}

		/// <summary>
		/// Send an email to a gamer who has lost its email account's password.
		/// </summary>
		/// <param name="toEmailAddress">Email address of the gamer to who the email will be sent.</param>
		/// <param name="fromEmailAddress">Email address of the company from which the email will be sent.</param>
		/// <param name="emailTitle">Title of the email to send.</param>
		/// <param name="emailBody">Body of the email to send. (needs to contain the [[SHORTCODE]] tag)</param>
		public static void Handling_SendLostPasswordEmail(string toEmailAddress, string fromEmailAddress, string emailTitle, string emailBody)
		{
			// The to email address should not be empty
			if (string.IsNullOrEmpty(toEmailAddress))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] The to email address is empty ›› Please enter a valid to email address");
			// The from email address should not be empty
			else if (string.IsNullOrEmpty(fromEmailAddress))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] The from email address is empty ›› Please enter a valid from email address");
			// The email title should not be empty
			else if (string.IsNullOrEmpty(emailTitle))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] The email title is empty ›› Please enter a valid email title");
			// The email body should not be empty
			else if (string.IsNullOrEmpty(emailBody))
				DebugLogs.LogError("[CotcSdkTemplate:AccountFeatures] The email body is empty ›› Please enter a valid email body");
			else
				Backend_SendResetPasswordEmail(toEmailAddress, fromEmailAddress, emailTitle, emailBody, SendResetPasswordEmail_OnSuccess, SendResetPasswordEmail_OnError);
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
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}

			// Call the API method which returns a Done result
			LoginFeatures.gamer.Account.Convert(network, accountID, accountSecret)
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

		/// <summary>
		/// Change a logged in gamer's email account's email address.
		/// </summary>
		/// <param name="newEmailAddress">New identifier of the gamer's email account.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_ChangeEmailAddress(string newEmailAddress, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}

			// Call the API method which returns a Done result
			LoginFeatures.gamer.Account.ChangeEmailAddress(newEmailAddress)
				// Result if everything went well
				.Done(delegate (Done changeDone)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:AccountFeatures] ChangeEmailAddress success ›› Successful: {0}", changeDone.Successful));
					
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
						ExceptionTools.LogCotcException("AccountFeatures", "ChangeEmailAddress", exception);
				});
		}

		/// <summary>
		/// Change a logged in gamer's email account's password.
		/// </summary>
		/// <param name="newPassword">New password of the gamer's email account.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_ChangePassword(string newPassword, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}

			// Call the API method which returns a Done result
			LoginFeatures.gamer.Account.ChangePassword(newPassword)
				// Result if everything went well
				.Done(delegate (Done changeDone)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:AccountFeatures] ChangePassword success ›› Successful: {0}", changeDone.Successful));
					
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
						ExceptionTools.LogCotcException("AccountFeatures", "ChangePassword", exception);
				});
		}

		/// <summary>
		/// Send an email to a gamer who has lost its email account's password.
		/// </summary>
		/// <param name="toEmailAddress">Email address of the gamer to who the email will be sent.</param>
		/// <param name="fromEmailAddress">Email address of the company from which the email will be sent.</param>
		/// <param name="emailTitle">Title of the email to send.</param>
		/// <param name="emailBody">Body of the email to send. (needs to contain the [[SHORTCODE]] tag)</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_SendResetPasswordEmail(string toEmailAddress, string fromEmailAddress, string emailTitle, string emailBody, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			// Need an initialized Cloud to proceed
			if (!CloudFeatures.IsCloudInitialized())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotSetup), ExceptionTools.notInitializedCloudErrorType));
				return;
			}

			// Call the API method which returns a Done result
			CloudFeatures.cloud.SendResetPasswordEmail(toEmailAddress, fromEmailAddress, emailTitle, emailBody)
				// Result if everything went well
				.Done(delegate (Done sendDone)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:AccountFeatures] SendResetPasswordEmail success ›› Successful: {0}", sendDone.Successful));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(sendDone);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("AccountFeatures", "SendResetPasswordEmail", exception);
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
				case ExceptionTools.notLoggedInErrorType:
				// Do whatever...
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "AccountFeatures", exceptionError));
				break;
			}
		}

		/// <summary>
		/// What to do if any ChangeEmailAddress request succeeded.
		/// </summary>
		/// <param name="changeDone">Request result details.</param>
		private static void ChangeEmailAddress_OnSuccess(Done changeDone)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any ChangeEmailAddress request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void ChangeEmailAddress_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				// Do whatever...
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "AccountFeatures", exceptionError));
				break;
			}
		}

		/// <summary>
		/// What to do if any ChangeEmailPassword request succeeded.
		/// </summary>
		/// <param name="changeDone">Request result details.</param>
		private static void ChangeEmailPassword_OnSuccess(Done changeDone)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any ChangeEmailPassword request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void ChangeEmailPassword_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				// Do whatever...
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "AccountFeatures", exceptionError));
				break;
			}
		}

		/// <summary>
		/// What to do if any SendResetPasswordEmail request succeeded.
		/// </summary>
		/// <param name="sendDone">Request result details.</param>
		private static void SendResetPasswordEmail_OnSuccess(Done sendDone)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any SendResetPasswordEmail request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void SendResetPasswordEmail_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud
				case ExceptionTools.notInitializedCloudErrorType:
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
