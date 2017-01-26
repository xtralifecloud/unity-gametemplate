using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class LoginHandler : MonoBehaviour
	{
		#region Instance Registering
		// Register this MonoBehaviour's instance at Awake to be sure it's done before any Start executes
		private void Awake()
		{
			MonoSingletons.Register<LoginHandler>(this);
		}
		#endregion

		#region Display
		// If the login status is displayed or not
		[SerializeField] private bool displayLoginStatus = true;

		// Reference to the login status UI element
		[SerializeField] private Text loginStatus = null;

		// Texts to display in case of logged out and logged in gamer
		[SerializeField] private string loggedOutText = "Logged out";
		[SerializeField] private string loggedInText = "Logged in as {0}\n({1})";

		// Display a logged out gamer login status at Start
		private void Start()
		{
			UpdateText_LoginStatus();
		}

		// Update the login status text UI element's text (if a gamer is logged in, format the logged in text with gamer's infos)
		public void UpdateText_LoginStatus(Gamer loggedInGamer = null)
		{
			// Update the login status text
			if (displayLoginStatus)
			{
				if (loggedInGamer != null)
					loginStatus.text = string.Format(loggedInText, loggedInGamer["profile"]["displayName"].AsString(), loggedInGamer.GamerId);
				else
					loginStatus.text = loggedOutText;
			}
			// Empty the login status text
			else
				loginStatus.text = "";
		}
		#endregion
	}
}
