using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	public class AchievementItemHandler : MonoBehaviour
	{
		#region Handling
		// Reference to the achievement item GameObject UI elements
		[SerializeField] private Image achievementItemBackground = null;
		//[SerializeField] private Image achievementIcon = null;
		[SerializeField] private Text achievementName = null;
		[SerializeField] private Text achievementProgress = null;
		[SerializeField] private GameObject achievementProgressBarLine = null;
		[SerializeField] private LayoutElement progressBarCurrent = null;
		[SerializeField] private LayoutElement progressBarMax = null;

		// The completed achievement background color
		[SerializeField] private Color completedBackgroundColor = new Color(0.9f, 1f, 0.9f, 1f);

		// Texts to display to show the achievement progress
		private const string progressText = "{0}: {1} / {2} ({3}%)";
		private const string completedText = "Completed!";
		private const string uncompletedText = "Uncompleted...";
		private const string floatStringFormat = "0.##";

		// Fill the leaderboard score with new data
		// TODO: You may want to replace the default achievement icons by your own ones, according to achievements names
		public void FillData(AchievementDefinition achievement)
		{
			// Update fields
			achievementName.text = achievement.Name;

			// If the achievement is completed, display it
			if (achievement.Progress == 1f)
			{
				achievementProgressBarLine.SetActive(false);
				achievementProgress.text = completedText;
				achievementItemBackground.color = completedBackgroundColor;
			}
			// Else, display a progress bar if the achievement progression is not of one-shot type
			else
			{
				if (achievement.Config["maxValue"].AsFloat() == 1f)
				{
					achievementProgressBarLine.SetActive(false);
					achievementProgress.text = uncompletedText;
				}
				else
				{
					achievementProgressBarLine.SetActive(true);
					achievementProgress.text = GetAchievementProgress(achievement);
					progressBarCurrent.flexibleWidth = achievement.Progress;
					progressBarMax.flexibleWidth = 1f - achievement.Progress;
				}
			}
		}

		// Format an achievement progress text
		private string GetAchievementProgress(AchievementDefinition achievement)
		{
			float currentProgress = achievement.Progress * achievement.Config["maxValue"].AsFloat();
			int currentProgressPercent = Mathf.FloorToInt(achievement.Progress * 100f);

			return string.Format(progressText, achievement.Config["unit"].AsString(), currentProgress.ToString(floatStringFormat), achievement.Config["maxValue"].AsString(floatStringFormat), currentProgressPercent.ToString());
		}
		#endregion
	}
}
