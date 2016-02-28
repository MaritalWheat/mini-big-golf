using UnityEngine;
using System.Collections;

public class DataManager : MonoBehaviour {

	public static DataManager Instance;

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
}
