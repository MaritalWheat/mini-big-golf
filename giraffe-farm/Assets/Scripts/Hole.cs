using UnityEngine;
using System.Collections;

public class Hole : MonoBehaviour {

	public MeshCollider blockMesh;

	void Start() {
		GameManager.GameReset += new GameResetEventHandler (Reset);
	}

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

	void Reset() {
		//Debug.LogError ("Reset Event test");
		blockMesh.enabled = true;
	}
}
