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
		[SerializeField] private GameObject noCurrencyText = null;
		[SerializeField] private GameObject noTransactionText = null;
		[SerializeField] private GameObject pageButtons = null;
		[SerializeField] private Button previousPageButton = null;
		[SerializeField] private Button nextPageButton = null;

		// Reference to the currency GameObject prefab and the currencies list scroll view
		[SerializeField] private GameObject currencyPrefab = null;
		[SerializeField] private GameObject transactionPrefab = null;
		[SerializeField] private GridLayoutGroup transactionItemsLayout = null;

		// GridLayout cells Y size for the according to the type of items to display
		[SerializeField] private float currencyGridCellSizeY = 125f;
		[SerializeField] private float transactionGridCellSizeY = 175f;

		// List of the currency/transaction GameObjects created on the transaction panel
		private List<GameObject> transactionItems = new List<GameObject>();

		// The last PagedList<Transaction> used to fill the transaction panel
		private PagedList<Transaction> currentTransactionsList = null;

		/// <summary>
		/// Hide the transaction panel at Start.
		/// </summary>
		private void Start()
		{
			ShowTransactionPanel(false);
		}

		/// <summary>
		/// Show or hide the transaction panel.
		/// </summary>
		/// <param name="show">If the panel should be shown.</param>
		public void ShowTransactionPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			transactionPanel.SetActive(show);
		}

		/// <summary>
		/// Fill the transaction panel with currencies then show it.
		/// </summary>
		/// <param name="currenciesList">List of the currencies to display.</param>
		/// <param name="panelTitle">Title of the panel.</param>
		public void FillAndShowTransactionPanel(Dictionary<string, Bundle> currenciesList, string panelTitle = "Currencies Balance")
		{
			// Hide the "no transaction" text and hide the previous page and next page buttons
			noTransactionText.SetActive(false);
			pageButtons.SetActive(false);

			// Destroy the previously created currency/transaction GameObjects if any exist and clear the list
			foreach (GameObject transactionCurrency in transactionItems)
				DestroyObject(transactionCurrency);
			
			transactionItems.Clear();

			// Adapt the GridLayout cells Y size for currencies
			transactionItemsLayout.cellSize = new Vector2(transactionItemsLayout.cellSize.x, currencyGridCellSizeY);

			// Set the transaction panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				transactionPanelTitle.text = panelTitle;

			// If there are currencies to display, fill the transaction panel with currency prefabs
			if ((currenciesList != null) && (currenciesList.Count > 0))
			{
				// Hide the "no currency" text
				noCurrencyText.SetActive(false);

				// TODO: You may want to display only currencies which are not achievement-progression-type currencies
				foreach (KeyValuePair<string, Bundle> currency in currenciesList)
				{
					// Create a transaction currency GameObject and hook it at the items scroll view
					GameObject prefabInstance = Instantiate<GameObject>(currencyPrefab);
					prefabInstance.transform.SetParent(transactionItemsLayout.transform, false);

					// Fill the newly created GameObject with currency data
					TransactionCurrencyHandler transactionCurrencyHandler = prefabInstance.GetComponent<TransactionCurrencyHandler>();
					transactionCurrencyHandler.FillData(currency.Key, currency.Value);

					// Add the newly created GameObject to the list
					transactionItems.Add(prefabInstance);
				}
			}
			// Else, show the "no currency" text
			else
				noCurrencyText.SetActive(true);
			
			// Show the transaction panel
			ShowTransactionPanel(true);
		}

		/// <summary>
		/// Fill the transaction panel with transactions then show it.
		/// </summary>
		/// <param name="transactionsList">List of the transactions to display.</param>
		/// <param name="panelTitle">Title of the panel.</param>
		public void FillAndShowPagedTransactionPanel(PagedList<Transaction> transactionsList, string panelTitle = "Transactions History")
		{
			// Hide the "no currency" text
			noCurrencyText.SetActive(false);

			// Destroy the previously created currency/transaction GameObjects if any exist and clear the list
			foreach (GameObject transactionItem in transactionItems)
				DestroyObject(transactionItem);

			transactionItems.Clear();

			// Adapt the GridLayout cells Y size for transactions
			transactionItemsLayout.cellSize = new Vector2(transactionItemsLayout.cellSize.x, transactionGridCellSizeY);

			// Set the transaction panel's title only if not null or empty
			if (!string.IsNullOrEmpty(panelTitle))
				transactionPanelTitle.text = panelTitle;

			// If there are transactions to display, fill the transaction panel with transaction prefabs
			if ((transactionsList != null) && (transactionsList.Count > 0))
			{
				// Hide the "no transaction" text and show the previous page and next page buttons
				noTransactionText.SetActive(false);
				pageButtons.SetActive(true);

				foreach (Transaction transaction in transactionsList)
				{
					// Create a transaction transaction GameObject and hook it at the items scroll view
					GameObject prefabInstance = Instantiate<GameObject>(transactionPrefab);
					prefabInstance.transform.SetParent(transactionItemsLayout.transform, false);

					// Fill the newly created GameObject with transaction data
					TransactionTransactionHandler transactionTransactionHandler = prefabInstance.GetComponent<TransactionTransactionHandler>();
					transactionTransactionHandler.FillData(transaction);

					// Add the newly created GameObject to the list
					transactionItems.Add(prefabInstance);
				}

				// Keep the last PagedList<Transaction> to allow usage of previous and next transaction page
				currentTransactionsList = transactionsList;
				previousPageButton.interactable = currentTransactionsList.HasPrevious;
				nextPageButton.interactable = currentTransactionsList.HasNext;
			}
			// Else, show the "no transaction" text and hide the previous page and next page buttons
			else
			{
				noTransactionText.SetActive(true);
				pageButtons.SetActive(false);
				currentTransactionsList = null;
			}

			// Show the transaction panel
			ShowTransactionPanel(true);
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
