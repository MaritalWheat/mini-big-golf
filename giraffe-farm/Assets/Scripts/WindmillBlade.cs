using UnityEngine;
using System.Collections;

public class WindmillBlade : MonoBehaviour {

	private Transform m_transform;
	[SerializeField] private float m_speed = 0.0f;

	void Awake () {
		if (m_transform == null) {
			m_transform = this.gameObject.transform;
		}
	}

	void Update () {
		m_transform.Rotate (Vector3.forward, Time.deltaTime * m_speed, Space.World);
	}
}
