using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class GamerVFSKeyHandler : MonoBehaviour
	{
		#region Handling
		// Reference to the gamer VFS key GameObject UI elements
		[SerializeField] private Text key = null;
		[SerializeField] private Text value = null;

		// Text to display to show the key type and name
		private const string keyNameText = "({0}) {1}";

		// Fill the gamer VFS key with new data
		public void FillData(string keyName, Bundle keyValue)
		{
			// Update fields
			key.text = string.Format(keyNameText, keyValue.Type, keyName);
			value.text = keyValue.ToString();
		}
		#endregion
	}
}
