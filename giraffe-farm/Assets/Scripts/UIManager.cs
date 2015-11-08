using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	private static UIManager Instance;

	public Text m_hitCounter;
	public Button m_startGameButton;
	
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
}
