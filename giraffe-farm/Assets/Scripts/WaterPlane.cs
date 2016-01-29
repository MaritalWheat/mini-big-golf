using UnityEngine;
using System.Collections;

public class WaterPlane : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			Debug.Log ("Game Over");
			PlayerManager.Reset ();
			CameraManager.HardReset ();
		}
	}
}
