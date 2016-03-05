using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GPGSRouter : MonoBehaviour {

	private static GPGSRouter Instance;


	void Start () {
		if (Instance == null) {
			Instance = this;
		}

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
	


	public static void ReportCoursePlayed(int coursesPlayed) {
		// post score 12345 to leaderboard ID "Cfji293fjsie_QA")
		//Debug.Log ("User authenticated - leaderboard": + Social.localUser.authenticated);
		GooglePlayGames.PlayGamesPlatform.Activate();

		Social.ReportScore (coursesPlayed, "CggIhKi7mWoQAhAF", (bool reportSuccess) => {
			PlayGamesPlatform.Instance.ShowLeaderboardUI("CggIhKi7mWoQAhAF");
		});
	}
}
