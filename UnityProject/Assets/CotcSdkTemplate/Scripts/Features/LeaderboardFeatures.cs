using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class LeaderboardFeatures
	{
		#region Handling
		// Get and display on a leaderboard panel all registered best scores for all gamers from a given leaderboard
		public static void DisplayAllHighScores(string boardName, int scoresPerPage)
		{
			// A LeaderboardHandler instance should be attached to an active object of the scene to display the result
			if (!MonoSingletons.HasInstance<LeaderboardHandler>())
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] No LeaderboardHandler instance found >> Please attach a LeaderboardHandler script on an active object of the scene");
			// The board name should not be empty
			else if (string.IsNullOrEmpty(boardName))
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] The board name is empty >> Please enter a valid board name");
			// The scores per page amount should be positive
			else if (scoresPerPage <= 0)
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] The scores per page amount is invalid >> Please enter a number superior to 0");
			else
				ListAllHighScores(boardName, scoresPerPage, 1, DisplayScores_OnSuccess, DisplayScores_OnError);
		}

		// Get the previous page of a previously obtained leaderboard
		public static void FetchPreviousLeaderboardPage(PagedList<Score> leaderboardScores)
		{
			FetchPrevious(leaderboardScores, DisplayScores_OnSuccess, DisplayScores_OnError);
		}

		// Get the next page of a previously obtained leaderboard
		public static void FetchNextLeaderboardPage(PagedList<Score> leaderboardScores)
		{
			FetchNext(leaderboardScores, DisplayScores_OnSuccess, DisplayScores_OnError);
		}
		#endregion

		#region Features
		// List all registered best scores for all gamers from a given leaderboard
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		public static void ListAllHighScores(string boardName, int scoresPerPage, int pageNumber, Action<string, PagedList<Score>> OnSuccess = null, Action<string, ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;

			// Call the API method which returns a Promise<PagedList<Score>> (promising a PagedList<Score> result)
			CloudFeatures.gamer.Scores.Domain(domain).BestHighScores(boardName, scoresPerPage, pageNumber)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						ExceptionTools.LogCotcException("LeaderboardFeatures", "ListAllHighScores", exception);

						// Call the OnError action if any callback registered to it
						if (OnError != null)
							OnError(boardName, ExceptionTools.GetExceptionError(exception));
					})
				// The result if everything went well
				.Done(delegate (PagedList<Score> scoresList)
					{
						Debug.Log(string.Format("[CotcSdkTemplate:LeaderboardFeatures] ListAllHighScores success >> {0}", scoresList));

						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(boardName, scoresList);
					});
		}

		// Get the previous page of a previously obtained leaderboard
		public static void FetchPrevious(PagedList<Score> leaderboardScores, Action<string, PagedList<Score>> OnSuccess = null, Action<string, ExceptionError> OnError = null)
		{
			if (leaderboardScores.HasPrevious)
			{
				// Call the API method which returns a Promise<PagedList<Score>> (promising a PagedList<Score> result)
				leaderboardScores.FetchPrevious()
					// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
					.Catch(delegate (Exception exception)
						{
							// The exception should always be of the CotcException type
							ExceptionTools.LogCotcException("LeaderboardFeatures", "FetchPrevious", exception);

							// Call the OnError action if any callback registered to it
							if (OnError != null)
								OnError(null, ExceptionTools.GetExceptionError(exception));
						})
					// The result if everything went well
					.Done(delegate (PagedList<Score> scoresList)
						{
							Debug.Log(string.Format("[CotcSdkTemplate:LeaderboardFeatures] FetchPrevious success >> {0}", scoresList));

							// Call the OnSuccess action if any callback registered to it
							if (OnSuccess != null)
								OnSuccess(null, scoresList);
						});
			}
			else
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] The current leaderboard has no previous page");
		}

		// Get the next page of a previously obtained leaderboard
		public static void FetchNext(PagedList<Score> leaderboardScores, Action<string, PagedList<Score>> OnSuccess = null, Action<string, ExceptionError> OnError = null)
		{
			if (leaderboardScores.HasNext)
			{
				// Call the API method which returns a Promise<PagedList<Score>> (promising a PagedList<Score> result)
				leaderboardScores.FetchNext()
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
					.Catch(delegate (Exception exception)
						{
							// The exception should always be of the CotcException type
							ExceptionTools.LogCotcException("LeaderboardFeatures", "FetchNext", exception);

							// Call the OnError action if any callback registered to it
							if (OnError != null)
								OnError(null, ExceptionTools.GetExceptionError(exception));
						})
					// The result if everything went well
					.Done(delegate (PagedList<Score> scoresList)
						{
							Debug.Log(string.Format("[CotcSdkTemplate:LeaderboardFeatures] FetchNext success >> {0}", scoresList));

							// Call the OnSuccess action if any callback registered to it
							if (OnSuccess != null)
								OnSuccess(null, scoresList);
						});
			}
			else
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] The current leaderboard has no next page");
		}
		#endregion

		#region Delegate Callbacks
		// What to do if any DisplayScores method succeeded
		private static void DisplayScores_OnSuccess(string boardName, PagedList<Score> scoresList)
		{
			MonoSingletons.Instance<LeaderboardHandler>().FillAndShowLeaderboardPanel(boardName, scoresList);
		}

		// What to do if any DisplayScores method failed
		private static void DisplayScores_OnError(string boardName, ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: no gamer ever scored on this leaderboard (board doesn't exist yet)
				case "MissingScore":
				MonoSingletons.Instance<LeaderboardHandler>().FillAndShowLeaderboardPanel(boardName, null);
				break;

				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:LeaderboardFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}
		#endregion
	}
}
