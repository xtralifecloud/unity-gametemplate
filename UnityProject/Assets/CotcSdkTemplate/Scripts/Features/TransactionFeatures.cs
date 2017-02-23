﻿using System;
using UnityEngine;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to use the CotcSdk's transaction features.
	/// </summary>
	public static class TransactionFeatures
	{
		#region Handling
		/// <summary>
		/// Get and display the current logged in gamer currencies balance.
		/// </summary>
		public static void Handling_DisplayBalance()
		{
			// A TransactionHandler instance should be attached to an active object of the scene to display the result
			if (!TransactionHandler.HasInstance)
				Debug.LogError("[CotcSdkTemplate:TransactionFeatures] No TransactionHandler instance found >> Please attach a TransactionHandler script on an active object of the scene");
			else
				Backend_Balance(DisplayBalance_OnSuccess, DisplayBalance_OnError);
		}

		/// <summary>
		/// Get and display the current logged in gamer's history of the given currency (or all currencies if null or empty).
		/// </summary>
		/// <param name="currencyName">Name of the currency to get.</param>
		/// <param name="transactionsPerPage">Number of transactions to get per page.</param>
		public static void Handling_DisplayCurrencyHistory(string currencyName, int transactionsPerPage)
		{
			// A TransactionHandler instance should be attached to an active object of the scene to display the result
			if (!TransactionHandler.HasInstance)
				Debug.LogError("[CotcSdkTemplate:TransactionFeatures] No TransactionHandler instance found >> Please attach a TransactionHandler script on an active object of the scene");
			else
				Backend_History(currencyName, transactionsPerPage, 0, DisplayCurrencyHistory_OnSuccess, DisplayCurrencyHistory_OnError);
		}

		/// <summary>
		/// Get and display the previous page of a previously obtained transaction page.
		/// </summary>
		/// <param name="transactions">Paged list of transactions.</param>
		public static void Handling_FetchPreviousTransactionPage(PagedList<Transaction> transactions)
		{
			Backend_FetchPrevious(transactions, DisplayCurrencyHistory_OnSuccess, DisplayCurrencyHistory_OnError);
		}

		/// <summary>
		/// Get and display the next page of a previously obtained transaction page.
		/// </summary>
		/// <param name="transactions">Paged list of transactions.</param>
		public static void Handling_FetchNextTransactionPage(PagedList<Transaction> transactions)
		{
			Backend_FetchNext(transactions, DisplayCurrencyHistory_OnSuccess, DisplayCurrencyHistory_OnError);
		}

		/// <summary>
		/// Post a new transaction of the given currency for the current logged in gamer.
		/// </summary>
		/// <param name="currencyName">Name of the currency to set.</param>
		/// <param name="currencyAmount">Amount (positive or negative) of the currency to set.</param>
		/// <param name="transactionDescription">Description of the transaction. (optional)</param>
		public static void Handling_PostTransaction(string currencyName, float currencyAmount, string transactionDescription)
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
				Backend_Post(transaction, transactionDescription, PostTransaction_OnSuccess, PostTransaction_OnError);
			}
		}
		#endregion

		#region Features
		/// <summary>
		/// Get the current logged in gamer currencies balance.
		/// </summary>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_Balance(Action<Bundle> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a Bundle result
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

		/// <summary>
		/// Get the current logged in gamer's history of the given currency (or all currencies if null or empty).
		/// </summary>
		/// <param name="currencyName">Name of the currency to get.</param>
		/// <param name="transactionsPerPage">Number of transactions to get per page.</param>
		/// <param name="transactionsOffset">Number of transactions to skip.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_History(string currencyName, int transactionsPerPage, int transactionsOffset, Action<PagedList<Transaction>> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a PagedList<Transaction> result
			CloudFeatures.gamer.Transactions.Domain(domain).History(currencyName, transactionsPerPage, transactionsOffset)
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
					Debug.Log(string.Format("[CotcSdkTemplate:TransactionFeatures] History success >> {0} transaction(s)", transactionsList.Count));
					
					// Call the OnSuccess action if any callback registered to it
					if (OnSuccess != null)
						OnSuccess(transactionsList);
				});
		}

		/// <summary>
		/// Get the previous page of a previously obtained transaction page.
		/// </summary>
		/// <param name="transactions">Paged list of transactions.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_FetchPrevious(PagedList<Transaction> transactions, Action<PagedList<Transaction>> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			if (transactions.HasPrevious)
			{
				// Call the API method which returns a PagedList<Transaction> result
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
						Debug.Log(string.Format("[CotcSdkTemplate:TransactionFeatures] FetchPrevious success >> {0} transaction(s)", transactionsList.Count));
						
						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(transactionsList);
					});
			}
			else
				Debug.LogError("[CotcSdkTemplate:TransactionFeatures] There is no previous page");
		}

		/// <summary>
		/// Get the next page of a previously obtained transaction page.
		/// </summary>
		/// <param name="transactions">Paged list of transactions.</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		public static void Backend_FetchNext(PagedList<Transaction> transactions, Action<PagedList<Transaction>> OnSuccess = null, Action<ExceptionError> OnError = null)
		{
			if (transactions.HasNext)
			{
				// Call the API method which returns a PagedList<Transaction> result
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
						Debug.Log(string.Format("[CotcSdkTemplate:TransactionFeatures] FetchNext success >> {0} transaction(s)", transactionsList.Count));
						
						// Call the OnSuccess action if any callback registered to it
						if (OnSuccess != null)
							OnSuccess(transactionsList);
					});
			}
			else
				Debug.LogError("[CotcSdkTemplate:TransactionFeatures] There is no next page");
		}

		/// <summary>
		/// Post a new transaction of the given currency for the current logged in gamer.
		/// </summary>
		/// <param name="transaction">Transaction currencies data under the Bundle format.</param>
		/// <param name="transactionDescription">Description of the transaction. (optional)</param>
		/// <param name="OnSuccess">The callback in case of request success.</param>
		/// <param name="OnError">The callback in case of request error.</param>
		/// <param name="domain">We use the "private" domain by default (each game holds its own data, not shared with the other games). You may configure shared domains on your FrontOffice.</param>
		public static void Backend_Post(Bundle transaction, string transactionDescription, Action<TransactionResult> OnSuccess = null, Action<ExceptionError> OnError = null, string domain = "private")
		{
			// Need an initialized Cloud and a logged in gamer to proceed
			if (!CloudFeatures.IsGamerLoggedIn())
				return;
			
			// Call the API method which returns a TransactionResult result
			CloudFeatures.gamer.Transactions.Domain(domain).Post(transaction, transactionDescription)
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
		/// <summary>
		/// What to do if any DisplayBalance request succeeded.
		/// </summary>
		/// <param name="currentBalance">List of current logged in gamer's currencies and their amounts under the Bundle format.</param>
		private static void DisplayBalance_OnSuccess(Bundle currentBalance)
		{
			TransactionHandler.Instance.FillAndShowTransactionPanel(currentBalance.AsDictionary());
		}

		/// <summary>
		/// What to do if any DisplayBalance request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
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

		/// <summary>
		/// What to do if any DisplayCurrencyHistory request succeeded.
		/// </summary>
		/// <param name="transactionsList">List of current logged in gamer's currencies history.</param>
		private static void DisplayCurrencyHistory_OnSuccess(PagedList<Transaction> transactionsList)
		{
			TransactionHandler.Instance.FillAndShowPagedTransactionPanel(transactionsList);
		}

		/// <summary>
		/// What to do if any DisplayCurrencyHistory request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
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

		/// <summary>
		/// What to do if any PostTransaction request succeeded.
		/// </summary>
		/// <param name="postedTransaction">Posted transaction details.</param>
		private static void PostTransaction_OnSuccess(TransactionResult postedTransaction)
		{
			// Do whatever...
		}

		/// <summary>
		/// What to do if any PostTransaction request failed.
		/// </summary>
		/// <param name="exceptionError">Request error details under the ExceptionError format.</param>
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
