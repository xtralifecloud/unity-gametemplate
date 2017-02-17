using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class LeaderboardScoreHandler : MonoBehaviour
	{
		#region Handling
		// Reference to the leaderboard score GameObject UI elements
		[SerializeField] private Image leaderboardScoreBackground = null;
		[SerializeField] private Text scoreRank = null;
		[SerializeField] private Image gamerAvatar = null;
		[SerializeField] private Text gamerNickname = null;
		[SerializeField] private Text scoreValue = null;
		[SerializeField] private Text scoreInfo = null;
		[SerializeField] private GameObject scoreInfoLine = null;

		// The current gamer score background color
		[SerializeField] private Color gamerScoreBackgroundColor = new Color(1f, 1f, 0.9f, 1f);

		// Texts to display to show the score rank
		private const string rankText = "# {0}";

		// Fill the leaderboard score with new data
		public void FillData(Score score, bool displayScoreInfo = true)
		{
			// Get the gamer info from score's Json
			Bundle gamerInfo = Bundle.FromJson(score.GamerInfo.ToJson());

			// Update fields
			scoreRank.text = string.Format(rankText, score.Rank);
			gamerNickname.text = gamerInfo["profile"]["displayName"].AsString();
			avatarUrlToDownload = gamerInfo["profile"]["avatar"].AsString();
			scoreValue.text = score.Value.ToString();
			scoreInfo.text = score.Info;
			scoreInfoLine.SetActive(displayScoreInfo && !string.IsNullOrEmpty(score.Info));

			// Change the background color to highlight if this is the current gamer's score
			if (gamerInfo["gamer_id"].AsString() == CloudFeatures.gamer.GamerId)
				leaderboardScoreBackground.color = gamerScoreBackgroundColor;
		}
		#endregion

		#region Avatars Downloading
		// Keep the avatar URL to download at Start
		private string avatarUrlToDownload = null;

		// Download avatar from the given URL at Start
		private void Start()
		{
			if (!string.IsNullOrEmpty(avatarUrlToDownload))
				StartCoroutine(UpdateAvatarFromURL());
		}

		// Actually, we need to wait the Start event to download the avatar as coroutines need the GameObject to be started to be launched
		// As we use FillData() just after the LeaderboardScoreHandler Instantiate in LeaderboardHandler, it hasn't gone through an Update yet and is not considered as active
		// TODO: You may want to cache the downloaded avatars to avoid to download them multiple times!
		private IEnumerator UpdateAvatarFromURL()
		{
			// Create a WWW handler and wait for the download request to complete
			WWW www = new WWW(avatarUrlToDownload);
			yield return www;

			// Replace the gamer avatar with the downloaded one if no error occured
			if (string.IsNullOrEmpty(www.error))
				gamerAvatar.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
		}
		#endregion
	}
}
