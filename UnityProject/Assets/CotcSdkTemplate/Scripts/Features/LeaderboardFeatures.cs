using System;
using System.Collections.Generic;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to use the CotcSdk's leaderboard features.
	/// </summary>
	public static class LeaderboardFeatures
	{
		#region Handling
		// Error messages corresponding to different cases
		private const string emptyLeaderboardErrorMessage = "\nThe selected leaderboard\nis currently empty.\n\nPlease post new\nscores and try again.\n";
		private const string neverScoredErrorMessage = "\nYou never scored on\nany leaderboard yet.\n\nPlease post new\nscores and try again.\n";
		private const string neverScoredOnLeaderboardErrorMessage = "\nYou never scored on the\ngiven leaderboard yet.\n\nPlease post new\nscores and try again.\n";
		private const string neverScoredWithFriendsErrorMessage = "\nYou and your friend(s)\nnever scored on the\nselected leaderboard yet.\n\nPlease post new\nscores and try again.\n";

		/// <summary>
		/// Get and display all gamers' high scores from the given leaderboard.
		/// </summary>
		/// <param name="boardName">Name of the board from wich to get the scores.</param>
		/// <param name="scoresPerPage">Number of scores to get per page.</param>
		/// <param name="centeredBoard">If the page to show is the one containing the current logged in gamer's score.</param>
		public static void Handling_DisplayAllHighScores(string boardName, int scoresPerPage, bool centeredBoard)
		{
			// A LeaderboardHandler instance should be attached to an active object of the scene to display the result
			if (!LeaderboardHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "LeaderboardFeatures", "LeaderboardHandler"));
			// The board name should not be empty
			else if (string.IsNullOrEmpty(boardName))
				DebugLogs.LogError("[CotcSdkTemplate:LeaderboardFeatures] The board name is empty ›› Please enter a valid board name");
			// The scores per page amount should be positive
			else if (scoresPerPage <= 0)
				DebugLogs.LogError("[CotcSdkTemplate:LeaderboardFeatures] The scores per page amount is invalid ›› Please enter a number superior to 0");
			else
			{
				LeaderboardHandler.Instance.ShowLeaderboardPanel(boardName);

				// Display only the page in which the logged in gamer's score is on the given leaderboard
				if (centeredBoard)
					Backend_CenteredScore(boardName, scoresPerPage, neverScoredOnLeaderboardErrorMessage, DisplayPagedScores_OnSuccess, DisplayPagedScores_OnError);
				// Display the first page of the given leaderboard
				else
					Backend_BestHighScores(boardName, scoresPerPage, 1, emptyLeaderboardErrorMessage, DisplayPagedScores_OnSuccess, DisplayPagedScores_OnError);
			}
		}

		/// <summary>
		/// Get and display all current logged in gamer's friends' high scores from the given leaderboard.
		/// </summary>
		/// <param name="boardName">Name of the board from wich to get the scores.</param>
		public static void Handling_DisplayFriendsHighScores(string boardName)
		{
			// A LeaderboardHandler instance should be attached to an active object of the scene to display the result
			if (!LeaderboardHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "LeaderboardFeatures", "LeaderboardHandler"));
			// The board name should not be empty
			else if (string.IsNullOrEmpty(boardName))
				DebugLogs.LogError("[CotcSdkTemplate:LeaderboardFeatures] The board name is empty ›› Please enter a valid board name");
			else
			{
				LeaderboardHandler.Instance.ShowLeaderboardPanel(boardName);
				Backend_ListFriendScores(boardName, neverScoredWithFriendsErrorMessage, DisplayNonpagedScores_OnSuccess, DisplayNonpagedScores_OnError);
			}
		}

		/// <summary>
		/// Get and display the current logged in gamer's best scores from all leaderboards in which he scored at least once.
		/// </summary>
		public static void Handling_DisplayGamerHighScores()
		{
			// A LeaderboardHandler instance should be attached to an active object of the scene to display the result
			if (!LeaderboardHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "LeaderboardFeatures", "LeaderboardHandler"));
			else
			{
				LeaderboardHandler.Instance.ShowLeaderboardPanel(LoginFeatures.gamer["profile"]["displayName"].AsString());
				Backend_ListUserBestScores(neverScoredErrorMessage, DisplayGamerHighScores_OnSuccess, DisplayGamerHighScores_OnError);
			}
		}

		/// <summary>
		/// Get and display the previous page of a previously obtained leaderboard page.
		/// </summary>
		/// <param name="scores">Paged list of scores.</param>
		public static void Handling_FetchPreviousLeaderboardPage(PagedList<Score> scores)
		{
			Backend_FetchPrevious(scores, DisplayPagedScores_OnSuccess, DisplayPagedScores_OnError);
		}

		/// <summary>
		/// Get and display the next page of a previously obtained leaderboard page.
		/// </summary>
		/// <param name="scores">Paged list of scores.</param>
		public static void Handling_FetchNextLeaderboardPage(PagedList<Score> scores)
		{
			Backend_FetchNext(scores, DisplayPagedScores_OnSuccess, DisplayPagedScores_OnError);
		}

		/// <summary>
		/// Post a new score on the given leaderboard for the current logged in gamer.
		/// </summary>
		/// <param name="boardName">Name of the board to wich to set the score.</param>
		/// <param name="scoreValue">Value of the score to set.</param>
		/// <param name="scoreDescription">Description of the score. (optional)</param>
		/// <param name="forceSave">If the score has to be saved even if it's not a better one. (optional)</param>
		public static void Handling_PostScore(string boardName, long scoreValue, string scoreDescription = null, bool forceSave = false)
		{
			// The board name should not be empty
			if (string.IsNullOrEmpty(boardName))
				DebugLogs.LogError("[CotcSdkTemplate:LeaderboardFeatures] The board name is empty ›› Please enter a valid board name");
			// The score value should be positive
			else if (scoreValue <= 0)
				DebugLogs.LogError("[CotcSdkTemplate:LeaderboardFeatures] The score value is invalid ›› Please enter a number superior to 0");
			else
				Backend_Post(scoreValue, boardName, ScoreOrder.HighToLow, PostScore_OnSuccess, PostScore_OnError, scoreDescription, forceSave);
		}
		#endregion

		#region Backend
		/// <summary>
		/// Get all gamers' high scores from the given leaderboard.
		/// </summary>
		/// <param name="boardName">Name of the board from wich to get the scores.</param>
		/// <param name="scoresPerPage">Number of scores to get per page.</param>
		/// <param name="pageNumber">Number of the page from wich to get the scores.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_BestHighScores(string boardName, int scoresPerPage, int pageNumber, string noScoreErrorMessage, Action<PagedList<Score>, string> OnSuccess = null, Action<ExceptionError, string> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType), noScoreErrorMessage);
				return;
			}

			// Call the API method which returns a PagedList<Score> result
			LoginFeatures.gamer.Scores.Domain(domain).BestHighScores(boardName, scoresPerPage, pageNumber)
				// Result if everything went well
				.Done(delegate (PagedList<Score> scoresList)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:LeaderboardFeatures] BestHighScores success ›› {0} score(s)", scoresList.Count));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(scoresList, noScoreErrorMessage);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception), noScoreErrorMessage);
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("LeaderboardFeatures", "BestHighScores", exception);
				});
		}

		/// <summary>
		/// Get all gamers' high scores from the given leaderboard. (only the page containing the current logged in gamer's score)
		/// </summary>
		/// <param name="boardName">Name of the board from wich to get the scores.</param>
		/// <param name="scoresPerPage">Number of scores to get per page.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_CenteredScore(string boardName, int scoresPerPage, string noScoreErrorMessage, Action<PagedList<Score>, string> OnSuccess = null, Action<ExceptionError, string> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType), noScoreErrorMessage);
				return;
			}

			// Call the API method which returns a PagedList<Score> result
			LoginFeatures.gamer.Scores.Domain(domain).PagedCenteredScore(boardName, scoresPerPage)
				// Result if everything went well
				.Done(delegate (PagedList<Score> scoresList)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:LeaderboardFeatures] CenteredScore success ›› {0} score(s)", scoresList.Count));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(scoresList, noScoreErrorMessage);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception), noScoreErrorMessage);
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("LeaderboardFeatures", "CenteredScore", exception);
				});
		}

		/// <summary>
		/// Get all current logged in gamer's friends' high scores from the given leaderboard.
		/// </summary>
		/// <param name="boardName">Name of the board from wich to get the scores.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_ListFriendScores(string boardName, string noScoreErrorMessage, Action<NonpagedList<Score>, string> OnSuccess = null, Action<ExceptionError, string> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType), noScoreErrorMessage);
				return;
			}

			// Call the API method which returns a NonpagedList<Score> result
			LoginFeatures.gamer.Scores.Domain(domain).ListFriendScores(boardName)
				// Result if everything went well
				.Done(delegate (NonpagedList<Score> scoresList)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:LeaderboardFeatures] ListFriendScores success ›› {0} score(s)", scoresList.Count));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(scoresList, noScoreErrorMessage);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception), noScoreErrorMessage);
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("LeaderboardFeatures", "ListFriendScores", exception);
				});
		}

		/// <summary>
		/// Get the current logged in gamer's best scores from all leaderboards in which he scored at least once.
		/// </summary>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_ListUserBestScores(string noScoreErrorMessage, Action<Dictionary<string, Score>, string> OnSuccess = null, Action<ExceptionError, string> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType), noScoreErrorMessage);
				return;
			}

			// Call the API method which returns a Dictionary<string, Score> result
			LoginFeatures.gamer.Scores.Domain(domain).ListUserBestScores()
				// Result if everything went well
				.Done(delegate (Dictionary<string, Score> scoresList)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:LeaderboardFeatures] ListUserBestScores success ›› {0} score(s)", scoresList.Count));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(scoresList, noScoreErrorMessage);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception), noScoreErrorMessage);
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("LeaderboardFeatures", "ListUserBestScores", exception);
				});
		}

		/// <summary>
		/// Get the previous page from the same board of a previously obtained leaderboard page.
		/// </summary>
		/// <param name="scores">Paged list of scores.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_FetchPrevious(PagedList<Score> scores, Action<PagedList<Score>, string> OnSuccess = null, Action<ExceptionError, string> OnError = null)
		{
			if (scores.HasPrevious)
			{
				// Call the API method which returns a PagedList<Score> result
				scores.FetchPrevious()
					// Result if everything went well
					.Done(delegate (PagedList<Score> scoresList)
					{
						DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:LeaderboardFeatures] FetchPrevious success ›› {0} score(s)", scoresList.Count));
						
						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(scoresList, null);
					},
					// Result if an error occured
					delegate (Exception exception)
					{
						// Call the OnError action if any callback registered to it
						if (OnError != null)
							OnError(ExceptionTools.GetExceptionError(exception), null);
						// Else, log the error (expected to be a CotcException)
						else
							ExceptionTools.LogCotcException("LeaderboardFeatures", "FetchPrevious", exception);
					});
			}
			else
				DebugLogs.LogError("[CotcSdkTemplate:LeaderboardFeatures] There is no previous page");
		}

		/// <summary>
		/// Get the next page from the same board of a previously obtained leaderboard page.
		/// </summary>
		/// <param name="scores">Paged list of scores.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_FetchNext(PagedList<Score> scores, Action<PagedList<Score>, string> OnSuccess = null, Action<ExceptionError, string> OnError = null)
		{
			if (scores.HasNext)
			{
				// Call the API method which returns a PagedList<Score> result
				scores.FetchNext()
					// Result if everything went well
					.Done(delegate (PagedList<Score> scoresList)
					{
						DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:LeaderboardFeatures] FetchNext success ›› {0} score(s)", scoresList.Count));
						
						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(scoresList, null);
					},
					// Result if an error occured
					delegate (Exception exception)
					{
						// Call the OnError action if any callback registered to it
						if (OnError != null)
							OnError(ExceptionTools.GetExceptionError(exception), null);
						// Else, log the error (expected to be a CotcException)
						else
							ExceptionTools.LogCotcException("LeaderboardFeatures", "FetchNext", exception);
					});
			}
			else
				DebugLogs.LogError("[CotcSdkTemplate:LeaderboardFeatures] There is no next page");
		}

		/// <summary>
		/// Post a new score on the given leaderboard for the current logged in gamer.
		/// </summary>
		/// <param name="scoreValue">Value of the score to set.</param>
		/// <param name="boardName">Name of the board to wich to set the score.</param>
		/// <param name="scoreOrder">Determines if the higher or the lower scores are the best ones.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="scoreDescription">Description of the score to set. (optional)</param>
		/// <param name="forceSave">If the score has to be saved even if it's not a better one. (optional)</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_Post(long scoreValue, string boardName, ScoreOrder scoreOrder, Action<PostedGameScore> OnSuccess = null, Action<ExceptionError> OnError = null, string scoreDescription = null, bool forceSave = false, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!LoginFeatures.IsGamerLoggedIn())
			{
				OnError(ExceptionTools.GetExceptionError(new CotcException(CotcSdk.ErrorCode.NotLoggedIn), ExceptionTools.notLoggedInErrorType));
				return;
			}

			// Call the API method which returns a PostedGameScore result
			LoginFeatures.gamer.Scores.Domain(domain).Post(scoreValue, boardName, scoreOrder, scoreDescription, forceSave)
				// Result if everything went well
				.Done(delegate (PostedGameScore postedScore)
				{
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:LeaderboardFeatures] Post success ›› Has Been Saved: {0}, Score Rank: {1}", postedScore.HasBeenSaved, postedScore.Rank));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(postedScore);
				},
				// Result if an error occured
				delegate (Exception exception)
				{
					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
					// Else, log the error (expected to be a CotcException)
					else
						ExceptionTools.LogCotcException("LeaderboardFeatures", "Post", exception);
				});
		}
		#endregion

		#region Delegate Callbacks
		/// <summary>
		/// What to do if any DisplayNonpagedScores request succeeded.
		/// </summary>
		/// <param name="scoresList">List of all gamers' high scores from the given leaderboard.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		private static void DisplayNonpagedScores_OnSuccess(NonpagedList<Score> scoresList, string noScoreErrorMessage)
		{
			LeaderboardHandler.Instance.FillNonpagedLeaderboardPanel(scoresList, noScoreErrorMessage);
		}

		/// <summary>
		/// What to do if any DisplayNonpagedScores request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		private static void DisplayNonpagedScores_OnError(ExceptionError exceptionError, string noScoreErrorMessage)
		{
			switch (exceptionError.type)
			{
				// Error type: no gamer ever scored on this leaderboard (board doesn't exist yet)
				case ExceptionTools.missingScoreErrorType:
				LeaderboardHandler.Instance.FillNonpagedLeaderboardPanel(null, noScoreErrorMessage);
				break;

				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				LeaderboardHandler.Instance.ShowError(ExceptionTools.notLoggedInMessage);
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "LeaderboardFeatures", exceptionError));
				LeaderboardHandler.Instance.ShowError(ExceptionTools.unhandledErrorMessage);
				break;
			}
		}

		/// <summary>
		/// What to do if any DisplayPagedScores request succeeded.
		/// </summary>
		/// <param name="scoresList">List of all gamers' high scores from the given leaderboard.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		private static void DisplayPagedScores_OnSuccess(PagedList<Score> scoresList, string noScoreErrorMessage)
		{
			LeaderboardHandler.Instance.FillPagedLeaderboardPanel(scoresList, noScoreErrorMessage);
		}

		/// <summary>
		/// What to do if any DisplayPagedScores request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		private static void DisplayPagedScores_OnError(ExceptionError exceptionError, string noScoreErrorMessage)
		{
			switch (exceptionError.type)
			{
				// Error type: no gamer ever scored on this leaderboard (board doesn't exist yet)
				case ExceptionTools.missingScoreErrorType:
				LeaderboardHandler.Instance.FillPagedLeaderboardPanel(null, noScoreErrorMessage);
				break;

				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				LeaderboardHandler.Instance.ShowError(ExceptionTools.notLoggedInMessage);
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "LeaderboardFeatures", exceptionError));
				LeaderboardHandler.Instance.ShowError(ExceptionTools.unhandledErrorMessage);
				break;
			}
		}

		/// <summary>
		/// What to do if any DisplayGamerHighScores request succeeded.
		/// </summary>
		/// <param name="scoresList">List of logged in gamer's best scores from all leaderboards in which he scored at least once.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		private static void DisplayGamerHighScores_OnSuccess(Dictionary<string, Score> scoresList, string noScoreErrorMessage)
		{
			LeaderboardHandler.Instance.FillMultipleLeaderboardPanel(scoresList, noScoreErrorMessage);
		}

		/// <summary>
		/// What to do if any DisplayGamerHighScores request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		private static void DisplayGamerHighScores_OnError(ExceptionError exceptionError, string noScoreErrorMessage)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				LeaderboardHandler.Instance.ShowError(ExceptionTools.notLoggedInMessage);
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "LeaderboardFeatures", exceptionError));
				LeaderboardHandler.Instance.ShowError(ExceptionTools.unhandledErrorMessage);
				break;
			}
		}

		/// <summary>
		/// What to do if any PostScore request succeeded.
		/// </summary>
		/// <param name="postedScore">Posted score details.</param>
		private static void PostScore_OnSuccess(PostedGameScore postedScore)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any PostScore request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
		private static void PostScore_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Error type: not initialized Cloud or no logged in gamer
				case ExceptionTools.notLoggedInErrorType:
				// Do whatever...
				break;

				// Unhandled error types
				default:
				DebugLogs.LogError(string.Format(ExceptionTools.unhandledErrorFormat, "LeaderboardFeatures", exceptionError));
				break;
			}
		}
		#endregion
	}
}
