using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed transaction transaction.
	/// </summary>
	public class TransactionTransactionHandler : MonoBehaviour
	{
		#region Handling
		// Reference to the transaction transaction GameObject UI elements
		[SerializeField] private Text transactionDate = null;
		[SerializeField] private Text transactionCurrencies = null;
		[SerializeField] private Text transactionDescription = null;

		/// <summary>
		/// Fill the transaction transaction with new data.
		/// </summary>
		/// <param name="transaction">Transaction currencies data under the Bundle format.</param>
		/// <param name="displayTransactionDescription">If the transaction description should be shown.</param>
		public void FillData(Transaction transaction, bool displayTransactionDescription = true)
		{
			// TODO: You may want to display culture dependent date formats
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

		/// <summary>
		/// Format a string from a currencies list Bundle.
		/// </summary>
		/// <param name="currenciesBundle">List of currencies under the Bundle format.</param>
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
