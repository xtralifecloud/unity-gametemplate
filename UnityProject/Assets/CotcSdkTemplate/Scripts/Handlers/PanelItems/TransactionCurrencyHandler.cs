using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class TransactionCurrencyHandler : MonoBehaviour
	{
		#region Handling
		// Reference to the transaction currency GameObject UI elements
		//[SerializeField] private Image currencyIcon = null;
		[SerializeField] private Text nameText = null;
		[SerializeField] private Text valueText = null;

		// Fill the transaction currency with new data
		// TODO: You may want to replace the default currency icons by your own ones, according to currencies names
		public void FillData(string currencyName, Bundle currencyValue)
		{
			// Update fields
			nameText.text = currencyName;
			valueText.text = currencyValue.ToString();
		}
		#endregion
	}
}
