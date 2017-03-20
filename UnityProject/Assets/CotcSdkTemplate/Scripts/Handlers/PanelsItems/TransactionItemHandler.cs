using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed transaction item.
	/// </summary>
	public class TransactionItemHandler : MonoBehaviour
	{
		#region Display
		// Reference to the transaction item GameObject UI elements
		[SerializeField] private Text dateText = null;
		[SerializeField] private Text currenciesText = null;
		[SerializeField] private Text descriptionText = null;

		/// <summary>
		/// Fill the transaction item with new data.
		/// </summary>
		/// <param name="transaction">Transaction currencies data under the Bundle format.</param>
		/// <param name="displayTransactionDescription">If the transaction description should be shown.</param>
		public void FillData(Transaction transaction, bool displayTransactionDescription = true)
		{
			// TODO: You may want to display culture dependent date formats
			// Update fields
			dateText.text = transaction.RunDate.ToString();
			currenciesText.text = CurrenciesToString(transaction.TxData);
			descriptionText.text = transaction.Description;

			// Display the transaction description only if there is one
			descriptionText.gameObject.SetActive(displayTransactionDescription && !string.IsNullOrEmpty(transaction.Description));
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
