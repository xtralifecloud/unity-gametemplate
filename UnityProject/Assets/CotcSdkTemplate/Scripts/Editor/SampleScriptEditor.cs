#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SampleScript))]
public class SampleScriptEditor : Editor
{
	#region Serialized Object References
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
	private SerializedProperty setGamerKey_JsonTypeSelected;
	private SerializedProperty setGamerKey_StringTypeSelected;
	private SerializedProperty setGamerKey_DoubleTypeSelected;
	private SerializedProperty setGamerKey_IntTypeSelected;
	private SerializedProperty setGamerKey_BoolTypeSelected;

	// DeleteGamerKey properties references
	private SerializedProperty deleteGamerKey_Key;
	#endregion

	#region Leaderboard References
	// DisplayAllHighScores properties references
	private SerializedProperty displayAllHighScores_BoardName;
	private SerializedProperty displayAllHighScores_ScoresPerPage;

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
	// PostTransaction properties references
	private SerializedProperty postTransaction_CurrencyName;
	private SerializedProperty postTransaction_CurrencyAmount;
	private SerializedProperty postTransaction_TransactionDescription;
	#endregion

	private void OnEnable()
	{
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
		setGamerKey_JsonTypeSelected = serializedObject.FindProperty("setGamerKey_JsonTypeSelected");
		setGamerKey_StringTypeSelected = serializedObject.FindProperty("setGamerKey_StringTypeSelected");
		setGamerKey_DoubleTypeSelected = serializedObject.FindProperty("setGamerKey_DoubleTypeSelected");
		setGamerKey_IntTypeSelected = serializedObject.FindProperty("setGamerKey_IntTypeSelected");
		setGamerKey_BoolTypeSelected = serializedObject.FindProperty("setGamerKey_BoolTypeSelected");

		// Find DeleteGamerKey properties references on the serialized object
		deleteGamerKey_Key = serializedObject.FindProperty("deleteGamerKey_Key");
		#endregion

		#region Leaderboard Find
		// Find DisplayAllHighScores properties references on the serialized object
		displayAllHighScores_BoardName = serializedObject.FindProperty("displayAllHighScores_BoardName");
		displayAllHighScores_ScoresPerPage = serializedObject.FindProperty("displayAllHighScores_ScoresPerPage");

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

	// Draw the custom inspector GUI
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

		#region Game VFS Foldout
		// Open / Close the foldout
		GUILayout.Space(verticalSpaces);
		gameVFSFoldoutState = EditorGUILayout.Foldout(gameVFSFoldoutState, "Game VFS", foldoutLabelToggle, foldoutStyle);

		if (gameVFSFoldoutState)
		{
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Display Game Key", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(displayGameKey_Key, new GUIContent("  > Key"));
		}
		#endregion

		#region Gamer VFS Foldout
		// Open / Close the foldout
		GUILayout.Space(verticalSpaces);
		gamerVFSFoldoutState = EditorGUILayout.Foldout(gamerVFSFoldoutState, "Gamer VFS", foldoutLabelToggle, foldoutStyle);

		if (gamerVFSFoldoutState)
		{
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Display Gamer Key", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(displayGamerKey_Key, new GUIContent("  > Key"));

			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Set Gamer Key", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(setGamerKey_Key, new GUIContent("  > Key"));
			EditorGUILayout.PropertyField(setGamerKey_Value, new GUIContent("  > Value"));
			EditorGUILayout.PropertyField(setGamerKey_JsonTypeSelected, new GUIContent("  > Json Type Selected"));
			EditorGUILayout.PropertyField(setGamerKey_StringTypeSelected, new GUIContent("  > String Type Selected"));
			EditorGUILayout.PropertyField(setGamerKey_DoubleTypeSelected, new GUIContent("  > Double Type Selected"));
			EditorGUILayout.PropertyField(setGamerKey_IntTypeSelected, new GUIContent("  > Int Type Selected"));
			EditorGUILayout.PropertyField(setGamerKey_BoolTypeSelected, new GUIContent("  > Bool Type Selected"));

			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Delete Gamer Key", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(deleteGamerKey_Key, new GUIContent("  > Key"));
		}
		#endregion

		#region Leaderboard Foldout
		// Open / Close the foldout
		GUILayout.Space(verticalSpaces);
		leaderboardFoldoutState = EditorGUILayout.Foldout(leaderboardFoldoutState, "Leaderboard", foldoutLabelToggle, foldoutStyle);

		if (leaderboardFoldoutState)
		{
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Display All High Scores", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(displayAllHighScores_BoardName, new GUIContent("  > Board Name"));
			EditorGUILayout.PropertyField(displayAllHighScores_ScoresPerPage, new GUIContent("  > Scores Per Page"));

			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Post Score", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(postScore_BoardName, new GUIContent("  > Board Name"));
			EditorGUILayout.PropertyField(postScore_ScoreValue, new GUIContent("  > Score Value"));
			EditorGUILayout.PropertyField(postScore_ScoreDescription, new GUIContent("  > Score Description"));
		}
		#endregion

		#region Login Foldout
		// Open / Close the foldout
		GUILayout.Space(verticalSpaces);
		loginFoldoutState = EditorGUILayout.Foldout(loginFoldoutState, "Login", foldoutLabelToggle, foldoutStyle);

		if (loginFoldoutState)
		{
			GUILayout.Space(verticalSpaces);
			EditorGUILayout.LabelField("  Login With Credentials", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(loginWithCredentials_GamerID, new GUIContent("  > Gamer ID"));
			EditorGUILayout.PropertyField(loginWithCredentials_GamerSecret, new GUIContent("  > Gamer Secret"));
		}
		#endregion

		#region Transaction Foldout
		// Open / Close the foldout
		GUILayout.Space(verticalSpaces);
		transactionFoldoutState = EditorGUILayout.Foldout(transactionFoldoutState, "Transaction", foldoutLabelToggle, foldoutStyle);

		if (transactionFoldoutState)
		{
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
