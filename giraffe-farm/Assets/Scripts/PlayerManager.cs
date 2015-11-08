using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	private static PlayerManager Instance;
    public GameObject m_ballPrefab;
    private GameObject m_startMarker;
	private GameObject m_ball;

	private int m_hits;

	void Start () {
       if (Instance == null) {
			Instance = this;
		}
        
    }
	
	void Update () {
		if (GameManager.CurrentGameState == GameManager.GameState.Unstarted) return;

	    if (m_startMarker == null)
        {
            m_startMarker = GameObject.Find("Start Marker");
			if (m_startMarker != null) {
				m_ball = GameObject.Instantiate(m_ballPrefab, m_startMarker.transform.position, Quaternion.identity) as GameObject;
			}
        }

		if (m_ball != null) {
			if (m_ball.GetComponent<Rigidbody> ().IsSleeping ()) {
				m_ball.GetComponent<Rigidbody> ().WakeUp ();
			}
		}
	}

	public static void Reset () {
		Instance.m_ball.transform.position = Instance.m_startMarker.transform.position;
		Instance.m_ball.GetComponent<Rigidbody> ().Sleep ();
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
