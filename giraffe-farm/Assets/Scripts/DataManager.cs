using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {

	public static DataManager Instance;

	private int m_waterDeaths = 0;

	void Start () {
		if (Instance == null) {
			Instance = this;
		}
	}

	public static bool TryGetCourseBestScore(int courseID) {
		return PlayerPrefs.HasKey (courseID.ToString ());
	}

	public static int GetCourseBestScore(int courseID) {
		return PlayerPrefs.GetInt (courseID.ToString ());
	}

	public static void SaveCourseBestScore(int courseID, int score) {
		if (TryGetCourseBestScore (courseID)) {
			if (GetCourseBestScore (courseID) <= score)
				return;
		}

		Debug.Log("Saving: " + courseID + ", " + score);
		PlayerPrefs.SetInt (courseID.ToString (), score);
		UIManager.SetCourseBestScore (courseID);
		Debug.Log("Saved: " + PlayerPrefs.GetInt(courseID.ToString()));
		PlayerPrefs.Save ();
	}

	public static void SaveLastCourse(int courseID) {
		PlayerPrefs.SetInt ("LastCourse", courseID);
		PlayerPrefs.Save ();
	}

	public static int LoadLastCourse() {
		if (!PlayerPrefs.HasKey ("LastCourse")) {
			return 12345;
		} else {
			return PlayerPrefs.GetInt ("LastCourse");
		}
	}

	public static void IncrementCoursesPlayedCount() {
		GPGSRouter.Increment (GPGSIds.achievement_just_a_taste, 1);
		int coursesPlayed = GetCoursesPlayedCount ();
		if (coursesPlayed == 0 || !Instance.GetFirstCourseLogged()) {
			PlayerPrefs.SetInt("FirstCourseLogged", 1);
			GPGSRouter.Unlock(GPGSIds.achievement_first_isnt_the_worst);
		}
		coursesPlayed++;
		PlayerPrefs.SetInt ("CoursesPlayed", coursesPlayed);
		GPGSRouter.ReportCoursePlayed (coursesPlayed);
	}

	public static void IncrementCoursesUnderPar() {
		GPGSRouter.Increment (GPGSIds.achievement_fast_five, 1);
		int coursesUnderPar = Instance.GetCoursesUnderParCount ();
		coursesUnderPar++;
		PlayerPrefs.SetInt ("CoursesPlayed", coursesUnderPar);
		GPGSRouter.ReportCourseUnderPar (coursesUnderPar);
	}

	public static void LogPerfectCourse() {
		GPGSRouter.Unlock (GPGSIds.achievement_the_critics_say__4_stars);
	}

	public static int GetCoursesPlayedCount() {
		if (!PlayerPrefs.HasKey ("CoursesPlayed")) {
			return 0;
		} else {
			return PlayerPrefs.GetInt ("CoursesPlayed");
		}
	}

	public static void LogWaterDeath() {
		Instance.m_waterDeaths++;
		if (Instance.m_waterDeaths >= 5) {
			GPGSRouter.Unlock(GPGSIds.achievement_liquid_aloha);
		}
	}

	public static void Reset() {
		Instance.m_waterDeaths = 0;
	}

	private bool GetFirstCourseLogged() {
		return PlayerPrefs.HasKey("FirstCourseLogged");
	}

	private int GetCoursesUnderParCount() {
		if (!PlayerPrefs.HasKey ("CoursesUnderPar")) {
			return 0;
		} else {
			return PlayerPrefs.GetInt ("CoursesUnderPar");
		}
	}
}
