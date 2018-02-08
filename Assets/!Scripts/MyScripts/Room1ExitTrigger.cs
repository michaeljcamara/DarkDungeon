using UnityEngine;
using System.Collections;

public class Room1ExitTrigger : MonoBehaviour {

	// Trigger room1 animation, which will close the entry door in room 1
	// lower the noise from the "outdoor" effects
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			SendMessageUpwards ("Room1Exited");
			this.gameObject.SetActive (false);
		}
	}
}
