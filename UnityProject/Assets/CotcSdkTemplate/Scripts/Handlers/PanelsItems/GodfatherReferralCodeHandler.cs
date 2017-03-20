using UnityEngine;
using UnityEngine.UI;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed godfather referral code.
	/// </summary>
	public class GodfatherReferralCodeHandler : MonoBehaviour
	{
		#region Display
		// Reference to the godfather referral code GameObject UI elements
		[SerializeField] private InputField referralCodeInput = null;

		/// <summary>
		/// Fill the godfather referral code with new data.
		/// </summary>
		/// <param name="referralCode">The referral code to display.</param>
		public void FillData(string referralCode)
		{
			// Update fields
			referralCodeInput.text = referralCode;
		}
		#endregion
	}
}
