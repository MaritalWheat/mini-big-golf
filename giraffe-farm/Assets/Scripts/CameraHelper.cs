using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraHelper : MonoBehaviour {

	private List<GameObject> m_activeTransparentObjects = new List<GameObject>();

	void Update () {
		RaycastHit[] hits;

		List<GameObject> lastHits = new List<GameObject> ();
		if (PlayerManager.Ball != null) {
			float distance = Vector3.Distance(PlayerManager.Ball.transform.position, this.transform.position);
			hits = Physics.RaycastAll (transform.position, PlayerManager.Ball.transform.position - this.transform.position, distance);

			for (int i = 0; i < hits.Length; i++) {
				GameObject curr = hits[i].transform.gameObject;

				if (curr.gameObject.name.Contains("Ball")) {
					Debug.Log("Nothing blocking view to ball.");
					break;
				}

				if (curr.gameObject.name.Contains("Windmill") || curr.gameObject.name.Contains("Castle")) {
					SetTransparent(curr);
					//inefficient prototype
					lastHits.Add (curr);
				}
			}

			for(int j = 0; j < m_activeTransparentObjects.Count; j++) {
				GameObject curr = m_activeTransparentObjects[j];
				if (!lastHits.Contains(curr)) {
					SetVisible(curr);
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

		foreach (GameObject obj in toActivate) {
			obj.SetActive(true);
		}
		
		foreach (GameObject obj in toDeactivate) {
			obj.SetActive (false);
		}

		/*Renderer[] rends = curr.GetComponentsInChildren<Renderer>();
		//Materials[] = rend.materials;
		for (int i = 0; i < rends.Length; i++) {
			for (int j = 0; j < rends[i].materials.Length; j++) {
				Material material = rends[i].materials[j];

				if (!material.name.Contains ("Green") || material.name.Contains("Back")) {

					Color color = rends[i].materials [j].color;

					if (!material.name.Contains("Back")) {
						color.a = 1.0f;
					}

					//change shader to transparent rendering mode
					/*material.SetInt("_SrcBlend", 1);
					material.SetInt("_DstBlend", 0);
					material.SetInt("_ZWrite", 1);
					material.DisableKeyword("_ALPHABLEND_ON");
					material.EnableKeyword("_ALPHATEST_ON");
					material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = 2000;*/
					/*rends[i].materials [j].color = color;
				}
			}
		}*/
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

		foreach (GameObject obj in toActivate) {
			obj.SetActive(true);
		}

		foreach (GameObject obj in toDeactivate) {
			obj.SetActive (false);
		}

		//Renderer[] rends = curr.GetComponentsInChildren<Renderer>();

		/*
		//Materials[] = rend.materials;
		for (int i = 0; i < rends.Length; i++) {
			for (int j = 0; j < rends[i].materials.Length; j++) {
				Material material = rends[i].materials[j];

				if (!material.name.Contains ("Green")) {
					Color color = material.color;
					if (material.name.Contains("Back")) {
						color.a = 0.0f;
					} else {
						color.a = 0.25f;
					}
					//change shader to transparent rendering mode
					//Debug.LogError("srcblend" + material.GetInt("_SrcBlend"));
					//Debug.LogError("dstblend" + material.GetInt("_DstBlend"));

					rends[i].materials [j].color = color;
				}
			}
		}*/
	}
}
