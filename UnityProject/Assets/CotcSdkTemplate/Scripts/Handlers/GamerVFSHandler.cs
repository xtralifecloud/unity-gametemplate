using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class GamerVFSHandler : MonoSingleton<GamerVFSHandler>
	{
		#region Display
		// Reference to the gamer VFS panel UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject gamerVFSPanel = null;
		[SerializeField] private GameObject noKeyText = null;

		// Reference to the key GameObject prefab and the keys list scroll view
		[SerializeField] private GameObject keyPrefab = null;
		[SerializeField] private Transform keysScrollViewContent = null;

		// List of the key GameObjects created on the gamer VFS panel
		private List<GameObject> gamerVFSKeys = new List<GameObject>();

		// Hide the gamer VFS panel at Start
		private void Start()
		{
			ShowGamerVFSPanel(false);
		}

		// Show or hide the gamer VFS panel
		public void ShowGamerVFSPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			gamerVFSPanel.SetActive(show);
		}

		// Fill the gamer VFS panel with keys then show it
		public void FillAndShowGamerVFSPanel(Dictionary<string, Bundle> keysList)
		{
			// Destroy the previously created key GameObjects if any exist and clear the list
			foreach (GameObject gamerVFSKey in gamerVFSKeys)
				DestroyObject(gamerVFSKey);

			gamerVFSKeys.Clear();

			// If there are keys to display, fill the gamer VFS panel with key prefabs
			if ((keysList != null) && (keysList.Count > 0))
			{
				// Hide the "no key" text
				noKeyText.SetActive(false);

				foreach (KeyValuePair<string, Bundle> keyValuePair in keysList)
				{
					// Create a gamer VFS key GameObject and hook it at the gamer VFS keys scroll view
					GameObject prefabInstance = Instantiate<GameObject>(keyPrefab);
					prefabInstance.transform.SetParent(keysScrollViewContent, false);

					// Fill the newly created GameObject with key data
					GamerVFSKeyHandler gamerVFSKeyHandler = prefabInstance.GetComponent<GamerVFSKeyHandler>();
					gamerVFSKeyHandler.FillData(keyValuePair.Key, keyValuePair.Value);

					// Add the newly created GameObject to the list
					gamerVFSKeys.Add(prefabInstance);
				}
			}
			// Else, show the "no key" text
			else
				noKeyText.SetActive(true);

			// Show the gamer VFS panel
			ShowGamerVFSPanel(true);
		}
		#endregion
	}
}
