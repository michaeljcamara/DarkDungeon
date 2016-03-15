using UnityEngine;
using System.Collections;

public class Room1Listener : MonoBehaviour {

	private Animator anim;

	void Start () {
		anim = GetComponent<Animator> ();
	}

	// Trigger when the player knocks all bottles off of the table.
	// Will move apart the crates and open the room 2 door
	public void Room1Cleared () {
		anim.SetBool ("isRoom1Clear", true);
	}

	// Trigger when the player attempts to leave the first room toward the
	// outdoors.  Will slam the door shut
	private void Room1Exited () {
		anim.SetBool ("isExitClosed", true);
	}
}
