#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// The custom editor for SampleScript.
/// </summary>
[CustomEditor(typeof(SampleScript))]
public class SampleScriptEditor : Editor
{
	#region Serialized Object References
	// CotcSdkTemplate's messages logging level
	private SerializedProperty cotcSdkTemplateLogLevel;

	#region Account References
	// ConvertAnonymousToEmail properties references
	private SerializedProperty convertAnonymousToEmail_Email;
	private SerializedProperty convertAnonymousToEmail_Password;

	// ChangeEmailAddress properties references
	private SerializedProperty changeEmailAddress_NewEmailAddress;

	// ChangeEmailPassword properties references
	private SerializedProperty changeEmailPassword_NewPassword;

	// SendLostPasswordEmail properties references
	private SerializedProperty sendLostPasswordEmail_ToEmailAddress;
	private SerializedProperty sendLostPasswordEmail_FromEmailAddress;
	private SerializedProperty sendLostPasswordEmail_EmailTitle;
	private SerializedProperty sendLostPasswordEmail_EmailBody;
	#endregion

	#region Community References
	// FindGamers properties references
	private SerializedProperty findGamers_MatchPattern;
	private SerializedProperty findGamers_UsersPerPage;

	// SendEventToGamer properties references
	private SerializedProperty sendEventToGamer_GamerID;
	private SerializedProperty sendEventToGamer_EventData;
	private SerializedProperty sendEventToGamer_Notification;

	// SendMessageToGamer properties references
	private SerializedProperty sendMessageToGamer_GamerID;
	private SerializedProperty sendMessageToGamer_Message;

	// SetRelationshipWithGamer properties references
	private SerializedProperty setRelationshipWithGamer_GamerID;
	private SerializedProperty setRelationshipWithGamer_Notification;
	private SerializedProperty setRelationshipWithGamer_Relationship;
	#endregion

	#region Game VFS References
	// DisplayGameKey properties references
	private SerializedProperty displayGameKey_Key;
	#endregion

	#region Gamer VFS References
	// DisplayGamerKey properties references
	private SerializedProperty displayGamerKey_Key;

	// SetGamerKey properties references
	private SerializedProperty setGamerKey_Key;
	private SerializedProperty setGamerKey_Value;
	private SerializedProperty setGamerKey_Type;

	// DeleteGamerKey properties references
	private SerializedProperty deleteGamerKey_Key;
	#endregion

	#region Godfather References
	// UseReferralCode properties references
	private SerializedProperty useReferralCode_ReferralCode;
	#endregion

	#region Leaderboard References
	// DisplayAllHighScores properties references
	private SerializedProperty displayAllHighScores_BoardName;
	private SerializedProperty displayAllHighScores_ScoresPerPage;
	private SerializedProperty displayAllHighScores_CenteredBoard;

	// DisplayFriendsHighScores properties references
	private SerializedProperty displayFriendsHighScores_BoardName;

	// PostScore properties references
	private SerializedProperty postScore_BoardName;
	private SerializedProperty postScore_ScoreValue;
	private SerializedProperty postScore_ScoreDescription;
	private SerializedProperty postScore_ForceSave;
	#endregion

	#region Login References
	// LoginWithCredentials properties references
	private SerializedProperty loginWithCredentials_GamerID;
	private SerializedProperty loginWithCredentials_GamerSecret;

	// LoginWithEmail properties references
	private SerializedProperty loginWithEmail_Email;
	private SerializedProperty loginWithEmail_Password;

	// LoginWithShortCode properties references
	private SerializedProperty loginWithShortCode_ShortCode;
	#endregion

	#region Transaction References
	// DisplayAllCurrenciesHistory properties references
	private SerializedProperty displayAllCurrenciesHistory_TransactionsPerPage;

	// DisplayCurrencyHistory properties references
	private SerializedProperty displayCurrencyHistory_CurrencyName;
	private SerializedProperty displayCurrencyHistory_TransactionsPerPage;

	// PostTransaction properties references
	private SerializedProperty postTransaction_CurrencyName;
	private SerializedProperty postTransaction_CurrencyAmount;
	private SerializedProperty postTransaction_TransactionDescription;
	#endregion

