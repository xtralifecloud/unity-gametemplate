using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class LoginFeatures
	{
		// Login with an anonymous account
		public static void LoginAnonymously()
		{
			// Need an initialized Cloud to proceed
			if (!CloudFeatures.IsCloudInitialized())
				return;

			// Call the API method which returns a Promise<Gamer> (promising a Gamer result)
			CloudFeatures.cloud.LoginAnonymously()
				// It may fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						CotcException cotcException = exception as CotcException;

						if (cotcException != null)
							Debug.LogError(string.Format("[CotcSdkTemplate:LoginFeatures] Failed to login >> ({0}) {1}: {2} >> {3}", cotcException.HttpStatusCode, cotcException.ErrorCode, cotcException.ErrorInformation, cotcException.ServerData));
						else
							Debug.LogError(string.Format("[CotcSdkTemplate:LoginFeatures] Failed to login >> {0}", exception));
					})
				// The result if everything went well
				.Done(delegate (Gamer loggedInGamer)
					{
						// Keep the Gamer's reference
						CloudFeatures.gamer = loggedInGamer;
						Debug.Log(string.Format("[CotcSdkTemplate:LoginFeatures] Gamer logged in anonymously >> {0}", loggedInGamer));

						// Call the GamerLoggedIn event if any callback registered to it
						if (GamerLoggedIn != null)
							GamerLoggedIn(CloudFeatures.gamer);
					});
		}

		#region Events Callbacks
		// Allow the registration of callbacks for when a gamer has logged in
		public static event Action<Gamer> GamerLoggedIn = null;
		#endregion
	}
}
