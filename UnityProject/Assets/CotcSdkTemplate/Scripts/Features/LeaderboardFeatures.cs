﻿using System;
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
				BestHighScores(boardName, scoresPerPage, 1, DisplayScores_OnSuccess, DisplayScores_OnError);
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

		// Post a new socre to a given leaderboard for the current logged in gamer
		public static void PostScore(string boardName, long scoreValue, string scoreDescription)
		{
			// The board name should not be empty
			if (string.IsNullOrEmpty(boardName))
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] The board name is empty >> Please enter a valid board name");
			// The score value should be positive
			else if (scoreValue <= 0)
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] The score value is invalid >> Please enter a number superior to 0");
			else
				Post(scoreValue, boardName, ScoreOrder.HighToLow, scoreDescription, PostScore_OnSuccess, PostScore_OnError);
		}
		#endregion

		#region Features
		// List all registered best scores for all gamers from a given leaderboard
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		public static void BestHighScores(string boardName, int scoresPerPage, int pageNumber, Action<string, PagedList<Score>> OnSuccess = null, Action<string, ExceptionError> OnError = null, string domain = "private")
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

		// Post a new score to a given leaderboard for the current logged in gamer
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		public static void Post(long scoreValue, string boardName, ScoreOrder scoreOrder, string scoreDescription, Action<PostedGameScore> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;

			// Call the API method which returns a Promise<PostedGameScore> (promising a PostedGameScore result)
			CloudFeatures.gamer.Scores.Domain(domain).Post(scoreValue, boardName, scoreOrder, scoreDescription)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						ExceptionTools.LogCotcException("LeaderboardFeatures", "Post", exception);

						// Call the OnError action if any callback registered to it
						if (OnError != null)
							OnError(ExceptionTools.GetExceptionError(exception));
					})
				// The result if everything went well
				.Done(delegate (PostedGameScore postedScore)
					{
						Debug.Log(string.Format("[CotcSdkTemplate:LeaderboardFeatures] Post success >> Has Been Saved: {0}, Score Rank: {1}", postedScore.HasBeenSaved, postedScore.Rank));

						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(postedScore);
					});
		}
		#endregion

		#region Delegate Callbacks
		// What to do if any DisplayScores request succeeded
		private static void DisplayScores_OnSuccess(string boardName, PagedList<Score> scoresList)
		{
			MonoSingletons.Instance<LeaderboardHandler>().FillAndShowLeaderboardPanel(boardName, scoresList);
		}

		// What to do if any DisplayScores request failed
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

		// What to do if any PostScore request succeeded
		private static void PostScore_OnSuccess(PostedGameScore postedScore)
		{
			// Do whatever...
		}

		// What to do if any PostScore request failed
		private static void PostScore_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:LeaderboardFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}
		#endregion
	}
}
