using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

	private static PlayerManager Instance;
    public GameObject m_ballPrefab;
    private GameObject m_startMarker;
	private GameObject m_ball;

	void Start () {
       if (Instance == null) {
			Instance = this;
		}
        
    }
	
	void Update () {
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
	}
}
