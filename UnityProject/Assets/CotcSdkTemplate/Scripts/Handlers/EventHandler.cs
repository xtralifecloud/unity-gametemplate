using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to display the CotcSdk's event features' results.
	/// </summary>
	public class EventHandler : MonoSingleton<EventHandler>
	{
		#region Display
		// Reference to the event GameObject prefab and the event items parent
		[SerializeField] private GameObject eventItemPrefab = null;
		[SerializeField] private GameObject eventItemsParent = null;

		// Speed at which the showing / hiding transitions are made
		[SerializeField][Range(0.1f, 10f)] private float transitionSpeed = 3f;

		// Size (in pixel) of the delta between event item's current position and its target position to consider transition has finished
		[SerializeField][Range(0.1f, 10f)] private float transitionSnap = 3f;

		// Number of seconds to wait once an event item if fully shown before hiding it
		[SerializeField][Range(0f, 30f)] private float transitionDelay = 3f;

		// The current event being displayed
		private GameObject currentDisplayedEvent = null;

		// Current displayed event item's transition state
		private enum ShowingTransitionState {InitEnable, InitPosition, Showing, Waiting, Hiding}
		private ShowingTransitionState showingTransitionState;

		// Target final Y position for the current displayed event item
		private float targetPositionY;

		// Time at which the event item's hiding transition will begin
		private float hidingTransitionTime;

		/// <summary>
		/// If there is a displayed event item, update its display at Update.
		/// </summary>
		private void Update()
		{
			if (currentDisplayedEvent != null)
				UpdateEventItemDisplay();
		}

		/// <summary>
		/// Update the current displayed event item position to get smooth transitions.
		/// </summary>
		private void UpdateEventItemDisplay()
		{
			switch (showingTransitionState)
			{
				// Enable the event item to allow the ContentSizeFitter's next update to resize the event item's height
				case ShowingTransitionState.InitEnable:
				currentDisplayedEvent.SetActive(true);
				showingTransitionState = ShowingTransitionState.InitPosition;
				break;

				// Once we know the final height of the event item, place it juste above its parent (assumed to be the fullscreen-stretched EventHandler)
				case ShowingTransitionState.InitPosition:
				currentDisplayedEvent.transform.localPosition = new Vector3(currentDisplayedEvent.transform.localPosition.x, (currentDisplayedEvent.transform.parent.GetComponent<RectTransform>().rect.height / 2f) + currentDisplayedEvent.GetComponent<RectTransform>().rect.height, currentDisplayedEvent.transform.localPosition.z);
				targetPositionY = currentDisplayedEvent.transform.parent.GetComponent<RectTransform>().rect.height / 2f;
				showingTransitionState = ShowingTransitionState.Showing;
				break;

				// Update the current displayed event item to smoothly show it
				case ShowingTransitionState.Showing:
				// If event item's position is near its target position, snap it, set the target position to above its parent, set the hiding transition triggering time, and go to the next step
				if (Mathf.Abs(currentDisplayedEvent.transform.localPosition.y - targetPositionY) <= transitionSnap)
				{
					currentDisplayedEvent.transform.localPosition = new Vector3(currentDisplayedEvent.transform.localPosition.x, targetPositionY, currentDisplayedEvent.transform.localPosition.z);
					targetPositionY = (currentDisplayedEvent.transform.parent.GetComponent<RectTransform>().rect.height / 2f) + currentDisplayedEvent.GetComponent<RectTransform>().rect.height;
					hidingTransitionTime = Time.time + transitionDelay;
					showingTransitionState = ShowingTransitionState.Waiting;
				}
				// Else, update event item's position to its target position
				else
					currentDisplayedEvent.transform.localPosition = new Vector3(currentDisplayedEvent.transform.localPosition.x, Mathf.Lerp(currentDisplayedEvent.transform.localPosition.y, targetPositionY, Time.deltaTime * transitionSpeed), currentDisplayedEvent.transform.localPosition.z);
				break;

				// Once the event item is fully shown, wait for a given delay before starting to hide it
				case ShowingTransitionState.Waiting:
				if (Time.time >= hidingTransitionTime)
					showingTransitionState = ShowingTransitionState.Hiding;
				break;

				// Update the current displayed event item to smoothly hide it
				case ShowingTransitionState.Hiding:
				// If event item's position is near its target position, destroy it, and end event item's display to allow the next pull
				if (Mathf.Abs(currentDisplayedEvent.transform.localPosition.y - targetPositionY) <= transitionSnap)
				{
					Destroy(currentDisplayedEvent);
					currentDisplayedEvent = null;
					PullNextEvent();
				}
				// Else, update event item's position to its target position
				else
					currentDisplayedEvent.transform.localPosition = new Vector3(currentDisplayedEvent.transform.localPosition.x, Mathf.Lerp(currentDisplayedEvent.transform.localPosition.y, targetPositionY, Time.deltaTime * transitionSpeed), currentDisplayedEvent.transform.localPosition.z);
				break;
			}
		}
		#endregion

		#region Events Pulling
		// List of pending events to display
		private List<GameObject> pendingEventItems = new List<GameObject>();

		/// <summary>
		/// Add a built event item to the pending list to show. (events are displayed one by one)
		/// </summary>
		/// <param name="pendingEventItem">Built event item to add to the pending list.</param>
		public void AddNewPendingEvent(GameObject pendingEventItem)
		{
			// Hook the event item at the event items parent, hide it, and add it to the pending events list
			pendingEventItem.transform.SetParent(eventItemsParent.transform, false);
			pendingEventItem.SetActive(false);
			pendingEventItems.Add(pendingEventItem);

			// Call the events pulling
			PullNextEvent();
		}

		/// <summary>
		/// Get next event from the pending list to display it. (FIFO: first in first out)
		/// </summary>
		private void PullNextEvent()
		{
			// If there is no currently displayed event and at least one pending event...
			if ((currentDisplayedEvent == null) && (pendingEventItems.Count > 0))
			{
				// Retrieve the pending event from the list and set it as the current displayed event
				currentDisplayedEvent = pendingEventItems[0];
				pendingEventItems.RemoveAt(0);

				// Set the event item's position far away from its parent to ensure it's not visible at its first enabling frame
				currentDisplayedEvent.transform.localPosition = new Vector3(currentDisplayedEvent.transform.localPosition.x, -(currentDisplayedEvent.transform.parent.GetComponent<RectTransform>().rect.height / 2f), currentDisplayedEvent.transform.localPosition.z);

				// Start the showing transition at the first initialization state
				showingTransitionState = ShowingTransitionState.InitEnable;
			}
		}
		#endregion

		#region Events Building
		/// <summary>
		/// Build a "BackOffice message" event type then add it to the pending list to display. (events are displayed one by one)
		/// </summary>
		/// <param name="backOfficeMessage">Message from the BackOffice.</param>
		public void BuildAndAddEventItem_BackOfficeMessage(string backOfficeMessage)
		{
			// Create an event item GameObject
			GameObject prefabInstance = Instantiate<GameObject>(eventItemPrefab);

			// Fill the newly created GameObject with event data
			EventItemHandler eventItemHandler = prefabInstance.GetComponent<EventItemHandler>();
			eventItemHandler.FillData(backOfficeMessage);

			// Add the newly created GameObject to the pending list
			AddNewPendingEvent(prefabInstance);
		}
		#endregion
	}
}
