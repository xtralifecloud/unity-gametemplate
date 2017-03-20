using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to display the CotcSdk's transaction features' results.
	/// </summary>
	public class TransactionHandler : MonoSingleton<TransactionHandler>
	{
		#region Display
		// Reference to the transaction panel UI elements
		[SerializeField] private GameObject outClickMask = null;
		[SerializeField] private GameObject transactionPanel = null;
		[SerializeField] private Text transactionPanelTitle = null;
		[SerializeField] private GameObject loadingBlock = null;
		[SerializeField] private Text errorText = null;
		[SerializeField] private GameObject noCurrencyText = null;
		[SerializeField] private GameObject noTransactionText = null;
		[SerializeField] private GameObject pageButtons = null;
		[SerializeField] private Button previousPageButton = null;
		[SerializeField] private Button nextPageButton = null;

		// Reference to the transaction GameObject prefabs and the transaction items layout
		[SerializeField] private GameObject transactionCurrencyPrefab = null;
		[SerializeField] private GameObject transactionItemPrefab = null;
		[SerializeField] private GridLayoutGroup transactionItemsLayout = null;

		// GridLayout cells Y size for the according to the type of items to display
		[SerializeField] private float currencyGridCellSizeY = 125f;
		[SerializeField] private float transactionGridCellSizeY = 175f;

		// List of the transaction GameObjects created on the transaction panel
		private List<GameObject> transactionItems = new List<GameObject>();

		// The last PagedList<Transaction> used to fill the transaction panel
		private PagedList<Transaction> currentTransactionsList = null;

		/// <summary>
		/// Hide the transaction panel at Start.
		/// </summary>
		private void Start()
		{
			HideTransactionPanel();
		}

		/// <summary>
		/// Hide the transaction panel.
		/// </summary>
		public void HideTransactionPanel()
		{
			// Hide the transaction panel with its outclickMask and loading block
			outClickMask.SetActive(false);
			transactionPanel.SetActive(false);
			ClearTransactionPanel(false);
		}

		/// <summary>
		/// Show the transaction panel.
		/// </summary>
		/// <param name="panelTitle">Title of the panel to display. (optional)</param>
		public void ShowTransactionPanel(string panelTitle = null)
		{
			// Set the transaction panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				transactionPanelTitle.text = panelTitle;

			// Show the transaction panel with its outclickMask and loading block
			outClickMask.SetActive(true);
			transactionPanel.SetActive(true);
			ClearTransactionPanel(true);
		}

		/// <summary>
		/// Clear the transaction panel container (loading block, texts, previously created items).
		/// </summary>
		/// <param name="showLoadingBlock">If the loading block should be shown.</param>
		public void ClearTransactionPanel(bool showLoadingBlock = false)
		{
			// Show/hide the loading block
			loadingBlock.SetActive(showLoadingBlock);

			// Hide all texts
			errorText.gameObject.SetActive(false);
			noCurrencyText.SetActive(false);
			noTransactionText.SetActive(false);

			// Hide the previous page and next page buttons
			pageButtons.SetActive(false);

			// Destroy the previously created transaction GameObjects if any exist and clear the list
			foreach (GameObject transactionItem in transactionItems)
				DestroyObject(transactionItem);

			transactionItems.Clear();
		}

		/// <summary>
		/// Fill the transaction panel with currencies then show it.
		/// </summary>
		/// <param name="currenciesList">List of the currencies to display.</param>
		public void FillTransactionPanel(Dictionary<string, Bundle> currenciesList)
		{
			// Clear the transaction panel
			ClearTransactionPanel(false);

			// Adapt the GridLayout cells Y size
			transactionItemsLayout.cellSize = new Vector2(transactionItemsLayout.cellSize.x, currencyGridCellSizeY);

			// If there are currencies to display, fill the transaction panel with currency prefabs
			if ((currenciesList != null) && (currenciesList.Count > 0))
				// TODO: You may want to display only currencies which are not achievement-progression-type currencies
				foreach (KeyValuePair<string, Bundle> currency in currenciesList)
				{
					// Create a transaction currency GameObject and hook it at the transaction items layout
					GameObject prefabInstance = Instantiate<GameObject>(transactionCurrencyPrefab);
					prefabInstance.transform.SetParent(transactionItemsLayout.transform, false);

					// Fill the newly created GameObject with currency data
					TransactionCurrencyHandler transactionCurrencyHandler = prefabInstance.GetComponent<TransactionCurrencyHandler>();
					transactionCurrencyHandler.FillData(currency.Key, currency.Value);

					// Add the newly created GameObject to the list
					transactionItems.Add(prefabInstance);
				}
			// Else, show the "no currency" text
			else
				noCurrencyText.SetActive(true);
		}

		/// <summary>
		/// Fill the transaction panel with transactions then show it.
		/// </summary>
		/// <param name="transactionsList">List of the transactions to display.</param>
		public void FillPagedTransactionPanel(PagedList<Transaction> transactionsList)
		{
			// Clear the transaction panel
			ClearTransactionPanel(false);

			// Adapt the GridLayout cells Y size for transactions
			transactionItemsLayout.cellSize = new Vector2(transactionItemsLayout.cellSize.x, transactionGridCellSizeY);

			// If there are transactions to display, fill the transaction panel with transaction prefabs
			if ((transactionsList != null) && (transactionsList.Count > 0))
			{
				foreach (Transaction transaction in transactionsList)
				{
					// Create a transaction item GameObject and hook it at the transaction items layout
					GameObject prefabInstance = Instantiate<GameObject>(transactionItemPrefab);
					prefabInstance.transform.SetParent(transactionItemsLayout.transform, false);

					// Fill the newly created GameObject with transaction data
					TransactionItemHandler transactionItemHandler = prefabInstance.GetComponent<TransactionItemHandler>();
					transactionItemHandler.FillData(transaction);

					// Add the newly created GameObject to the list
					transactionItems.Add(prefabInstance);
				}

				// Keep the last PagedList<Transaction> to allow fetching of previous and next transaction pages and show the previous page and next page buttons
				currentTransactionsList = transactionsList;
				previousPageButton.interactable = currentTransactionsList.HasPrevious;
				nextPageButton.interactable = currentTransactionsList.HasNext;
				pageButtons.SetActive(true);
			}
			// Else, show the "no transaction" text and hide the previous page and next page buttons
			else
			{
				noTransactionText.SetActive(true);
				currentTransactionsList = null;
			}
		}

		/// <summary>
		/// Clear the transaction panel and show an error message.
		/// </summary>
		/// <param name="errorMessage">Error message to display.</param>
		public void ShowError(string errorMessage)
		{
			// Clear the transaction panel
			ClearTransactionPanel(false);

			// Set and show the error message
			errorText.text = string.Format("\n{0}\n", errorMessage);
			errorText.gameObject.SetActive(true);
		}
		#endregion

		#region Buttons Actions
		/// <summary>
		/// Ask for the previous page on the current transaction panel.
		/// </summary>
		public void Button_PreviousPage()
		{
			// Disable buttons to avoid concurrent calls
			previousPageButton.interactable = false;
			nextPageButton.interactable = false;

			// Call for the previous page
			if (currentTransactionsList != null)
				TransactionFeatures.Handling_FetchPreviousTransactionPage(currentTransactionsList);
		}

		/// <summary>
		/// Ask for the next page on the current transaction panel.
		/// </summary>
		public void Button_NextPage()
		{
			// Disable buttons to avoid concurrent calls
			previousPageButton.interactable = false;
			nextPageButton.interactable = false;

			// Call for the next page
			if (currentTransactionsList != null)
				TransactionFeatures.Handling_FetchNextTransactionPage(currentTransactionsList);
		}
		#endregion

		#region Screen Orientation
		/// <summary>
		/// Adapt the layout display when the current screen orientation changes.
		/// </summary>
		public void OnRectTransformDimensionsChange()
		{
			// If on landscape orientation use 2 columns, else (portrait) use 1 column
			transactionItemsLayout.constraintCount = Screen.width > Screen.height ? 2 : 1;
		}
		#endregion
	}
}
