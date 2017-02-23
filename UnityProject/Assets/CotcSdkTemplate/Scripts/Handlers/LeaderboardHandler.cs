using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to display the CotcSdk's leaderboard features' results.
	/// </summary>
	public class LeaderboardHandler : MonoSingleton<LeaderboardHandler>
	{
		#region Display
		// Reference to the leaderboard panel UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject leaderboardPanel = null;
		[SerializeField] private Text leaderboardPanelTitle = null;
		[SerializeField] private GameObject noScorePostedInBoardText = null;
		[SerializeField] private GameObject noScoreInBoardText = null;
		[SerializeField] private GameObject noScorePostedText = null;
		[SerializeField] private GameObject pageButtons = null;
		[SerializeField] private Button previousPageButton = null;
		[SerializeField] private Button nextPageButton = null;

		// Reference to the score GameObject prefabs and the scores list scroll view
		[SerializeField] private GameObject scorePrefab = null;
		[SerializeField] private GameObject gamerScorePrefab = null;
		[SerializeField] private GridLayoutGroup leaderboardScoresLayout = null;

		// List of the score GameObjects created on the leaderboard panel
		private List<GameObject> leaderboardScores = new List<GameObject>();

		// The last PagedList<Score> used to fill the leaderboard panel
		private PagedList<Score> currentScoresList = null;

		/// <summary>
		/// Hide the leaderboard panel at Start.
		/// </summary>
		private void Start()
		{
			ShowLeaderboardPanel(false);
		}

		/// <summary>
		/// Show or hide the leaderboard panel.
		/// </summary>
		/// <param name="show">If the panel should be shown.</param>
		public void ShowLeaderboardPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			leaderboardPanel.SetActive(show);
		}

		/// <summary>
		/// Fill the leaderboard panel with nonpaged scores then show it.
		/// </summary>
		/// <param name="scoresList">List of the scores to display.</param>
		/// <param name="panelTitle">Title of the panel.</param>
		public void FillAndShowNonpagedLeaderboardPanel(NonpagedList<Score> scoresList, string panelTitle = "Leaderboard Scores")
		{
			// Hide the "no score in board" (for paged list) and "no score posted" (for multiple boards) texts and hide the previous page and next page buttons
			noScoreInBoardText.SetActive(false);
			noScorePostedText.SetActive(false);
			pageButtons.SetActive(false);

			// Destroy the previously created score GameObjects if any exist and clear the list
			foreach (GameObject leaderboardScore in leaderboardScores)
				DestroyObject(leaderboardScore);
			
			leaderboardScores.Clear();

			// Set the leaderboard panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				leaderboardPanelTitle.text = panelTitle;
			
			// If there are scores to display, fill the leaderboard panel with score prefabs
			if ((scoresList != null) && (scoresList.Count > 0))
			{
				// Hide the "no score posted in board" text
				noScorePostedInBoardText.SetActive(false);

				foreach (Score score in scoresList)
				{
					// Create a leaderboard score GameObject and hook it at the leaderboard scores scroll view
					GameObject prefabInstance = Instantiate<GameObject>(scorePrefab);
					prefabInstance.transform.SetParent(leaderboardScoresLayout.transform, false);

					// Fill the newly created GameObject with score data
					LeaderboardScoreHandler leaderboardScoreHandler = prefabInstance.GetComponent<LeaderboardScoreHandler>();
					leaderboardScoreHandler.FillData(score);

					// Add the newly created GameObject to the list
					leaderboardScores.Add(prefabInstance);
				}
			}
			// Else, show the "no score posted in board" text
			else
				noScorePostedInBoardText.SetActive(true);
			
			// Show the leaderboard panel
			ShowLeaderboardPanel(true);
		}

		/// <summary>
		/// Fill the leaderboard panel with paged scores then show it.
		/// </summary>
		/// <param name="scoresList">List of the scores to display.</param>
		/// <param name="panelTitle">Title of the panel.</param>
		public void FillAndShowPagedLeaderboardPanel(PagedList<Score> scoresList, string panelTitle = "Leaderboard Scores")
		{
			// Hide the "no score posted in board" (for nonpaged list) and "no score posted" (for multiple boards) texts
			noScorePostedInBoardText.SetActive(false);
			noScorePostedText.SetActive(false);

			// Destroy the previously created score GameObjects if any exist and clear the list
			foreach (GameObject leaderboardScore in leaderboardScores)
				DestroyObject(leaderboardScore);
			
			leaderboardScores.Clear();

			// Set the leaderboard panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				leaderboardPanelTitle.text = panelTitle;

			// If there are scores to display, fill the leaderboard panel with score prefabs
			if ((scoresList != null) && (scoresList.Count > 0))
			{
				// Hide the "no score in board" text and show the previous page and next page buttons
				noScoreInBoardText.SetActive(false);
				pageButtons.SetActive(true);

				foreach (Score score in scoresList)
				{
					// Create a leaderboard score GameObject and hook it at the leaderboard scores scroll view
					GameObject prefabInstance = Instantiate<GameObject>(scorePrefab);
					prefabInstance.transform.SetParent(leaderboardScoresLayout.transform, false);

					// Fill the newly created GameObject with score data
					LeaderboardScoreHandler leaderboardScoreHandler = prefabInstance.GetComponent<LeaderboardScoreHandler>();
					leaderboardScoreHandler.FillData(score);

					// Add the newly created GameObject to the list
					leaderboardScores.Add(prefabInstance);
				}

				// Keep the last PagedList<Score> to allow usage of previous and next leaderboard page
				currentScoresList = scoresList;
				previousPageButton.interactable = currentScoresList.HasPrevious;
				nextPageButton.interactable = currentScoresList.HasNext;
			}
			// Else, show the "no score in board" text and hide the previous page and next page buttons
			else
			{
				noScoreInBoardText.SetActive(true);
				pageButtons.SetActive(false);
				currentScoresList = null;
			}
			
			// Show the leaderboard panel
			ShowLeaderboardPanel(true);
		}

		/// <summary>
		/// Fill the leaderboard panel with scores from multiple boards then show it.
		/// </summary>
		/// <param name="scoresList">List of the scores to display.</param>
		/// <param name="panelTitle">Title of the panel.</param>
		public void FillAndShowMultipleLeaderboardPanel(Dictionary<string, Score> scoresList, string panelTitle = "Gamer Best Scores")
		{
			// Hide the "no score posted in board" (for nonpaged list) and the "no score in board" (for paged list) texts and hide the previous page and next page buttons
			noScorePostedInBoardText.SetActive(false);
			noScoreInBoardText.SetActive(false);
			pageButtons.SetActive(false);

			// Destroy the previously created score GameObjects if any exist and clear the list
			foreach (GameObject leaderboardScore in leaderboardScores)
				DestroyObject(leaderboardScore);
			
			leaderboardScores.Clear();

			// Set the leaderboard panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				leaderboardPanelTitle.text = panelTitle;

			// If there are scores to display, fill the leaderboard panel with score prefabs
			if ((scoresList != null) && (scoresList.Count > 0))
			{
				// Hide the "no score posted" text
				noScorePostedText.SetActive(false);

				foreach (KeyValuePair<string, Score> score in scoresList)
				{
					// Create a leaderboard gamer score GameObject and hook it at the leaderboard scores scroll view
					GameObject prefabInstance = Instantiate<GameObject>(gamerScorePrefab);
					prefabInstance.transform.SetParent(leaderboardScoresLayout.transform, false);

					// Fill the newly created GameObject with score data
					LeaderboardGamerScoreHandler leaderboardGamerScoreHandler = prefabInstance.GetComponent<LeaderboardGamerScoreHandler>();
					leaderboardGamerScoreHandler.FillData(score.Value, score.Key);

					// Add the newly created GameObject to the list
					leaderboardScores.Add(prefabInstance);
				}
			}
			// Else, show the "no score posted" text
			else
				noScorePostedText.SetActive(true);
			
			// Show the leaderboard panel
			ShowLeaderboardPanel(true);
		}
		#endregion

		#region Buttons Actions
		/// <summary>
		/// Ask for the previous page on the current leaderboard panel.
		/// </summary>
		public void Button_PreviousPage()
		{
			// Disable buttons to avoid concurrent calls
			previousPageButton.interactable = false;
			nextPageButton.interactable = false;

			// Call for the previous page
			if (currentScoresList != null)
				LeaderboardFeatures.Handling_FetchPreviousLeaderboardPage(currentScoresList);
		}

		/// <summary>
		/// Ask for the next page on the current leaderboard panel.
		/// </summary>
		public void Button_NextPage()
		{
			// Disable buttons to avoid concurrent calls
			previousPageButton.interactable = false;
			nextPageButton.interactable = false;

			// Call for the next page
			if (currentScoresList != null)
				LeaderboardFeatures.Handling_FetchNextLeaderboardPage(currentScoresList);
		}
		#endregion

		#region Screen Orientation
		/// <summary>
		/// Adapt the layout display when the current screen orientation changes.
		/// </summary>
		public void OnRectTransformDimensionsChange()
		{
			// If on landscape orientation use 2 columns, else (portrait) use 1 column
			leaderboardScoresLayout.constraintCount = Screen.width > Screen.height ? 2 : 1;
		}
		#endregion
	}
}
