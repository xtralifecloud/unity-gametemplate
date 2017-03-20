using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed leaderboard gamer score.
	/// </summary>
	public class LeaderboardGamerScoreHandler : MonoBehaviour
	{
		#region Display
		// Reference to the leaderboard gamer score GameObject UI elements
		[SerializeField] private GameObject boardNameLine = null;
		[SerializeField] private Text boardNameText = null;
		[SerializeField] private Text rankText = null;
		[SerializeField] private Text valueText = null;
		[SerializeField] private Text infoText = null;

		// Text to display to show the score rank
		private const string rankFormat = "# {0}";

		/// <summary>
		/// Fill the leaderboard gamer score with new data.
		/// </summary>
		/// <param name="score">The score details.</param>
		/// <param name="scoreBoardName">Name of the score board.</param>
		/// <param name="displayScoreInfo">If the score description should be shown.</param>
		public void FillData(Score score, string scoreBoardName = null, bool displayScoreInfo = true)
		{
			// Display the board name only if there is one
			if (!string.IsNullOrEmpty(scoreBoardName))
			{
				boardNameLine.SetActive(true);
				boardNameText.text = scoreBoardName;
			}
			else
				boardNameLine.SetActive(false);

			// Update fields
			rankText.text = string.Format(rankFormat, score.Rank);
			valueText.text = score.Value.ToString();
			infoText.text = score.Info;

			// Display the score info only if there is one
			infoText.gameObject.SetActive(displayScoreInfo && !string.IsNullOrEmpty(score.Info));
		}
		#endregion
	}
}
