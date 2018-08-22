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

		/// <summary>
		/// Use a referral code to set the current logged in gamer's godfather.
		/// </summary>
		/// <param name="referralCode">Godfather's code to use to set the current logged in gamer as its godchild.</param>
		public static void Handling_UseReferralCode(string referralCode)
		{
			// The referral code should not be empty
			if (string.IsNullOrEmpty(referralCode))
				DebugLogs.LogError("[CotcSdkTemplate:GodfatherFeatures] The referral code is empty ›› Please enter a valid referral code");
			else
				Backend_UseCode(referralCode, UseReferralCode_OnSuccess, UseReferralCode_OnError);
		}

		/// <summary>
		/// Get and display the list of current logged in gamer's godchildren.
		/// </summary>
		public static void Handling_DisplayGodchildren()
		{
			// A GodfatherHandler instance should be attached to an active object of the scene to display the result
			if (!GodfatherHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "GodfatherFeatures", "GodfatherHandler"));
			else
			{
				GodfatherHandler.Instance.ShowGodfatherPanel("Current Godchildren");
				Backend_GetGodchildren(DisplayGodchildren_OnSuccess, DisplayGodchildren_OnError);
			}
		}

		/// <summary>
		/// Get and display the current logged in gamer's godfather.
		/// </summary>
		public static void Handling_DisplayGodfather()
		{
			// A GodfatherHandler instance should be attached to an active object of the scene to display the result
			if (!GodfatherHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "GodfatherFeatures", "GodfatherHandler"));
			else
			{
				GodfatherHandler.Instance.ShowGodfatherPanel("Current Godfather");
				Backend_GetGodfather(DisplayGodfather_OnSuccess, DisplayGodfather_OnError);
			}
		}
		#endregion

		#region Backend
		/// <summary>
		/// Get the logged in gamer's referral code.
		/// </summary>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_GenerateCode(Action<string> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}
			
			// Call the API method which returns a string result
			LoginFeatures.gamer.Godfather.Domain(domain).GenerateCode()
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

		/// <summary>
		/// Use a referral code to set the current logged in gamer's godfather.
		/// </summary>
		/// <param name="referralCode">Godfather's code to use to set the current logged in gamer as its godchild.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_UseCode(string referralCode, Action<Done> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}

			// Call the API method which returns a string result
			LoginFeatures.gamer.Godfather.Domain(domain).UseCode(referralCode)
				// Result if everything went well
				.Done(delegate (Done useDone)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:GodfatherFeatures] UseCode success ›› Successful: {0}", useDone.Successful));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(useDone);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("GodfatherFeatures", "UseCode", exception);
				});
		}

		/// <summary>
		/// Get the list of current logged in gamer's godchildren.
		/// </summary>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_GetGodchildren(Action<NonpagedList<GamerInfo>> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}

			// Call the API method which returns a NonpagedList<GamerInfo> result
			LoginFeatures.gamer.Godfather.Domain(domain).GetGodchildren()
				// Result if everything went well
				.Done(delegate (NonpagedList<GamerInfo> godchildrenList)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:GodfatherFeatures] GetGodchildren success ›› {0} godchild(ren)", godchildrenList.Count));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(godchildrenList);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("GodfatherFeatures", "GetGodchildren", exception);
				});
		}

		/// <summary>
		/// Get the current logged in gamer's godfather.
		/// </summary>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_GetGodfather(Action<GamerInfo> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}

			// Call the API method which returns a GamerInfo result
			LoginFeatures.gamer.Godfather.Domain(domain).GetGodfather()
				// Result if everything went well
				.Done(delegate (GamerInfo godfather)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:GodfatherFeatures] GetGodfather success ›› Godfather: {0}", godfather));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(godfather);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("GodfatherFeatures", "GetGodfather", exception);
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
				case ExceptionTools.notLoggedInErrorType:
				GodfatherHandler.Instance.ShowError(ExceptionTools.notLoggedInMessage);
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "GodfatherFeatures", exceptionError));
				GodfatherHandler.Instance.ShowError(ExceptionTools.unhandledErrorMessage);
				break;
			}
		}

		/// <summary>
		/// What to do if any UseReferralCode request succeeded.
		/// </summary>
		/// <param name="useDone">Request result details.</param>
		private static void UseReferralCode_OnSuccess(Done useDone)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any UseReferralCode request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void UseReferralCode_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				// Do whatever...
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "GodfatherFeatures", exceptionError));
				GodfatherHandler.Instance.ShowError(ExceptionTools.unhandledErrorMessage);
				break;
			}
		}

		/// <summary>
		/// What to do if any DisplayGodchildren request succeeded.
		/// </summary>
		/// <param name="godchildrenList">List of current logged in gamer's godchildren.</param>
		private static void DisplayGodchildren_OnSuccess(NonpagedList<GamerInfo> godchildrenList)
		{
			GodfatherHandler.Instance.FillGodfatherPanel(godchildrenList);
		}

		/// <summary>
		/// What to do if any DisplayGodchildren request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void DisplayGodchildren_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				GodfatherHandler.Instance.ShowError(ExceptionTools.notLoggedInMessage);
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "GodfatherFeatures", exceptionError));
				GodfatherHandler.Instance.ShowError(ExceptionTools.unhandledErrorMessage);
				break;
			}
		}

		/// <summary>
		/// What to do if any DisplayGodfather request succeeded.
		/// </summary>
		/// <param name="godfather">Current logged in gamer's godfather.</param>
		private static void DisplayGodfather_OnSuccess(GamerInfo godfather)
		{
			GodfatherHandler.Instance.FillGodfatherPanel(godfather);
		}

		/// <summary>
		/// What to do if any DisplayGodfather request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void DisplayGodfather_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
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
