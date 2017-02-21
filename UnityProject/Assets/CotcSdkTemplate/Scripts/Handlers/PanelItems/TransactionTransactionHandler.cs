using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class TransactionTransactionHandler : MonoBehaviour
	{
		#region Handling
		// Reference to the transaction transaction GameObject UI elements
		[SerializeField] private Text transactionDate = null;
		[SerializeField] private Text transactionCurrencies = null;
		[SerializeField] private Text transactionDescription = null;

		// Fill the transaction currency with new data
		// TODO: You may want to display culture dependent date formats
		public void FillData(Transaction transaction, bool displayTransactionDescription = true)
		{
			// Update fields
			transactionDate.text = transaction.RunDate.ToString();
			transactionCurrencies.text = CurrenciesToString(transaction.TxData);
			transactionDescription.text = transaction.Description;

			// Display the transaction description only if there is one
			transactionDescription.gameObject.SetActive(displayTransactionDescription && !string.IsNullOrEmpty(transaction.Description));
		}
		#endregion

		#region Currencies Formating
		// Text to format a currency data
		private const string currencyFormat = "{0}: {1}";

		// Format a string from a currencies list Bundle
		private string CurrenciesToString(Bundle currenciesBundle)
		{
			// Get currencies as a Dictionary and instantiate a StringBuilder
			Dictionary<string, Bundle> currencies = currenciesBundle.AsDictionary();
			StringBuilder finalString = new StringBuilder();

			bool comma = false;

			// Append each currency to the formated string
			foreach (KeyValuePair<string, Bundle> currency in currencies)
			{
				if (comma)
					finalString.Append(", ");
				else
					comma = true;
				
				finalString.Append(string.Format(currencyFormat, currency.Key, currency.Value.ToString()));
			}

			// Return the final built string
			return finalString.ToString();
		}
		#endregion
	}
}
