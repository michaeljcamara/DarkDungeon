using UnityEngine;
using System.Collections;

public class Table : MonoBehaviour {

	// When a bottle has been knocked off the table, use a bounding box to determine
	// if any other bottles currently exist on the table.  If not, then the room is cleared
	void OnCollisionExit (Collision collision) {

		// Stop any current checks being done for the clear status
		StopAllCoroutines ();

		// If only 1 object is in the bounding box (this table), start small timer
		// before declaring the room as clear
		Bounds boundingBox = this.GetComponent<Collider> ().bounds;
		if (Physics.OverlapBox (boundingBox.center, boundingBox.extents).Length == 1) {
			StartCoroutine (CheckClearStatus (boundingBox));
		}
	}

	// Whenever a bottle collides with the table, stop checking for the clear status
	void OnCollisionEnter (Collision collision) {
		StopAllCoroutines ();
	}

	// When all bottles are knocked off the table, begin this brief timer to see if any bottles
	// fall back onto the table.  If there are still no bottles on the table after this, then
	// the room is cleared.
	IEnumerator CheckClearStatus (Bounds boundingBox) {
		yield return new WaitForSeconds (0.5f);
		if (Physics.OverlapBox (boundingBox.center, boundingBox.extents).Length == 1) {
			GetComponentInParent<Room1Listener> ().SendMessage ("Room1Cleared");
		}
	}
}