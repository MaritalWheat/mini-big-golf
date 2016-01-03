using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	private static PlayerManager Instance;
    public GameObject m_ballPrefab;
    private GameObject m_startMarker;
	private GameObject m_ball;

	private int m_hits;
	private Vector3 m_savedVelocity;
	private Vector3 m_savedAngularVelocity;


	public static int Hits {
		get { return Instance.m_hits; }
	}

	void Start () {
       if (Instance == null) {
			Instance = this;
		}
        
    }
	
	void Update () {
		if (GameManager.CurrentGameState == GameManager.GameState.Unstarted || GameManager.CurrentGameState == 
		    GameManager.GameState.Paused) return;

		if (m_ball != null) {
			Rigidbody rigidbody = m_ball.GetComponent<Rigidbody>();
			if (rigidbody.IsSleeping ()) {
				rigidbody.WakeUp ();
			}
			if (rigidbody.velocity.magnitude < 0.15f) {
				rigidbody.Sleep();
			}
		}
	}

	public static void OnStart() {
		if (Instance.m_startMarker == null)
		{
			Instance.m_startMarker = GameObject.Find("Start Marker");
		}

		if (Instance.m_startMarker != null) {
			Instance.m_ball = GameObject.Instantiate(Instance.m_ballPrefab, Instance.m_startMarker.transform.position, Quaternion.identity) as GameObject;
		} 
	}

	public static void OnPause() {
		Rigidbody rigidbody = Instance.m_ball.GetComponent<Rigidbody> ();
		Instance.m_savedVelocity = rigidbody.velocity;
		Instance.m_savedAngularVelocity = rigidbody.angularVelocity;
		rigidbody.isKinematic = true;
	}

	public static void OnUnpause() {
		Rigidbody rigidbody = Instance.m_ball.GetComponent<Rigidbody> ();
		rigidbody.velocity = Instance.m_savedVelocity;
		rigidbody.angularVelocity = Instance.m_savedAngularVelocity;
		rigidbody.isKinematic = false;
	}

	public void OnResetClick() {
		PlayerManager.Reset();
		GameManager.ResetGame ();
	}

	public void OnMainMenu() {
		ResetHits ();
		GameObject.Destroy (m_ball);
		GameManager.ResetGameToMenu ();
	}

	public static void Reset () {
		Instance.m_ball.transform.position = Instance.m_startMarker.transform.position;
		Instance.m_ball.transform.rotation = Quaternion.identity;
		Rigidbody rigidbody = Instance.m_ball.GetComponent<Rigidbody> ();
		rigidbody.Sleep ();
		rigidbody.velocity = Vector3.zero;

		Instance.ResetHits ();
	}

	private void ResetHits() {
		m_hits = 0;
		UIManager.UpdateHits (m_hits);
	}

	public static void IncrementHits() {
		Instance.m_hits++;
		UIManager.UpdateHits (Instance.m_hits);
	}
}
