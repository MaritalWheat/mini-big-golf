using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIManager : MonoBehaviour {

	private static UIManager Instance;

	public Text m_coursePar;
	public Text m_hitCounter;
	public Text m_bestScore;
	public Text m_postGameHitCount;
	public Text m_postGameTimeStat;
	public Text m_title;

	public Button m_startGameButton;
	public Button m_pauseGameButton;
	public Button m_achievementsButton;
	public Button m_leaderboardsButton;

	public GameObject m_postCourseStats;
	public GameObject m_cameraNavigatorAnchor;
	public GameObject m_pauseMenu;
	public GameObject m_powerBar;
	public List<GameObject> m_starsFilled = new List<GameObject>();
	public List<GameObject> m_starsEmpty = new List<GameObject>();

	private bool m_displayCameraNavigatorAnchor;
	
	void Start () {
		if (Instance == null) {
			Instance = this;
		}

		float referenceWidth = 1920.0f;
		float currentWidth = Screen.width;

		Canvas canvas = FindObjectOfType<Canvas> ();
		canvas.scaleFactor = canvas.scaleFactor * (currentWidth / referenceWidth);
		Debug.Log ("Resolution:" + " " + Screen.height + "x" + Screen.width);
	}

	public static void UpdateHits(int newHitCount) {
		int score = newHitCount - CourseCreator.CoursePar;
		if (score > 0) {
			Instance.m_hitCounter.text = "SCORE: +" + (score);
		} else {
			Instance.m_hitCounter.text = "SCORE: " + (score);
		}
	}

	public static void SetCoursePar(int coursePar) {
		Instance.m_coursePar.text = "PAR: " + coursePar;
	}

	public static void SetCourseBestScore(int courseID) {
		if (!DataManager.TryGetCourseBestScore (courseID)) {
			Instance.m_bestScore.text = "BEST: --";
		} else {
			int score = DataManager.GetCourseBestScore(courseID);
			if (score > 0) {
				Instance.m_bestScore.text = "BEST: +" + (score);
			} else {
				Instance.m_bestScore.text = "BEST: " + (score);
			}
		}
	}

	public static void SetupPregame() {
		Instance.m_hitCounter.gameObject.SetActive (false);
		Instance.m_pauseGameButton.gameObject.SetActive (false);
		Instance.m_cameraNavigatorAnchor.gameObject.SetActive (false);
		Instance.m_postCourseStats.gameObject.SetActive (false);
		Instance.m_pauseMenu.gameObject.SetActive (false);
		Instance.m_powerBar.gameObject.SetActive (false);
		Instance.m_coursePar.gameObject.SetActive (true);
		Instance.m_bestScore.gameObject.SetActive (true);
		Instance.m_startGameButton.gameObject.SetActive (true);
		Instance.m_title.gameObject.SetActive (true);
		Instance.m_achievementsButton.gameObject.SetActive (true);
		Instance.m_leaderboardsButton.gameObject.SetActive (true);
	}

	public static void OnStart() {
		Instance.m_startGameButton.gameObject.SetActive (false);
		Instance.m_title.gameObject.SetActive (false);
		Instance.m_coursePar.gameObject.SetActive (false);
		Instance.m_bestScore.gameObject.SetActive (false);
		Instance.m_achievementsButton.gameObject.SetActive (false);
		Instance.m_leaderboardsButton.gameObject.SetActive (false);
		Instance.m_cameraNavigatorAnchor.gameObject.SetActive (true);
		Instance.m_hitCounter.gameObject.SetActive (true);
		Instance.m_pauseGameButton.gameObject.SetActive (true);
		Instance.m_powerBar.gameObject.SetActive (true);
	}

	public static void SetupPostGameImmediate() {
		Instance.m_cameraNavigatorAnchor.gameObject.SetActive (false);
	}

	public static void SetupPostGame(GameManager.GameStats gameStats) {
		Instance.m_hitCounter.gameObject.SetActive (false);
		Instance.m_powerBar.gameObject.SetActive (false);
		Instance.m_pauseGameButton.gameObject.SetActive (false);
		Instance.m_postCourseStats.gameObject.SetActive (true);

		//make sure all grey stars are reset
		for (int i = 0; i < Instance.m_starsEmpty.Count; i++) {
			Instance.m_starsEmpty[i].SetActive(true);
		}

		//set stats
		int score = gameStats.GetNumHits () - CourseCreator.CoursePar;
		DataManager.SaveCourseBestScore (CourseCreator.CourseID, score);
		string scoreText = "";
		if (score > 0) {
			scoreText += "+" + score;
		} else {
			DataManager.IncrementCoursesUnderPar();
			scoreText += score;
		}
		Instance.m_postGameHitCount.text = scoreText;
		Instance.m_postGameTimeStat.text = Math.Round((double)(gameStats.GetTimePlayed ()), 2).ToString();

		//set star score - should maybe move the logic somewhere else besides the UI class?
		int starScore = 0;

		if (score < 0) {
			starScore += 2;
		} else if (score == 0) {
			starScore += 1;
		}

		if (gameStats.GetTimePlayed() < ((float)(CourseCreator.Course.Count) * 2.0f)) {
			starScore += 2;
		} else if (gameStats.GetTimePlayed() < ((float)(CourseCreator.Course.Count) * 5.0f)) {
			starScore += 1;
		}

		if (starScore >= 4) {
			starScore = 4;
			DataManager.LogPerfectCourse();
		} else if (starScore == 0) {
			starScore = 1;
		}

		for (int i = 0; i < starScore; i++) {
			Instance.m_starsEmpty[i].SetActive(false);
		}
	}

	public static void OnReset() {
		Instance.m_pauseMenu.gameObject.SetActive (false);
		Instance.m_postCourseStats.gameObject.SetActive (false);
		Instance.m_pauseGameButton.gameObject.SetActive (true);
		Instance.m_hitCounter.gameObject.SetActive (true);
		Instance.m_cameraNavigatorAnchor.gameObject.SetActive (true);
		Instance.m_powerBar.gameObject.SetActive (true);
	}

	public static void OnPause() {
		Instance.m_cameraNavigatorAnchor.gameObject.SetActive (false);
		Instance.m_hitCounter.gameObject.SetActive (false);
		Instance.m_pauseGameButton.gameObject.SetActive (false);
		Instance.m_powerBar.gameObject.SetActive (false);
		Instance.m_pauseMenu.gameObject.SetActive (true);
	}

	public static void OnUnpause() {
		Instance.m_pauseMenu.gameObject.SetActive (false);
		Instance.m_cameraNavigatorAnchor.gameObject.SetActive (Instance.m_displayCameraNavigatorAnchor);
		Instance.m_hitCounter.gameObject.SetActive (true);
		Instance.m_pauseGameButton.gameObject.SetActive (true);
		Instance.m_powerBar.gameObject.SetActive (true);
	}

	public static void DisplayBallControls(bool showControls) {
		if (GameManager.CurrentGameState == GameManager.GameState.Ended)
			return;
		Instance.m_cameraNavigatorAnchor.gameObject.SetActive (showControls);
		//Instance.m_powerBar.gameObject.SetActive (showControls);
		Instance.m_displayCameraNavigatorAnchor = showControls;
	}

	public static void DisplayCourseControls(bool showControls) {
		Instance.m_coursePar.gameObject.SetActive (showControls);
		Instance.m_bestScore.gameObject.SetActive (showControls);
		Instance.m_startGameButton.gameObject.SetActive (showControls);
		//Instance.m_title.gameObject.SetActive (showControls);
	}
}
