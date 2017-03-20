using UnityEngine;
using UnityEngine.UI;

using CotcSdk;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Methods to fill a displayed achievement item.
	/// </summary>
	public class AchievementItemHandler : MonoBehaviour
	{
		#region Display
		// Reference to the achievement item GameObject UI elements
		[SerializeField] private Image achievementItemBackground = null;
		//[SerializeField] private Image achievementIcon = null;
		[SerializeField] private Text nameText = null;
		[SerializeField] private Text progressText = null;
		[SerializeField] private GameObject achievementProgressBarLine = null;
		[SerializeField] private LayoutElement progressBarCurrent = null;
		[SerializeField] private LayoutElement progressBarMax = null;

		// The completed achievement background color
		[SerializeField] private Color completedBackgroundColor = new Color(0.9f, 1f, 0.9f, 1f);

		// Texts to display to show the achievement progress
		private const string progressFormat = "{0}: {1} / {2} ({3}%)";
		private const string completedText = "Completed!";
		private const string uncompletedText = "Uncompleted...";
		private const string floatStringFormat = "0.##";

		/// <summary>
		/// Fill the achievement item with new data.
		/// </summary>
		/// <param name="achievement">The achievement details.</param>
		public void FillData(AchievementDefinition achievement)
		{
			// TODO: You may want to replace the default achievement icons by your own ones, according to achievements names
			// Update fields
			nameText.text = achievement.Name;

			// If the achievement is completed, display it
			if (achievement.Progress == 1f)
			{
				achievementProgressBarLine.SetActive(false);
				progressText.text = completedText;
				achievementItemBackground.color = completedBackgroundColor;
			}
			// Else, display a progress bar if the achievement progression is not of one-shot type
			else
			{
				if (achievement.Config["maxValue"].AsFloat() == 1f)
				{
					achievementProgressBarLine.SetActive(false);
					progressText.text = uncompletedText;
				}
				else
				{
					achievementProgressBarLine.SetActive(true);
					progressText.text = GetAchievementProgress(achievement);
					progressBarCurrent.flexibleWidth = achievement.Progress;
					progressBarMax.flexibleWidth = 1f - achievement.Progress;
				}
			}
		}
		#endregion

		#region Achievement Progress Formating
		/// <summary>
		/// Format an achievement progress text.
		/// </summary>
		/// <param name="achievement">The achievement details.</param>
		private string GetAchievementProgress(AchievementDefinition achievement)
		{
			float currentProgress = achievement.Progress * achievement.Config["maxValue"].AsFloat();
			int currentProgressPercent = Mathf.FloorToInt(achievement.Progress * 100f);

			return string.Format(progressFormat, achievement.Config["unit"].AsString(), currentProgress.ToString(floatStringFormat), achievement.Config["maxValue"].AsString(floatStringFormat), currentProgressPercent.ToString());
		}
		#endregion
	}
}
