using UnityEngine;

using CotcSdk;
using CotcSdkTemplate;

public class SampleScript : MonoBehaviour
{
	// Initialize the CotcSdk's Cloud at start
	private void Start()
	{
		CloudFeatures.CloudInitialized += OnCloudInitialized;
		CloudFeatures.InitializeCloud();
	}

	// What to do once the CotcSdk's Cloud is initialized
	private void OnCloudInitialized(Cloud cloud)
	{
		LoginFeatures.GamerLoggedIn += OnGamerLoggedIn;
		LoginFeatures.AutoLogin();
	}

	// What to do once a gamer has logged in
	private void OnGamerLoggedIn(Gamer gamer)
	{
		// Do whatever...
	}
}
