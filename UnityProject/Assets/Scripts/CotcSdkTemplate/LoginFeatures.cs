//using UnityEngine;

//using CotcSdk;

namespace CotcSdkTemplate
{
	public static class LoginFeatures
	{
//		// Signs in with an anonymous account
//		public void DoLogin() {
//			// Call the API method which returns an Promise<Gamer> (promising a Gamer result).
//			// It may fail, in which case the .Then or .Done handlers are not called, so you
//			// should provide a .Catch handler.
//			cloud.LoginAnonymously()
//				.Then(gamer => DidLogin(gamer))
//				.Catch(ex => {
//					// The exception should always be CotcException
//					CotcException error = (CotcException)ex;
//					Debug.LogError("Failed to login: " + error.ErrorCode + " (" + error.HttpStatusCode + ")");
//				});
//		}
//	
//		// Invoked when any sign in operation has completed
//		private void DidLogin(Gamer newGamer)
//		{
//			if (gamer != null) {
//				Debug.LogWarning("Current gamer " + gamer.GamerId + " has been dismissed");
//				eventLoop.Stop();
//			}
//			gamer = newGamer;
//			eventLoop = gamer.StartEventLoop();
//			eventLoop.ReceivedEvent += Loop_ReceivedEvent;
//			Debug.Log("Signed in successfully (ID = " + gamer.GamerId + ")");
//		}
	}
}
