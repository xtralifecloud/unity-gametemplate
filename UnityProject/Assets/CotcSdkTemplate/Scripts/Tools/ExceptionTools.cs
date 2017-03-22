using System;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to handle exception errors.
	/// </summary>
	public static class ExceptionTools
	{
		#region Exceptions Handling
		// Error types corresponding to different cases
		public const string keyNotFoundErrorType = "KeyNotFound";
		public const string missingScoreErrorType = "MissingScore";
		public const string notInitializedCloudErrorType = "NotInitializedCloud";
		public const string notLoggedInErrorType = "NotLoggedIn";

		// Error messages corresponding to different cases
		public const string noInstanceErrorFormat = "[CotcSdkTemplate:{0}] No {1} instance found ›› Please attach a {1} script on an active object of the scene";
		public const string unhandledErrorFormat = "[CotcSdkTemplate:{0}] An unhandled error occured ›› {1}";

		// Error formats corresponding to different cases
		public const string notInitializedCloudMessage = "You need an initialized Cloud\ninstance to use this feature.";
		public const string notLoggedInMessage = "You need be logged in\nto use this feature.";
		public const string unhandledErrorMessage = "An unhandled error occured.\n(please check console logs)";

		/// <summary>
		/// Return an exception (expected to be a CotcException) under the ExceptionError format.
		/// </summary>
		/// <param name="exception">The original Exception.</param>
		/// <param name="exceptionType">To set the exception type in case the exception is not of CotcException type or doesn't contain any server data. (optional)</param>
		public static ExceptionError GetExceptionError(Exception exception, string exceptionType = null)
		{
			// The exception should always be of the CotcException type
			CotcException cotcException = exception as CotcException;

			if ((cotcException != null) && (cotcException.ServerData != null))
				return new ExceptionError(cotcException.ServerData["name"].AsString(), cotcException.ServerData["message"].AsString());
			else
				return new ExceptionError(string.IsNullOrEmpty(exceptionType) ? "UnknownException" : exceptionType, exception.ToString());
		}

		/// <summary>
		/// Log an exception (expected to be a CotcException) into the console.
		/// </summary>
		/// <param name="className">Name of the involved class.</param>
		/// <param name="methodName">Name of the involved method.</param>
		/// <param name="exception">The original Exception.</param>
		public static void LogCotcException(string className, string methodName, Exception exception)
		{
			// The exception should always be of the CotcException type
			CotcException cotcException = exception as CotcException;

			if (cotcException == null)
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:{0}] {1} exception ›› {2}", className, methodName, exception));
			else if ((cotcException.ServerData != null) && !string.IsNullOrEmpty(cotcException.ServerData.ToString()))
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:{0}] {1} exception ›› ({2}) {3}: {4} ›› {5}", className, methodName, cotcException.HttpStatusCode, cotcException.ErrorCode, cotcException.ErrorInformation, cotcException.ServerData));
			else
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:{0}] {1} exception ›› ({2}) {3}: {4}", className, methodName, cotcException.HttpStatusCode, cotcException.ErrorCode, cotcException.ErrorInformation));
		}
		#endregion
	}
}
