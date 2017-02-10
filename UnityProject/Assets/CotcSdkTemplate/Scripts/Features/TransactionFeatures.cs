using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class TransactionFeatures
	{
		#region Handling
		// Check variables to post a new transaction of the given currency for the current logged in gamer
		public static void PostTransaction(string currencyName, float currencyAmount, string transactionDescription)
		{
			// The currency name should not be empty
			if (string.IsNullOrEmpty(currencyName))
				Debug.LogError("[CotcSdkTemplate:TransactionFeatures] The currency name is empty >> Please enter a valid currency name");
			// The currency amount should be different from 0
			else if (currencyAmount == 0)
				Debug.LogError("[CotcSdkTemplate:TransactionFeatures] The currency amount is invalid >> Please enter a number different from 0");
			else
			{
				Bundle transaction = Bundle.CreateObject(currencyName, currencyAmount);
				Post(transaction, transactionDescription, PostTransaction_OnSuccess, PostTransaction_OnError);
			}
		}
		#endregion

		#region Features
		// Post a new transaction of the given currency for the current logged in gamer
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		public static void Post(Bundle transaction, string description, Action<TransactionResult> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;

			// Call the API method which returns a Promise<TransactionResult> (promising a TransactionResult result)
			CloudFeatures.gamer.Transactions.Domain(domain).Post(transaction, description)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						ExceptionTools.LogCotcException("TransactionFeatures", "Post", exception);

						// Call the OnError action if any callback registered to it
						if (OnError != null)
							OnError(ExceptionTools.GetExceptionError(exception));
					})
				// The result if everything went well
				.Done(delegate (TransactionResult postedTransaction)
					{
						Debug.Log(string.Format("[CotcSdkTemplate:TransactionFeatures] Post success >> New Balance: {0}, Triggered Achievements Count: {1}", postedTransaction.Balance, postedTransaction.TriggeredAchievements.Count));

						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(postedTransaction);
					});
		}
		#endregion

		#region Delegate Callbacks
		// What to do if any PostTransaction request succeeded
		private static void PostTransaction_OnSuccess(TransactionResult postedTransaction)
		{
			// Do whatever...
		}

		// What to do if any PostTransaction request failed
		private static void PostTransaction_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:TransactionFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}
		#endregion
	}
}
