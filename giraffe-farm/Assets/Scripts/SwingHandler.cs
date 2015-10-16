using UnityEngine;
using System.Collections;

public class SwingHandler : MonoBehaviour {

    public Rigidbody m_ballRigidBody;

    private enum SwingState {
		Unstarted,
		Started,
		Ended
	}

	private SwingState m_currentSwingState;
    private float m_ballThrust;

	void Start () {
		m_currentSwingState = SwingState.Unstarted;
	}
	
	void Update () {
		m_currentSwingState = SetCurrentSwingState ();

        if (m_currentSwingState == SwingState.Ended) {
            Debug.Log("Performing swing.");
            m_ballRigidBody.AddForce(m_ballRigidBody.transform.forward * m_ballThrust);
            m_currentSwingState = SwingState.Unstarted;
		} else {

		}
	}

	private SwingState SetCurrentSwingState () {
        Debug.Log("Force: " + m_ballThrust);
        if (m_currentSwingState == SwingState.Unstarted) {
            if (Input.GetMouseButton(0)) {
                Debug.Log("Starting swing.");
                return SwingState.Started;
            }
		} else if (m_currentSwingState == SwingState.Started) {
			if (Input.GetMouseButtonUp(0)) {
                Debug.Log("Ending swing.");
                return SwingState.Ended;
			} else {
                m_ballThrust = m_ballThrust + (1000.0f * Time.deltaTime);
                return SwingState.Started;
            }
		}

        if (m_ballThrust > 0.0f)
        {
            m_ballThrust -= 1000.0f * Time.deltaTime;
            if (m_ballThrust < 0.0f)
            {
                m_ballThrust = 0.0f;
            }
        }
		return SwingState.Unstarted;
	}
}
