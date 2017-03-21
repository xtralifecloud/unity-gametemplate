using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed godfather gamer.
	/// </summary>
	public class GodfatherGamerHandler : MonoBehaviour
	{
		#region Display
		// Reference to the godfather gamer GameObject UI elements
		[SerializeField] private Image gamerAvatar = null;
		[SerializeField] private Image loading = null;
		[SerializeField] private Text gamerNicknameText = null;
		[SerializeField] private InputField gamerGamerIDInput = null;

		/// <summary>
		/// Fill the godfather gamer with new data.
		/// </summary>
		/// <param name="gamerProfile">Profile of the gamer under the Bundle format.</param>
		/// <param name="gamerGamerID">The gamerID of the gamer to display. (optional)</param>
		public void FillData(Bundle gamerProfile, string gamerGamerID = null)
		{
			// Update fields
			avatarUrlToDownload = gamerProfile["avatar"].AsString();
			gamerNicknameText.text = gamerProfile["displayName"].AsString();
			gamerGamerIDInput.text = gamerGamerID;

			// Hide the loading animation and show the gamer avatar
			gamerAvatar.gameObject.SetActive(true);
			loading.gameObject.SetActive(false);

			// Display the gamer gamerID only if there is one
			gamerGamerIDInput.gameObject.SetActive(!string.IsNullOrEmpty(gamerGamerID));
		}
		#endregion

		#region Avatars Downloading
		// Keep the avatar URL to download it at Start
		private string avatarUrlToDownload = null;

		/// <summary>
		/// Download avatar from the given URL at Start.
		/// </summary>
		private void Start()
		{
			if (!string.IsNullOrEmpty(avatarUrlToDownload))
				StartCoroutine(UpdateAvatarFromURL());
		}

		/// <summary>
		/// Download the avatar image from a URL. Actually, we need to wait the Start event to download the avatar as coroutines need the GameObject to be started to be launched.
		/// As we use FillData() just after the GodfatherGamerHandler Instantiate in GodfatherHandler, it hasn't gone through an Update yet and is not considered as active.
		/// </summary>
		private IEnumerator UpdateAvatarFromURL()
		{
			// Show the loading animation and hide the gamer avatar while it's downloaded from URL
			gamerAvatar.gameObject.SetActive(false);
			loading.gameObject.SetActive(true);

			// TODO: You may want to cache the downloaded avatars to avoid to download them multiple times!
			// Create a WWW handler and wait for the download request to complete
			WWW www = new WWW(avatarUrlToDownload);
			yield return www;

			// Replace the gamer avatar with the downloaded one if no error occured and hide the loading animation
			if (string.IsNullOrEmpty(www.error))
			{
				gamerAvatar.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
				gamerAvatar.gameObject.SetActive(true);
				loading.gameObject.SetActive(false);
			}
		}
		#endregion
	}
}
