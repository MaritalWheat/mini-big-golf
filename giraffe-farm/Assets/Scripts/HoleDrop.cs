using UnityEngine;
using System.Collections;

public class HoleDrop : MonoBehaviour {

	public BoxCollider holeCoverMesh;

	
	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			holeCoverMesh.enabled = false;
			Debug.Log("Game Over!");
			GameManager.EndGameImmediate();
			//calls given method after given time
			Invoke("FinishCourse", 2.0f);
		}
	}

	void FinishCourse() {
		GameManager.EndGame ();
	}
}
