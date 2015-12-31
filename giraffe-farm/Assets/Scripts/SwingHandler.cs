﻿using UnityEngine;
using System.Collections;

public class SwingHandler : MonoBehaviour {

    private Rigidbody m_ballRigidBody;

    private enum SwingState {
		Unstarted,
		Started,
		Ended
	}

	private SwingState m_currentSwingState;
    private Vector3 m_swingInputStartPosition;
    private Vector3 m_swingInputEndPosition;
    private Vector3 m_swingInputDirection;
    private float m_swingTime;
	private Vector3 m_move; // the world-relative desired move direction, calculated from the camForward and user input.
	private Transform m_cam; // A reference to the main camera in the scenes transform
	private Vector3 m_camForward; // The current forward direction of the camera

	void Start () {
		m_currentSwingState = SwingState.Unstarted;
	}
	
	void Update () {
		if (GameManager.CurrentGameState == GameManager.GameState.Unstarted || GameManager.CurrentGameState ==
			GameManager.GameState.Paused) {
			m_currentSwingState = SwingState.Unstarted;
			return;
		}

		if (m_cam == null && Camera.main != null)
		{
			m_cam = Camera.main.transform;
		}

        if (m_ballRigidBody == null) {
			GameObject ball = GameObject.FindGameObjectWithTag ("Player");
			if (ball != null) {
				m_ballRigidBody = GameObject.FindGameObjectWithTag ("Player").GetComponent<Rigidbody> ();
			}
		} else {
			m_currentSwingState = SetCurrentSwingState ();
		}
	}

    void FixedUpdate()
    {
		if (m_cam == null || m_ballRigidBody == null)
			return;

        if (m_currentSwingState == SwingState.Ended)
        {
            Debug.Log("Performing swing.");
            float velocity = Vector3.Distance(m_swingInputStartPosition, m_swingInputEndPosition) / m_swingTime;
            //Debug.Log("Acceleration: " + velocity);
            
			m_camForward = Vector3.Scale(m_cam.forward, new Vector3(1, 0, 1)).normalized;
			m_move = (m_swingInputDirection.z * m_camForward + m_swingInputDirection.x * m_cam.right).normalized;
			m_ballRigidBody.AddForce(m_move * velocity);
            m_currentSwingState = SwingState.Unstarted;
			PlayerManager.IncrementHits();
        }
    }

	private SwingState SetCurrentSwingState () {
        //Debug.Log("Force: " + m_ballThrust);
		if (GameManager.CurrentGameState == GameManager.GameState.Ended) {
			return SwingState.Unstarted;
		} else if (m_currentSwingState == SwingState.Unstarted) {
            if (Input.GetMouseButton(0)) {
                Debug.Log("Starting swing.");
                m_swingInputStartPosition = Input.mousePosition;
                m_swingTime = 0.0f;
                return SwingState.Started;
            }
		} else if (m_currentSwingState == SwingState.Started) {
			if (Input.GetMouseButtonUp(0)) {
                Debug.Log("Ending swing.");
                m_swingInputEndPosition = Input.mousePosition;
                Vector3 rawDirection = m_swingInputStartPosition - m_swingInputEndPosition;
                m_swingInputDirection = new Vector3(-rawDirection.x, 0.0f, -rawDirection.y);
                Debug.Log("Swing direction: " + m_swingInputDirection);
                return SwingState.Ended;
			} else {
                m_swingTime += Time.deltaTime;
                return SwingState.Started;
            }
		}

		return SwingState.Unstarted;
	}
}
