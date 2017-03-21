using System;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to use the CotcSdk's event features.
	/// </summary>
	public static class EventFeatures
	{
		#region Handling
		// The object handling the events loop to retrieve server pending events
		public static DomainEventLoop eventLoop = null;

		// If the events loop registering / unregistering and the received events should be logged into the console
		private static bool verboseEventLoop = true;

		/// <summary>
		/// Start the events loop to retrieve server pending events as soon as possible.
		/// </summary>
		/// <param name="loggedInGamer">The logged in gamer's instance.</param>
		/// <param name="verbose">If the events loop registering / unregistering and the received events should be logged into the console.</param>
		public static void Handling_StartEventsListening(Gamer loggedInGamer, bool verbose = true)
		{
			// Start the events loop from the currently logged in gamer then register a received event callback
			eventLoop = loggedInGamer.StartEventLoop();
			eventLoop.ReceivedEvent += OnEventReceived;

			// Set if the events handling should be verbose
			verboseEventLoop = verbose;

			if (verboseEventLoop)
				DebugLogs.LogVerbose("[CotcSdkTemplate:EventFeatures] Registered to gamer's events loop");
		}

		/// <summary>
		/// Stop the events loop to retrieve server pending events. A stopped events loop shouldn't be started again.
		/// </summary>
		public static void Handling_StopEventsListening()
		{
			// Stop the events loop
			if (eventLoop != null)
			{
				eventLoop.Stop();
				eventLoop = null;

				if (verboseEventLoop)
					DebugLogs.LogVerbose("[CotcSdkTemplate:EventFeatures] Unregistered from gamer's events loop");
			}
		}
		#endregion

		#region Delegate Callbacks
		// String format to describe a soon deprecated event
		private static string soonDeprecatedEvent = "[CotcSdkTemplate:EventFeatures] The {0} event type may be deprecated soon ›› If you want to handle it, using gamer.Community.SendEvent() to send your own custom event is safer";

		// String format to describe no callback registered for a given event
		private const string noEventCallbackFormat = "[CotcSdkTemplate:EventFeatures] No callback registered to the {0} event ›› Please ensure an active script registered to it to prevent events loss";

		/// <summary>
		/// Once an event is retrieved from the server, check its type and call the corresponding callback.
		/// </summary>
		/// <param name="sender">The event loop instance which raised the event.</param>
		/// <param name="eventData">Received event's data.</param>
		private static void OnEventReceived(DomainEventLoop sender, EventLoopArgs eventData)
		{
			// Get the Bundle data from the raised event
			Bundle eventBundle = eventData.Message;

			if (verboseEventLoop)
				DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:EventFeatures] Received event ›› {0}", eventBundle.ToString()));

			switch (eventBundle["type"].AsString())
			{
				// Event type: message sent from the BackOffice
				case "backoffice":
				if (Event_BackOfficeMessage == null)
					DebugLogs.LogError(string.Format(noEventCallbackFormat, "Event_BackOfficeMessage"));
				else
					Event_BackOfficeMessage(eventBundle);
				break;

				// Event type: another gamer added the currently logged in one as friend (automatic)
				case "friend.add":
				DebugLogs.LogWarning(string.Format(soonDeprecatedEvent, "friend.add"));
				break;

				// Event type: another gamer added the currently logged in one as blacklisted (automatic)
				case "friend.blacklist":
				DebugLogs.LogWarning(string.Format(soonDeprecatedEvent, "friend.blacklist"));
				break;

				// Event type: another gamer removed the currently logged in one from friend/blacklisted (automatic)
				case "friend.forget":
				DebugLogs.LogWarning(string.Format(soonDeprecatedEvent, "friend.forget"));
				break;

				// Event type: another gamer used the currently logged in one's referral code (automatic)
				case "godchildren":
				if (Event_NewGodchild == null)
					DebugLogs.LogError(string.Format(noEventCallbackFormat, "Event_NewGodchild"));
				else
					Event_NewGodchild(eventBundle);
				break;

				// Event type: message sent from another gamer (our "custom" events sent by gamer.Community.SendEvent() are of this type)
				case "user":
				OnCustomEventReceived(eventBundle);
				break;

				// Unhandled event types
				default:
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:EventFeatures] An unhandled event has been received ›› {0}", eventBundle.ToString()));
				break;
			}
		}

		/// <summary>
		/// Once a "custom" event (sent by gamer.Community.SendEvent()) is retrieved from the server, check its type and call the corresponding callback.
		/// </summary>
		/// <param name="eventBundle">Received event's data under Bundle format.</param>
		private static void OnCustomEventReceived(Bundle eventBundle)
		{
			// Get the custom event data under Bundle format from string json format
			Bundle customEvent = Bundle.FromJson(eventBundle["event"]);

			switch (customEvent["type"].AsString())
			{
				// Event type: another gamer sent a message to the currently logged in one (custom)
				case "friend_message":
				if (Event_FriendMessage == null)
					DebugLogs.LogError(string.Format(noEventCallbackFormat, "Event_FriendMessage"));
				else
					Event_FriendMessage(customEvent);
				break;

				// Event type: another gamer added the currently logged in one as friend (custom)
				case "friend_add":
				if (Event_FriendRelationshipChanged == null)
					DebugLogs.LogError(string.Format(noEventCallbackFormat, "Event_FriendRelationshipChanged"));
				else
					Event_FriendRelationshipChanged(customEvent, FriendRelationshipStatus.Add);
				break;

				// Event type: another gamer added the currently logged in one as blacklisted (custom)
				case "friend_blacklist":
				if (Event_FriendRelationshipChanged == null)
					DebugLogs.LogError(string.Format(noEventCallbackFormat, "Event_FriendRelationshipChanged"));
				else
					Event_FriendRelationshipChanged(customEvent, FriendRelationshipStatus.Blacklist);
				break;

				// Event type: another gamer removed the currently logged in one from friend/blacklisted (custom)
				case "friend_forget":
				if (Event_FriendRelationshipChanged == null)
					DebugLogs.LogError(string.Format(noEventCallbackFormat, "Event_FriendRelationshipChanged"));
				else
					Event_FriendRelationshipChanged(customEvent, FriendRelationshipStatus.Forget);
				break;

				// Unhandled custom event types
				default:
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:EventFeatures] An unhandled custom event has been received ›› {0}", customEvent.ToString()));
				break;
			}
		}
		#endregion

		#region Events Callbacks
		// Allow the registration of callbacks to call for different received events
		public static event Action<Bundle> Event_BackOfficeMessage = OnBackOfficeMessage;
		public static event Action<Bundle> Event_FriendMessage = OnFriendMessage;
		public static event Action<Bundle, FriendRelationshipStatus> Event_FriendRelationshipChanged = OnFriendRelationshipChanged;
		public static event Action<Bundle> Event_NewGodchild = OnNewGodchild;

		/// <summary>
		/// When a "BackOffice message" event is received, display it on the event handler.
		/// </summary>
		/// <param name="eventData">Event details under the expected format {"type":"...","event":{"message":"..."}}.</param>
		private static void OnBackOfficeMessage(Bundle eventData)
		{
			// An EventHandler instance should be attached to an active object of the scene to display the result
			if (!EventHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "EventFeatures", "EventHandler"));
			else
			{
				string backOfficeMessage = eventData["event"]["message"].AsString();
				EventHandler.Instance.BuildAndAddEventItem_BackOfficeMessage(backOfficeMessage);
			}
		}

		/// <summary>
		/// When a "friend's message" event is received, display it on the event handler.
		/// </summary>
		/// <param name="eventData">Event details under the expected format {"type":"...","message":"...","friendMessage":"...","friendProfile":{...}}.</param>
		private static void OnFriendMessage(Bundle eventData)
		{
			// An EventHandler instance should be attached to an active object of the scene to display the result
			if (!EventHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "EventFeatures", "EventHandler"));
			else
			{
				string message = eventData["message"].AsString();
				string friendMessage = eventData["friendMessage"].AsString();
				Bundle friendProfile = Bundle.FromJson(eventData["friendProfile"]);
				EventHandler.Instance.BuildAndAddEventItem_FriendMessage(message, friendProfile, friendMessage);
			}
		}

		/// <summary>
		/// When a "friend's relationship changed" event is received, display it on the event handler.
		/// </summary>
		/// <param name="eventData">Event details under the expected format {"type":"...","message":"...","friendProfile":{...}}.</param>
		/// <param name="relationship">Type of relationship which has been set.</param>
		private static void OnFriendRelationshipChanged(Bundle eventData, FriendRelationshipStatus relationship)
		{
			// An EventHandler instance should be attached to an active object of the scene to display the result
			if (!EventHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "EventFeatures", "EventHandler"));
			else
			{
				string message = eventData["message"].AsString();
				Bundle friendProfile = Bundle.FromJson(eventData["friendProfile"]);
				EventHandler.Instance.BuildAndAddEventItem_FriendRelationshipChanged(message, friendProfile, relationship);
			}
		}

		/// <summary>
		/// When a "new godchild" event is received, display it on the event handler.
		/// </summary>
		/// <param name="eventData">Event details under the expected format {"type":"...","event":{"godchildren":{"gamer_id":"...","profile":{...}}}}.</param>
		private static void OnNewGodchild(Bundle eventData)
		{
			// An EventHandler instance should be attached to an active object of the scene to display the result
			if (!EventHandler.HasInstance)
				DebugLogs.LogError(string.Format(ExceptionTools.noInstanceErrorFormat, "EventFeatures", "EventHandler"));
			else
			{
				Bundle godchildProfile = eventData["event"]["godchildren"]["profile"];
				EventHandler.Instance.BuildAndAddEventItem_NewGodchild("You've got a new godchild!", godchildProfile);
			}
		}
		#endregion
	}
}
