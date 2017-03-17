using System;
using System.Collections.Generic;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to use the CotcSdk's godfather features.
	/// </summary>
	public static class GodfatherFeatures
	{
		#region Handling
		/// <summary>
		/// Get and display the logged in gamer's referral code.
		/// </summary>
		public static void Handling_DisplayReferralCode()
		{
			// A GodfatherHandler instance should be attached to an active object of the scene to display the result
			if (!GodfatherHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "GodfatherFeatures", "GodfatherHandler"));
			else
			{
				GodfatherHandler.Instance.ShowGodfatherPanel("Referral Code");
				Backend_GenerateCode(DisplayReferralCode_OnSuccess, DisplayReferralCode_OnError);
			}
		}
		#endregion

		#region Features
		/// <summary>
		/// Get the logged in gamer's referral code.
		/// </summary>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_GenerateCode(Action<string> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(ErrorCode.NotLoggedIn), "NotLoggedIn"));
				return;
			}
			
			// Call the API method which returns a string result
			CloudFeatures.gamer.Godfather.Domain(domain).GenerateCode()
				// Result if everything went well
				.Done(delegate (string referralCode)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:GodfatherFeatures] GenerateCode success ›› Referral Code: {0}", referralCode));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(referralCode);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("GodfatherFeatures", "GenerateCode", exception);
				});
		}
		#endregion

		#region Delegate Callbacks
		/// <summary>
		/// What to do if any DisplayReferralCode request succeeded.
		/// </summary>
		/// <param name="referralCode">The referral code to give to potential godchildren.</param>
		private static void DisplayReferralCode_OnSuccess(string referralCode)
		{
			GodfatherHandler.Instance.FillGodfatherPanel(referralCode);
		}

		/// <summary>
		/// What to do if any DisplayReferralCode request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void DisplayReferralCode_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case "NotLoggedIn":
				GodfatherHandler.Instance.ShowError(ExceptionTools.notLoggedInMessage);
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "GodfatherFeatures", exceptionError));
				GodfatherHandler.Instance.ShowError(ExceptionTools.unhandledErrorMessage);
				break;
			}
		}
		#endregion
	}
}
