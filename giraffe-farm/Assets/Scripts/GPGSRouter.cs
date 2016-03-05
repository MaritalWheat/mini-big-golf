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
		Social.ReportScore (coursesPlayed, "CggIhKi7mWoQAhAF", (bool reportSuccess) => {
		});
	}

	//called from UI button
	public void ShowAchievementsUI() {
		Social.Active.ShowAchievementsUI ();
	}

	//called from UI button
	public void ShowLeaderboardsUI() {
		Social.Active.ShowLeaderboardUI ();
	}
}
