using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PowerBar : MonoBehaviour {

	[SerializeField] private GameObject m_beginning;
	[SerializeField] private GameObject m_middle;
	[SerializeField] private GameObject m_end;

	private static PowerBar Instance;
	private Vector3 m_scaleTarget;

	void Start () {
		if (Instance == null) { 
			Instance = this;
		}
	}

	void Update () {
		m_middle.transform.localScale = Vector3.Lerp(m_middle.transform.localScale, m_scaleTarget, Time.deltaTime);
	}

	public static void SetFill (float fillPercentage) {
		float minXScale = 0.55f;
		float maxXScale = 10.55f;

		float xValue = ((maxXScale - minXScale) * fillPercentage) >= 0.55f ? ((maxXScale - minXScale) * fillPercentage) : 0.55f;
		Instance.m_scaleTarget = new Vector3 (xValue, Instance.m_middle.transform.localScale.y, Instance.m_middle.transform.localScale.z);

		if (fillPercentage > 99.0f) {
			Instance.m_end.gameObject.SetActive(false);
		} else {
			if (Instance.m_end.gameObject.activeSelf) {
				Instance.m_end.gameObject.SetActive(true);
			}
		}
	}
}
