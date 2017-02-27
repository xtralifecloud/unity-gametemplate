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
		/// <summary>
		/// Return an exception (expected to be a CotcException) under the ExceptionError format.
		/// </summary>
		/// <param name="exception">The original Exception.</param>
		public static ExceptionError GetExceptionError(Exception exception)
		{
			// The exception should always be of the CotcException type
			CotcException cotcException = exception as CotcException;

			if ((cotcException != null) && (cotcException.ServerData != null))
				return new ExceptionError(cotcException.ServerData["name"].AsString(), cotcException.ServerData["message"].AsString());
			else
				return new ExceptionError("UnknownException", exception.ToString());
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

			if (cotcException != null)
			{
				if ((cotcException.ServerData != null) && !string.IsNullOrEmpty(cotcException.ServerData.ToString()))
					DebugLogs.LogError(string.Format("[CotcSdkTemplate:{0}] {1} exception ›› ({2}) {3}: {4} ›› {5}", className, methodName, cotcException.HttpStatusCode, cotcException.ErrorCode, cotcException.ErrorInformation, cotcException.ServerData));
				else
					DebugLogs.LogError(string.Format("[CotcSdkTemplate:{0}] {1} exception ›› ({2}) {3}: {4}", className, methodName, cotcException.HttpStatusCode, cotcException.ErrorCode, cotcException.ErrorInformation));
			}
			else
				DebugLogs.LogError(string.Format("[CotcSdkTemplate:{0}] {1} exception ›› {2}", className, methodName, exception));
		}
		#endregion
	}
}
