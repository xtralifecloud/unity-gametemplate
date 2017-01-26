using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class LeaderboardHandler : MonoBehaviour
	{
		#region Instance Registering
		// Register this MonoBehaviour's instance at Awake to be sure it's done before any Start executes
		private void Awake()
		{
			MonoSingletons.Register<LeaderboardHandler>(this);
		}
		#endregion

		#region Display
		// Reference to the leaderboard UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject leaderboardPanel = null;
		[SerializeField] private Text leaderboardPanelTitle = null;
		[SerializeField] private GameObject noScoreText = null;
		[SerializeField] private Button previousPageButton = null;
		[SerializeField] private Button nextPageButton = null;

		// Reference to the score GameObject prefab and the scores list scroll view
		[SerializeField] private GameObject scorePrefab = null;
		[SerializeField] private Transform scoresScrollViewContent = null;

		// List of the score GameObjects created on the leaderboard
		private List<GameObject> leaderboardScores = new List<GameObject>();

		// The last PagedList<Score> used to fill the leaderboard
		private PagedList<Score> currentScoresList = null;

		// Hide the leaderboard's panel at Start
		private void Start()
		{
			ShowLeaderboardPanel(false);
		}

		// Show or hide the leaderboard's panel
		public void ShowLeaderboardPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			leaderboardPanel.SetActive(show);
		}

		// Fill the leaderboard's panel with scores then show it
		public void FillAndShowLeaderboardPanel(string boardName, PagedList<Score> scoresList)
		{
			// Destroy the previously created score GameObjects if any exist and clear the list
			foreach (GameObject leaderboardScore in leaderboardScores)
				DestroyObject(leaderboardScore);

			leaderboardScores.Clear();

			// Set the leaderboard's title
			if (!string.IsNullOrEmpty(boardName))
				leaderboardPanelTitle.text = boardName;

			// If there are scores to display, fill the leaderboard panel with score prefabs
			if (scoresList != null)
			{
				// Hide the "no score" text and show the previous page and next page buttons
				noScoreText.SetActive(false);
				previousPageButton.gameObject.SetActive(true);
				nextPageButton.gameObject.SetActive(true);

				foreach (Score score in scoresList)
				{
					// Create a leaderboard score GameObject and hook it at the leaderboard scores scroll view
					GameObject prefabInstance = Instantiate<GameObject>(scorePrefab);
					prefabInstance.transform.SetParent(scoresScrollViewContent, false);

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

				// Ensure the next page button stays at the end of the scores list
				nextPageButton.transform.SetAsLastSibling();
			}
			// Else, show the "no score" text, hide the previous page and next page buttons, and prevent usage of previous and next leaderboard page
			else
			{
				noScoreText.SetActive(true);
				previousPageButton.gameObject.SetActive(false);
				nextPageButton.gameObject.SetActive(false);
				currentScoresList = null;
			}

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
	}
}
