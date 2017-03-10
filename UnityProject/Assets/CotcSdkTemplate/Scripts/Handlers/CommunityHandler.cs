using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to display the CotcSdk's community features' results.
	/// </summary>
	public class CommunityHandler : MonoSingleton<CommunityHandler>
	{
		#region Display
		// Reference to the community panel UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject communityPanel = null;
		[SerializeField] private Text communityPanelTitle = null;
		[SerializeField] private GameObject noFriendText = null;

		// Reference to the community GameObject prefabs and the community items layout
		[SerializeField] private GameObject communityFriendPrefab = null;
		[SerializeField] private GridLayoutGroup communityItemsLayout = null;

		// List of the community GameObjects created on the community panel
		private List<GameObject> communityItems = new List<GameObject>();

		/// <summary>
		/// Hide the community panel at Start.
		/// </summary>
		private void Start()
		{
			ShowCommunityPanel(false);
		}

		/// <summary>
		/// Show or hide the community panel.
		/// </summary>
		/// <param name="show">If the panel should be shown.</param>
		public void ShowCommunityPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			communityPanel.SetActive(show);
		}

		/// <summary>
		/// Fill the community panel with friends (or blacklisted gamers) then show it.
		/// </summary>
		/// <param name="friendsList">List of the friends to display.</param>
		/// <param name="panelTitle">Title of the panel.</param>
		public void FillAndShowCommunityPanel(NonpagedList<GamerInfo> friendsList, string panelTitle = "Friends List")
		{
			// Destroy the previously created community GameObjects if any exist and clear the list
			foreach (GameObject communityItem in communityItems)
				DestroyObject(communityItem);
			
			communityItems.Clear();

			// Set the community panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				communityPanelTitle.text = panelTitle;

			// If there are friends to display, fill the community panel with friend prefabs
			if ((friendsList != null) && (friendsList.Count > 0))
			{
				// Hide the "no friend" text
				noFriendText.SetActive(false);

				foreach (GamerInfo friend in friendsList)
				{
					// Create a community friend GameObject and hook it at the community items layout
					GameObject prefabInstance = Instantiate<GameObject>(communityFriendPrefab);
					prefabInstance.transform.SetParent(communityItemsLayout.transform, false);

					// Fill the newly created GameObject with friend data
					CommunityFriendHandler communityFriendHandler = prefabInstance.GetComponent<CommunityFriendHandler>();
					communityFriendHandler.FillData(friend["profile"]);

					// Add the newly created GameObject to the list
					communityItems.Add(prefabInstance);
				}
			}
			// Else, show the "no friend" text
			else
				noFriendText.SetActive(true);
			
			// Show the community panel
			ShowCommunityPanel(true);
		}
		#endregion

		#region Screen Orientation
		/// <summary>
		/// Adapt the layout display when the current screen orientation changes.
		/// </summary>
		public void OnRectTransformDimensionsChange()
		{
			// If on landscape orientation use 2 columns, else (portrait) use 1 column
			communityItemsLayout.constraintCount = Screen.width > Screen.height ? 2 : 1;
		}
		#endregion
	}
}
