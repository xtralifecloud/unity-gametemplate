using UnityEngine;
using UnityEngine.UI;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed event item.
	/// </summary>
	public class EventItemHandler : MonoBehaviour
	{
		#region Display
		// Reference to the event item GameObject UI elements
		[SerializeField] private Text eventMessageText = null;

		// Reference to the event linked items layout
		[SerializeField] private VerticalLayoutGroup eventLinkedItemsLayout = null;

		/// <summary>
		/// Fill the event item with new data.
		/// </summary>
		/// <param name="eventMessage">Message of the event.</param>
		/// <param name="linkedItem">An item to link to the displayed event. (optional)</param>
		public void FillData(string eventMessage, GameObject linkedItem = null)
		{
			// Update fields
			eventMessageText.text = eventMessage;

			// If there is an item to link to the event, set the item's transform parent as the event's one
			if (linkedItem != null)
				linkedItem.transform.SetParent(eventLinkedItemsLayout.transform, false);
		}
		#endregion
	}
}
