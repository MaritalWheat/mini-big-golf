using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraHelper : MonoBehaviour {

	private List<GameObject> m_activeTransparentObjects = new List<GameObject>();
	private float m_timeDelay;

	void Update () {
		m_timeDelay += Time.deltaTime;

		if (m_timeDelay > 0.1f) {
			m_timeDelay = 0.0f;
			RaycastHit[] hits;

			List<GameObject> lastHits = new List<GameObject> ();
			if (PlayerManager.Ball != null) {
				float distance = Vector3.Distance (PlayerManager.Ball.transform.position, this.transform.position);
				hits = Physics.RaycastAll (transform.position, PlayerManager.Ball.transform.position - this.transform.position, distance);

				for (int i = 0; i < hits.Length; i++) {
					GameObject curr = hits [i].transform.gameObject;

					if (curr.gameObject.name.Contains ("Ball")) {
						break;
					}

					if (curr.gameObject.name.Contains ("Windmill") || curr.gameObject.name.Contains ("Castle")) {
						SetTransparent (curr);
						//inefficient prototype
						lastHits.Add (curr);
					}
				}

				for (int j = 0; j < m_activeTransparentObjects.Count; j++) {
					GameObject curr = m_activeTransparentObjects [j];
					if (!lastHits.Contains (curr)) {
						SetVisible (curr);
					}
				}
			}
		}
	}

	private void SetVisible(GameObject curr) {
		
		m_activeTransparentObjects.Remove (curr);

		Transform[] children = curr.GetComponentsInChildren<Transform> (true);
		List<GameObject> toActivate = new List<GameObject> ();
		List<GameObject> toDeactivate = new List<GameObject> ();

		for(int i = 0; i < children.Length; i++) {
			if (curr.gameObject != children[i].gameObject) {
				if(children[i].gameObject.name.Contains("Trans")) {
					toDeactivate.Add(children[i].gameObject);
				} else {
					toActivate.Add(children[i].gameObject);
				}
			}
		}

		foreach (GameObject obj in toDeactivate) {
			Transform[] objChildren = obj.GetComponentsInChildren<Transform>();
			foreach (Transform objChild in objChildren) {
				MeshRenderer renderer = objChild.GetComponentInChildren<MeshRenderer>();
				if (renderer != null) {
					renderer.enabled = false;
				}
			}
		}
		
		foreach (GameObject obj in toActivate) {
			Transform[] objChildren = obj.GetComponentsInChildren<Transform>();
			foreach (Transform objChild in objChildren) {
				MeshRenderer renderer = objChild.GetComponentInChildren<MeshRenderer>();
				if (renderer != null) {
					renderer.enabled = true;
				}
			}

		}
	}

	private void SetTransparent(GameObject curr) {
		if (m_activeTransparentObjects.Contains (curr))
			return;

		m_activeTransparentObjects.Add(curr.gameObject);

		Transform[] children = curr.GetComponentsInChildren<Transform> (true);
		List<GameObject> toActivate = new List<GameObject> ();
		List<GameObject> toDeactivate = new List<GameObject> ();
		for(int i = 0; i < children.Length; i++) {
			if (curr.gameObject != children[i].gameObject) {
				if(children[i].gameObject.name.Contains("Trans")) {
					toActivate.Add(children[i].gameObject);
				} else {
					toDeactivate.Add(children[i].gameObject);
				}
			}
		}

		foreach (GameObject obj in toDeactivate) {
			Transform[] objChildren = obj.GetComponentsInChildren<Transform>();
			foreach (Transform objChild in objChildren) {
				MeshRenderer renderer = objChild.GetComponentInChildren<MeshRenderer>();
				if (renderer != null) {
					renderer.enabled = false;
				}
			}
		}

		foreach (GameObject obj in toActivate) {
			Transform[] objChildren = obj.GetComponentsInChildren<Transform>();
			foreach (Transform objChild in objChildren) {

				MeshRenderer renderer = objChild.GetComponentInChildren<MeshRenderer>();
				if (renderer != null) {
					renderer.enabled = true;
				}
			}
		}
	}
}
