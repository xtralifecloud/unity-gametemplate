using System;
using System.Collections.Generic;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class LeaderboardFeatures
	{
		#region Handling
		// Check variables to get and display on a leaderboard panel all registered best scores for all gamers from a given leaderboard
		public static void DisplayAllHighScores(string boardName, int scoresPerPage, bool centeredBoard)
		{
			// A LeaderboardHandler instance should be attached to an active object of the scene to display the result
			if (!LeaderboardHandler.HasInstance)
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] No LeaderboardHandler instance found >> Please attach a LeaderboardHandler script on an active object of the scene");
			// The board name should not be empty
			else if (string.IsNullOrEmpty(boardName))
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] The board name is empty >> Please enter a valid board name");
			// The scores per page amount should be positive
			else if (scoresPerPage <= 0)
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] The scores per page amount is invalid >> Please enter a number superior to 0");
			else
			{
				// Display only the page in which the logged in gamer's score is on the given leaderboard
				if (centeredBoard)
					CenteredScore(boardName, scoresPerPage, DisplayNonpagedScores_OnSuccess, DisplayNonpagedScores_OnError);
				// Display the first page of the given leaderboard
				else
					BestHighScores(boardName, scoresPerPage, 1, DisplayPagedScores_OnSuccess, DisplayPagedScores_OnError);
			}
		}

		// Get and display on a leaderboard panel logged in gamer's high scores from all leaderboards in which he scored
		public static void DisplayGamerHighScores()
		{
			// A LeaderboardHandler instance should be attached to an active object of the scene to display the result
			if (!LeaderboardHandler.HasInstance)
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] No LeaderboardHandler instance found >> Please attach a LeaderboardHandler script on an active object of the scene");
			else
				ListUserBestScores(DisplayGamerHighScores_OnSuccess, DisplayGamerHighScores_OnError);
		}

		// Get and display the previous page of a previously obtained leaderboard page
		public static void FetchPreviousLeaderboardPage(PagedList<Score> scores)
		{
			FetchPrevious(scores, DisplayPagedScores_OnSuccess, DisplayPagedScores_OnError);
		}

		// Get and display the next page of a previously obtained leaderboard page
		public static void FetchNextLeaderboardPage(PagedList<Score> scores)
		{
			FetchNext(scores, DisplayPagedScores_OnSuccess, DisplayPagedScores_OnError);
		}

		// Check variables to post a new score to a given leaderboard for the current logged in gamer
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
		private static void BestHighScores(string boardName, int scoresPerPage, int pageNumber, Action<string, PagedList<Score>> OnSuccess = null, Action<string, ExceptionError> OnError = null, string domain = "private")
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
					ExceptionTools.LogCotcException("LeaderboardFeatures", "BestHighScores", exception);

					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(boardName, ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (PagedList<Score> scoresList)
				{
					Debug.Log(string.Format("[CotcSdkTemplate:LeaderboardFeatures] BestHighScores success >> {0} scores", scoresList.Count));

					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(boardName, scoresList);
				});
		}

		// List all registered best scores for all gamers from a given leaderboard (only the current logged in gamer score's page)
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		private static void CenteredScore(string boardName, int scoresPerPage, Action<string, NonpagedList<Score>> OnSuccess = null, Action<string, ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Promise<NonpagedList<Score>> (promising a NonpagedList<Score> result)
			CloudFeatures.gamer.Scores.Domain(domain).CenteredScore(boardName, scoresPerPage)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("LeaderboardFeatures", "CenteredScore", exception);

					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(boardName, ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (NonpagedList<Score> scoresList)
				{
					Debug.Log(string.Format("[CotcSdkTemplate:LeaderboardFeatures] CenteredScore success >> {0} scores", scoresList.Count));

					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(boardName, scoresList);
				});
		}

		// Get the previous page of a previously obtained leaderboard page
		private static void FetchPrevious(PagedList<Score> scores, Action<string, PagedList<Score>> OnSuccess = null, Action<string, ExceptionError> OnError = null)
		{
			if (scores.HasPrevious)
			{
				// Call the API method which returns a Promise<PagedList<Score>> (promising a PagedList<Score> result)
				scores.FetchPrevious()
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
						Debug.Log(string.Format("[CotcSdkTemplate:LeaderboardFeatures] FetchPrevious success >> {0} scores", scoresList.Count));

						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(null, scoresList);
					});
			}
			else
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] There is no previous page");
		}

		// Get the next page of a previously obtained leaderboard page
		private static void FetchNext(PagedList<Score> scores, Action<string, PagedList<Score>> OnSuccess = null, Action<string, ExceptionError> OnError = null)
		{
			if (scores.HasNext)
			{
				// Call the API method which returns a Promise<PagedList<Score>> (promising a PagedList<Score> result)
				scores.FetchNext()
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
						Debug.Log(string.Format("[CotcSdkTemplate:LeaderboardFeatures] FetchNext success >> {0} scores", scoresList.Count));

						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(null, scoresList);
					});
			}
			else
				Debug.LogError("[CotcSdkTemplate:LeaderboardFeatures] There is no next page");
		}

		// List logged in gamer's high scores from all leaderboards in which he scored
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		private static void ListUserBestScores(Action<Dictionary<string, Score>> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Promise<Dictionary<string, Score>> (promising a Dictionary<string, Score> result)
			CloudFeatures.gamer.Scores.Domain(domain).ListUserBestScores()
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("LeaderboardFeatures", "ListUserBestScores", exception);

					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (Dictionary<string, Score> scoresList)
				{
					Debug.Log(string.Format("[CotcSdkTemplate:LeaderboardFeatures] ListUserBestScores success >> {0} scores", scoresList.Count));

					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(scoresList);
				});
		}

		// Post a new score to a given leaderboard for the current logged in gamer
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		private static void Post(long scoreValue, string boardName, ScoreOrder scoreOrder, string scoreDescription, Action<PostedGameScore> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
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
		// What to do if any DisplayNonpagedScores request succeeded
		private static void DisplayNonpagedScores_OnSuccess(string boardName, NonpagedList<Score> scoresList)
		{
			LeaderboardHandler.Instance.FillAndShowNonpagedLeaderboardPanel(boardName, scoresList);
		}

		// What to do if any DisplayNonpagedScores request failed
		private static void DisplayNonpagedScores_OnError(string boardName, ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: no gamer ever scored on this leaderboard (board doesn't exist yet)
				case "MissingScore":
				LeaderboardHandler.Instance.FillAndShowNonpagedLeaderboardPanel(boardName, null);
				break;

				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:LeaderboardFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}

		// What to do if any DisplayPagedScores request succeeded
		private static void DisplayPagedScores_OnSuccess(string boardName, PagedList<Score> scoresList)
		{
			LeaderboardHandler.Instance.FillAndShowPagedLeaderboardPanel(boardName, scoresList);
		}

		// What to do if any DisplayPagedScores request failed
		private static void DisplayPagedScores_OnError(string boardName, ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: no gamer ever scored on this leaderboard (board doesn't exist yet)
				case "MissingScore":
				LeaderboardHandler.Instance.FillAndShowPagedLeaderboardPanel(boardName, null);
				break;

				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:LeaderboardFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}

		// What to do if any DisplayGamerHighScores request succeeded
		private static void DisplayGamerHighScores_OnSuccess(Dictionary<string, Score> scoresList)
		{
			LeaderboardHandler.Instance.FillAndShowMultipleLeaderboardPanel(scoresList);
		}

		// What to do if any DisplayGamerHighScores request failed
		private static void DisplayGamerHighScores_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
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
