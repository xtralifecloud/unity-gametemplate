using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed VFS key.
	/// </summary>
	public class VFSKeyHandler : MonoBehaviour
	{
		#region Display
		// Reference to the gamer VFS key GameObject UI elements
		[SerializeField] private Text keyText = null;
		[SerializeField] private Text valueText = null;

		// Text to display to show the key type and name
		private const string keyNameText = "({0}) {1}";

		/// <summary>
		/// Fill the gamer VFS key with new data.
		/// </summary>
		/// <param name="keyName">Name of the key.</param>
		/// <param name="keyValue">Value of the key under the Bundle format.</param>
		public void FillData(string keyName, Bundle keyValue)
		{
			// Update fields
			keyText.text = string.Format(keyNameText, keyValue.Type, keyName);
			valueText.text = keyValue.ToString();
		}
		#endregion
	}
}
