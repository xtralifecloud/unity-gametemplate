using System.Collections.Generic;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to display the CotcSdk's event features' results.
	/// </summary>
	public class EventHandler : MonoSingleton<EventHandler>
	{
		#region Display
		// Reference to the event GameObject prefabs and the event items parent
		[SerializeField] private GameObject eventItemPrefab = null;
		[SerializeField] private GameObject eventItemsParent = null;
		[SerializeField] private GameObject achievementItemPrefab = null;
		[SerializeField] private GameObject communityFriendPrefab = null;
		[SerializeField] private GameObject godfatherGamerPrefab = null;

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
				RectTransform ip_parentRectTransform = currentDisplayedEvent.transform.parent.GetComponent<RectTransform>();
				float ip_parentRectOriginY = ip_parentRectTransform.rect.height * (1f - ip_parentRectTransform.pivot.y);
				currentDisplayedEvent.transform.localPosition = new Vector3(currentDisplayedEvent.transform.localPosition.x, ip_parentRectOriginY + currentDisplayedEvent.GetComponent<RectTransform>().rect.height, currentDisplayedEvent.transform.localPosition.z);
				targetPositionY = ip_parentRectOriginY;
				showingTransitionState = ShowingTransitionState.Showing;
				break;

				// Update the current displayed event item to smoothly show it
				case ShowingTransitionState.Showing:
				// If event item's position is near its target position, snap it, set the target position to above its parent, set the hiding transition triggering time, and go to the next step
				if (Mathf.Abs(currentDisplayedEvent.transform.localPosition.y - targetPositionY) <= transitionSnap)
				{
					RectTransform s_parentRectTransform = currentDisplayedEvent.transform.parent.GetComponent<RectTransform>();
					float s_parentRectOriginY = s_parentRectTransform.rect.height * (1f - s_parentRectTransform.pivot.y);
					currentDisplayedEvent.transform.localPosition = new Vector3(currentDisplayedEvent.transform.localPosition.x, targetPositionY, currentDisplayedEvent.transform.localPosition.z);
					targetPositionY = s_parentRectOriginY + currentDisplayedEvent.GetComponent<RectTransform>().rect.height;
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
		private void AddNewPendingEvent(GameObject pendingEventItem)
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
		/// Add an event with an optional linked item to the pending list to display. (events are displayed one by one)
		/// </summary>
		/// <param name="eventMessage">Message of the event.</param>
		/// <param name="linkedItem">GameObject to display with this event. (optional)</param>
		public void AddEventItem(string eventMessage, GameObject linkedItem = null)
		{
			// Create an event item GameObject
			GameObject eventItemPrefabInstance = Instantiate<GameObject>(eventItemPrefab);

			// Fill the newly created GameObject with event data
			EventItemHandler eventItemHandler = eventItemPrefabInstance.GetComponent<EventItemHandler>();
			eventItemHandler.FillData(eventMessage, linkedItem);

			// Add the newly created GameObject to the pending list
			AddNewPendingEvent(eventItemPrefabInstance);
		}

		/// <summary>
		/// Build an "achievement unlocked" event type then add it to the pending list to display. (events are displayed one by one)
		/// </summary>
		/// <param name="eventMessage">Message of the event. (describes an unlocked achievement)</param>
		/// <param name="achievement">The unlocked achievement under the AchievementDefinition format.</param>
		public void BuildAndAddEventItem_AchievementUnlocked(string eventMessage, AchievementDefinition achievement)
		{
			// Create an event achievement item GameObject
			GameObject achievementItemPrefabInstance = Instantiate<GameObject>(achievementItemPrefab);

			// Fill the newly created GameObject with event friend data
			AchievementItemHandler achievementItemHandler = achievementItemPrefabInstance.GetComponent<AchievementItemHandler>();
			achievementItemHandler.FillData(achievement);

			// Add an event item with the newly created GameObject linked to it
			AddEventItem(eventMessage, achievementItemPrefabInstance);
		}

		/// <summary>
		/// Build a "BackOffice message" event type then add it to the pending list to display. (events are displayed one by one)
		/// </summary>
		/// <param name="eventMessage">Message of the event. (message from the BackOffice)</param>
		public void BuildAndAddEventItem_BackOfficeMessage(string eventMessage)
		{
			// Add an event item
			AddEventItem(eventMessage);
		}

		/// <summary>
		/// Build a "friend's message" event type then add it to the pending list to display. (events are displayed one by one)
		/// </summary>
		/// <param name="eventMessage">Message of the event. (describes a friend's message is received)</param>
		/// <param name="friendProfile">Profile of the friend whith who the relationship changed under the Bundle format.</param>
		/// <param name="friendMessage">The message from friend to display.</param>
		public void BuildAndAddEventItem_FriendMessage(string eventMessage, Bundle friendProfile, string friendMessage)
		{
			// Create a community friend GameObject
			GameObject communityFriendPrefabInstance = Instantiate<GameObject>(communityFriendPrefab);

			// Fill the newly created GameObject with event friend data
			CommunityFriendHandler communityFriendHandler = communityFriendPrefabInstance.GetComponent<CommunityFriendHandler>();
			communityFriendHandler.FillData(friendProfile, friendMessage);

			// Add an event item with the newly created GameObject linked to it
			AddEventItem(eventMessage, communityFriendPrefabInstance);
		}

		/// <summary>
		/// Build a "friend's relationship changed" event type then add it to the pending list to display. (events are displayed one by one)
		/// </summary>
		/// <param name="eventMessage">Message of the event. (describes the relationship change)</param>
		/// <param name="friendProfile">Profile of the friend whith who the relationship changed under the Bundle format.</param>
		/// <param name="relationship">Type of relationship which has been set.</param>
		public void BuildAndAddEventItem_FriendRelationshipChanged(string eventMessage, Bundle friendProfile, FriendRelationshipStatus relationship)
		{
			// Create a community friend GameObject
			GameObject communityFriendPrefabInstance = Instantiate<GameObject>(communityFriendPrefab);

			// Fill the newly created GameObject with event friend data
			CommunityFriendHandler communityFriendHandler = communityFriendPrefabInstance.GetComponent<CommunityFriendHandler>();
			communityFriendHandler.FillData(friendProfile, relationship);

			// Add an event item with the newly created GameObject linked to it
			AddEventItem(eventMessage, communityFriendPrefabInstance);
		}

		/// <summary>
		/// Build a "new godchild" event type then add it to the pending list to display. (events are displayed one by one)
		/// </summary>
		/// <param name="eventMessage">Message of the event. (describes there is a new godchild)</param>
		/// <param name="godchildProfile">Profile of the new godchild under the Bundle format.</param>
		public void BuildAndAddEventItem_NewGodchild(string eventMessage, Bundle godchildProfile)
		{
			// Create a godfather gamer GameObject
			GameObject godfatherGamerPrefabInstance = Instantiate<GameObject>(godfatherGamerPrefab);

			// Fill the newly created GameObject with event gamer data
			GodfatherGamerHandler godfatherGamerHandler = godfatherGamerPrefabInstance.GetComponent<GodfatherGamerHandler>();
			godfatherGamerHandler.FillData(godchildProfile);

			// Add an event item with the newly created GameObject linked to it
			AddEventItem(eventMessage, godfatherGamerPrefabInstance);
		}
		#endregion
	}
}
