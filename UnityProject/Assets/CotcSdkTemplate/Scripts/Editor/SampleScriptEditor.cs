#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SampleScript))]
public class SampleScriptEditor : Editor
{
	#region Serialized Object References
	// DisplayAllHighScores properties references
	private SerializedProperty displayAllHighScores_BoardName;
	private SerializedProperty displayAllHighScores_ScoresPerPage;

	// PostScore properties references
	private SerializedProperty postScore_BoardName;
	private SerializedProperty postScore_ScoreValue;
	private SerializedProperty postScore_ScoreDescription;

	// PostTransaction properties references
	private SerializedProperty postTransaction_CurrencyName;
	private SerializedProperty postTransaction_CurrencyAmount;
	private SerializedProperty postTransaction_TransactionDescription;

	private void OnEnable()
	{
		// Find DisplayAllHighScores properties references on the serialized object
		displayAllHighScores_BoardName = serializedObject.FindProperty("displayAllHighScores_BoardName");
		displayAllHighScores_ScoresPerPage = serializedObject.FindProperty("displayAllHighScores_ScoresPerPage");

		// Find PostScore properties references on the serialized object
		postScore_BoardName = serializedObject.FindProperty("postScore_BoardName");
		postScore_ScoreValue = serializedObject.FindProperty("postScore_ScoreValue");
		postScore_ScoreDescription = serializedObject.FindProperty("postScore_ScoreDescription");

		// Find PostTransaction properties references on the serialized object
		postTransaction_CurrencyName = serializedObject.FindProperty("postTransaction_CurrencyName");
		postTransaction_CurrencyAmount = serializedObject.FindProperty("postTransaction_CurrencyAmount");
		postTransaction_TransactionDescription = serializedObject.FindProperty("postTransaction_TransactionDescription");
	}
	#endregion

	#region Inspector GUI
	// GUI options
	private const bool foldoutLabelToggle = true;
	private static GUIStyle foldoutStyle = null;
	private const float verticalSpaces = 5f;

	// The foldouts states
	private bool leaderboardFoldoutState = true;
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

		// Open / Close the Leaderboard foldout
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

		// Open / Close the Transaction foldout
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

		GUILayout.Space(verticalSpaces);

		// Update the edited serialized properties values on the object
		serializedObject.ApplyModifiedProperties();
	}
	#endregion
}
#endif
