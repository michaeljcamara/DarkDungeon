using UnityEngine;
using System.Collections;

public class Bottle : MonoBehaviour {

	// Store two distinct sound effects: one for ball collisions, one for other collision
    [SerializeField]
    private AudioSource ballAudio, otherAudio;

	// Play a "clink" sound effect upon collision
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Ball") {
			ballAudio.Play ();
		} else {
			otherAudio.Play ();
		}
	}
}