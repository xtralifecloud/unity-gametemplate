using UnityEngine;

using CotcSdkTemplate;

public class SampleScript : MonoBehaviour
{
	private void Start()
	{
		// Initialize the CotcSdk's Cloud at start
		CloudFeatures.InitializeCloud();
	}
}
