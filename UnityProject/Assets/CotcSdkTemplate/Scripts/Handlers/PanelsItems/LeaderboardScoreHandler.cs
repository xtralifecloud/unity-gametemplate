using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed leaderboard score.
	/// </summary>
	public class LeaderboardScoreHandler : MonoBehaviour
	{
		#region Display
		// Reference to the leaderboard score GameObject UI elements
		[SerializeField] private Image leaderboardScoreBackground = null;
		[SerializeField] private Text rankText = null;
		[SerializeField] private Image gamerAvatar = null;
		[SerializeField] private Image loading = null;
		[SerializeField] private Text gamerNicknameText = null;
		[SerializeField] private Text valueText = null;
		[SerializeField] private Text infoText = null;
		[SerializeField] private GameObject scoreInfoLine = null;

		// The current gamer score background color
		[SerializeField] private Color gamerScoreBackgroundColor = new Color(1f, 1f, 0.9f, 1f);

		// Text to display to show the score rank
		private const string rankFormat = "# {0}";

		/// <summary>
		/// Fill the leaderboard score with new data.
		/// </summary>
		/// <param name="score">The score details.</param>
		/// <param name="displayScoreInfo">If the score description should be shown.</param>
		public void FillData(Score score, bool displayScoreInfo = true)
		{
			// Get the gamer info from score's Json
			Bundle gamerInfo = Bundle.FromJson(score.GamerInfo.ToJson());

			// Update fields
			rankText.text = string.Format(rankFormat, score.Rank);
			gamerNicknameText.text = gamerInfo["profile"]["displayName"].AsString();
			avatarUrlToDownload = gamerInfo["profile"]["avatar"].AsString();
			valueText.text = score.Value.ToString();
			infoText.text = score.Info;

			// Hide the loading animation and show the gamer avatar
			gamerAvatar.gameObject.SetActive(true);
			loading.gameObject.SetActive(false);

			// Display the score info only if there is one
			scoreInfoLine.SetActive(displayScoreInfo && !string.IsNullOrEmpty(score.Info));

			// Change the background color to highlight if this is the current gamer's score
			if (gamerInfo["gamer_id"].AsString() == LoginFeatures.gamer.GamerId)
				leaderboardScoreBackground.color = gamerScoreBackgroundColor;
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
		/// As we use FillData() just after the LeaderboardScoreHandler Instantiate in LeaderboardHandler, it hasn't gone through an Update yet and is not considered as active.
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
