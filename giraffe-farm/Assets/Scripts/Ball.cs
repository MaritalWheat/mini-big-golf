using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	void OnCollisionEnter(Collision other) {
		bool hitWall = false;
		foreach(ContactPoint cp in other.contacts) {
			float dot = Vector3.Dot(cp.normal, transform.up);
			if(dot < 0.9f && dot > -0.9f) {
				hitWall = true;
				break;  //No need to keep checking once you've found a wall
			}
		}
		
		if(hitWall) {
			//At least one collision was not with the floor (or ceiling)
			//Handle wall collisions here
			Debug.Log ("Hit wall!");
		}
	}
}
