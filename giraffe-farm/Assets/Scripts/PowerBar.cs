using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerBar : MonoBehaviour {
	
	[SerializeField] private GameObject m_middle;
	[SerializeField] private GameObject m_end;

	private static PowerBar Instance;
	private Vector3 m_scaleTarget;
	private bool m_maxReached;

	void Start () {
		if (Instance == null) { 
			Instance = this;
		}
	}

	void Update () {
		m_middle.transform.localScale = Vector3.Lerp(m_middle.transform.localScale, m_scaleTarget, Time.deltaTime);
		if ((Mathf.Abs (m_middle.transform.localScale.x - m_scaleTarget.x) < 0.01f) && m_maxReached) {
			Instance.m_end.gameObject.SetActive (true);
		} else {
			if (Instance.m_end.gameObject.activeSelf) {
				Instance.m_end.gameObject.SetActive (false);
			}
		}

	}

	public static void SetFill (float fillPercentage) {
		float minXScale = 0.55f;
		float maxXScale = 11.85f;

		float xValue = ((maxXScale - minXScale) * fillPercentage) >= 0.55f ? ((maxXScale - minXScale) * fillPercentage) : 0.55f;
		Instance.m_scaleTarget = new Vector3 (xValue, Instance.m_middle.transform.localScale.y, Instance.m_middle.transform.localScale.z);

		if (fillPercentage > 0.98f) {
			Instance.m_maxReached = true;
			//Instance.m_end.gameObject.SetActive(true);
		} else {
			Instance.m_maxReached = false;
			/*if (Instance.m_end.gameObject.activeSelf) {
				Instance.m_end.gameObject.SetActive(false);
			}*/
		}
	}
}
