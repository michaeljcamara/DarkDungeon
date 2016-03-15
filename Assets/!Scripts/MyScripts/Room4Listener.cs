using UnityEngine;
using System.Collections;

public class Room4Listener : MonoBehaviour {

	private Animator anim;
	private AudioSource audio;

	void Start () {
		anim = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();
	}

	// Trigger when the room is entered by the player.
	// Will cause the room to fill with blue light, start boss music, and
	// cause enemy to spawn.
	private void Room4Entered () {
		anim.SetBool ("isRoom4Entered", true);
	}

	// Trigger when the player has died and therefore exited the room.
	// This will reset the room to its default state
	private void Room4Exited () {
		anim.SetBool ("isRoom4Entered", false);
	}

	// Trigger when the player has defeated the enemy.
	// Will start some particle effects and play victory music
	private void GameOver () {
		anim.SetBool ("isGameOver", true);
	}
}