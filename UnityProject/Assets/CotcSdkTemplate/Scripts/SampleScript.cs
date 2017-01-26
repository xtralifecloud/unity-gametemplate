using UnityEngine;
using UnityEngine.UI;

using CotcSdk;
using CotcSdkTemplate;

public class SampleScript : MonoBehaviour
{
	#region Cloud + Login
	// Initialize the CotcSdk's Cloud at start
	private void Start()
	{
		CloudFeatures.CloudInitialized += OnCloudInitialized;
		CloudFeatures.InitializeCloud();
	}

	// What to do once the CotcSdk's Cloud is initialized
	private void OnCloudInitialized(Cloud cloud)
	{
		LoginFeatures.GamerLoggedIn += OnGamerLoggedIn;
		LoginFeatures.AutoLogin();
	}

	// What to do once a gamer has logged in
	private void OnGamerLoggedIn(Gamer gamer)
	{
		// Do whatever...
	}
	#endregion

	#region Leaderboard
	// References to the leaderboard InputField UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField displayAllHighScores_BoardName = null;
	[SerializeField] private InputField displayAllHighScores_ScoresPerPage = null;

	// When the corresponding button is clicked, display all high scores from the given leaderboard name
	public void Button_DisplayAllHighScores()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string boardName = "TestLeaderboard";
		int scoresPerPage = 10;

		// Check the boardName value
		if (displayAllHighScores_BoardName != null)
			boardName = string.IsNullOrEmpty(displayAllHighScores_BoardName.text) ? boardName : displayAllHighScores_BoardName.text;
		else
			Debug.LogWarning("[SampleScript:Leaderboard] DisplayAllHighScores_BoardName InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Check the scoresPerPage value
		if (displayAllHighScores_ScoresPerPage != null)
			scoresPerPage = string.IsNullOrEmpty(displayAllHighScores_ScoresPerPage.text) ? scoresPerPage : int.Parse(displayAllHighScores_ScoresPerPage.text);
		else
			Debug.LogWarning("[SampleScript:Leaderboard] displayAllHighScores_ScoresPerPage InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Call the template method
		LeaderboardFeatures.DisplayAllHighScores(boardName, scoresPerPage);
	}
	#endregion
}
