using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
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

		// List of the currency/transaction GameObjects created on the transaction panel
		private List<GameObject> transactionItems = new List<GameObject>();

		// The last PagedList<Transaction> used to fill the transaction panel
		private PagedList<Transaction> currentTransactionsList = null;

		// Hide the transaction panel at Start
		private void Start()
		{
			ShowTransactionPanel(false);
		}

		// Show or hide the transaction panel
		public void ShowTransactionPanel(bool show = true)
		{
			outClickMask.SetActive(show);
			transactionPanel.SetActive(show);
		}

		// Fill the transaction panel with currencies then show it
		public void FillAndShowTransactionPanel(string panelTitle, Dictionary<string, Bundle> currenciesList)
		{
			// Hide the "no transaction" text and hide the previous page and next page buttons
			noTransactionText.SetActive(false);
			pageButtons.SetActive(false);

			// Destroy the previously created currency/transaction GameObjects if any exist and clear the list
			foreach (GameObject transactionCurrency in transactionItems)
				DestroyObject(transactionCurrency);
			
			transactionItems.Clear();

			// Set the leaderboard panel's title
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

		// Fill the transaction panel with transactions history then show it
		public void FillAndShowPagedTransactionPanel(string panelTitle, PagedList<Transaction> transactionsList)
		{
			// Hide the "no currency" text
			noCurrencyText.SetActive(false);

			// Destroy the previously created currency/transaction GameObjects if any exist and clear the list
			foreach (GameObject transactionItem in transactionItems)
				DestroyObject(transactionItem);

			transactionItems.Clear();

			// Set the leaderboard panel's title
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
		// Ask for the previous page on the current transaction panel
		public void Button_PreviousPage()
		{
			// Disable buttons to avoid concurrent calls
			previousPageButton.interactable = false;
			nextPageButton.interactable = false;

			// Call for the previous page
			if (currentTransactionsList != null)
				TransactionFeatures.FetchPreviousTransactionPage(currentTransactionsList);
		}

		// Ask for the next page on the current leaderboard panel
		public void Button_NextPage()
		{
			// Disable buttons to avoid concurrent calls
			previousPageButton.interactable = false;
			nextPageButton.interactable = false;

			// Call for the next page
			if (currentTransactionsList != null)
				TransactionFeatures.FetchNextTransactionPage(currentTransactionsList);
		}
		#endregion

		#region Screen Orientation
		// Adapt the layout display when the current screen orientation changes
		public void OnRectTransformDimensionsChange()
		{
			// If on landscape orientation use 2 columns, else (portrait) use 1 column
			transactionItemsLayout.constraintCount = Screen.width > Screen.height ? 2 : 1;
		}
		#endregion
	}
}
