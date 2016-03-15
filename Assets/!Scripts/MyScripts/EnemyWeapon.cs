using UnityEngine;
using System.Collections;

public class EnemyWeapon : MonoBehaviour {

	// When the enemy's weapon hits the play, propagate collision message to parent
	void OnTriggerEnter (Collider collider) {
		this.SendMessageUpwards ("HitPlayer");
	}
}