using UnityEngine;
using System.Collections;

public class WindmillBlade : MonoBehaviour {

	private Transform m_transform;

	[SerializeField] private Transform m_targetTransform;
	[SerializeField] private float m_speed = 0.0f;

	void Awake () {
		if (m_transform == null) {
			m_transform = this.gameObject.transform;
		}
	}

	void Update () {
		m_transform.RotateAround (m_targetTransform.position, m_targetTransform.forward, Time.deltaTime * m_speed);
	}
}
