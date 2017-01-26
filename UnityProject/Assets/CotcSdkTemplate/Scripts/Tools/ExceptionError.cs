namespace CotcSdkTemplate
{
	public class ExceptionError
	{
		public string type = "";
		public string message = "";

		public ExceptionError(string _type, string _message)
		{
			type = _type;
			message = _message;
		}

		public override string ToString()
		{
			return "{type: " + type
				+ ", message: " + message
				+ "}";
		}
	}
}
