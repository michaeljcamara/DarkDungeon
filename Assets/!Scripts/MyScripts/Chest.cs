using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	// Trigger room animation on click, which will open chest and reveal sword
	void OnMouseDown () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, 5) == true) {
			GetComponentInParent<Room3Listener> ().SendMessage ("ChestOpened");
			this.GetComponent<AudioSource> ().Play ();
		}
	}
}