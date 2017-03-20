using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed community friend.
	/// </summary>
	public class CommunityFriendHandler : MonoBehaviour
	{
		#region Display
		// Reference to the community friend GameObject UI elements
		[SerializeField] private Image communityFriendBackground = null;
		[SerializeField] private Image friendAvatar = null;
		[SerializeField] private Image loading = null;
		[SerializeField] private Text friendNicknameText = null;
		[SerializeField] private Text friendMessageText = null;

		// The current relationship status background color and status texts
		[SerializeField] private Color friendRelationshipBackgroundColor = new Color(0.9f, 1f, 0.9f, 1f);
		[SerializeField] private Color blacklistRelationshipBackgroundColor = new Color(1f, 0.9f, 0.9f, 1f);
		[SerializeField] private Color forgotRelationshipBackgroundColor = new Color(0.9f, 0.9f, 0.9f, 1f);
		[SerializeField] private string friendRelationshipStatusText = "New friend";
		[SerializeField] private string blacklistRelationshipStatusText = "Blacklisted";
		[SerializeField] private string forgotRelationshipStatusText = "Forgotten";

		/// <summary>
		/// Fill the community friend with new data. (friend message)
		/// </summary>
		/// <param name="friendProfile">Profile of the friend under the Bundle format.</param>
		/// <param name="friendMessage">The message from friend to display.</param>
		public void FillData(Bundle friendProfile, string friendMessage = null)
		{
			// Update fields
			avatarUrlToDownload = friendProfile["avatar"].AsString();
			friendNicknameText.text = friendProfile["displayName"].AsString();
			friendMessageText.text = friendMessage;

			// Display the friend message only if there is one
			friendMessageText.gameObject.SetActive(!string.IsNullOrEmpty(friendMessage));

			// Hide the loading animation and show the friend avatar
			friendAvatar.gameObject.SetActive(true);
			loading.gameObject.SetActive(false);
		}

		/// <summary>
		/// Fill the community friend with new data. (relationship changed)
		/// </summary>
		/// <param name="friendProfile">Profile of the friend under the Bundle format.</param>
		/// <param name="relationship">Type of relationship which has been set.</param>
		public void FillData(Bundle friendProfile, FriendRelationshipStatus relationship)
		{
			// Update fields
			avatarUrlToDownload = friendProfile["avatar"].AsString();
			friendNicknameText.text = friendProfile["displayName"].AsString();

			switch (relationship)
			{
				case FriendRelationshipStatus.Add:
				friendMessageText.text = friendRelationshipStatusText;
				communityFriendBackground.color = friendRelationshipBackgroundColor;
				break;

				case FriendRelationshipStatus.Blacklist:
				friendMessageText.text = blacklistRelationshipStatusText;
				communityFriendBackground.color = blacklistRelationshipBackgroundColor;
				break;

				case FriendRelationshipStatus.Forget:
				friendMessageText.text = forgotRelationshipStatusText;
				communityFriendBackground.color = forgotRelationshipBackgroundColor;
				break;
			}
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
		/// As we use FillData() just after the CommunityFriendHandler Instantiate in CommunityHandler, it hasn't gone through an Update yet and is not considered as active.
		/// </summary>
		private IEnumerator UpdateAvatarFromURL()
		{
			// Show the loading animation and hide the friend avatar while it's downloaded from URL
			friendAvatar.gameObject.SetActive(false);
			loading.gameObject.SetActive(true);

			// TODO: You may want to cache the downloaded avatars to avoid to download them multiple times!
			// Create a WWW handler and wait for the download request to complete
			WWW www = new WWW(avatarUrlToDownload);
			yield return www;

			// Replace the friend avatar with the downloaded one if no error occured and hide the loading animation
			if (string.IsNullOrEmpty(www.error))
			{
				friendAvatar.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
				friendAvatar.gameObject.SetActive(true);
				loading.gameObject.SetActive(false);
			}
		}
		#endregion
	}
}
