using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	public static class TransactionFeatures
	{
		#region Handling
		// Check variables to get and display the current currencies balance
		public static void DisplayBalance()
		{
			// A TransactionHandler instance should be attached to an active object of the scene to display the result
			if (!TransactionHandler.HasInstance)
				Debug.LogError("[CotcSdkTemplate:TransactionFeatures] No TransactionHandler instance found >> Please attach a TransactionHandler script on an active object of the scene");
			else
				Balance(DisplayBalance_OnSuccess, DisplayBalance_OnError);
		}

		// Check variables to get and display the gamer's history of the given currency (or all currencies if null or empty)
		public static void DisplayCurrencyHistory(string currencyName, int currenciesPerPage)
		{
			// A TransactionHandler instance should be attached to an active object of the scene to display the result
			if (!TransactionHandler.HasInstance)
				Debug.LogError("[CotcSdkTemplate:TransactionFeatures] No TransactionHandler instance found >> Please attach a TransactionHandler script on an active object of the scene");
			else
				History(currencyName, currenciesPerPage, 0, DisplayCurrencyHistory_OnSuccess, DisplayCurrencyHistory_OnError);
		}

		// Get and display the previous page of a previously obtained transaction page
		public static void FetchPreviousTransactionPage(PagedList<Transaction> transactions)
		{
			FetchPrevious(transactions, DisplayCurrencyHistory_OnSuccess, DisplayCurrencyHistory_OnError);
		}

		// Get and display the next page of a previously obtained transaction page
		public static void FetchNextTransactionPage(PagedList<Transaction> transactions)
		{
			FetchNext(transactions, DisplayCurrencyHistory_OnSuccess, DisplayCurrencyHistory_OnError);
		}

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
		// Get the current currencies balance
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		private static void Balance(Action<Bundle> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Promise<Bundle> (promising a Bundle result)
			CloudFeatures.gamer.Transactions.Domain(domain).Balance()
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("TransactionFeatures", "Balance", exception);

					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (Bundle currentBalance)
				{
					Debug.Log(string.Format("[CotcSdkTemplate:TransactionFeatures] Balance success >> Current Balance: {0}", currentBalance));

					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(currentBalance);
				});
		}

		// Get the gamer's history of the given currency (or all currencies if null or empty)
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		private static void History(string currencyName, int currenciesPerPage, int currenciesOffset, Action<PagedList<Transaction>> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Promise<PagedList<Transaction>> (promising a PagedList<Transaction> result)
			CloudFeatures.gamer.Transactions.Domain(domain).History(currencyName, currenciesPerPage, currenciesOffset)
				// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
				.Catch(delegate (Exception exception)
				{
					// The exception should always be of the CotcException type
					ExceptionTools.LogCotcException("TransactionFeatures", "History", exception);

					// Call the OnError action if any callback registered to it
					if (OnError != null)
						OnError(ExceptionTools.GetExceptionError(exception));
				})
				// The result if everything went well
				.Done(delegate (PagedList<Transaction> transactionsList)
				{
					Debug.Log(string.Format("[CotcSdkTemplate:TransactionFeatures] History success >> {0} transactions", transactionsList.Count));

					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(transactionsList);
				});
		}

		// Get the previous page of a previously obtained transaction page
		private static void FetchPrevious(PagedList<Transaction> transactions, Action<PagedList<Transaction>> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			if (transactions.HasPrevious)
			{
				// Call the API method which returns a Promise<PagedList<Transaction>> (promising a PagedList<Transaction> result)
				transactions.FetchPrevious()
					// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
					.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						ExceptionTools.LogCotcException("TransactionFeatures", "FetchPrevious", exception);

						// Call the OnError action if any callback registered to it
						if (OnError != null)
							OnError(ExceptionTools.GetExceptionError(exception));
					})
					// The result if everything went well
					.Done(delegate (PagedList<Transaction> transactionsList)
					{
						Debug.Log(string.Format("[CotcSdkTemplate:TransactionFeatures] FetchPrevious success >> {0} transactions", transactionsList.Count));

						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(transactionsList);
					});
			}
			else
				Debug.LogError("[CotcSdkTemplate:TransactionFeatures] There is no previous page");
		}

		// Get the next page of a previously obtained transaction page
		private static void FetchNext(PagedList<Transaction> transactions, Action<PagedList<Transaction>> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			if (transactions.HasNext)
			{
				// Call the API method which returns a Promise<PagedList<Transaction>> (promising a PagedList<Transaction> result)
				transactions.FetchNext()
					// May fail, in which case the .Then or .Done handlers are not called, so you should provide a .Catch handler
					.Catch(delegate (Exception exception)
					{
						// The exception should always be of the CotcException type
						ExceptionTools.LogCotcException("TransactionFeatures", "FetchNext", exception);

						// Call the OnError action if any callback registered to it
						if (OnError != null)
							OnError(ExceptionTools.GetExceptionError(exception));
					})
					// The result if everything went well
					.Done(delegate (PagedList<Transaction> transactionsList)
					{
						Debug.Log(string.Format("[CotcSdkTemplate:TransactionFeatures] FetchNext success >> {0} transactions", transactionsList.Count));

						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(transactionsList);
					});
			}
			else
				Debug.LogError("[CotcSdkTemplate:TransactionFeatures] There is no next page");
		}

		// Post a new transaction of the given currency for the current logged in gamer
		// We use the "private" domain by default (each game has its own data, not shared with the other games)
		private static void Post(Bundle transaction, string description, Action<TransactionResult> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
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
		// What to do if any DisplayBalance request succeeded
		private static void DisplayBalance_OnSuccess(Bundle currentBalance)
		{
			TransactionHandler.Instance.FillAndShowTransactionPanel("Balance", currentBalance.AsDictionary());
		}

		// What to do if any DisplayBalance request failed
		private static void DisplayBalance_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:TransactionFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}

		// What to do if any DisplayCurrencyHistory request succeeded
		private static void DisplayCurrencyHistory_OnSuccess(PagedList<Transaction> transactionsList)
		{
			TransactionHandler.Instance.FillAndShowPagedTransactionPanel("Transactions", transactionsList);
		}

		// What to do if any DisplayCurrencyHistory request failed
		private static void DisplayCurrencyHistory_OnError(ExceptionError exceptionError)
		{
			switch (exceptionError.type)
			{
				// Unhandled error types
				default:
				Debug.LogError(string.Format("[CotcSdkTemplate:TransactionFeatures] An unhandled error occured >> {0}", exceptionError));
				break;
			}
		}

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
