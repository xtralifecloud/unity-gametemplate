namespace CotcSdkTemplate
{
	public class JsonExceptionData
	{
		public string name = "";
		public string message = "";

		public override string ToString()
		{
			return "{name: " + name
				+ ", message: " + message
				+ "}";
		}
	}
}
