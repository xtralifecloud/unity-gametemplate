﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class AchievementHandler : MonoBehaviour
	{
		#region Instance Registering
		// Register this MonoBehaviour's instance at Awake to be sure it's done before any Start executes
		private void Awake()
		{
			MonoSingletons.Register<AchievementHandler>(this);
		}
		#endregion

		#region Display
		// Reference to the achievement panel UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject achievementPanel = null;
		[SerializeField] private GameObject noAchievementText = null;

		// Reference to the achievement GameObject prefab and the achievements list scroll view
		[SerializeField] private GameObject achievementPrefab = null;
		[SerializeField] private Transform achievementScrollViewContent = null;

		// List of the achievement GameObjects created on the achievement panel
		private List<GameObject> achievementItems = new List<GameObject>();

		// Hide the achievement panel at Start
		private void Start()
		{
			ShowAchievementPanel(false);
		}

		// Show or hide the achievement panel
		public void ShowAchievementPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			achievementPanel.SetActive(show);
		}

		// Fill the achievement panel with achievements then show it
		public void FillAndShowAchievementPanel(Dictionary<string, AchievementDefinition> achievementsList)
		{
			// Destroy the previously created achievement GameObjects if any exist and clear the list
			foreach (GameObject achievementItem in achievementItems)
				DestroyObject(achievementItem);

			achievementItems.Clear();

			// If there are achievements to display, fill the achievement panel with achievement prefabs
			if ((achievementsList != null) && (achievementsList.Count > 0))
			{
				// Hide the "no achievement" text
				noAchievementText.SetActive(false);

				foreach (KeyValuePair<string, AchievementDefinition> achievement in achievementsList)
				{
					// Create an achievement item GameObject and hook it at the achievements scroll view
					GameObject prefabInstance = Instantiate<GameObject>(achievementPrefab);
					prefabInstance.transform.SetParent(achievementScrollViewContent, false);

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
	}
}
