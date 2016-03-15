﻿using UnityEngine;
using System.Collections;

public class Room2EntryTrigger : MonoBehaviour {
	
	// Trigger room2 animation, which will turn on fire (particle), light, and sound effects
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			this.GetComponentInParent<Room2Listener> ().SendMessage ("Room2Entered");
			this.gameObject.SetActive (false);
		}
	}
}