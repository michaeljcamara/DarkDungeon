using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

	// Trigger room animation on click, which will rotate lever and open room3 door
	void OnMouseDown () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, 5) == true) {
			GetComponentInParent<Room2Listener> ().SendMessage ("LeverPulled");
		}
	}
}