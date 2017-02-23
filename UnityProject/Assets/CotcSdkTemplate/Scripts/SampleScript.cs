using UnityEngine;
using UnityEngine.UI;

using CotcSdk;
using CotcSdkTemplate;

/// <summary>
/// A sample script to show a way to use the CotcSdkTemplate features.
/// </summary>
public class SampleScript : MonoBehaviour
{
	#region Cloud + Login
	/// <summary>
	/// Initialize the CotcSdk's Cloud instance at Start.
	/// </summary>
	private void Start()
	{
		// Register to the CloudInitialized event
		CloudFeatures.Event_CloudInitialized += OnCloudInitialized;

		// Call the template method
		CloudFeatures.Handling_InitializeCloud();
	}

	/// <summary>
	/// What to do once the CotcSdk's Cloud instance is initialized. (here we call an AutoLogin)
	/// </summary>
	/// <param name="cloud">The initialized Cloud instance.</param>
	private void OnCloudInitialized(Cloud cloud)
	{
		// Register to the GamerLoggedIn and GamerLoggedOut events
		LoginFeatures.Event_GamerLoggedIn += OnGamerLoggedIn;
		LoginFeatures.Event_GamerLoggedOut += OnGamerLoggedOut;

		// Call the template method
		LoginFeatures.Handling_AutoLogin();
	}

	/// <summary>
	/// What to do once a gamer has logged in.
	/// </summary>
	/// <param name="gamer">The logged in Gamer instance.</param>
	private void OnGamerLoggedIn(Gamer gamer)
	{
		// Do whatever...
	}

	/// <summary>
	/// What to do once a gamer has logged out.
	/// </summary>
	private void OnGamerLoggedOut()
	{
		// Do whatever...
	}
	#endregion

	#region Achievement
	/// <summary>
	/// When the corresponding button is clicked, get and display logged in gamer's progress on all game's achievements.
	/// </summary>
	public void Button_DisplayAchievements()
	{
		// Call the template method
		AchievementFeatures.Handling_DisplayAchievements();
	}
	#endregion

	#region Game VFS
	/// <summary>
	/// When the corresponding button is clicked, get and display the value of all keys associated to the current game.
	/// </summary>
	public void Button_DisplayAllGameKeys()
	{
		// Call the template method
		GameVFSFeatures.Handling_DisplayGameKey(null);
	}

	// References to the game VFS UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField displayGameKey_Key = null;

	/// <summary>
	/// When the corresponding button is clicked, get and display the value of the given key associated to the current game.
	/// </summary>
	public void Button_DisplayGameKey()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string key = "TestString";

		// Check the key value
		if (displayGameKey_Key == null)
			Debug.LogWarning("[SampleScript:GameVFS] displayGameKey_Key InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(displayGameKey_Key.text))
			key = displayGameKey_Key.text;

		// Call the template method
		GameVFSFeatures.Handling_DisplayGameKey(key);
	}
	#endregion

	#region Gamer VFS
	/// <summary>
	/// When the corresponding button is clicked, get and display the value of all keys associated to the current logged in gamer.
	/// </summary>
	public void Button_DisplayAllGamerKeys()
	{
		// Call the template method
		GamerVFSFeatures.Handling_DisplayGamerKey(null);
	}

	// References to the gamer VFS UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField displayGamerKey_Key = null;

	/// <summary>
	/// When the corresponding button is clicked, get and display the value of the given key associated to the current logged in gamer.
	/// </summary>
	public void Button_DisplayGamerKey()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string key = "TestString";

		// Check the key value
		if (displayGamerKey_Key == null)
			Debug.LogWarning("[SampleScript:GamerVFS] displayGamerKey_Key InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(displayGamerKey_Key.text))
			key = displayGamerKey_Key.text;

		// Call the template method
		GamerVFSFeatures.Handling_DisplayGamerKey(key);
	}

	// References to the gamer VFS UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField setGamerKey_Key = null;
	[SerializeField] private InputField setGamerKey_Value = null;
	[SerializeField] private ToggleGroup setGamerKey_Type = null;

	/// <summary>
	/// When the corresponding button is clicked, create / update the value of the given key associated to the current logged in gamer.
	/// </summary>
	public void Button_SetGamerKey()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string key = "TestString";
		string value = "Test value.";
		Bundle.DataType type = Bundle.DataType.String;

