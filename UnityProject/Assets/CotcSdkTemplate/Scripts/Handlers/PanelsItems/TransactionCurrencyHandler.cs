using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed transaction currency.
	/// </summary>
	public class TransactionCurrencyHandler : MonoBehaviour
	{
		#region Display
		// Reference to the transaction currency GameObject UI elements
		//[SerializeField] private Image currencyIcon = null;
		[SerializeField] private Text nameText = null;
		[SerializeField] private Text valueText = null;

		/// <summary>
		/// Fill the transaction currency with new data.
		/// </summary>
		/// <param name="currencyName">Name of the currency.</param>
		/// <param name="currencyValue">Value of the currency under the Bundle format.</param>
		public void FillData(string currencyName, Bundle currencyValue)
		{
			// TODO: You may want to replace the default currency icons by your own ones, according to currencies names
			// Update fields
			nameText.text = currencyName;
			valueText.text = currencyValue.ToString();
		}
		#endregion
	}
}
