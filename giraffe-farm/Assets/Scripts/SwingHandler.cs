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
    private Vector3 m_swingInputStartPosition;
    private Vector3 m_swingInputEndPosition;
    private Vector3 m_swingInputDirection;
    private float m_swingTime;

	void Start () {
		m_currentSwingState = SwingState.Unstarted;
	}
	
	void Update () {
		m_currentSwingState = SetCurrentSwingState ();

        
	}

    void FixedUpdate()
    {
        if (m_currentSwingState == SwingState.Ended)
        {
            Debug.Log("Performing swing.");
            /*float rotationRadians = Mathf.Atan(m_swingInputDirection.z / m_swingInputDirection.x);
            float rotationDegrees = 0.0f;
            if (m_swingInputDirection.x > 0.0f)
            {
                rotationDegrees = 90.0f - (Mathf.Rad2Deg * rotationRadians);
            } else
            {
                rotationDegrees = -(90.0f + (Mathf.Rad2Deg * rotationRadians));
            }
            Debug.Log("Degrees: " + rotationDegrees);
            Transform parentTransform = m_ballRigidBody.gameObject.GetComponentInParent<Transform>();
            m_ballRigidBody.constraints = RigidbodyConstraints.FreezePosition;
            parentTransform.Rotate(Vector3.up, rotationDegrees, Space.Self);
            m_ballRigidBody.MoveRotation(Quaternion.Euler(new Vector3(rotationDegrees, 0.0f)));
            m_ballRigidBody.WakeUp();*/
            float velocity = Vector3.Distance(m_swingInputStartPosition, m_swingInputEndPosition) / m_swingTime;
            Debug.Log("Acceleration: " + velocity);
            m_ballRigidBody.AddForce(Vector3.Normalize(m_swingInputDirection) * velocity);
            m_currentSwingState = SwingState.Unstarted;
        }
        else
        {

        }
    }

	private SwingState SetCurrentSwingState () {
        //Debug.Log("Force: " + m_ballThrust);
        if (m_currentSwingState == SwingState.Unstarted) {
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
