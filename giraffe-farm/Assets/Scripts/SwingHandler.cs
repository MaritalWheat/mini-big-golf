using UnityEngine;
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
	private bool m_ignoreInput;

	[SerializeField] private GameObject m_arrowPrefab;
	private GameObject m_arrow;
	private GameObject m_crumbOne;
	private GameObject m_crumbTwo;
	private static SwingHandler Instance; 

	public static bool IgnoreInput {
		get { return Instance.m_ignoreInput; }
		set { Instance.m_ignoreInput = value; }
	}


	void Start () {
		if (Instance == null) {
			Instance = this;
		}
		m_currentSwingState = SwingState.Unstarted;
		m_arrow = GameObject.Instantiate (m_arrowPrefab);
		m_arrow.transform.localScale = m_arrow.transform.localScale * 0.6f;
		m_crumbOne = GameObject.Instantiate (m_arrowPrefab);
		m_crumbOne.transform.localScale = m_crumbOne.transform.localScale * 0.8f;
		m_crumbTwo = GameObject.Instantiate (m_arrowPrefab);
		m_crumbTwo.transform.localScale = m_crumbTwo.transform.localScale * 1.0f;
	}
	
	void Update () {
		if (GameManager.CurrentGameState == GameManager.GameState.Unstarted || GameManager.CurrentGameState ==
			GameManager.GameState.Paused || m_ignoreInput || PlayerManager.IsRolling) {
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

		if (m_currentSwingState != SwingState.Started) {
			m_arrow.SetActive (false);
			m_crumbOne.SetActive (false);
			m_crumbTwo.SetActive (false);
		}

		if (m_currentSwingState == SwingState.Ended) {
			float velocity = (Vector3.Distance (m_swingInputStartPosition, m_swingInputEndPosition) * 4.0f) / (0.25f * Screen.height);
			if (velocity > 6.0f) {
				velocity = 6.0f;
			}

			m_camForward = Vector3.Scale (m_cam.forward, new Vector3 (1, 0, 1));
			m_move = (m_swingInputDirection.z * m_camForward + m_swingInputDirection.x * m_cam.right);
			m_ballRigidBody.AddForce (m_move * velocity);
			m_currentSwingState = SwingState.Unstarted;
			PlayerManager.SetRollState (true);
			PlayerManager.IncrementHits ();

			PowerBar.SetFill (m_ballRigidBody.velocity.magnitude / 6.0f);

		} else if (m_currentSwingState == SwingState.Started) {

			Vector3 currPos = Input.mousePosition;
			Vector3 rawDirection = m_swingInputStartPosition - currPos;
			Vector3 swingInputDir = new Vector3 (rawDirection.x, 0.0f, rawDirection.y);
			
			float velocity = Vector3.Distance (m_swingInputStartPosition, currPos) / (0.25f * Screen.height);
			if (velocity > 6.0f) {
				velocity = 6.0f;
			}

			Vector3 camForward = Vector3.Scale (m_cam.forward, new Vector3 (1, 0, 1));
			Vector3 moveVector = (swingInputDir.z * camForward + swingInputDir.x * m_cam.right);
			Vector3 target = PlayerManager.Ball.transform.position + moveVector.normalized;
			target.y += 0.02f;
			m_arrow.transform.position = target;
			target = PlayerManager.Ball.transform.position + (moveVector.normalized * 0.67f);
			target.y += 0.02f;
			m_crumbOne.transform.position = target;
			target = PlayerManager.Ball.transform.position + (moveVector.normalized * 0.33f);
			target.y += 0.02f;
			m_crumbTwo.transform.position = target;
			m_crumbTwo.transform.LookAt (m_arrow.transform.position);
			m_crumbOne.transform.LookAt (m_arrow.transform.position);
			target = m_arrow.transform.position;
			target = PlayerManager.Ball.transform.position + (moveVector.normalized * 1.33f);
			target.y += 0.02f;
			m_arrow.transform.LookAt (target);

			m_arrow.SetActive (true);
			m_crumbOne.SetActive (true);
			m_crumbTwo.SetActive (true);

			PowerBar.SetFill (velocity / 6.0f);
		} else {
			PowerBar.SetFill (m_ballRigidBody.velocity.magnitude / 6.0f);
		}
	}

	private SwingState SetCurrentSwingState () {
        //Debug.Log("Force: " + m_ballThrust);
		if (GameManager.CurrentGameState == GameManager.GameState.Ended) {
			return SwingState.Unstarted;
		} else if (m_currentSwingState == SwingState.Unstarted) {
			if (Input.GetMouseButton(0)) {
                m_swingInputStartPosition = Input.mousePosition;
                m_swingTime = 0.0f;
                return SwingState.Started;
            }
		} else if (m_currentSwingState == SwingState.Started) {
			if (Input.GetMouseButtonUp(0)) {
                m_swingInputEndPosition = Input.mousePosition;
                Vector3 rawDirection = m_swingInputStartPosition - m_swingInputEndPosition;
                m_swingInputDirection = new Vector3(rawDirection.x, 0.0f, rawDirection.y);
                return SwingState.Ended;
			} else {
                m_swingTime += Time.deltaTime;
                return SwingState.Started;
            }
		}

		return SwingState.Unstarted;
	}
}
