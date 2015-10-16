using UnityEngine;
using System.Collections;

public class SwingHandler : MonoBehaviour {

	private enum SwingState {
		Unstarted,
		Started,
		Ended
	}

	private SwingState m_currentSwingState;

	// Use this for initialization
	void Start () {
		m_currentSwingState = SwingState.Unstarted;
	}
	
	// Update is called once per frame
	void Update () {
		m_currentSwingState = SetCurrentSwingState ();

		if (m_currentSwingState == SwingState.Started) {
			
		} else {

		}
	}

	private SwingState SetCurrentSwingState () {
		if (m_currentSwingState == SwingState.Unstarted) {
			if (Input.GetMouseButtonDown(0)) {
				return SwingState.Started;
			}
		} else if (m_currentSwingState == SwingState.Started) {
			if (Input.GetMouseButtonUp(0)) {
				return SwingState.Unstarted;
			}
		}

		return SwingState.Unstarted;
	}
}
