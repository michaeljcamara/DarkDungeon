using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {

	// Allow user to easily choose how this laser will move in the scene
	[SerializeField]
	private enum MoveType {
		None,
		Vertical,
		Horizontal,
		Rotational,
	};

	// Allow method for movement to be stored as a variable
	delegate void MovementMethod ();

	private MovementMethod move;

	[SerializeField]
	private MoveType moveType = MoveType.None;

	// The starting location, serving as a pivot point
	private Vector3 startPos;

	// -1 or 1 for negative or positive movement along an axis
	[SerializeField]
	private int direction;

	// Speed the laser moves in its specified direction
	[SerializeField]
	private float speed;

	[SerializeField]
	private float distance;

    private AudioSource audioSource;

	// Store the desired movement method at startup to allow quicker subsequent updates
	void Start () {

		switch (moveType) {
		case(MoveType.Rotational):
			move = RotationalMovement;
			break;
		case(MoveType.Horizontal):
			move = HorizontalMovement;
			break;
		case(MoveType.Vertical):
			move = VerticalMovement;
			break;
		default:
			move = DoNothing;
			break;
		}

		startPos = this.transform.position;
        audioSource = GetComponent<AudioSource>();
	}

	// Use the movement method determined at startup
	void FixedUpdate () {
		move ();
	}

	// Move laser along X axis, pivoting around starting position
	void HorizontalMovement () {
		this.transform.Translate (direction * speed * Time.deltaTime, 0, 0, Space.World);

		if (Mathf.Abs (this.transform.position.x - startPos.x) > distance) {
			direction *= -1;
		}
	}

	// Move laser along Y axis, pivoting around starting position
	void VerticalMovement () {
		this.transform.Translate (0, direction * speed * Time.deltaTime, 0, Space.World);

		if (Mathf.Abs (this.transform.position.y - startPos.y) > distance) {
			direction *= -1;
		}
	}

	// Rotate clockwise or counterclockwise along its center
	void RotationalMovement () {
		this.transform.Rotate (Vector3.back, direction * speed * 30 * Time.deltaTime, Space.World);
	}

	// When laser is triggered by player, send message to room 2 listener to shut door
	// and disable all currently active lasers
	void OnTriggerEnter (Collider collider) {

		if (collider.gameObject.tag == "Player") {
			this.SendMessageUpwards ("LaserTriggered");
		}
		audioSource.Play ();
	}

	// Dummy method to prevent null pointer from delegate
	void DoNothing () {
	}
}