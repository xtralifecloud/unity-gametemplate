using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to display the CotcSdk's game and gamer VFS (Virtual File System) features' results.
	/// </summary>
	public class VFSHandler : MonoSingleton<VFSHandler>
	{
		#region Display
		// Reference to the VFS panel UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject VFSPanel = null;
		[SerializeField] private Text VFSPanelTitle = null;
		[SerializeField] private GameObject loadingBlock = null;
		[SerializeField] private Text errorText = null;
		[SerializeField] private GameObject noKeyText = null;

		// Reference to the VFS GameObject prefabs and the VFS items layout
		[SerializeField] private GameObject VFSKeyPrefab = null;
		[SerializeField] private VerticalLayoutGroup VFSItemsLayout = null;

		// List of the VFS GameObjects created on the VFS panel
		private List<GameObject> VFSItems = new List<GameObject>();

		/// <summary>
		/// Hide the VFS panel at Start.
		/// </summary>
		private void Start()
		{
			HideVFSPanel();
		}

		/// <summary>
		/// Hide the VFS panel.
		/// </summary>
		public void HideVFSPanel()
		{
			// Hide the VFS panel with its outclickMask and loading block
			outClickMask.SetActive(false);
			VFSPanel.SetActive(false);
			ClearVFSPanel(false);
		}

		/// <summary>
		/// Show the VFS panel.
		/// </summary>
		/// <param name="panelTitle">Title of the panel to display. (optional)</param>
		public void ShowVFSPanel(string panelTitle = null)
		{
			// Set the VFS panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				VFSPanelTitle.text = panelTitle;
			
			// Show the VFS panel with its outclickMask and loading block
			outClickMask.SetActive(true);
			VFSPanel.SetActive(true);
			ClearVFSPanel(true);
		}

		/// <summary>
		/// Clear the VFS panel container (loading block, texts, previously created items).
		/// </summary>
		/// <param name="showLoadingBlock">If the loading block should be shown.</param>
		public void ClearVFSPanel(bool showLoadingBlock = false)
		{
			// Show/hide the loading block
			loadingBlock.SetActive(showLoadingBlock);

			// Hide all texts
			errorText.gameObject.SetActive(false);
			noKeyText.SetActive(false);

			// Destroy the previously created VFS GameObjects if any exist and clear the list
			foreach (GameObject VFSItems in VFSItems)
				DestroyObject(VFSItems);

			VFSItems.Clear();
		}

		/// <summary>
		/// Fill the VFS panel with keys then show it.
		/// </summary>
		/// <param name="keysList">List of the keys to display.</param>
		public void FillVFSPanel(Dictionary<string, Bundle> keysList)
		{
			// Clear the VFS panel
			ClearVFSPanel(false);

			// If there are keys to display, fill the VFS panel with key prefabs
			if ((keysList != null) && (keysList.Count > 0))
				foreach (KeyValuePair<string, Bundle> keyValuePair in keysList)
				{
					// Create a VFS key GameObject and hook it at the VFS items layout
					GameObject prefabInstance = Instantiate<GameObject>(VFSKeyPrefab);
					prefabInstance.transform.SetParent(VFSItemsLayout.transform, false);

					// Fill the newly created GameObject with key data
					VFSKeyHandler VFSKeyHandler = prefabInstance.GetComponent<VFSKeyHandler>();
					VFSKeyHandler.FillData(keyValuePair.Key, keyValuePair.Value);

					// Add the newly created GameObject to the list
					VFSItems.Add(prefabInstance);
				}
			// Else, show the "no key" text
			else
				noKeyText.SetActive(true);
		}

		/// <summary>
		/// Clear the VFS panel and show an error message.
		/// </summary>
		/// <param name="errorMessage">Error message to display.</param>
		public void ShowError(string errorMessage)
		{
			// Clear the VFS panel
			ClearVFSPanel(false);

			// Set and show the error message
			errorText.text = string.Format("\n{0}\n", errorMessage);
			errorText.gameObject.SetActive(true);
		}
		#endregion
	}
}
