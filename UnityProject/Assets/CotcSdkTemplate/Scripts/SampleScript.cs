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
		// Register to the GamerLoggedIn and GamerLoggedOut events
		LoginFeatures.GamerLoggedIn += OnGamerLoggedIn;
		LoginFeatures.GamerLoggedOut += OnGamerLoggedOut;

		// Call the template method
		LoginFeatures.AutoLogin();
	}

	// What to do once a gamer has logged in
	private void OnGamerLoggedIn(Gamer gamer)
	{
		// Do whatever...
	}

	// What to do once a gamer has logged out
	private void OnGamerLoggedOut()
	{
		// Do whatever...
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

	#region Game VFS
	// When the corresponding button is clicked, read and display the value of all keys associated to the current game
	public void Button_DisplayAllGameKeys()
	{
		// Call the template method
		GameVFSFeatures.DisplayGameKey(null);
	}

	// References to the game VFS UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField displayGameKey_Key = null;

	// When the corresponding button is clicked, read and display the value of the given key associated to the current game
	public void Button_DisplayGameKey()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string key = "TestString";

		// Check the key value
		if (displayGameKey_Key != null)
			key = string.IsNullOrEmpty(displayGameKey_Key.text) ? key : displayGameKey_Key.text;
		else
			Debug.LogWarning("[SampleScript:GameVFS] displayGameKey_Key InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Call the template method
		GameVFSFeatures.DisplayGameKey(key);
	}
	#endregion

	#region Gamer VFS
	// When the corresponding button is clicked, read and display the value of all keys associated to the current logged in gamer
	public void Button_DisplayAllGamerKeys()
	{
		// Call the template method
		GamerVFSFeatures.DisplayGamerKey(null);
	}

	// References to the gamer VFS UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField displayGamerKey_Key = null;

	// When the corresponding button is clicked, read and display the value of the given key associated to the current logged in gamer
	public void Button_DisplayGamerKey()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string key = "TestString";

		// Check the key value
		if (displayGamerKey_Key != null)
			key = string.IsNullOrEmpty(displayGamerKey_Key.text) ? key : displayGamerKey_Key.text;
		else
			Debug.LogWarning("[SampleScript:GamerVFS] displayGamerKey_Key InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Call the template method
		GamerVFSFeatures.DisplayGamerKey(key);
	}

	// References to the gamer VFS UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField setGamerKey_Key = null;
	[SerializeField] private InputField setGamerKey_Value = null;
	[SerializeField] private ToggleGroup setGamerKey_Type = null;

	// When the corresponding button is clicked, create / update the value of the given key associated to the current logged in gamer
	public void Button_SetGamerKey()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string key = "TestString";
		string value = "Test value.";
		Bundle.DataType type = Bundle.DataType.String;

		// Check the key value
		if (setGamerKey_Key != null)
			key = string.IsNullOrEmpty(setGamerKey_Key.text) ? key : setGamerKey_Key.text;
		else
			Debug.LogWarning("[SampleScript:GamerVFS] setGamerKey_Key InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Check the value value
		if (setGamerKey_Value != null)
			value = string.IsNullOrEmpty(setGamerKey_Value.text) ? value : setGamerKey_Value.text;
		else
			Debug.LogWarning("[SampleScript:GamerVFS] setGamerKey_Value InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Check the type value
		if (setGamerKey_Type != null)
			// This foreach should give only one active toggle
			foreach (Toggle activeToggle in setGamerKey_Type.ActiveToggles())
				switch (activeToggle.name)
				{
					case "Toggle-JsonType":
					type = Bundle.DataType.Object;
					break;
					
					case "Toggle-StringType":
					type = Bundle.DataType.String;
					break;
					
					case "Toggle-DoubleType":
					type = Bundle.DataType.Double;
					break;
					
					case "Toggle-IntType":
					type = Bundle.DataType.Integer;
					break;
					
					case "Toggle-BoolType":
					type = Bundle.DataType.Boolean;
					break;
				}
		else
			Debug.LogWarning("[SampleScript:GamerVFS] setGamerKey_Type ToggleGroup reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Call the template method
		GamerVFSFeatures.SetGamerKey(type, key, value);
	}

	// References to the gamer VFS UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField deleteGamerKey_Key = null;

	// When the corresponding button is clicked, delete the given key associated to the current logged in gamer
	public void Button_DeleteGamerKey()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string key = "TestString";

		// Check the key value
		if (deleteGamerKey_Key != null)
			key = string.IsNullOrEmpty(deleteGamerKey_Key.text) ? key : deleteGamerKey_Key.text;
		else
			Debug.LogWarning("[SampleScript:GamerVFS] deleteGamerKey_Key InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Call the template method
		GamerVFSFeatures.DeleteGamerKey(key);
	}
	#endregion

	#region Leaderboard
	// References to the leaderboard UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField displayAllHighScores_BoardName = null;
	[SerializeField] private InputField displayAllHighScores_ScoresPerPage = null;
	[SerializeField] private Toggle displayAllHighScores_CenteredBoard = null;

	// When the corresponding button is clicked, display all high scores from the given leaderboard
	public void Button_DisplayAllHighScores()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string boardName = "TestBoard";
		int scoresPerPage = 10;
		bool centeredBoard = false;

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

		// Check the centeredBoard value
		if (displayAllHighScores_CenteredBoard != null)
			centeredBoard = displayAllHighScores_CenteredBoard.isOn;
		else
			Debug.LogWarning("[SampleScript:Leaderboard] displayAllHighScores_CenteredBoard InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Call the template method
		LeaderboardFeatures.DisplayAllHighScores(boardName, scoresPerPage, centeredBoard);
	}

	// When the corresponding button is clicked, display logged in gamer's high scores from all leaderboards in which he scored
	public void Button_DisplayGamerHighScores()
	{
		// Call the template method
		LeaderboardFeatures.DisplayGamerHighScores();
	}

	// References to the leaderboard UI elements (their serialized references are directly assigned in the scene)
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

	#region Login
	// When the corresponding button is clicked, login with a new anonymous account
	public void Button_LoginAsAnonymous()
	{
		// Call the template method
		LoginFeatures.LoginAsAnonymous();
	}

	// References to the login UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField loginWithCredentials_GamerID = null;
	[SerializeField] private InputField loginWithCredentials_GamerSecret = null;

	// When the corresponding button is clicked, login with a previously created account
	public void Button_LoginWithCredentials()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string gamerID = "";
		string gamerSecret = "";

		// Check the gamerID value
		if (loginWithCredentials_GamerID != null)
			gamerID = string.IsNullOrEmpty(loginWithCredentials_GamerID.text) ? gamerID : loginWithCredentials_GamerID.text;
		else
			Debug.LogWarning("[SampleScript:Login] loginWithCredentials_GamerID InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Check the gamerSecret value
		if (loginWithCredentials_GamerSecret != null)
			gamerSecret = string.IsNullOrEmpty(loginWithCredentials_GamerSecret.text) ? gamerSecret : loginWithCredentials_GamerSecret.text;
		else
			Debug.LogWarning("[SampleScript:Login] loginWithCredentials_GamerSecret InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");

		// Call the template method
		LoginFeatures.LoginWithCredentials(gamerID, gamerSecret);
	}

	// When the corresponding button is clicked, logout the current logged in gamer
	public void Button_LogoutGamer()
	{
		// Call the template method
		LoginFeatures.LogoutGamer();
	}
	#endregion

	#region Transaction
	// References to the transaction UI elements (their serialized references are directly assigned in the scene)
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