	/// <summary>
	/// Find the properties references on the serialized object when this script is enabled.
	/// </summary>
	private void OnEnable()
	{
		// Find cotcSdkTemplateLogLevel property references on the serialized object
		cotcSdkTemplateLogLevel = serializedObject.FindProperty("cotcSdkTemplateLogLevel");

		#region Account Find
		// Find ConvertAnonymousToEmail properties references on the serialized object
		convertAnonymousToEmail_Email = serializedObject.FindProperty("convertAnonymousToEmail_Email");
		convertAnonymousToEmail_Password = serializedObject.FindProperty("convertAnonymousToEmail_Password");

		// Find ChangeEmailAddress properties references on the serialized object
		changeEmailAddress_NewEmailAddress = serializedObject.FindProperty("changeEmailAddress_NewEmailAddress");

		// Find ChangeEmailPassword properties references on the serialized object
		changeEmailPassword_NewPassword = serializedObject.FindProperty("changeEmailPassword_NewPassword");

		// Find SendLostPasswordEmail properties references on the serialized object
		sendLostPasswordEmail_ToEmailAddress = serializedObject.FindProperty("sendLostPasswordEmail_ToEmailAddress");
		sendLostPasswordEmail_FromEmailAddress = serializedObject.FindProperty("sendLostPasswordEmail_FromEmailAddress");
		sendLostPasswordEmail_EmailTitle = serializedObject.FindProperty("sendLostPasswordEmail_EmailTitle");
		sendLostPasswordEmail_EmailBody = serializedObject.FindProperty("sendLostPasswordEmail_EmailBody");
		#endregion

		#region Community Find
		// Find FindGamers properties references on the serialized object
		findGamers_MatchPattern = serializedObject.FindProperty("findGamers_MatchPattern");
		findGamers_UsersPerPage = serializedObject.FindProperty("findGamers_UsersPerPage");

		// Find SendEventToGamer properties references on the serialized object
		sendEventToGamer_GamerID = serializedObject.FindProperty("sendEventToGamer_GamerID");
		sendEventToGamer_EventData = serializedObject.FindProperty("sendEventToGamer_EventData");
		sendEventToGamer_Notification = serializedObject.FindProperty("sendEventToGamer_Notification");

		// Find SendMessageToGamer properties references on the serialized object
		sendMessageToGamer_GamerID = serializedObject.FindProperty("sendMessageToGamer_GamerID");
		sendMessageToGamer_Message = serializedObject.FindProperty("sendMessageToGamer_Message");

		// Find SetRelationshipWithGamer properties references on the serialized object
		setRelationshipWithGamer_GamerID = serializedObject.FindProperty("setRelationshipWithGamer_GamerID");
		setRelationshipWithGamer_Notification = serializedObject.FindProperty("setRelationshipWithGamer_Notification");
		setRelationshipWithGamer_Relationship = serializedObject.FindProperty("setRelationshipWithGamer_Relationship");
		#endregion

		#region Game VFS Find
		// Find DisplayGameKey properties references on the serialized object
		displayGameKey_Key = serializedObject.FindProperty("displayGameKey_Key");
		#endregion

		#region Gamer VFS Find
		// Find DisplayGamerKey properties references on the serialized object
		displayGamerKey_Key = serializedObject.FindProperty("displayGamerKey_Key");

		// Find SetGamerKey properties references on the serialized object
		setGamerKey_Key = serializedObject.FindProperty("setGamerKey_Key");
		setGamerKey_Value = serializedObject.FindProperty("setGamerKey_Value");
		setGamerKey_Type = serializedObject.FindProperty("setGamerKey_Type");

		// Find DeleteGamerKey properties references on the serialized object
		deleteGamerKey_Key = serializedObject.FindProperty("deleteGamerKey_Key");
		#endregion

		#region Godfather
		// Find UseReferralCode properties references on the serialized object
		useReferralCode_ReferralCode = serializedObject.FindProperty("useReferralCode_ReferralCode");
		#endregion

		#region Leaderboard Find
		// Find DisplayAllHighScores properties references on the serialized object
		displayAllHighScores_BoardName = serializedObject.FindProperty("displayAllHighScores_BoardName");
		displayAllHighScores_ScoresPerPage = serializedObject.FindProperty("displayAllHighScores_ScoresPerPage");
		displayAllHighScores_CenteredBoard = serializedObject.FindProperty("displayAllHighScores_CenteredBoard");

		// Find DisplayFriendsHighScores properties references on the serialized object
		displayFriendsHighScores_BoardName = serializedObject.FindProperty("displayFriendsHighScores_BoardName");

		// Find PostScore properties references on the serialized object
		postScore_BoardName = serializedObject.FindProperty("postScore_BoardName");
		postScore_ScoreValue = serializedObject.FindProperty("postScore_ScoreValue");
		postScore_ScoreDescription = serializedObject.FindProperty("postScore_ScoreDescription");
		postScore_ForceSave = serializedObject.FindProperty("postScore_ForceSave");
		#endregion

		#region Login Find
		// Find LoginWithCredentials properties references on the serialized object
		loginWithCredentials_GamerID = serializedObject.FindProperty("loginWithCredentials_GamerID");
		loginWithCredentials_GamerSecret = serializedObject.FindProperty("loginWithCredentials_GamerSecret");

		// Find LoginWithEmail properties references on the serialized object
		loginWithEmail_Email = serializedObject.FindProperty("loginWithEmail_Email");
		loginWithEmail_Password = serializedObject.FindProperty("loginWithEmail_Password");

		// Find LoginWithShortCode properties references on the serialized object
		loginWithShortCode_ShortCode = serializedObject.FindProperty("loginWithShortCode_ShortCode");
		#endregion

		#region Transaction Find
		// Find DisplayAllCurrenciesHistory properties references on the serialized object
		displayAllCurrenciesHistory_TransactionsPerPage = serializedObject.FindProperty("displayAllCurrenciesHistory_TransactionsPerPage");

		// Find DisplayCurrencyHistory properties references on the serialized object
		displayCurrencyHistory_CurrencyName = serializedObject.FindProperty("displayCurrencyHistory_CurrencyName");
		displayCurrencyHistory_TransactionsPerPage = serializedObject.FindProperty("displayCurrencyHistory_TransactionsPerPage");

		// Find PostTransaction properties references on the serialized object
		postTransaction_CurrencyName = serializedObject.FindProperty("postTransaction_CurrencyName");
		postTransaction_CurrencyAmount = serializedObject.FindProperty("postTransaction_CurrencyAmount");
		postTransaction_TransactionDescription = serializedObject.FindProperty("postTransaction_TransactionDescription");
		#endregion
	}
	#endregion

