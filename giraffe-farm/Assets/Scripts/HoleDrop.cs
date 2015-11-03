using UnityEngine;
using System.Collections;

public class HoleDrop : MonoBehaviour {

	public BoxCollider holeCoverMesh;
	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			holeCoverMesh.enabled = false;
			//Debug.Log ("Game Over");
			//PlayerManager.Reset();
		}
	}
}
