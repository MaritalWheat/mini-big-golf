using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public class GameStats
	{
		private float m_timePlayed;
		private int m_numHits;

		public GameStats(float timePlayed, int numHits) {
			m_timePlayed = timePlayed;
			m_numHits = numHits;
		}

		public float GetTimePlayed() {
			return m_timePlayed;
		}

		public int GetNumHits() {
			return m_numHits;
		}
	}

	private static GameManager Instance;
	private GameState m_currentState;
	private float m_gameStartTime;

	public static GameState CurrentGameState {
		get { return Instance.m_currentState; }
	}

	public enum GameState {
		Unstarted,
		Started,
		Ended,
		Paused
	}

	void Start () {
		if (Instance == null) {
			Instance = this;
			m_currentState = GameState.Unstarted;
		}
	}

	//called from UI Start Button
	public void StartGame() {
		m_currentState = GameState.Started;
		m_gameStartTime = Time.time;
		//should probably use an event here eventually, this will be unscaleable...
		UIManager.StartGame ();
	}

	//called from UI Pause Button
	public void PauseGame() {

	}

	public static void EndGame() {
		Instance.m_currentState = GameState.Ended;

		float timePlayed = Time.time - Instance.m_gameStartTime;
		UIManager.PostGame (new GameStats(timePlayed, PlayerManager.Hits));
	}

	public static void ResetGame() {
		Instance.m_currentState = GameState.Started;
		Instance.m_gameStartTime = Time.time;

		UIManager.Reset ();
	}
}
