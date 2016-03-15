using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	// Distance to hold ball away from player
	private float depth;

	// Indicate whether ball is held by player or not
	private bool isHeld;

	private Rigidbody body;
	private SphereCollider sphereCollider;

	void Start () {
		depth = 1.5f;
		isHeld = false;
		body = this.GetComponent<Rigidbody> ();
		sphereCollider = this.GetComponent<SphereCollider> ();

		// Ensure that ball can be triggered with mouse when collider deactivated
		Physics.queriesHitTriggers = true;
	}
		
	// If ball is held by player, then keep holding it until thrown
	void Update () {
		if (isHeld == true) {
			HoldObject (depth);
		}
	}

	// Keep ball at arm-length away from player, based on mouse position
	void HoldObject (float depth) {
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = depth;
		Vector3 worldMousePos = Camera.main.ScreenToWorldPoint (mousePos);
		this.transform.position = new Vector3 (worldMousePos.x, worldMousePos.y, worldMousePos.z);
	}

	// When mouse is clicked, either throw the ball (if held) or pick up a new ball (if not held)
	public void OnMouseDown () {

		// Create a ray into the scene based on mouse position
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		// If object is held, throw it
		if (isHeld == true) {

			// Re-enable collider and gravity to allow collision with bottles
			sphereCollider.enabled = true;
			body.useGravity = true;

			// Ensure no additional velocity is incorporate in physics
			body.velocity = new Vector3 (0, 0, 0);

			// Aim for a target in the scene based on mouse position
			Vector3 target = ray.direction;
			target.x *= 500;
			target.z *= 500;
			target.y = (target.y + 0.5f) * 250;

			// Throw the ball!
			body.AddForce (target, ForceMode.Acceleration);
			this.GetComponent<AudioSource> ().Play ();
			isHeld = false;
			 
			// Create a new ball on the crates in front of table
			Instantiate (this, new Vector3 (6, 5, -9), new Quaternion ());
		} 

		// If ball not currently held, then hold it (if close enough)
		else if (Physics.Raycast (ray, 5) == true) {
			sphereCollider.enabled = false;
			body.useGravity = false;
			isHeld = true;
		}
	}

	void OnDrawGizmos () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Gizmos.DrawRay (ray.origin, ray.GetPoint (1000));
	}
}