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
	
	public static void Increment(string achievementID, int incrementBy) {
		// increment achievement (achievement ID "Cfjewijawiu_QA") by 5 steps
		PlayGamesPlatform.Instance.IncrementAchievement(
			achievementID, incrementBy, (bool success) => {
			// handle success or failure
		});
	}
	
	public static void Unlock(string achievementID) {
		// unlock achievement (achievement ID "Cfjewijawiu_QA")
		Social.ReportProgress(achievementID, 100.0f, (bool success) => {
			// handle success or failure
		});
	}
	
	public static void ReportCoursePlayed(int coursesPlayed) {
		Social.ReportScore (coursesPlayed, GPGSIds.leaderboard_courses_completed, (bool reportSuccess) => {
		});
	}

	public static void ReportCourseUnderPar(int coursesUnderPar) {
		Social.ReportScore (coursesUnderPar, GPGSIds.leaderboard_courses_under_par, (bool reportSuccess) => {
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
