using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static GameManager Instance;
	private GameState m_currentState;

	public static GameState CurrentGameState {
		get { return Instance.m_currentState; }
	}

	public enum GameState {
		Unstarted,
		Started,
		Paused
	}

	void Start () {
		if (Instance == null) {
			Instance = this;
			m_currentState = GameState.Unstarted;
		}
	}

	public void StartGame() {
		m_currentState = GameState.Started;

		//should probably use an event here eventually, this will be unscaleable...
		UIManager.StartGame ();
	}
}
