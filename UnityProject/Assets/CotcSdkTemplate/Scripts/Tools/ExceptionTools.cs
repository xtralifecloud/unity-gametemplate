using System;
using UnityEngine;

using CotcSdk;
using LitJson;

namespace CotcSdkTemplate
{
	public static class ExceptionTools
	{
		#region Exceptions Handling
		// Return an ExceptionError object from an Exception
		public static ExceptionError GetExceptionError(Exception exception)
		{
			// The exception should always be of the CotcException type
			CotcException cotcException = exception as CotcException;

			if (cotcException != null)
			{
				JsonExceptionData jsonExceptionData = JsonMapper.ToObject<JsonExceptionData>(cotcException.ServerData.ToString());
				return new ExceptionError(jsonExceptionData.name, jsonExceptionData.message);
			}
			else
				return new ExceptionError("UnknownException", exception.ToString());
		}

		// Log an exception into the console
		public static void LogCotcException(string className, string methodName, Exception exception)
		{
			// The exception should always be of the CotcException type
			CotcException cotcException = exception as CotcException;

			if (cotcException != null)
				Debug.LogError(string.Format("[CotcSdkTemplate:{0}] {1} exception >> ({2}) {3}: {4} >> {5}", className, methodName, cotcException.HttpStatusCode, cotcException.ErrorCode, cotcException.ErrorInformation, cotcException.ServerData));
			else
				Debug.LogError(string.Format("[CotcSdkTemplate:{0}] {1} exception >> {2}", className, methodName, exception));
		}
		#endregion
	}
}
