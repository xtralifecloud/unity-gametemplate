using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class VFSHandler : MonoSingleton<VFSHandler>
	{
		#region Display
		// Reference to the VFS panel UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject VFSPanel = null;
		[SerializeField] private GameObject noKeyText = null;

		// Reference to the key GameObject prefab and the keys list scroll view
		[SerializeField] private GameObject keyPrefab = null;
		[SerializeField] private VerticalLayoutGroup VFSKeysLayout = null;

		// List of the key GameObjects created on the VFS panel
		private List<GameObject> VFSKeys = new List<GameObject>();

		// Hide the VFS panel at Start
		private void Start()
		{
			ShowVFSPanel(false);
		}

		// Show or hide the VFS panel
		public void ShowVFSPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			VFSPanel.SetActive(show);
		}

		// Fill the VFS panel with keys then show it
		public void FillAndShowVFSPanel(Dictionary<string, Bundle> keysList)
		{
			// Destroy the previously created key GameObjects if any exist and clear the list
			foreach (GameObject VFSKey in VFSKeys)
				DestroyObject(VFSKey);

			VFSKeys.Clear();

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
