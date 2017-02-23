using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to display the CotcSdk's achievement features' results.
	/// </summary>
	public class AchievementHandler : MonoSingleton<AchievementHandler>
	{
		#region Display
		// Reference to the achievement panel UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject achievementPanel = null;
		[SerializeField] private Text achievementPanelTitle = null;
		[SerializeField] private GameObject noAchievementText = null;

		// Reference to the achievement GameObject prefab and the achievements list scroll view
		[SerializeField] private GameObject achievementPrefab = null;
		[SerializeField] private GridLayoutGroup achievementItemsLayout = null;

		// List of the achievement GameObjects created on the achievement panel
		private List<GameObject> achievementItems = new List<GameObject>();

		/// <summary>
		/// Hide the achievement panel at Start.
		/// </summary>
		private void Start()
		{
			ShowAchievementPanel(false);
		}

		/// <summary>
		/// Show or hide the achievement panel.
		/// </summary>
		/// <param name="show">If the panel should be shown.</param>
		public void ShowAchievementPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			achievementPanel.SetActive(show);
		}

		/// <summary>
		/// Fill the achievement panel with achievements then show it.
		/// </summary>
		/// <param name="achievementsList">List of the achievements to display.</param>
		/// <param name="panelTitle">Title of the panel.</param>
		public void FillAndShowAchievementPanel(Dictionary<string, AchievementDefinition> achievementsList, string panelTitle = "Achievements Progress")
		{
			// Destroy the previously created achievement GameObjects if any exist and clear the list
			foreach (GameObject achievementItem in achievementItems)
				DestroyObject(achievementItem);
			
			achievementItems.Clear();

			// Set the achievement panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				achievementPanelTitle.text = panelTitle;

			// If there are achievements to display, fill the achievement panel with achievement prefabs
			if ((achievementsList != null) && (achievementsList.Count > 0))
			{
				// Hide the "no achievement" text
				noAchievementText.SetActive(false);

				foreach (KeyValuePair<string, AchievementDefinition> achievement in achievementsList)
				{
					// Create an achievement item GameObject and hook it at the achievements scroll view
					GameObject prefabInstance = Instantiate<GameObject>(achievementPrefab);
					prefabInstance.transform.SetParent(achievementItemsLayout.transform, false);

					// Fill the newly created GameObject with achievement data
					AchievementItemHandler achievementItemHandler = prefabInstance.GetComponent<AchievementItemHandler>();
					achievementItemHandler.FillData(achievement.Value);

					// Add the newly created GameObject to the list
					achievementItems.Add(prefabInstance);
				}
			}
			// Else, show the "no achievement" text
			else
				noAchievementText.SetActive(true);
			
			// Show the achievement panel
			ShowAchievementPanel(true);
		}
		#endregion

		#region Screen Orientation
		/// <summary>
		/// Adapt the layout display when the current screen orientation changes.
		/// </summary>
		public void OnRectTransformDimensionsChange()
		{
			// If on landscape orientation use 2 columns, else (portrait) use 1 column
			achievementItemsLayout.constraintCount = Screen.width > Screen.height ? 2 : 1;
		}
		#endregion
	}
}
