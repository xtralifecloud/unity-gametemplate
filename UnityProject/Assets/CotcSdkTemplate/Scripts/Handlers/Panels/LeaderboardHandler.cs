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
		[SerializeField] private GameObject loadingBlock = null;
		[SerializeField] private Text errorText = null;
		[SerializeField] private Text noScoreText = null;
		[SerializeField] private GameObject pageButtons = null;
		[SerializeField] private Button previousPageButton = null;
		[SerializeField] private Button nextPageButton = null;

		// Reference to the leaderboard GameObject prefabs and the leaderboard items layout
		[SerializeField] private GameObject leaderboardScorePrefab = null;
		[SerializeField] private GameObject leaderboardGamerScorePrefab = null;
		[SerializeField] private GridLayoutGroup leaderboardItemsLayout = null;

		// List of the leaderboard GameObjects created on the leaderboard panel
		private List<GameObject> leaderboardItems = new List<GameObject>();

		// The last PagedList<Score> used to fill the leaderboard panel
		private PagedList<Score> currentScoresList = null;

		/// <summary>
		/// Hide the leaderboard panel at Start.
		/// </summary>
		private void Start()
		{
			HideLeaderboardPanel();
		}

		/// <summary>
		/// Hide the leaderboard panel.
		/// </summary>
		public void HideLeaderboardPanel()
		{
			// Hide the leaderboard panel with its outclickMask and loading block
			outClickMask.SetActive(false);
			leaderboardPanel.SetActive(false);
			ClearLeaderboardPanel(false);
		}

		/// <summary>
		/// Show the leaderboard panel.
		/// </summary>
		/// <param name="panelTitle">Title of the panel to display. (optional)</param>
		public void ShowLeaderboardPanel(string panelTitle = null)
		{
			// Set the leaderboard panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				leaderboardPanelTitle.text = panelTitle;

			// Show the leaderboard panel with its outclickMask and loading block
			outClickMask.SetActive(true);
			leaderboardPanel.SetActive(true);
			ClearLeaderboardPanel(true);
		}

		/// <summary>
		/// Clear the leaderboard panel container (loading block, texts, page buttons, previously created items).
		/// </summary>
		/// <param name="showLoadingBlock">If the loading block should be shown.</param>
		public void ClearLeaderboardPanel(bool showLoadingBlock = false)
		{
			// Show/hide the loading block
			loadingBlock.SetActive(showLoadingBlock);

			// Hide all texts
			errorText.gameObject.SetActive(false);
			noScoreText.gameObject.SetActive(false);

			// Hide the previous page and next page buttons
			pageButtons.SetActive(false);

			// Destroy the previously created leaderboard GameObjects if any exist and clear the list
			foreach (GameObject leaderboardItem in leaderboardItems)
				DestroyObject(leaderboardItem);

			leaderboardItems.Clear();
		}

		/// <summary>
		/// Fill the leaderboard panel with nonpaged scores then show it.
		/// </summary>
		/// <param name="scoresList">List of the scores to display.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		public void FillNonpagedLeaderboardPanel(NonpagedList<Score> scoresList, string noScoreErrorMessage = "(no score to display)")
		{
			// Clear the leaderboard panel
			ClearLeaderboardPanel(false);

			// If there are scores to display, fill the leaderboard panel with score prefabs
			if ((scoresList != null) && (scoresList.Count > 0))
				foreach (Score score in scoresList)
				{
					// Create a leaderboard score GameObject and hook it at the leaderboard items layout
					GameObject prefabInstance = Instantiate<GameObject>(leaderboardScorePrefab);
					prefabInstance.transform.SetParent(leaderboardItemsLayout.transform, false);

					// Fill the newly created GameObject with score data
					LeaderboardScoreHandler leaderboardScoreHandler = prefabInstance.GetComponent<LeaderboardScoreHandler>();
					leaderboardScoreHandler.FillData(score);

					// Add the newly created GameObject to the list
					leaderboardItems.Add(prefabInstance);
				}
			// Else, show the "no score" text
			else
			{
				noScoreText.text = noScoreErrorMessage;
				noScoreText.gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Fill the leaderboard panel with paged scores then show it.
		/// </summary>
		/// <param name="scoresList">List of the scores to display.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		public void FillPagedLeaderboardPanel(PagedList<Score> scoresList, string noScoreErrorMessage = "(no score to display)")
		{
			// Clear the leaderboard panel
			ClearLeaderboardPanel(false);

			// If there are scores to display, fill the leaderboard panel with score prefabs
			if ((scoresList != null) && (scoresList.Count > 0))
			{
				foreach (Score score in scoresList)
				{
					// Create a leaderboard score GameObject and hook it at the leaderboard items layout
					GameObject prefabInstance = Instantiate<GameObject>(leaderboardScorePrefab);
					prefabInstance.transform.SetParent(leaderboardItemsLayout.transform, false);

					// Fill the newly created GameObject with score data
					LeaderboardScoreHandler leaderboardScoreHandler = prefabInstance.GetComponent<LeaderboardScoreHandler>();
					leaderboardScoreHandler.FillData(score);

					// Add the newly created GameObject to the list
					leaderboardItems.Add(prefabInstance);
				}

				// Keep the last PagedList<Score> to allow fetching of previous and next leaderboard pages and show the previous page and next page buttons
				currentScoresList = scoresList;
				previousPageButton.interactable = currentScoresList.HasPrevious;
				nextPageButton.interactable = currentScoresList.HasNext;
				pageButtons.SetActive(true);
			}
			// Else, show the "no score" text and reset the current scores list
			else
			{
				noScoreText.text = noScoreErrorMessage;
				noScoreText.gameObject.SetActive(true);
				currentScoresList = null;
			}
		}

		/// <summary>
		/// Fill the leaderboard panel with scores from multiple boards then show it.
		/// </summary>
		/// <param name="scoresList">List of the scores to display.</param>
		/// <param name="noScoreErrorMessage">Error message to display in case of no score to display.</param>
		public void FillMultipleLeaderboardPanel(Dictionary<string, Score> scoresList, string noScoreErrorMessage = "(no score to display)")
		{
			// Clear the leaderboard panel
			ClearLeaderboardPanel(false);

			// If there are scores to display, fill the leaderboard panel with gamer score prefabs
			if ((scoresList != null) && (scoresList.Count > 0))
				foreach (KeyValuePair<string, Score> score in scoresList)
				{
					// Create a leaderboard gamer score GameObject and hook it at the leaderboard items layout
					GameObject prefabInstance = Instantiate<GameObject>(leaderboardGamerScorePrefab);
					prefabInstance.transform.SetParent(leaderboardItemsLayout.transform, false);

					// Fill the newly created GameObject with score data
					LeaderboardGamerScoreHandler leaderboardGamerScoreHandler = prefabInstance.GetComponent<LeaderboardGamerScoreHandler>();
					leaderboardGamerScoreHandler.FillData(score.Value, score.Key);

					// Add the newly created GameObject to the list
					leaderboardItems.Add(prefabInstance);
				}
			// Else, show the "no score" text
			else
			{
				noScoreText.text = noScoreErrorMessage;
				noScoreText.gameObject.SetActive(true);
			}
		}

		/// <summary>
		/// Clear the leaderboard panel and show an error message.
		/// </summary>
		/// <param name="errorMessage">Error message to display.</param>
		public void ShowError(string errorMessage)
		{
			// Clear the leaderboard panel
			ClearLeaderboardPanel(false);

			// Set and show the error message
			errorText.text = string.Format("\n{0}\n", errorMessage);
			errorText.gameObject.SetActive(true);
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
			leaderboardItemsLayout.constraintCount = Screen.width > Screen.height ? 2 : 1;
		}
		#endregion
	}
}
