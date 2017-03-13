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
		[SerializeField] private GameObject loadingBlock = null;
		[SerializeField] private Text errorText = null;
		[SerializeField] private GameObject noAchievementText = null;

		// Reference to the achievement GameObject prefabs and the achievement items layout
		[SerializeField] private GameObject achievementItemPrefab = null;
		[SerializeField] private GridLayoutGroup achievementItemsLayout = null;

		// List of the achievement GameObjects created on the achievement panel
		private List<GameObject> achievementItems = new List<GameObject>();

		/// <summary>
		/// Hide the achievement panel at Start.
		/// </summary>
		private void Start()
		{
			HideAchievementPanel();
		}

		/// <summary>
		/// Hide the achievement panel.
		/// </summary>
		public void HideAchievementPanel()
		{
			// Hide the achievement panel with its outclickMask and loading block
			outClickMask.SetActive(false);
			achievementPanel.SetActive(false);
			ClearAchievementPanel(false);
		}

		/// <summary>
		/// Show the achievement panel.
		/// </summary>
		/// <param name="panelTitle">Title of the panel to display. (optional)</param>
		public void ShowAchievementPanel(string panelTitle = null)
		{
			// Set the achievement panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				achievementPanelTitle.text = panelTitle;
			
			// Show the achievement panel with its outclickMask and loading block
			outClickMask.SetActive(true);
			achievementPanel.SetActive(true);
			ClearAchievementPanel(true);
		}

		/// <summary>
		/// Clear the achievement panel container (loading block, texts, previously created items).
		/// </summary>
		/// <param name="showLoadingBlock">If the loading block should be shown.</param>
		public void ClearAchievementPanel(bool showLoadingBlock = false)
		{
			// Show/hide the loading block
			loadingBlock.SetActive(showLoadingBlock);

			// Hide all texts
			errorText.gameObject.SetActive(false);
			noAchievementText.SetActive(false);

			// Destroy the previously created achievement GameObjects if any exist and clear the list
			foreach (GameObject achievementItem in achievementItems)
				DestroyObject(achievementItem);

			achievementItems.Clear();
		}

		/// <summary>
		/// Fill the achievement panel with achievements.
		/// </summary>
		/// <param name="achievementsList">List of the achievements to display.</param>
		public void FillAchievementPanel(Dictionary<string, AchievementDefinition> achievementsList)
		{
			// Clear the achievement panel
			ClearAchievementPanel(false);

			// If there are achievements to display, fill the achievement panel with achievement prefabs
			if ((achievementsList != null) && (achievementsList.Count > 0))
				foreach (KeyValuePair<string, AchievementDefinition> achievement in achievementsList)
				{
					// Create an achievement item GameObject and hook it at the achievement items layout
					GameObject prefabInstance = Instantiate<GameObject>(achievementItemPrefab);
					prefabInstance.transform.SetParent(achievementItemsLayout.transform, false);

					// Fill the newly created GameObject with achievement data
					AchievementItemHandler achievementItemHandler = prefabInstance.GetComponent<AchievementItemHandler>();
					achievementItemHandler.FillData(achievement.Value);

					// Add the newly created GameObject to the list
					achievementItems.Add(prefabInstance);
				}
			// Else, show the "no achievement" text
			else
				noAchievementText.SetActive(true);
		}

		/// <summary>
		/// Clear the achievement panel and show an error message.
		/// </summary>
		/// <param name="errorMessage">Error message to display.</param>
		public void ShowError(string errorMessage)
		{
			// Clear the achievement panel
			ClearAchievementPanel(false);

			// Set and show the error message
			errorText.text = string.Format("\n{0}\n", errorMessage);
			errorText.gameObject.SetActive(true);
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