		// Check the key value
		if (setGamerKey_Key == null)
			Debug.LogWarning("[SampleScript:GamerVFS] setGamerKey_Key InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(setGamerKey_Key.text))
			key = setGamerKey_Key.text;

		// Check the value value
		if (setGamerKey_Value == null)
			Debug.LogWarning("[SampleScript:GamerVFS] setGamerKey_Value InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(setGamerKey_Value.text))
			value = setGamerKey_Value.text;
		
		// Check the type value
		if (setGamerKey_Type == null)
			Debug.LogWarning("[SampleScript:GamerVFS] setGamerKey_Type ToggleGroup reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		// This foreach should give only one active toggle
		else foreach (Toggle activeToggle in setGamerKey_Type.ActiveToggles())
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

		// Call the template method
		GamerVFSFeatures.Handling_SetGamerKey(type, key, value);
	}

	// References to the gamer VFS UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField deleteGamerKey_Key = null;

	/// <summary>
	/// When the corresponding button is clicked, delete the given key associated to the current logged in gamer.
	/// </summary>
	public void Button_DeleteGamerKey()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string key = "TestString";

		// Check the key value
		if (deleteGamerKey_Key == null)
			Debug.LogWarning("[SampleScript:GamerVFS] deleteGamerKey_Key InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(deleteGamerKey_Key.text))
			key = deleteGamerKey_Key.text;

		// Call the template method
		GamerVFSFeatures.Handling_DeleteGamerKey(key);
	}
	#endregion

	#region Leaderboard
	// References to the leaderboard UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField displayAllHighScores_BoardName = null;
	[SerializeField] private InputField displayAllHighScores_ScoresPerPage = null;
	[SerializeField] private Toggle displayAllHighScores_CenteredBoard = null;

	/// <summary>
	/// When the corresponding button is clicked, get and display all gamers' high scores from the given leaderboard.
	/// </summary>
	public void Button_DisplayAllHighScores()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string boardName = "TestBoard";
		int scoresPerPage = 10;
		bool centeredBoard = false;

		// Check the boardName value
		if (displayAllHighScores_BoardName == null)
			Debug.LogWarning("[SampleScript:Leaderboard] displayAllHighScores_BoardName InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(displayAllHighScores_BoardName.text))
			boardName = displayAllHighScores_BoardName.text;
		
		// Check the scoresPerPage value
		if (displayAllHighScores_ScoresPerPage == null)
			Debug.LogWarning("[SampleScript:Leaderboard] displayAllHighScores_ScoresPerPage InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(displayAllHighScores_ScoresPerPage.text))
			scoresPerPage = int.Parse(displayAllHighScores_ScoresPerPage.text);
		
		// Check the centeredBoard value
		if (displayAllHighScores_CenteredBoard == null)
			Debug.LogWarning("[SampleScript:Leaderboard] displayAllHighScores_CenteredBoard InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else
			centeredBoard = displayAllHighScores_CenteredBoard.isOn;

		// Call the template method
		LeaderboardFeatures.Handling_DisplayAllHighScores(boardName, scoresPerPage, centeredBoard);
	}

	/// <summary>
	/// When the corresponding button is clicked, get and display the current logged in gamer's best scores from all leaderboards in which he scored at least once.
	/// </summary>
	public void Button_DisplayGamerHighScores()
	{
		// Call the template method
		LeaderboardFeatures.Handling_DisplayGamerHighScores();
	}

	// References to the leaderboard UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField postScore_BoardName = null;
	[SerializeField] private InputField postScore_ScoreValue = null;
	[SerializeField] private InputField postScore_ScoreDescription = null;

	/// <summary>
	/// When the corresponding button is clicked, post a new score on the given leaderboard for the current logged in gamer.
	/// </summary>
	public void Button_PostScore()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string boardName = "TestBoard";
		long scoreValue = 42L;
		string scoreDescription = "This is a test score";

		// Check the boardName value
		if (postScore_BoardName == null)
			Debug.LogWarning("[SampleScript:Leaderboard] postScore_BoardName InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(postScore_BoardName.text))
			boardName = postScore_BoardName.text;
		
		// Check the scoreValue value
		if (postScore_ScoreValue == null)
			Debug.LogWarning("[SampleScript:Leaderboard] postScore_ScoreValue InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(postScore_ScoreValue.text))
			scoreValue = long.Parse(postScore_ScoreValue.text);
		
		// Check the scoreDescription value
		if (postScore_ScoreDescription == null)
			Debug.LogWarning("[SampleScript:Leaderboard] postScore_ScoreDescription InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else
			scoreDescription = postScore_ScoreDescription.text;
		
		// Call the template method
		LeaderboardFeatures.Handling_PostScore(boardName, scoreValue, scoreDescription);
	}
	#endregion

	#region Login
	/// <summary>
	/// When the corresponding button is clicked, login the gamer with a new anonymous account.
	/// </summary>
	public void Button_LoginAsAnonymous()
	{
		// Call the template method
		LoginFeatures.Handling_LoginAsAnonymous();
	}

	// References to the login UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField loginWithCredentials_GamerID = null;
	[SerializeField] private InputField loginWithCredentials_GamerSecret = null;

	/// <summary>
	/// When the corresponding button is clicked, login the gamer with a previously created account.
	/// </summary>
	public void Button_LoginWithCredentials()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string gamerID = "";
		string gamerSecret = "";

		// Check the gamerID value
		if (loginWithCredentials_GamerID == null)
			Debug.LogWarning("[SampleScript:Login] loginWithCredentials_GamerID InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(loginWithCredentials_GamerID.text))
			gamerID = loginWithCredentials_GamerID.text;
		
		// Check the gamerSecret value
		if (loginWithCredentials_GamerSecret == null)
			Debug.LogWarning("[SampleScript:Login] loginWithCredentials_GamerSecret InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(loginWithCredentials_GamerSecret.text))
			gamerSecret = loginWithCredentials_GamerSecret.text;
		
		// Call the template method
		LoginFeatures.Handling_LoginWithCredentials(gamerID, gamerSecret);
	}

	/// <summary>
	/// When the corresponding button is clicked, logout the current logged in gamer.
	/// </summary>
	public void Button_LogoutGamer()
	{
		// Call the template method
		LoginFeatures.Handling_LogoutGamer();
	}
	#endregion

	#region Transaction
	/// <summary>
	/// When the corresponding button is clicked, get and display the current logged in gamer currencies balance.
	/// </summary>
	public void Button_DisplayBalance()
	{
		// Call the template method
		TransactionFeatures.Handling_DisplayBalance();
	}

	// References to the transaction UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField displayAllCurrenciesHistory_TransactionsPerPage = null;

	/// <summary>
	/// When the corresponding button is clicked, get and display the current logged in gamer's history of all currencies.
	/// </summary>
	public void Button_DisplayAllCurrenciesHistory()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		int transactionsPerPage = 20;

		// Check the currenciesPerPage value
		if (displayAllCurrenciesHistory_TransactionsPerPage == null)
			Debug.LogWarning("[SampleScript:Transaction] displayAllCurrenciesHistory_TransactionsPerPage InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(displayAllCurrenciesHistory_TransactionsPerPage.text))
			transactionsPerPage = int.Parse(displayAllCurrenciesHistory_TransactionsPerPage.text);
		
		// Call the template method
		TransactionFeatures.Handling_DisplayCurrencyHistory(null, transactionsPerPage);
	}

	// References to the transaction UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField displayCurrencyHistory_CurrencyName = null;
	[SerializeField] private InputField displayCurrencyHistory_TransactionsPerPage = null;

	/// <summary>
	/// When the corresponding button is clicked, get and display the current logged in gamer's history of the given currency.
	/// </summary>
	public void Button_DisplayCurrencyHistory()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string currencyName = "TestCurrency";
		int transactionsPerPage = 20;

		// Check the currencyName value
		if (displayCurrencyHistory_CurrencyName == null)
			Debug.LogWarning("[SampleScript:Transaction] displayCurrencyHistory_CurrencyName InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(displayCurrencyHistory_CurrencyName.text))
			currencyName = displayCurrencyHistory_CurrencyName.text;
		
		// Check the currenciesPerPage value
		if (displayCurrencyHistory_TransactionsPerPage == null)
			Debug.LogWarning("[SampleScript:Transaction] displayCurrencyHistory_TransactionsPerPage InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(displayCurrencyHistory_TransactionsPerPage.text))
			transactionsPerPage = int.Parse(displayCurrencyHistory_TransactionsPerPage.text);
		
		// Call the template method
		TransactionFeatures.Handling_DisplayCurrencyHistory(currencyName, transactionsPerPage);
	}

	// References to the transaction UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField postTransaction_CurrencyName = null;
	[SerializeField] private InputField postTransaction_CurrencyAmount = null;
	[SerializeField] private InputField postTransaction_TransactionDescription = null;

	/// <summary>
	/// When the corresponding button is clicked, post a new transaction of the given currency for the current logged in gamer.
	/// </summary>
	public void Button_PostTransaction()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string currencyName = "TestCurrency";
		float currencyAmount = 50f;
		string transactionDescription = "This is a test transaction";

		// Check the currencyName value
		if (postTransaction_CurrencyName == null)
			Debug.LogWarning("[SampleScript:Transaction] postTransaction_CurrencyName InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(postTransaction_CurrencyName.text))
			currencyName = postTransaction_CurrencyName.text;
		
		// Check the currencyAmount value
		if (postTransaction_CurrencyAmount == null)
			Debug.LogWarning("[SampleScript:Transaction] postTransaction_CurrencyAmount InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else if (!string.IsNullOrEmpty(postTransaction_CurrencyAmount.text))
			currencyAmount = float.Parse(postTransaction_CurrencyAmount.text);
		
		// Check the transactionDescription value
		if (postTransaction_TransactionDescription == null)
			Debug.LogWarning("[SampleScript:Transaction] postTransaction_TransactionDescription InputField reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values");
		else
			transactionDescription = postTransaction_TransactionDescription.text;
		
		// Call the template method
		TransactionFeatures.Handling_PostTransaction(currencyName, currencyAmount, transactionDescription);
	}
	#endregion
}
