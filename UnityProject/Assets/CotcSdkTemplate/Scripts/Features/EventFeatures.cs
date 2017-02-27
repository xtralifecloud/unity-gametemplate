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

		/// <summary>
		/// Once an event is retrieved from the server, check its type and call the corresponding callback.
		/// </summary>
		/// <param name="sender">The event loop instance which raised the event.</param>
		/// <param name="eventData">Received event's data.</param>
		private static void OnEventReceived(DomainEventLoop sender, EventLoopArgs eventData)
		{
			Bundle eventBundle = eventData.Message;

			if (verboseEventLoop)
				DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:EventFeatures] Received event ›› {0}", eventBundle.ToString()));

			switch (eventBundle["type"].AsString())
			{
				// Event type: message sent from the BackOffice
				case "backoffice":
				DebugLogs.LogVerbose(string.Format("[CotcSdkTemplate:EventFeatures] BackOffice message ›› Event: {0}, Notification: {1}", eventBundle["event"].ToString(), eventBundle["osn"].ToString()));
				break;

				// Unhandled event types
				default:
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:EventFeatures] An unhandled event has been received ›› {0}", eventBundle.ToString()));
				break;
			}
		}
		#endregion
	}
}
