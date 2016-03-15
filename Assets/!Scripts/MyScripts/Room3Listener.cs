using UnityEngine;
using System.Collections;

public class Room3Listener : MonoBehaviour {

	private Animator anim;
	private AudioSource audio;

	void Start () {
		anim = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();
	}

	// Trigger when the user clicks on the chest.
	// Will open the chest, revealing a sword
	private void ChestOpened () {
		anim.SetBool ("isChestOpened", true);
	}
		
	// Trigger when the user clicks on the sword inside of the chest.
	// Will cause the player to "equip" the sword and open the final door
	private void SwordTaken () {
		anim.SetBool ("isSwordTaken", true);
	}
}