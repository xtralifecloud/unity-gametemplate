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
		[SerializeField] private GameObject noCurrencyText = null;

		// Reference to the currency GameObject prefab and the currencies list scroll view
		[SerializeField] private GameObject currencyPrefab = null;
		[SerializeField] private GridLayoutGroup transactionCurrenciesLayout = null;

		// List of the currency GameObjects created on the transaction panel
		private List<GameObject> transactionCurrencies = new List<GameObject>();

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
		public void FillAndShowTransactionPanel(Dictionary<string, Bundle> currenciesList)
		{
			// Destroy the previously created currency GameObjects if any exist and clear the list
			foreach (GameObject transactionCurrency in transactionCurrencies)
				DestroyObject(transactionCurrency);
			
			transactionCurrencies.Clear();

			// If there are currencies to display, fill the transaction panel with currency prefabs
			if ((currenciesList != null) && (currenciesList.Count > 0))
			{
				// Hide the "no currency" text
				noCurrencyText.SetActive(false);

				// TODO: You may want to display only currencies which are not achievement-progression-type currencies
				foreach (KeyValuePair<string, Bundle> currency in currenciesList)
				{
					// Create a currency item GameObject and hook it at the currencies scroll view
					GameObject prefabInstance = Instantiate<GameObject>(currencyPrefab);
					prefabInstance.transform.SetParent(transactionCurrenciesLayout.transform, false);

					// Fill the newly created GameObject with currency data
					TransactionCurrencyHandler transactionCurrencyHandler = prefabInstance.GetComponent<TransactionCurrencyHandler>();
					transactionCurrencyHandler.FillData(currency.Key, currency.Value);

					// Add the newly created GameObject to the list
					transactionCurrencies.Add(prefabInstance);
				}
			}
			// Else, show the "no currency" text
			else
				noCurrencyText.SetActive(true);
			
			// Show the transaction panel
			ShowTransactionPanel(true);
		}
		#endregion

		#region Screen Orientation
		// Adapt the layout display when the current screen orientation changes
		public void OnRectTransformDimensionsChange()
		{
			// If on landscape orientation use 2 columns, else (portrait) use 1 column
			transactionCurrenciesLayout.constraintCount = Screen.width > Screen.height ? 2 : 1;
		}
		#endregion
	}
}
