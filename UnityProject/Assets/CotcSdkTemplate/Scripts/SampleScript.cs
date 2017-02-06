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
		// Register to the CloudInitialized event
		CloudFeatures.CloudInitialized += OnCloudInitialized;

		// Call the template method
		CloudFeatures.InitializeCloud();
	}

	// What to do once the CotcSdk's Cloud is initialized
	private void OnCloudInitialized(Cloud cloud)
	{
		// Register to the GamerLoggedIn event
		LoginFeatures.GamerLoggedIn += OnGamerLoggedIn;

		// Call the template method
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

	// When the corresponding button is clicked, display all high scores from the given leaderboard
	public void Button_DisplayAllHighScores()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string boardName = "TestBoard";
		int scoresPerPage = 10;

		// Check the boardName value
		if (displayAllHighScores_BoardName != null)
			boardName = string.IsNullOrEmpty(displayAllHighScores_BoardName.text) ? boardName : displayAllHighScores_BoardName.text;
		else
			Debug.LogWarning("[SampleScript:Leaderboard] displayAllHighScores_BoardName InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Check the scoresPerPage value
		if (displayAllHighScores_ScoresPerPage != null)
			scoresPerPage = string.IsNullOrEmpty(displayAllHighScores_ScoresPerPage.text) ? scoresPerPage : int.Parse(displayAllHighScores_ScoresPerPage.text);
		else
			Debug.LogWarning("[SampleScript:Leaderboard] displayAllHighScores_ScoresPerPage InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Call the template method
		LeaderboardFeatures.DisplayAllHighScores(boardName, scoresPerPage);
	}

	// References to the leaderboard InputField UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField postScore_BoardName = null;
	[SerializeField] private InputField postScore_ScoreValue = null;
	[SerializeField] private InputField postScore_ScoreDescription = null;

	// When the corresponding button is clicked, post a new score to the given leaderboard
	public void Button_PostScore()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string boardName = "TestBoard";
		long scoreValue = 42L;
		string scoreDescription = "This is a test score";

		// Check the boardName value
		if (postScore_BoardName != null)
			boardName = string.IsNullOrEmpty(postScore_BoardName.text) ? boardName : postScore_BoardName.text;
		else
			Debug.LogWarning("[SampleScript:Leaderboard] postScore_BoardName InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Check the scoreValue value
		if (postScore_ScoreValue != null)
			scoreValue = string.IsNullOrEmpty(postScore_ScoreValue.text) ? scoreValue : long.Parse(postScore_ScoreValue.text);
		else
			Debug.LogWarning("[SampleScript:Leaderboard] postScore_ScoreValue InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Check the scoreDescription value
		if (postScore_ScoreDescription != null)
			scoreDescription = postScore_ScoreDescription.text;
		else
			Debug.LogWarning("[SampleScript:Leaderboard] postScore_ScoreDescription InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Call the template method
		LeaderboardFeatures.PostScore(boardName, scoreValue, scoreDescription);
	}
	#endregion

	#region Achievement
	// When the corresponding button is clicked, display all game's achievements
	public void Button_DisplayAchievements()
	{
		// Call the template method
		AchievementFeatures.DisplayAchievements();
	}
	#endregion

	#region Transaction
	// References to the transaction InputField UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField postTransaction_CurrencyName = null;
	[SerializeField] private InputField postTransaction_CurrencyAmount = null;
	[SerializeField] private InputField postTransaction_TransactionDescription = null;

	// When the corresponding button is clicked, post a new transaction of the given currency
	public void Button_PostTransaction()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string currencyName = "TestCurrency";
		float currencyAmount = 50f;
		string transactionDescription = "This is a test transaction";

		// Check the currencyName value
		if (postTransaction_CurrencyName != null)
			currencyName = string.IsNullOrEmpty(postTransaction_CurrencyName.text) ? currencyName : postTransaction_CurrencyName.text;
		else
			Debug.LogWarning("[SampleScript:Transaction] postTransaction_CurrencyName InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Check the currencyAmount value
		if (postTransaction_CurrencyAmount != null)
			currencyAmount = string.IsNullOrEmpty(postTransaction_CurrencyAmount.text) ? currencyAmount : float.Parse(postTransaction_CurrencyAmount.text);
		else
			Debug.LogWarning("[SampleScript:Transaction] postTransaction_CurrencyAmount InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Check the transactionDescription value
		if (postTransaction_TransactionDescription != null)
			transactionDescription = postTransaction_TransactionDescription.text;
		else
			Debug.LogWarning("[SampleScript:Transaction] postTransaction_TransactionDescription InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		
		// Call the template method
		TransactionFeatures.PostTransaction(currencyName, currencyAmount, transactionDescription);
	}
	#endregion
}
