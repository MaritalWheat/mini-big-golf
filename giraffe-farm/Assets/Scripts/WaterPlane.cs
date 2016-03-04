using UnityEngine;
using System.Collections;

public class WaterPlane : MonoBehaviour {

	[SerializeField] AudioClip m_waterSplashAudioClip;

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			Debug.Log ("Game Over");
			AudioManager.PlaySoundAtObject(this.gameObject, m_waterSplashAudioClip);
			PlayerManager.Reset ();
			CameraManager.HardReset ();
		}
	}
}
