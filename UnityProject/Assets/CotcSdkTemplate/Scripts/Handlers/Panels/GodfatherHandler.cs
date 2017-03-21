using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to display the CotcSdk's godfather features' results.
	/// </summary>
	public class GodfatherHandler : MonoSingleton<GodfatherHandler>
	{
		#region Display
		// Reference to the godfather panel UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject godfatherPanel = null;
		[SerializeField] private Text godfatherPanelTitle = null;
		[SerializeField] private GameObject loadingBlock = null;
		[SerializeField] private Text errorText = null;
		[SerializeField] private Text noGodchildText = null;
		[SerializeField] private Text noGodfatherText = null;

		// Reference to the godfather GameObject prefabs and the godfather items layout
		[SerializeField] private GameObject godfatherReferralCodePrefab = null;
		[SerializeField] private GameObject godfatherGamerPrefab = null;
		[SerializeField] private GridLayoutGroup godfatherItemsLayout = null;

		// List of the godfather GameObjects created on the godfather panel
		private List<GameObject> godfatherItems = new List<GameObject>();

		/// <summary>
		/// Hide the godfather panel at Start.
		/// </summary>
		private void Start()
		{
			HideGodfatherPanel();
		}

		/// <summary>
		/// Hide the godfather panel.
		/// </summary>
		public void HideGodfatherPanel()
		{
			// Hide the godfather panel with its outclickMask and loading block
			outClickMask.SetActive(false);
			godfatherPanel.SetActive(false);
			ClearGodfatherPanel(false);
		}

		/// <summary>
		/// Show the godfather panel.
		/// </summary>
		/// <param name="panelTitle">Title of the panel to display. (optional)</param>
		public void ShowGodfatherPanel(string panelTitle = null)
		{
			// Set the godfather panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				godfatherPanelTitle.text = panelTitle;
			
			// Show the godfather panel with its outclickMask and loading block
			outClickMask.SetActive(true);
			godfatherPanel.SetActive(true);
			ClearGodfatherPanel(true);
		}

		/// <summary>
		/// Clear the godfather panel container (loading block, texts, previously created items).
		/// </summary>
		/// <param name="showLoadingBlock">If the loading block should be shown.</param>
		public void ClearGodfatherPanel(bool showLoadingBlock = false)
		{
			// Show/hide the loading block
			loadingBlock.SetActive(showLoadingBlock);

			// Hide all texts
			errorText.gameObject.SetActive(false);
			noGodchildText.gameObject.SetActive(false);
			noGodfatherText.gameObject.SetActive(false);

			// Destroy the previously created godfather GameObjects if any exist and clear the list
			foreach (GameObject godfatherItem in godfatherItems)
				DestroyObject(godfatherItem);

			godfatherItems.Clear();
		}

		/// <summary>
		/// Fill the godfather panel with referral code.
		/// </summary>
		/// <param name="referralCode">The referral code to display.</param>
		public void FillGodfatherPanel(string referralCode)
		{
			// Clear the godfather panel
			ClearGodfatherPanel(false);

			// Create a godfather referral code GameObject and hook it at the godfather items layout
			GameObject prefabInstance = Instantiate<GameObject>(godfatherReferralCodePrefab);
			prefabInstance.transform.SetParent(godfatherItemsLayout.transform, false);

			// Fill the newly created GameObject with referral code data
			GodfatherReferralCodeHandler godfatherReferralCodeHandler = prefabInstance.GetComponent<GodfatherReferralCodeHandler>();
			godfatherReferralCodeHandler.FillData(referralCode);

			// Add the newly created GameObject to the list
			godfatherItems.Add(prefabInstance);
		}

		/// <summary>
		/// Fill the godfather panel with godchildren list.
		/// </summary>
		/// <param name="godchildrenList">Godchildren gamer info.</param>
		public void FillGodfatherPanel(NonpagedList<GamerInfo> godchildrenList)
		{
			// Clear the godfather panel
			ClearGodfatherPanel(false);

			// If there are godchildren to display, fill the godfather panel with gamer prefabs
			if ((godchildrenList != null) && (godchildrenList.Count > 0))
				foreach (GamerInfo godchild in godchildrenList)
				{
					// Create a godfather gamer GameObject and hook it at the godfather items layout
					GameObject prefabInstance = Instantiate<GameObject>(godfatherGamerPrefab);
					prefabInstance.transform.SetParent(godfatherItemsLayout.transform, false);

					// Fill the newly created GameObject with gamer data
					GodfatherGamerHandler godfatherGamerHandler = prefabInstance.GetComponent<GodfatherGamerHandler>();
					godfatherGamerHandler.FillData(godchild["profile"], godchild.GamerId);

					// Add the newly created GameObject to the list
					godfatherItems.Add(prefabInstance);
				}
			// Else, show the "no godchild" text
			else
				noGodchildText.gameObject.SetActive(true);
		}

		/// <summary>
		/// Fill the godfather panel with godfather.
		/// </summary>
		/// <param name="godfather">Godfather gamer info.</param>
		public void FillGodfatherPanel(GamerInfo godfather)
		{
			// Clear the godfather panel
			ClearGodfatherPanel(false);

			// If there is godfather to display, fill the godfather panel with gamer prefab
			if (!string.IsNullOrEmpty(godfather.GamerId))
			{
				// Create a godfather gamer GameObject and hook it at the godfather items layout
				GameObject prefabInstance = Instantiate<GameObject>(godfatherGamerPrefab);
				prefabInstance.transform.SetParent(godfatherItemsLayout.transform, false);

				// Fill the newly created GameObject with gamer data
				GodfatherGamerHandler godfatherGamerHandler = prefabInstance.GetComponent<GodfatherGamerHandler>();
				godfatherGamerHandler.FillData(godfather["profile"], godfather.GamerId);

				// Add the newly created GameObject to the list
				godfatherItems.Add(prefabInstance);
			}
			// Else, show the "no godfather" text
			else
				noGodfatherText.gameObject.SetActive(true);
		}

		/// <summary>
		/// Clear the godfather panel and show an error message.
		/// </summary>
		/// <param name="errorMessage">Error message to display.</param>
		public void ShowError(string errorMessage)
		{
			// Clear the godfather panel
			ClearGodfatherPanel(false);

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
			godfatherItemsLayout.constraintCount = Screen.width > Screen.height ? 2 : 1;
		}
		#endregion
	}
}
