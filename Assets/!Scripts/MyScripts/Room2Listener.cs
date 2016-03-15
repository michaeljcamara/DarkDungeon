using UnityEngine;
using System.Collections;

public class Room2Listener : MonoBehaviour {

	private Animator anim;
	private AudioSource audio;

	void Start () {
		anim = GetComponent<Animator> ();
		audio = GetComponent<AudioSource> ();
	}

	// Trigger when the user enters the room.  Will turn on some lights, fire
	// particle effects, and sound effects (e.g. fire crackling);
	private void Room2Entered () {
		anim.SetBool ("isRoom2Entered", true);
	}

	// Trigger when the user clicks on the lever.  Will reset any existing timer and
	// will begin a new ticking timer with a corresponding UI element.
	// The player will have until the timer stops to cross the laser obstacle course to
	// reach the room 3 door.
	private void LeverPulled () {
		ResetTimer ();

		anim.ResetTrigger ("isLaserTriggered");
		anim.SetTrigger ("isLeverPulled");

		StopAllCoroutines ();
		StartCoroutine (DelayAnimation ());

	}

	// Trigger after the user clicks on the lever.  Will ensure that
	// the timer lasts for 15 seconds
	IEnumerator DelayAnimation () {

		// Wait for lever to be pulled completely before starting timer
		yield return new WaitForSeconds (2f);
		audio.enabled = true;

		// Count down timer over 15 seconds
		for (int i = 0; i < 16; i++) {
			UIManager.UpdateTimer ((15 - i).ToString ());
			yield return new WaitForSeconds (1);

			// Increase pitch of timer to encourage expediency
			audio.pitch *= 1.07f;
		}
			
		// Turn off sound and ui element
		ResetTimer ();

		// Set animator parameter to return room2 to default state
		anim.SetTrigger ("isDoorClosed");
	}

	// Trigger when the player runs into one of the active lasers.
	// Will turn off all lasers and close the door to room 3
	private void LaserTriggered () {
		StopAllCoroutines ();
		ResetTimer ();
		anim.SetTrigger ("isLaserTriggered");
	}

	// Reset the timer UI and corresponding sound effect
	private void ResetTimer () {
		audio.enabled = false;
		audio.pitch = 1;
		UIManager.UpdateTimer ("");
	}
}