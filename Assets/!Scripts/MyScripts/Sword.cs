using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {

	// How quickly the sword will swing
	[SerializeField]
	private float speed;

	// How long the swinging animation is
	[SerializeField]
	private float duration;

	// Indicate if the sword if currently held
	private bool isHeld;

	// Indicate if the player is already swinging the sword
	private bool isSwinging;

	private AudioSource audio;

	void Awake () {
		isHeld = false;
		isSwinging = false;
		audio = this.GetComponent<AudioSource> ();
	}

	void OnMouseDown () {

		// Only pick up the sword if one isn't already held
		if (isHeld == false) {

			// Only pick up the sword if it is close enough to the player
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, 5) == true) {

				// Trigger animation for room 3
				GetComponentInParent<Room3Listener> ().SendMessage ("SwordTaken");
			
				// Pick up the sword!
				GetSword ();

				// Then destroy the sword in the chest
				Destroy (this);
			}
		}
	}

	void GetSword () {

		// Create a copy of the sword in the chest
		Sword newSword = (Sword)Instantiate (this);

		// Make this sword the child of the main game camera
		Transform newTransform = newSword.transform;
		newTransform.parent = Camera.main.transform;

		// Normalize current transform
		newTransform.localPosition = Vector3.zero;
		newTransform.rotation = new Quaternion ();

		// Adjust transform to proper position and orienation
		newTransform.Translate (0.43f, -.35f, 0.92f, Camera.main.transform);
		newTransform.localScale = new Vector3 (1.5f, 1.5f, 1.5f);
		newTransform.Rotate (Vector3.up, 60, Space.Self);

		// Mark this sword as held
		newSword.isHeld = true;
	}

	//
	void Update () {

		// Swing sword if user left-clicks while not already swinging held sword
		if (Input.GetMouseButtonDown (0) == true && isHeld == true && isSwinging == false) {
			StartCoroutine (Swing ());
		}
	}

	// Swing sword to damage enemy
	IEnumerator Swing () {

		// Keep isSwinging true for duration of this method
		isSwinging = true;

		// Turn on collider to damage enemy
		this.GetComponent<BoxCollider> ().enabled = true;

		// Play sound effect (quick whooshing sound)
		audio.Play ();

		// Swing down
		for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime) {
			elapsed += Time.deltaTime;
			this.transform.Rotate (speed * 15f * Time.deltaTime, speed * 0f * Time.deltaTime, speed * 30f * Time.deltaTime, Space.Self);
			this.transform.Translate (-speed * 0.1f * Time.deltaTime, 0, speed * 0.1f * Time.deltaTime, Camera.main.transform);
			yield return new WaitForEndOfFrame ();
		}

		// Return from swing
		for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime) {
			elapsed += Time.deltaTime;
			this.transform.Rotate (-speed * 15f * Time.deltaTime, -speed * 0f * Time.deltaTime, -speed * 30f * Time.deltaTime, Space.Self);
			this.transform.Translate (speed * 0.1f * Time.deltaTime, 0, -speed * 0.1f * Time.deltaTime, Camera.main.transform);
			yield return new WaitForEndOfFrame ();
		}

		// Turn off collider to prevent untriggered damange to enemy
		this.GetComponent<BoxCollider> ().enabled = false;

		// Reset isSwinging to resting state
		isSwinging = false;
	}

	// Adjust transform to proper position and orienation
	void ResetTransform () {
		this.transform.localPosition = Vector3.zero;
		this.transform.Translate (0.43f, -.35f, 0.92f, Camera.main.transform);
		this.transform.rotation = new Quaternion ();
		this.transform.Rotate (Vector3.up, 60, Space.Self);
	}
}
