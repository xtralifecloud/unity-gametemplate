using UnityEngine;
using UnityEngine.UI;

using CotcSdk;
using CotcSdkTemplate;

/// <summary>
/// A sample script to show a way to use the CotcSdkTemplate features.
/// </summary>
public class SampleScript : MonoBehaviour
{
	#region Initialization (Cloud + Event + Login)
	// TODO: You may use the following credentials on your CotcGameObject GameObject instance to land on the sandbox XlGameTemplate test environment:
	// API Key: jO6h9ndwmHRS0OX
	// API Secret: u5irMS8QqNp5YWzkDnML

	// CotcSdkTemplate's messages logging level
	[SerializeField] private CotcSdkTemplate.LogLevel cotcSdkTemplateLogLevel = CotcSdkTemplate.LogLevel.Verbose;

	// No InputField reference debug message format
	private const string noReferenceFormat = "[SampleScript:{0}] {1} reference is null >> Please assign it on the SampleScript script's instance attached to an object in the scene if you wish to replace the default hardcoded values";

	/// <summary>
	/// Set CotcSdkTemplate's messages logging level and initialize the CotcSdk's Cloud instance at Start.
	/// </summary>
	private void Start()
	{
		// Set the logging level for CotcSdkTemplate's messages
		DebugLogs.logLevel = cotcSdkTemplateLogLevel;

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
	/// What to do once a gamer has logged in. (here we start to listen for server's events)
	/// </summary>
	/// <param name="gamer">The logged in Gamer instance.</param>
	private void OnGamerLoggedIn(Gamer gamer)
	{
		// Register to gamer's events loop to start retrieving events messages from the server
		EventFeatures.Handling_StartEventsListening(gamer);
	}

	/// <summary>
	/// What to do once a gamer has logged out. (here we stop to listen for server's events)
	/// </summary>
	private void OnGamerLoggedOut()
	{
		// Unregister from gamer's events loop to stop retrieving events messages from the server
		EventFeatures.Handling_StopEventsListening();
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

	#region Community
	/// <summary>
	/// When the corresponding button is clicked, get and display the list of current logged in gamer's blacklisted gamers.
	/// </summary>
	public void Button_DisplayBlacklistedGamers()
	{
		// Call the template method
		CommunityFeatures.Handling_DisplayBlacklistedGamers();
	}

	/// <summary>
	/// When the corresponding button is clicked, get and display the list of current logged in gamer's friends.
	/// </summary>
	public void Button_DisplayFriends()
	{
		// Call the template method
		CommunityFeatures.Handling_DisplayFriends();
	}

	// References to the community UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField findGamers_MatchPattern = null;
	[SerializeField] private InputField findGamers_UsersPerPage = null;

	/// <summary>
	/// When the corresponding button is clicked, get and display a list of gamers matching with the given match pattern (tested against display name and email).
	/// </summary>
	public void Button_FindGamers()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string matchPattern = "Guest";
		int usersPerPage = 20;

		// Check the filter value
		if (findGamers_MatchPattern == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Community", "findGamers_MatchPattern"));
		else if (!string.IsNullOrEmpty(findGamers_MatchPattern.text))
			matchPattern = findGamers_MatchPattern.text;

		// Check the usersPerPage value
		if (findGamers_UsersPerPage == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Community", "findGamers_UsersPerPage"));
		else if (!string.IsNullOrEmpty(findGamers_UsersPerPage.text))
			usersPerPage = int.Parse(findGamers_UsersPerPage.text);

		// Call the template method
		CommunityFeatures.Handling_FindGamers(matchPattern, usersPerPage);
	}

	// References to the community UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField sendEventToGamer_GamerID = null;
	[SerializeField] private InputField sendEventToGamer_EventData = null;
	[SerializeField] private InputField sendEventToGamer_Notification = null;

	/// <summary>
	/// When the corresponding button is clicked, send an event from the current logged in gamer to the given other gamer.
	/// </summary>
	public void Button_SendEventToGamer()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string gamerID = null;
		string eventData = "{\"type\":\"message\", \"data\":\"Hi!\"}";
		string notification = "{\"en\":\"Received message!\"}";

		// Check the gamerID value
		if (sendEventToGamer_GamerID == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Community", "sendEventToGamer_GamerID"));
		else if (!string.IsNullOrEmpty(sendEventToGamer_GamerID.text))
			gamerID = sendEventToGamer_GamerID.text;

		// Check the eventData value
		if (sendEventToGamer_EventData == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Community", "sendEventToGamer_EventData"));
		else if (!string.IsNullOrEmpty(sendEventToGamer_EventData.text))
			eventData = sendEventToGamer_EventData.text;

		// Check the notification value
		if (sendEventToGamer_Notification == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Community", "sendEventToGamer_Notification"));
		else
			notification = sendEventToGamer_Notification.text;
		
		// Call the template method
		CommunityFeatures.Handling_SendEventToGamer(gamerID, eventData, notification);
	}

	// References to the community UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField sendMessageToGamer_GamerID = null;
	[SerializeField] private InputField sendMessageToGamer_Message = null;

	/// <summary>
	/// When the corresponding button is clicked, send a friend message event from the current logged in gamer to the given other gamer.
	/// </summary>
	public void Button_SendMessageToGamer()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string gamerID = null;
		string message = "Hey buddy! :D";

		// Check the gamerID value
		if (sendMessageToGamer_GamerID == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Community", "sendMessageToGamer_GamerID"));
		else if (!string.IsNullOrEmpty(sendMessageToGamer_GamerID.text))
			gamerID = sendMessageToGamer_GamerID.text;

		// Check the eventData value
		if (sendMessageToGamer_Message == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Community", "sendMessageToGamer_Message"));
		else if (!string.IsNullOrEmpty(sendMessageToGamer_Message.text))
			message = sendMessageToGamer_Message.text;

		// Call the template method
		CommunityFeatures.Handling_SendMessageToGamer(gamerID, message);
	}

	// References to the community UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField setRelationshipWithGamer_GamerID = null;
	[SerializeField] private InputField setRelationshipWithGamer_Notification = null;
	[SerializeField] private ToggleGroup setRelationshipWithGamer_Relationship = null;

	/// <summary>
	/// When the corresponding button is clicked, set the relationship between the current logged in gamer and the given other gamer.
	/// </summary>
	public void Button_SetRelationshipWithGamer()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string gamerID = null;
		string notification = "{\"en\":\"Friend's relationship changed!\"}";
		FriendRelationshipStatus relationship = FriendRelationshipStatus.Add;

		// Check the gamerID value
		if (setRelationshipWithGamer_GamerID == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Community", "setRelationshipWithGamer_GamerID"));
		else if (!string.IsNullOrEmpty(setRelationshipWithGamer_GamerID.text))
			gamerID = setRelationshipWithGamer_GamerID.text;

		// Check the notification value
		if (setRelationshipWithGamer_Notification == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Community", "setRelationshipWithGamer_Notification"));
		else
			notification = setRelationshipWithGamer_Notification.text;

		// Check the relationship value
		if (setRelationshipWithGamer_Relationship == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Community", "setRelationshipWithGamer_Relationship"));
		// This foreach should give only one active toggle
		else foreach (Toggle activeToggle in setRelationshipWithGamer_Relationship.ActiveToggles())
			switch (activeToggle.name)
			{
				case "Toggle-Friend":
				relationship = FriendRelationshipStatus.Add;
				break;
				
				case "Toggle-Blacklist":
				relationship = FriendRelationshipStatus.Blacklist;
				break;
				
				case "Toggle-Forget":
				relationship = FriendRelationshipStatus.Forget;
				break;
			}

		// Call the template method
		CommunityFeatures.Handling_SetGamerRelationship(gamerID, relationship, notification);
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
			Debug.LogWarning(string.Format(noReferenceFormat, "GameVFS", "displayGameKey_Key"));
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
			Debug.LogWarning(string.Format(noReferenceFormat, "GamerVFS", "displayGamerKey_Key"));
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
			Debug.LogWarning(string.Format(noReferenceFormat, "GamerVFS", "setGamerKey_Key"));
		else if (!string.IsNullOrEmpty(setGamerKey_Key.text))
			key = setGamerKey_Key.text;

		// Check the value value
		if (setGamerKey_Value == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "GamerVFS", "setGamerKey_Value"));
		else if (!string.IsNullOrEmpty(setGamerKey_Value.text))
			value = setGamerKey_Value.text;
		
		// Check the type value
		if (setGamerKey_Type == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "GamerVFS", "setGamerKey_Type"));
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
			Debug.LogWarning(string.Format(noReferenceFormat, "GamerVFS", "deleteGamerKey_Key"));
		else if (!string.IsNullOrEmpty(deleteGamerKey_Key.text))
			key = deleteGamerKey_Key.text;

		// Call the template method
		GamerVFSFeatures.Handling_DeleteGamerKey(key);
	}
	#endregion

	#region Godfather
	/// <summary>
	/// When the corresponding button is clicked, get and display the logged in gamer's referral code.
	/// </summary>
	public void Button_DisplayReferralCode()
	{
		// Call the template method
		GodfatherFeatures.Handling_DisplayReferralCode();
	}

	// References to the godfather UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField useReferralCode_ReferralCode = null;

	/// <summary>
	/// When the corresponding button is clicked, use a referral code to set the current logged in gamer's godfather.
	/// </summary>
	public void Button_UseReferralCode()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string referralCode = null;

		// Check the referralCode value
		if (useReferralCode_ReferralCode == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Godfather", "useReferralCode_ReferralCode"));
		else if (!string.IsNullOrEmpty(useReferralCode_ReferralCode.text))
			referralCode = useReferralCode_ReferralCode.text;

		// Call the template method
		GodfatherFeatures.Handling_UseReferralCode(referralCode);
	}

	/// <summary>
	/// When the corresponding button is clicked, get and display the list of current logged in gamer's godchildren.
	/// </summary>
	public void Button_DisplayGodchildren()
	{
		// Call the template method
		GodfatherFeatures.Handling_DisplayGodchildren();
	}

	/// <summary>
	/// When the corresponding button is clicked, get and display the current logged in gamer's godfather.
	/// </summary>
	public void Button_DisplayGodfather()
	{
		// Call the template method
		GodfatherFeatures.Handling_DisplayGodfather();
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
			Debug.LogWarning(string.Format(noReferenceFormat, "Leaderboard", "displayAllHighScores_BoardName"));
		else if (!string.IsNullOrEmpty(displayAllHighScores_BoardName.text))
			boardName = displayAllHighScores_BoardName.text;
		
		// Check the scoresPerPage value
		if (displayAllHighScores_ScoresPerPage == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Leaderboard", "displayAllHighScores_ScoresPerPage"));
		else if (!string.IsNullOrEmpty(displayAllHighScores_ScoresPerPage.text))
			scoresPerPage = int.Parse(displayAllHighScores_ScoresPerPage.text);
		
		// Check the centeredBoard value
		if (displayAllHighScores_CenteredBoard == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Leaderboard", "displayAllHighScores_CenteredBoard"));
		else
			centeredBoard = displayAllHighScores_CenteredBoard.isOn;

		// Call the template method
		LeaderboardFeatures.Handling_DisplayAllHighScores(boardName, scoresPerPage, centeredBoard);
	}

	// References to the leaderboard UI elements (their serialized references are directly assigned in the scene)
	[SerializeField] private InputField displayFriendsHighScores_BoardName = null;

	/// <summary>
	/// When the corresponding button is clicked, get and display all current logged in gamer's friends' high scores from the given leaderboard.
	/// </summary>
	public void Button_DisplayFriendsHighScores()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string boardName = "TestBoard";

		// Check the boardName value
		if (displayFriendsHighScores_BoardName == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Leaderboard", "displayFriendsHighScores_BoardName"));
		else if (!string.IsNullOrEmpty(displayFriendsHighScores_BoardName.text))
			boardName = displayFriendsHighScores_BoardName.text;

		// Call the template method
		LeaderboardFeatures.Handling_DisplayFriendsHighScores(boardName);
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
	[SerializeField] private Toggle postScore_ForceSave = null;

	/// <summary>
	/// When the corresponding button is clicked, post a new score on the given leaderboard for the current logged in gamer.
	/// </summary>
	public void Button_PostScore()
	{
		// Default hardcoded values to use if no InputField elements references are assigned
		string boardName = "TestBoard";
		long scoreValue = 42L;
		string scoreDescription = "This is a test score";
		bool forceSave = false;

		// Check the boardName value
		if (postScore_BoardName == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Leaderboard", "postScore_BoardName"));
		else if (!string.IsNullOrEmpty(postScore_BoardName.text))
			boardName = postScore_BoardName.text;
		
		// Check the scoreValue value
		if (postScore_ScoreValue == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Leaderboard", "postScore_ScoreValue"));
		else if (!string.IsNullOrEmpty(postScore_ScoreValue.text))
			scoreValue = long.Parse(postScore_ScoreValue.text);
		
		// Check the scoreDescription value
		if (postScore_ScoreDescription == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Leaderboard", "postScore_ScoreDescription"));
		else
			scoreDescription = postScore_ScoreDescription.text;
		
		// Check the forceSave value
		if (postScore_ForceSave == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Leaderboard", "postScore_ForceSave"));
		else
			forceSave = postScore_ForceSave.isOn;

		// Call the template method
		LeaderboardFeatures.Handling_PostScore(boardName, scoreValue, scoreDescription, forceSave);
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
		string gamerID = null;
		string gamerSecret = null;

		// Check the gamerID value
		if (loginWithCredentials_GamerID == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Login", "loginWithCredentials_GamerID"));
		else if (!string.IsNullOrEmpty(loginWithCredentials_GamerID.text))
			gamerID = loginWithCredentials_GamerID.text;
		
		// Check the gamerSecret value
		if (loginWithCredentials_GamerSecret == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Login", "loginWithCredentials_GamerSecret"));
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
			Debug.LogWarning(string.Format(noReferenceFormat, "Transaction", "displayAllCurrenciesHistory_TransactionsPerPage"));
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
			Debug.LogWarning(string.Format(noReferenceFormat, "Transaction", "displayCurrencyHistory_CurrencyName"));
		else if (!string.IsNullOrEmpty(displayCurrencyHistory_CurrencyName.text))
			currencyName = displayCurrencyHistory_CurrencyName.text;
		
		// Check the currenciesPerPage value
		if (displayCurrencyHistory_TransactionsPerPage == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Transaction", "displayCurrencyHistory_TransactionsPerPage"));
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
			Debug.LogWarning(string.Format(noReferenceFormat, "Transaction", "postTransaction_CurrencyName"));
		else if (!string.IsNullOrEmpty(postTransaction_CurrencyName.text))
			currencyName = postTransaction_CurrencyName.text;
		
		// Check the currencyAmount value
		if (postTransaction_CurrencyAmount == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Transaction", "postTransaction_CurrencyAmount"));
		else if (!string.IsNullOrEmpty(postTransaction_CurrencyAmount.text))
			currencyAmount = float.Parse(postTransaction_CurrencyAmount.text);
		
		// Check the transactionDescription value
		if (postTransaction_TransactionDescription == null)
			Debug.LogWarning(string.Format(noReferenceFormat, "Transaction", "postTransaction_TransactionDescription"));
		else
			transactionDescription = postTransaction_TransactionDescription.text;
		
		// Call the template method
		TransactionFeatures.Handling_PostTransaction(currencyName, currencyAmount, transactionDescription);
	}
	#endregion
}
