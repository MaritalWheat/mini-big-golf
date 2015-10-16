using UnityEngine;
using System.Collections;

public class SwingHandler : MonoBehaviour {

	private enum SwingState {
		Unstarted,
		Started,
		Ended
	}

	private SwingState m_currentSwingState;
    public Rigidbody m_ballRigidBody;

	void Start () {
		m_currentSwingState = SwingState.Unstarted;
	}
	
	void Update () {
		m_currentSwingState = SetCurrentSwingState ();

        if (m_currentSwingState == SwingState.Started) {
            Debug.Log("Performing swing.");
            m_ballRigidBody.AddForce(m_ballRigidBody.transform.forward * 1000.0f);
		} else {

		}
	}

	private SwingState SetCurrentSwingState () {
		if (m_currentSwingState == SwingState.Unstarted) {
			if (Input.GetMouseButtonDown(0)) {
                Debug.Log("Starting swing.");
                return SwingState.Started;
			}
		} else if (m_currentSwingState == SwingState.Started) {
			if (Input.GetMouseButtonUp(0)) {
                Debug.Log("Ending swing.");
                return SwingState.Unstarted;
			}
		}

		return SwingState.Unstarted;
	}
}
