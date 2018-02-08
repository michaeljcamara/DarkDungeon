using UnityEngine;
using System.Collections;

public class FieryDeathTrigger : MonoBehaviour {

    [SerializeField]
    private Transform respawnLocation;

	// If player enters the fire pit in room 2, teleport back to room 2 entry door
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
            collider.transform.position = respawnLocation.position;
        }
	}
}