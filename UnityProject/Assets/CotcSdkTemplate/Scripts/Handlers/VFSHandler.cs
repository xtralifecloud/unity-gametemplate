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
		[SerializeField] private GameObject noKeyText = null;

		// Reference to the key GameObject prefab and the keys list scroll view
		[SerializeField] private GameObject keyPrefab = null;
		[SerializeField] private VerticalLayoutGroup VFSKeysLayout = null;

		// List of the key GameObjects created on the VFS panel
		private List<GameObject> VFSKeys = new List<GameObject>();

		/// <summary>
		/// Hide the VFS panel at Start.
		/// </summary>
		private void Start()
		{
			ShowVFSPanel(false);
		}

		/// <summary>
		/// Show or hide the VFS panel.
		/// </summary>
		/// <param name="show">If the panel should be shown.</param>
		public void ShowVFSPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			VFSPanel.SetActive(show);
		}

		/// <summary>
		/// Fill the VFS panel with keys then show it.
		/// </summary>
		/// <param name="keysList">List of the keys to display.</param>
		/// <param name="panelTitle">Title of the panel.</param>
		public void FillAndShowVFSPanel(Dictionary<string, Bundle> keysList, string panelTitle = "VFS Keys")
		{
			// Destroy the previously created key GameObjects if any exist and clear the list
			foreach (GameObject VFSKey in VFSKeys)
				DestroyObject(VFSKey);
			
			VFSKeys.Clear();

			// Set the VFS panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				VFSPanelTitle.text = panelTitle;

			// If there are keys to display, fill the VFS panel with key prefabs
			if ((keysList != null) && (keysList.Count > 0))
			{
				// Hide the "no key" text
				noKeyText.SetActive(false);

				foreach (KeyValuePair<string, Bundle> keyValuePair in keysList)
				{
					// Create a VFS key GameObject and hook it at the VFS keys scroll view
					GameObject prefabInstance = Instantiate<GameObject>(keyPrefab);
					prefabInstance.transform.SetParent(VFSKeysLayout.transform, false);

					// Fill the newly created GameObject with key data
					VFSKeyHandler VFSKeyHandler = prefabInstance.GetComponent<VFSKeyHandler>();
					VFSKeyHandler.FillData(keyValuePair.Key, keyValuePair.Value);

					// Add the newly created GameObject to the list
					VFSKeys.Add(prefabInstance);
				}
			}
			// Else, show the "no key" text
			else
				noKeyText.SetActive(true);
			
			// Show the VFS panel
			ShowVFSPanel(true);
		}
		#endregion
	}
}
