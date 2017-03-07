using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed event friend item.
	/// </summary>
	public class EventFriendItemHandler : MonoBehaviour
	{
		#region Handling
		// Reference to the event friend item GameObject UI elements
		[SerializeField] private Image eventFriendItemBackground = null;
		[SerializeField] private Image friendAvatar = null;
		[SerializeField] private Text friendNicknameText = null;
		[SerializeField] private Text relationshipStatusText = null;

		// The current relationship status background color and status texts
		[SerializeField] private Color friendRelationshipBackgroundColor = new Color(0.9f, 1f, 0.9f, 1f);
		[SerializeField] private Color blacklistRelationshipBackgroundColor = new Color(1f, 0.9f, 0.9f, 1f);
		[SerializeField] private Color forgotRelationshipBackgroundColor = new Color(0.9f, 0.9f, 0.9f, 1f);
		[SerializeField] private string friendRelationshipStatusText = "New friend";
		[SerializeField] private string blacklistRelationshipStatusText = "Blacklisted";
		[SerializeField] private string forgotRelationshipStatusText = "Forgotten";

		/// <summary>
		/// Fill the event friend item with new data.
		/// </summary>
		/// <param name="friendProfile">Profile of the friend whith who the relationship changed under the Bundle format.</param>
		/// <param name="relationship">Type of relationship which has been set.</param>
		public void FillData(Bundle friendProfile, FriendRelationshipStatus relationship)
		{
			// Update fields
			friendNicknameText.text = friendProfile["displayName"].AsString();
			avatarUrlToDownload = friendProfile["avatar"].AsString();

			switch (relationship)
			{
				case FriendRelationshipStatus.Add:
				eventFriendItemBackground.color = friendRelationshipBackgroundColor;
				relationshipStatusText.text = friendRelationshipStatusText;
				break;

				case FriendRelationshipStatus.Blacklist:
				eventFriendItemBackground.color = blacklistRelationshipBackgroundColor;
				relationshipStatusText.text = blacklistRelationshipStatusText;
				break;

				case FriendRelationshipStatus.Forget:
				eventFriendItemBackground.color = forgotRelationshipBackgroundColor;
				relationshipStatusText.text = forgotRelationshipStatusText;
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
		/// Download the avatar image from an URL. Actually, we need to wait the Start event to download the avatar as coroutines need the GameObject to be started to be launched.
		/// As we use FillData() just after the LeaderboardScoreHandler Instantiate in LeaderboardHandler, it hasn't gone through an Update yet and is not considered as active.
		/// </summary>
		private IEnumerator UpdateAvatarFromURL()
		{
			// TODO: You may want to cache the downloaded avatars to avoid to download them multiple times!
			// Create a WWW handler and wait for the download request to complete
			WWW www = new WWW(avatarUrlToDownload);
			yield return www;

			// Replace the gamer avatar with the downloaded one if no error occured
			if (string.IsNullOrEmpty(www.error))
				friendAvatar.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
		}
		#endregion
	}
}
