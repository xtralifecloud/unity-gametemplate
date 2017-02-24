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
	// The project's MonoScript reference
	MonoScript script = null;

	// CotcSdkTemplate's messages logging level
	private SerializedProperty cotcSdkTemplateLogLevel;

	#region Gamer VFS References
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

	#region Leaderboard References
	// DisplayAllHighScores properties references
	private SerializedProperty displayAllHighScores_BoardName;
	private SerializedProperty displayAllHighScores_ScoresPerPage;
	private SerializedProperty displayAllHighScores_CenteredBoard;

	// PostScore properties references
	private SerializedProperty postScore_BoardName;
	private SerializedProperty postScore_ScoreValue;
	private SerializedProperty postScore_ScoreDescription;
	#endregion

	#region Login References
	// LoginWithCredentials properties references
	private SerializedProperty loginWithCredentials_GamerID;
	private SerializedProperty loginWithCredentials_GamerSecret;
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
		// Get the project's MonoScript reference
		script = MonoScript.FromMonoBehaviour((SampleScript)target);

		// Find cotcSdkTemplateLogLevel property references on the serialized object
		cotcSdkTemplateLogLevel = serializedObject.FindProperty("cotcSdkTemplateLogLevel");

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

		#region Leaderboard Find
		// Find DisplayAllHighScores properties references on the serialized object
		displayAllHighScores_BoardName = serializedObject.FindProperty("displayAllHighScores_BoardName");
		displayAllHighScores_ScoresPerPage = serializedObject.FindProperty("displayAllHighScores_ScoresPerPage");
		displayAllHighScores_CenteredBoard = serializedObject.FindProperty("displayAllHighScores_CenteredBoard");

		// Find PostScore properties references on the serialized object
		postScore_BoardName = serializedObject.FindProperty("postScore_BoardName");
		postScore_ScoreValue = serializedObject.FindProperty("postScore_ScoreValue");
		postScore_ScoreDescription = serializedObject.FindProperty("postScore_ScoreDescription");
		#endregion

		#region Login Find
		// Find LoginWithCredentials properties references on the serialized object
		loginWithCredentials_GamerID = serializedObject.FindProperty("loginWithCredentials_GamerID");
		loginWithCredentials_GamerSecret = serializedObject.FindProperty("loginWithCredentials_GamerSecret");
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
	private bool gameVFSFoldoutState = true;
	private bool gamerVFSFoldoutState = true;
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
		script = EditorGUILayout.ObjectField("Script", script, typeof(MonoScript), false) as MonoScript;

		// Display the CotcSdkTemplate's messages logging level
		EditorGUILayout.PropertyField(cotcSdkTemplateLogLevel, new GUIContent("CotcSdkTemplate Logging Level"));

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

			// Show PostScore properties references on the inspector
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Post Score", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(postScore_BoardName, new GUIContent("  > Board Name"));
			EditorGUILayout.PropertyField(postScore_ScoreValue, new GUIContent("  > Score Value"));
			EditorGUILayout.PropertyField(postScore_ScoreDescription, new GUIContent("  > Score Description"));
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
