using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GPGSRouter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// authenticate user:
		GooglePlayGames.PlayGamesPlatform.Activate();
		Social.localUser.Authenticate((bool success) => {
			//mWaitingForAuth = false;
			if (success) {
				Debug.Log("Welcome " + Social.localUser.userName);
				string token = GooglePlayGames.PlayGamesPlatform.Instance.GetToken();
				Debug.Log(token);
			} else {
				Debug.Log("Authentication failed.");
			}
		});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
