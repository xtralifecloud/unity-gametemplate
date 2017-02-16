using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class LeaderboardHandler : MonoSingleton<LeaderboardHandler>
	{
		#region Display
		// Reference to the leaderboard panel UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject leaderboardPanel = null;
		[SerializeField] private Text leaderboardPanelTitle = null;
		[SerializeField] private GameObject noScoreText = null;
		[SerializeField] private GameObject noScorePostedText = null;
		[SerializeField] private GameObject pageButtons = null;
		[SerializeField] private Button previousPageButton = null;
		[SerializeField] private Button nextPageButton = null;

		// Reference to the score GameObject prefab and the scores list scroll view
		[SerializeField] private GameObject scorePrefab = null;
		[SerializeField] private GridLayoutGroup leaderboardScoresLayout = null;

		// List of the score GameObjects created on the leaderboard panel
		private List<GameObject> leaderboardScores = new List<GameObject>();

		// The last PagedList<Score> used to fill the leaderboard panel
		private PagedList<Score> currentScoresList = null;

		// Hide the leaderboard panel at Start
		private void Start()
		{
			ShowLeaderboardPanel(false);
		}

		// Show or hide the leaderboard panel
		public void ShowLeaderboardPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			leaderboardPanel.SetActive(show);
		}

		// Fill the leaderboard panel with paged scores then show it
		public void FillAndShowPagedLeaderboardPanel(string boardName, PagedList<Score> scoresList)
		{
			// Hide the "no score posted" (for nonpaged list) text
			noScorePostedText.SetActive(false);

			// Destroy the previously created score GameObjects if any exist and clear the list
			foreach (GameObject leaderboardScore in leaderboardScores)
				DestroyObject(leaderboardScore);

			leaderboardScores.Clear();

			// Set the leaderboard panel's title
			if (!string.IsNullOrEmpty(boardName))
				leaderboardPanelTitle.text = boardName;

			// If there are scores to display, fill the leaderboard panel with score prefabs
			if ((scoresList != null) && (scoresList.Count > 0))
			{
				// Hide the "no score" text and show the previous page and next page buttons
				noScoreText.SetActive(false);
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
			// Else, show the "no score" text, hide the previous page and next page buttons, and prevent usage of previous and next leaderboard page
			else
			{
				noScoreText.SetActive(true);
				pageButtons.SetActive(false);
				currentScoresList = null;
			}

			// Show the leaderboard panel
			ShowLeaderboardPanel(true);
		}

		// Fill the leaderboard panel with nonpaged scores then show it
		public void FillAndShowNonpagedLeaderboardPanel(string boardName, NonpagedList<Score> scoresList)
		{
			// Hide the "no score" (for paged list) text and show the previous page and next page buttons
			noScoreText.SetActive(false);
			pageButtons.SetActive(false);

			// Destroy the previously created score GameObjects if any exist and clear the list
			foreach (GameObject leaderboardScore in leaderboardScores)
				DestroyObject(leaderboardScore);

			leaderboardScores.Clear();

			// Set the leaderboard panel's title
			if (!string.IsNullOrEmpty(boardName))
				leaderboardPanelTitle.text = boardName;

			// If there are scores to display, fill the leaderboard panel with score prefabs
			if ((scoresList != null) && (scoresList.Count > 0))
			{
				// Hide the "no score posted" text and show the previous page and next page buttons
				noScorePostedText.SetActive(false);

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
			// Else, show the "no score posted" text, hide the previous page and next page buttons, and prevent usage of previous and next leaderboard page
			else
				noScorePostedText.SetActive(true);

			// Show the leaderboard panel
			ShowLeaderboardPanel(true);
		}
		#endregion

		#region Buttons Actions
		// Ask for the previous page on the current leaderboard
		public void Button_PreviousPage()
		{
			// Disable buttons to avoid concurrent calls
			previousPageButton.interactable = false;
			nextPageButton.interactable = false;

			// Call for the previous page
			if (currentScoresList != null)
				LeaderboardFeatures.FetchPreviousLeaderboardPage(currentScoresList);
		}

		// Ask for the next page on the current leaderboard
		public void Button_NextPage()
		{
			// Disable buttons to avoid concurrent calls
			previousPageButton.interactable = false;
			nextPageButton.interactable = false;

			// Call for the next page
			if (currentScoresList != null)
				LeaderboardFeatures.FetchNextLeaderboardPage(currentScoresList);
		}
		#endregion

		#region Screen Orientation
		// Adapt the layout display when the current screen orientation changes
		public void OnRectTransformDimensionsChange()
		{
			// If on landscape orientation use 2 columns, else (portrait) use 1 column
			leaderboardScoresLayout.constraintCount = Screen.width > Screen.height ? 2 : 1;
		}
		#endregion
	}
}
