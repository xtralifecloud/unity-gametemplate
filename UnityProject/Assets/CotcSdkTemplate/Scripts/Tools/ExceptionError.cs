namespace CotcSdkTemplate
{
	/// <summary>
	/// Represents an exception error with its type and message data.
	/// </summary>
	public class ExceptionError
	{
		// Format to display the error data under string Format
		private const string toStringFormat = "[type: {0}, message: {1}]";

		// Error data
		public string type = "";
		public string message = "";

		/// <summary>
		/// Initialize a new instance of the ExceptionError class.
		/// </summary>
		/// <param name="_type">The error type.</param>
		/// <param name="_message">The error description.</param>
		public ExceptionError(string _type, string _message)
		{
			type = _type;
			message = _message;
		}

		/// <summary>
		/// Return a string that represents the current object's fields values.
		/// </summary>
		public override string ToString()
		{
			return string.Format(toStringFormat, type, message);
		}
	}
}
