using UnityEngine;
using System.Collections;

public class FieryDeathTrigger : MonoBehaviour {

	// If player enters the fire pit in room 2, teleport back to room 2 entry door
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			collider.transform.position = new Vector3 (6, 2, 8.8f);
		}
	}
}