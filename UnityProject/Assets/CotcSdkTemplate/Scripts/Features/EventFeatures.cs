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
		private static DomainEventLoop eventLoop = null;

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
				DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:EventFeatures] Registered to gamer's events loop"));
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
					DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:EventFeatures] Unregistered from gamer's events loop"));
			}
		}
		#endregion

		#region Delegate Callbacks
		// If the events loop registering / unregistering and the received events should be logged into the console
		private static bool verboseEventLoop = true;

		// Allow the registration of callbacks for when a BackOffice message is received
		public static event Action<Bundle> Event_BackOfficeMessage = OnBackOfficeMessage;

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
					DebugLogs.LogError("[CotcSdkTemplate:EventFeatures] No callback registered to the Event_BackOfficeMessage event ›› Please ensure an active script registered to it to avoid events loss");
				else
					Event_BackOfficeMessage(eventBundle);
				break;

				// Unhandled event types
				default:
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:EventFeatures] An unhandled event has been received ›› {0}", eventBundle.ToString()));
				break;
			}
		}

		/// <summary>
		/// When a BackOffice message event is received, display it on the event handler.
		/// </summary>
		/// <param name="backOfficeMessage">BackOffice message under the format {"message":"Test message!"}.</param>
		private static void OnBackOfficeMessage(Bundle backOfficeMessage)
		{
			string messageField = "message";

			// An EventHandler instance should be attached to an active object of the scene to display the result
			if (!EventHandler.HasInstance)
				DebugLogs.LogError("[CotcSdkTemplate:EventFeatures] No EventHandler instance found ›› Please attach an EventHandler script on an active object of the scene");
			else if (!backOfficeMessage["event"].Has(messageField))
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:EventFeatures] No {0} field found in the BackOffice message event", messageField));
			else
				EventHandler.Instance.BuildAndAddEventItem_BackOfficeMessage(backOfficeMessage["event"][messageField]);
		}
		#endregion
	}
}
