using UnityEngine;
using System.Collections;

public class Hole : MonoBehaviour {

	public MeshCollider blockMesh;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			blockMesh.enabled = false;
			//Debug.Log ("Game Over");
			//PlayerManager.Reset();
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Player") {
			blockMesh.enabled = true;
			//Debug.Log ("Game Over");
			//PlayerManager.Reset();
		}
	}
}
