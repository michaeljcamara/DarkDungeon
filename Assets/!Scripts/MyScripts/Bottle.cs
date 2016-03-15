using UnityEngine;
using System.Collections;

public class Bottle : MonoBehaviour {

	// Store two distinct sound effects: one for ball collisions, one for other collision
	private AudioSource[] audios;

	void Start () {
		audios = this.GetComponents<AudioSource> ();
	}

	// Play a "clink" sound effect upon collision
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Ball") {
			audios [0].Play ();
		} else {
			audios [1].Play ();
		}
	}
}