using System.Collections;
using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class LeaderboardGamerScoreHandler : MonoBehaviour
	{
		#region Handling
		// Reference to the leaderboard score GameObject UI elements
		[SerializeField] private GameObject boardNameLine = null;
		[SerializeField] private Text boardName = null;
		[SerializeField] private Text scoreRank = null;
		[SerializeField] private Text scoreValue = null;
		[SerializeField] private Text scoreInfo = null;

		// Texts to display to show the score rank
		private const string rankText = "# {0}";

		// Fill the leaderboard score with new data
		public void FillData(Score score, string scoreBoardName = null, bool displayScoreInfo = true)
		{
			// Display the board name only if there is one
			if (!string.IsNullOrEmpty(scoreBoardName))
			{
				boardNameLine.SetActive(true);
				boardName.text = scoreBoardName;
			}
			else
				boardNameLine.SetActive(false);

			// Update fields
			scoreRank.text = string.Format(rankText, score.Rank);
			scoreValue.text = score.Value.ToString();
			scoreInfo.text = score.Info;
			scoreInfo.gameObject.SetActive(displayScoreInfo && !string.IsNullOrEmpty(score.Info));
		}
		#endregion
	}
}
