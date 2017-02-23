using System;
using System.Collections.Generic;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to use the CotcSdk's achievement features.
	/// </summary>
	public static class AchievementFeatures
	{
		#region Handling
		/// <summary>
		/// Get and display logged in gamer's progress on all game's achievements.
		/// </summary>
		public static void Handling_DisplayAchievements()
		{
			// An AchievementHandler instance should be attached to an active object of the scene to display the result
			if (!AchievementHandler.HasInstance)
				Debug.LogError("[CotcSdkTemplate:AchievementFeatures] No AchievementHandler instance found >> Please attach a AchievementHandler script on an active object of the scene");
			else
				Backend_ListAchievements(DisplayAchievements_OnSuccess, DisplayAchievements_OnError);
		}
		#endregion

		#region Features
		/// <summary>
		/// Get logged in gamer's progress on all game's achievements.
		/// </summary>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_ListAchievements(Action<Dictionary<string, AchievementDefinition>> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Dictionary<string, AchievementDefinition> result
			CloudFeatures.gamer.Achievements.Domain(domain).List()
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("AchievementFeatures", "List", exception);
					
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (Dictionary<string, AchievementDefinition> achievementsList)
				{
					Debug.Log(string.Format("[CotcSdkTemplate:AchievementFeatures] List success >> {0} achievement(s)", achievementsList.Count));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(achievementsList);
				});
		}
		#endregion

		#region Delegate Callbacks
		/// <summary>
		/// What to do if any DisplayAchievements request succeeded.
		/// </summary>
		/// <param name="achievementsList">List of logged in gamer's progress on all game's achievements.</param>
		private static void DisplayAchievements_OnSuccess(Dictionary<string, AchievementDefinition> achievementsList)
		{
			AchievementHandler.Instance.FillAndShowAchievementPanel(achievementsList);
		}

		/// <summary>
		/// What to do if any DisplayAchievements request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void DisplayAchievements_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:AchievementFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}
		#endregion
	}
}
