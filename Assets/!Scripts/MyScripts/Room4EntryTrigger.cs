using UnityEngine;
using System.Collections;

public class Room4EntryTrigger : MonoBehaviour {

	// Trigger room4 animation, which will flood the room with blue light and cause
	// the enemy to spawn
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			this.GetComponentInParent<Room4Listener> ().SendMessage ("Room4Entered");
		}
	}
}
