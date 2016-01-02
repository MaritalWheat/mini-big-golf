using UnityEngine;
using UnityEngine.EventSystems;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

public class LongPress : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	
	private bool m_isDown;
	private float m_downTime;
	private GameObject m_parent;

	public void OnPointerDown(PointerEventData eventData) {
		this.m_isDown = true;
		this.m_downTime = Time.realtimeSinceStartup;
	}
	
	public void OnPointerUp(PointerEventData eventData) {
		this.m_isDown = false;
	}
	
	void Update() {
		if (m_parent == null) {
			m_parent = this.GetComponentInParent<Transform> ().gameObject;
			return;
		}

		if (!this.m_isDown) return;
		if (Time.realtimeSinceStartup - this.m_downTime > 0.1f) {
			//ugh I hate hacks like this, but for the sake of t
			if (m_parent.gameObject.name.Contains("Right")) {
				CameraManager.MoveCameraRight();
			} else if (m_parent.gameObject.name.Contains("Left")) {
				CameraManager.MoveCameraLeft();
			}
		}
	}
	
}
