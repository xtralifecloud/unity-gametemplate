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
		[SerializeField] private GameObject loadingBlock = null;
		[SerializeField] private Text errorText = null;
		[SerializeField] private GameObject noFriendText = null;
		[SerializeField] private GameObject noUserText = null;

		// Reference to the community GameObject prefabs and the community items layout
		[SerializeField] private GameObject communityFriendPrefab = null;
		[SerializeField] private GameObject communityUserPrefab = null;
		[SerializeField] private GridLayoutGroup communityItemsLayout = null;

		// List of the community GameObjects created on the community panel
		private List<GameObject> communityItems = new List<GameObject>();

		/// <summary>
		/// Hide the community panel at Start.
		/// </summary>
		private void Start()
		{
			HideCommunityPanel();
		}

		/// <summary>
		/// Hide the community panel.
		/// </summary>
		public void HideCommunityPanel()
		{
			// Hide the community panel with its outclickMask and loading block
			outClickMask.SetActive(false);
			communityPanel.SetActive(false);
			ClearCommunityPanel(false);
		}

		/// <summary>
		/// Show the community panel.
		/// </summary>
		/// <param name="panelTitle">Title of the panel to display. (optional)</param>
		public void ShowCommunityPanel(string panelTitle = null)
		{
			// Set the community panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				communityPanelTitle.text = panelTitle;

			// Show the achievement panel with its outclickMask and loading block
			outClickMask.SetActive(true);
			communityPanel.SetActive(true);
			ClearCommunityPanel(true);
		}

		/// <summary>
		/// Clear the community panel container (loading block, texts, previously created items).
		/// </summary>
		/// <param name="showLoadingBlock">If the loading block should be shown.</param>
		public void ClearCommunityPanel(bool showLoadingBlock = false)
		{
			// Show/hide the loading block
			loadingBlock.SetActive(showLoadingBlock);

			// Hide all texts
			errorText.gameObject.SetActive(false);
			noFriendText.SetActive(false);
			noUserText.SetActive(false);

			// Destroy the previously created community GameObjects if any exist and clear the list
			foreach (GameObject communityItem in communityItems)
				DestroyObject(communityItem);

			communityItems.Clear();
		}

		/// <summary>
		/// Fill the community panel with friends (or blacklisted gamers).
		/// </summary>
		/// <param name="friendsList">List of the friends to display.</param>
		public void FillCommunityPanel(NonpagedList<GamerInfo> friendsList)
		{
			// Clear the community panel
			ClearCommunityPanel(false);

			// If there are friends to display, fill the community panel with friend prefabs
			if ((friendsList != null) && (friendsList.Count > 0))
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
			// Else, show the "no friend" text
			else
				noFriendText.SetActive(true);
		}

		/// <summary>
		/// Fill the community panel with users.
		/// </summary>
		/// <param name="usersList">List of the users to display.</param>
		public void FillCommunityPanel(PagedList<UserInfo> usersList)
		{
			// Clear the community panel
			ClearCommunityPanel(false);

			// If there are users to display, fill the community panel with user prefabs
			if ((usersList != null) && (usersList.Count > 0))
				foreach (UserInfo user in usersList)
				{
					// Create a community user GameObject and hook it at the community items layout
					GameObject prefabInstance = Instantiate<GameObject>(communityUserPrefab);
					prefabInstance.transform.SetParent(communityItemsLayout.transform, false);

					// Fill the newly created GameObject with user data
					CommunityUserHandler communityUserHandler = prefabInstance.GetComponent<CommunityUserHandler>();
					communityUserHandler.FillData(user.UserId, user["profile"]);

					// Add the newly created GameObject to the list
					communityItems.Add(prefabInstance);
				}
			// Else, show the "no user" text
			else
				noUserText.SetActive(true);
		}

		/// <summary>
		/// Clear the community panel and show an error message.
		/// </summary>
		/// <param name="errorMessage">Error message to display.</param>
		public void ShowError(string errorMessage)
		{
			// Clear the community panel
			ClearCommunityPanel(false);

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
			communityItemsLayout.constraintCount = Screen.width > Screen.height ? 2 : 1;
		}
		#endregion
	}
}
