using UnityEngine;
using System.Collections;

public class WallHitDetectionHelper : MonoBehaviour {

	private bool m_wallHit;
	public AudioClip m_hitClip;

	void FixedUpdate() {
		if (PlayerManager.Ball != null) {
			this.transform.position = new Vector3(PlayerManager.Ball.GetComponent<Rigidbody>().worldCenterOfMass.x, PlayerManager.Ball.GetComponent<Rigidbody>().worldCenterOfMass.y + 0.05f, 
			                                      PlayerManager.Ball.GetComponent<Rigidbody>().worldCenterOfMass.z);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag != "Player") {
			Debug.Log (other.gameObject.name);
			//lol late nite hack to avoid hole logic colliders
			if (other.gameObject.name.Contains("Model")) {
				AudioManager.PlaySoundAtObject (this.gameObject, m_hitClip);
			}
		}
	}
}
