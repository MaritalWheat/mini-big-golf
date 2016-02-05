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
	private float m_gamePauseStartTime;
	private float m_gamePauseTotalTime;

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
			PreGame();
			CameraManager.FadeCameraOnLaunch ();
		}
	}

	public void PreGame() {
		UIManager.SetupPregame ();
	}

	//called from UI Start Button
	public void StartGame() {
		m_currentState = GameState.Started;
		m_gameStartTime = Time.time;
		m_gamePauseTotalTime = 0.0f;
		//should probably use an event here eventually, this will be unscaleable...
		UIManager.OnStart ();
		PlayerManager.OnStart ();
	}

	//called from UI Pause Button
	public void PauseGame() {
		if (m_currentState != GameState.Paused) {
			m_currentState = GameState.Paused;
			m_gamePauseStartTime = Time.time;
			PlayerManager.OnPause();
			UIManager.OnPause();
			CameraManager.OnPause ();
			CameraManager.BlurBackgroundOnPause();
		} else {
			UnpauseGame(); //temporary until menu is created
		}
	}

	//called from UI Unpause Button
	public void UnpauseGame() {
		m_gamePauseTotalTime += Time.time - m_gamePauseStartTime;
		m_currentState = GameState.Started;
		PlayerManager.OnUnpause ();
		UIManager.OnUnpause ();
		CameraManager.UnblurBackgroundOnPause ();
		CameraManager.Reset (); 
	}

	public void UnpauseGamePostReset() {
		PlayerManager.OnUnpausePostReset ();
		UIManager.OnUnpause ();
		CameraManager.HardReset ();
	}

	public static void EndGame() {
		Instance.m_currentState = GameState.Ended;

		float timePlayed = (Time.time - Instance.m_gameStartTime) - Instance.m_gamePauseTotalTime;
		UIManager.SetupPostGame (new GameStats(timePlayed, PlayerManager.Hits));
		CameraManager.BlurBackgroundOnPause ();
	}

	public static void ResetGame() {
		bool paused = false;
		if (Instance.m_currentState == GameState.Paused)
			paused = true;
		Instance.m_currentState = GameState.Started;
		Instance.m_gameStartTime = Time.time;
		Instance.m_gamePauseTotalTime = 0.0f;

		UIManager.OnReset ();
		PlayerManager.Reset ();
		CameraManager.HardReset ();
		CameraManager.UnblurBackgroundOnPause ();

		if (paused)
			Instance.UnpauseGamePostReset ();
	}

	public static void ResetGameToMenu() {
		bool paused = false;
		if (Instance.m_currentState == GameState.Paused)
			paused = true;

		if (paused) 
			Instance.UnpauseGamePostReset ();

		Instance.m_currentState = GameState.Unstarted;
		UIManager.SetupPregame ();
		CameraManager.HardReset ();
		CameraManager.UnblurBackgroundOnPause ();

	}
}
