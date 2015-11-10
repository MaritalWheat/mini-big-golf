using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	private static UIManager Instance;

	public Text m_hitCounter;
	public Button m_startGameButton;
	public GameObject m_postCourseStats;
	
	void Start () {
		if (Instance == null) {
			Instance = this;
		}
	}

	public static void UpdateHits(int newHitCount) {
		Instance.m_hitCounter.text = "Hits: " + newHitCount;
	}

	public static void StartGame() {
		Instance.m_startGameButton.gameObject.SetActive (false);
	}

	public static void PostGame() {
		Instance.m_hitCounter.gameObject.SetActive (false);
		Instance.m_postCourseStats.gameObject.SetActive (true);
	}
}
