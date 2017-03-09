using UnityEngine;
using UnityEngine.UI;

namespace CotcSdkTemplate
{
	/// <summary>
	/// Allow to obtain an animated Image from a sprites sheet.
	/// </summary>
	public class AnimatedSprite : MonoBehaviour
	{
		// Image to animate
		[SerializeField] private Image imageRenderer = null;

		// List of sprites to cycle through to animate the image
		[SerializeField] private Sprite[] spritesList = null;

		// Animation speed
		[SerializeField] private float framesPerSecond = 30f;

		/// <summary>
		/// Update the image sprite when necessary to animate it.
		/// </summary>
		private void Update()
		{
			imageRenderer.sprite = spritesList[(int)(Time.time * framesPerSecond) % spritesList.Length];
		}
	}
}
