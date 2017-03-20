using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed community user.
	/// </summary>
	public class CommunityUserHandler : MonoBehaviour
	{
		#region Display
		// Reference to the community user GameObject UI elements
		[SerializeField] private Image userAvatar = null;
		[SerializeField] private Image loading = null;
		[SerializeField] private Text userNicknameText = null;
		[SerializeField] private InputField userGamerIDInput = null;

		/// <summary>
		/// Fill the community user with new data.
		/// </summary>
		/// <param name="userGamerID">The gamerID of the user to display.</param>
		/// <param name="userProfile">Profile of the user under the Bundle format.</param>
		public void FillData(string userGamerID, Bundle userProfile)
		{
			// Update fields
			avatarUrlToDownload = userProfile["avatar"].AsString();
			userNicknameText.text = userProfile["displayName"].AsString();
			userGamerIDInput.text = userGamerID;

			// Hide the loading animation and show the user avatar
			userAvatar.gameObject.SetActive(true);
			loading.gameObject.SetActive(false);
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
		/// As we use FillData() just after the CommunityUserHandler Instantiate in CommunityHandler, it hasn't gone through an Update yet and is not considered as active.
		/// </summary>
		private IEnumerator UpdateAvatarFromURL()
		{
			// Show the loading animation and hide the user avatar while it's downloaded from URL
			userAvatar.gameObject.SetActive(false);
			loading.gameObject.SetActive(true);

			// TODO: You may want to cache the downloaded avatars to avoid to download them multiple times!
			// Create a WWW handler and wait for the download request to complete
			WWW www = new WWW(avatarUrlToDownload);
			yield return www;

			// Replace the user avatar with the downloaded one if no error occured and hide the loading animation
			if (string.IsNullOrEmpty(www.error))
			{
				userAvatar.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
				userAvatar.gameObject.SetActive(true);
				loading.gameObject.SetActive(false);
			}
		}
		#endregion
	}
}