	#region Inspector GUI
	// GUI options
	private const bool foldoutLabelToggle = true;
	private static GUIStyle foldoutStyle = null;
	private const float verticalSpaces = 5f;

	// The foldouts states
	private bool accountFoldoutState = true;
	private bool communityFoldoutState = true;
	private bool gameVFSFoldoutState = true;
	private bool gamerVFSFoldoutState = true;
	private bool godfatherFoldoutState = true;
	private bool leaderboardFoldoutState = true;
	private bool loginFoldoutState = true;
	private bool transactionFoldoutState = true;

	/// <summary>
	/// Draw the custom inspector GUI in place of the default one.
	/// </summary>
	public override void OnInspectorGUI()
	{
		// Edit foldouts style only once
		if (foldoutStyle == null)
		{
			foldoutStyle = new GUIStyle(EditorStyles.foldout);
			foldoutStyle.fontStyle = FontStyle.Bold;
			foldoutStyle.fontSize = 13;
		}

		// Get the current value of the serialized properties
		serializedObject.Update();

		// Display the SampleScript script field
		EditorGUI.BeginDisabledGroup(true);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
		EditorGUI.EndDisabledGroup();

		// Display the CotcSdkTemplate's messages logging level
		EditorGUILayout.PropertyField(cotcSdkTemplateLogLevel, new GUIContent("CotcSdkTemplate Logging Level"));

		#region Account Foldout
		// Open / Close the Account foldout
		GUILayout.Space(verticalSpaces);
		accountFoldoutState = EditorGUILayout.Foldout(accountFoldoutState, "Account", foldoutLabelToggle, foldoutStyle);

		if (accountFoldoutState)
		{
			// Show ConvertAnonymousToEmail properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Convert Anonymous To Email", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(convertAnonymousToEmail_Email, new GUIContent("  > Email"));
			EditorGUILayout.PropertyField(convertAnonymousToEmail_Password, new GUIContent("  > Password"));

			// Show ChangeEmailAddress properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Change Email Address", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(changeEmailAddress_NewEmailAddress, new GUIContent("  > New Email Address"));

			// Show ChangeEmailPassword properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Change Email Password", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(changeEmailPassword_NewPassword, new GUIContent("  > New Password"));

			// Show SendLostPasswordEmail properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Send Lost Password Email", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(sendLostPasswordEmail_ToEmailAddress, new GUIContent("  > To Email Address"));
			EditorGUILayout.PropertyField(sendLostPasswordEmail_FromEmailAddress, new GUIContent("  > From Email Address"));
			EditorGUILayout.PropertyField(sendLostPasswordEmail_EmailTitle, new GUIContent("  > Email Title"));
			EditorGUILayout.PropertyField(sendLostPasswordEmail_EmailBody, new GUIContent("  > Email Body"));
		}
		#endregion

		#region Community Foldout
		// Open / Close the Community foldout
		GUILayout.Space(verticalSpaces);
		communityFoldoutState = EditorGUILayout.Foldout(communityFoldoutState, "Community", foldoutLabelToggle, foldoutStyle);

		if (communityFoldoutState)
		{
			// Show FindGamers properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Find Gamers", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(findGamers_MatchPattern, new GUIContent("  > Match Pattern"));
			EditorGUILayout.PropertyField(findGamers_UsersPerPage, new GUIContent("  > Users Per Page"));

			// Show SendEventToGamer properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Send Event To Gamer", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(sendEventToGamer_GamerID, new GUIContent("  > Gamer ID"));
			EditorGUILayout.PropertyField(sendEventToGamer_EventData, new GUIContent("  > Event Data"));
			EditorGUILayout.PropertyField(sendEventToGamer_Notification, new GUIContent("  > Notification"));

			// Show SendMessageToGamer properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Send Message To Gamer", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(sendMessageToGamer_GamerID, new GUIContent("  > Gamer ID"));
			EditorGUILayout.PropertyField(sendMessageToGamer_Message, new GUIContent("  > Message"));

			// Show SetRelationshipWithGamer properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Set Relationship With Gamer", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(setRelationshipWithGamer_GamerID, new GUIContent("  > Gamer ID"));
			EditorGUILayout.PropertyField(setRelationshipWithGamer_Notification, new GUIContent("  > Notification"));
			EditorGUILayout.PropertyField(setRelationshipWithGamer_Relationship, new GUIContent("  > Relationship"));
		}
		#endregion

		#region Game VFS Foldout
		// Open / Close the Game VFS foldout
		GUILayout.Space(verticalSpaces);
		gameVFSFoldoutState = EditorGUILayout.Foldout(gameVFSFoldoutState, "Game VFS", foldoutLabelToggle, foldoutStyle);

		if (gameVFSFoldoutState)
		{
			// Show DisplayGameKey properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Display Game Key", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(displayGameKey_Key, new GUIContent("  > Key"));
		}
		#endregion

		#region Gamer VFS Foldout
		// Open / Close the Gamer VFS foldout
		GUILayout.Space(verticalSpaces);
		gamerVFSFoldoutState = EditorGUILayout.Foldout(gamerVFSFoldoutState, "Gamer VFS", foldoutLabelToggle, foldoutStyle);

		if (gamerVFSFoldoutState)
		{
			// Show DisplayGamerKey properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Display Gamer Key", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(displayGamerKey_Key, new GUIContent("  > Key"));

			// Show SetGamerKey properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Set Gamer Key", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(setGamerKey_Key, new GUIContent("  > Key"));
			EditorGUILayout.PropertyField(setGamerKey_Value, new GUIContent("  > Value"));
			EditorGUILayout.PropertyField(setGamerKey_Type, new GUIContent("  > Type"));

			// Show DeleteGamerKey properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Delete Gamer Key", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(deleteGamerKey_Key, new GUIContent("  > Key"));
		}
		#endregion

		#region Godfather Foldout
		// Open / Close the Godfather foldout
		GUILayout.Space(verticalSpaces);
		godfatherFoldoutState = EditorGUILayout.Foldout(godfatherFoldoutState, "Godfather", foldoutLabelToggle, foldoutStyle);

		if (godfatherFoldoutState)
		{
			// Show UseReferralCode properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Use Referral Code", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(useReferralCode_ReferralCode, new GUIContent("  > Referral Code"));
		}
		#endregion

		#region Leaderboard Foldout
		// Open / Close the Leaderboard foldout
		GUILayout.Space(verticalSpaces);
		leaderboardFoldoutState = EditorGUILayout.Foldout(leaderboardFoldoutState, "Leaderboard", foldoutLabelToggle, foldoutStyle);

		if (leaderboardFoldoutState)
		{
			// Show DisplayAllHighScores properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Display All High Scores", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(displayAllHighScores_BoardName, new GUIContent("  > Board Name"));
			EditorGUILayout.PropertyField(displayAllHighScores_ScoresPerPage, new GUIContent("  > Scores Per Page"));
			EditorGUILayout.PropertyField(displayAllHighScores_CenteredBoard, new GUIContent("  > Centered Board"));

			// Show DisplayFriendsHighScores properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Display Friends High Scores", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(displayFriendsHighScores_BoardName, new GUIContent("  > Board Name"));

			// Show PostScore properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Post Score", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(postScore_BoardName, new GUIContent("  > Board Name"));
			EditorGUILayout.PropertyField(postScore_ScoreValue, new GUIContent("  > Score Value"));
			EditorGUILayout.PropertyField(postScore_ScoreDescription, new GUIContent("  > Score Description"));
			EditorGUILayout.PropertyField(postScore_ForceSave, new GUIContent("  > Force Save"));
		}
		#endregion

		#region Login Foldout
		// Open / Close the Login foldout
		GUILayout.Space(verticalSpaces);
		loginFoldoutState = EditorGUILayout.Foldout(loginFoldoutState, "Login", foldoutLabelToggle, foldoutStyle);

		if (loginFoldoutState)
		{
			// Show LoginWithCredentials properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Login With Credentials", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(loginWithCredentials_GamerID, new GUIContent("  > Gamer ID"));
			EditorGUILayout.PropertyField(loginWithCredentials_GamerSecret, new GUIContent("  > Gamer Secret"));

			// Show LoginWithEmail properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Login With Email", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(loginWithEmail_Email, new GUIContent("  > Email"));
			EditorGUILayout.PropertyField(loginWithEmail_Password, new GUIContent("  > Password"));

			// Show LoginWithShortCode properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Login With Short Code", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(loginWithShortCode_ShortCode, new GUIContent("  > Short Code"));
		}
		#endregion

		#region Transaction Foldout
		// Open / Close the Transaction foldout
		GUILayout.Space(verticalSpaces);
		transactionFoldoutState = EditorGUILayout.Foldout(transactionFoldoutState, "Transaction", foldoutLabelToggle, foldoutStyle);

		if (transactionFoldoutState)
		{
			// Show DisplayAllCurrenciesHistory properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Display All Currencies History", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(displayAllCurrenciesHistory_TransactionsPerPage, new GUIContent("  > Transactions Per Page"));

			// Show DisplayCurrencyHistory properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Display Currency History", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(displayCurrencyHistory_CurrencyName, new GUIContent("  > Currency Name"));
			EditorGUILayout.PropertyField(displayCurrencyHistory_TransactionsPerPage, new GUIContent("  > Transactions Per Page"));

			// Show PostTransaction properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Post Transaction", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(postTransaction_CurrencyName, new GUIContent("  > Currency Name"));
			EditorGUILayout.PropertyField(postTransaction_CurrencyAmount, new GUIContent("  > Currency Amount"));
			EditorGUILayout.PropertyField(postTransaction_TransactionDescription, new GUIContent("  > Transaction Description"));
		}
		#endregion

		GUILayout.Space(verticalSpaces);

		// Update the edited serialized properties values on the object
		serializedObject.ApplyModifiedProperties();
	}
	#endregion
}
#endif
