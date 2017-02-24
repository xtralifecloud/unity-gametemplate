﻿using UnityEngine;

namespace CotcSdkTemplate
{
	// Enum of available cumulative logging levels
	public enum LogLevel { None, Error, Warning, Verbose }

	/// <summary>
	/// Methods to handle console logs.
	/// </summary>
	public static class DebugLogs
	{
		#region Logs Handling
		// Current logging level allowed (not allowed logs won't display)
		public static LogLevel logLevel = LogLevel.Verbose;

		/// <summary>
		/// Log an error message to console.
		/// </summary>
		/// <param name="message">Message to log.</param>
		/// <param name="context">The involved object reference. (optional)</param>
		public static void LogError(object message, Object context = null)
		{
			if (logLevel >= LogLevel.Error)
				Debug.LogError(message, context);
		}

		/// <summary>
		/// Log a warning message to console.
		/// </summary>
		/// <param name="message">Message to log.</param>
		/// <param name="context">The involved object reference. (optional)</param>
		public static void LogWarning(object message, Object context = null)
		{
			if (logLevel >= LogLevel.Warning)
				Debug.LogWarning(message, context);
		}

		/// <summary>
		/// Log a verbose message to console.
		/// </summary>
		/// <param name="message">Message to log.</param>
		/// <param name="context">The involved object reference. (optional)</param>
		public static void LogVerbose(object message, Object context = null)
		{
			if (logLevel >= LogLevel.Verbose)
				Debug.Log(message, context);
		}
		#endregion
	}
}
