using System;
using System.Collections.Generic;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class AchievementFeatures
	{
		#region Handling
		// Get and display on an achievement panel all game's achievements
		public static void DisplayAchievements()
		{
			// A AchievementHandler instance should be attached to an active object of the scene to display the result
			if (!MonoSingletons.HasInstance<AchievementHandler>())
				Debug.LogError("[CotcSdkTemplate:AchievementFeatures] No AchievementHandler instance found >> Please attach a AchievementHandler script on an active object of the scene");
			else
				ListAchievements(DisplayAchievements_OnSuccess, DisplayAchievements_OnError);
		}
		#endregion

		#region Features
		// List all registered game's achievements
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		public static void ListAchievements(Action<Dictionary<string, AchievementDefinition>> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Promise<Dictionary<string, AchievementDefinition>> (promising a Dictionary<string, AchievementDefinition> result)
			CloudFeatures.gamer.Achievements.Domain(domain).List()
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						ExceptionTools.LogCotcException("AchievementFeatures", "ListAllAchievements", exception);

						// Call the OnError action if any callback registered to it
						if (OnError != null)
							OnError(ExceptionTools.GetExceptionError(exception));
					})
				// The result if everything went well
				.Done(delegate (Dictionary<string, AchievementDefinition> achievementsList)
					{
						Debug.Log(string.Format("[CotcSdkTemplate:AchievementFeatures] ListAllAchievements success >> {0}", achievementsList));

						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(achievementsList);
					});
		}
		#endregion

		#region Delegate Callbacks
		// What to do if any DisplayAchievements request succeeded
		private static void DisplayAchievements_OnSuccess(Dictionary<string, AchievementDefinition> achievementsList)
		{
			MonoSingletons.Instance<AchievementHandler>().FillAndShowAchievementPanel(achievementsList);
		}

		// What to do if any DisplayAchievements request failed
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
