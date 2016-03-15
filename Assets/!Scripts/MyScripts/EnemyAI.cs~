using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnemyAI : MonoBehaviour {

	private Animation anim;
	private GameObject player;
	private Transform trans;
	private BoxCollider leftSpearCollider, rightSpearCollider;
	private AudioSource mainAudio, leftSpearAudio, rightSpearAudio;

	// Indicate if the enemy cannot be damaged
	private bool isInvulnerable;

	// The max health for the enemy
	[SerializeField]
	private int health = 5;

	void Start () {

		isInvulnerable = false;

		trans = this.GetComponent<Transform> ();
		player = GameObject.FindGameObjectWithTag ("Player");
		anim = this.GetComponent<Animation> ();

		// Get colliders and audio for the attached spears on this enemy
		GameObject leftSpear = GameObject.FindGameObjectWithTag ("Left Spear");
		GameObject rightSpear = GameObject.FindGameObjectWithTag ("Right Spear");

		leftSpearCollider = leftSpear.GetComponent<BoxCollider> ();
		rightSpearCollider = rightSpear.GetComponent<BoxCollider> ();
	
		leftSpearAudio = leftSpear.GetComponent<AudioSource> ();
		rightSpearAudio = rightSpear.GetComponent<AudioSource> ();
		mainAudio = this.GetComponent<AudioSource> ();

		// Add event listener for the death of the player
		UIManager.OnPlayerDeath += OnPlayerDeath;
	}

	void Update () {

		// Continually turn to face the player
		StartCoroutine (FacePlayer ());

		// Check how far away the player is from the enemy
		float distanceToPlayer = Vector3.Magnitude (trans.position - player.transform.position);

		// Only excecute action if currently in the waiting animation
		if (anim.IsPlaying ("wait")) {
		
			// If Player is too close, move away
			if (distanceToPlayer < 3.5f) {
				StartCoroutine (MoveBack ());
			}

			// If player is within range, Attack!
			else if (distanceToPlayer < 6.6f) {
				Swing ();
			} 

			// If player is too far away, move closer
			else {
				StartCoroutine (MoveForward ());
			}
		}
	}

	// Swing either the right or left spear
	void Swing () {

		Vector3 relativePoint = trans.InverseTransformPoint (player.transform.position);

		// If player is to the left of the enemy, swing left spear
		if (relativePoint.x <= 0) {
			leftSpearCollider.enabled = true;
			anim.Play ("swing_left");
			leftSpearAudio.Play ();
		} 

		// If player is to the right of the enemy, swing right spear
		else {
			rightSpearCollider.enabled = true;
			anim.Play ("swing_right");
			rightSpearAudio.Play ();
		}
	}

	// Turn off weapon colliders when not actively swinging
	void DisableWeapons () {
		leftSpearCollider.enabled = false;
		rightSpearCollider.enabled = false;
	}

	// This triggers when the enemy's spears have contacted the player.
	// Cause player to lose a health, and force enemy to move back
	void HitPlayer () {
		player.SendMessage ("HitPlayer");
		DisableWeapons ();
		StartCoroutine (MoveBack ());
	}

	// This triggers when the player swings a sword at the enemy.
	// Play growling sound effect, recoil animation, and reduce health
	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "Sword" && isInvulnerable == false) {
			DisableWeapons ();
			mainAudio.Play ();
			anim.Play ("hurt");
			health--;
			StartCoroutine (InvulnerableDelay ());
		}
	}

	// Allow the enemy to have a short window of invulnerability after each hit
	IEnumerator InvulnerableDelay () {
		isInvulnerable = true;
		yield return new WaitForSeconds (1);
		isInvulnerable = false;
	}

	// Continually rotate in the direction of the player
	IEnumerator FacePlayer () {
		Quaternion targetRot;
		while (true) {
			// Determine angle needed to face the player
			targetRot = Quaternion.LookRotation (player.transform.position - trans.position, Vector3.up);
			targetRot.x = 0f;
			targetRot.z = 0f;

			// Rotate to face player
			trans.rotation = Quaternion.RotateTowards (trans.rotation, targetRot, 0.17f * Time.deltaTime);
			yield return new WaitForEndOfFrame ();		
		}
	}

	// Move back a short distance, provided that there is sufficient distance behiind it
	IEnumerator MoveBack () {

		// Determine if enemy should move back or not (true if no object is behind enemy)
		Vector3 adjustedEnemyPos = new Vector3 (trans.position.x, trans.position.y + 2.5f, trans.position.z);

		if (!Physics.Raycast (adjustedEnemyPos, trans.forward * -1, 8)) {

			// Move for the duration of the indicated clip
			float duration = anim.GetClip ("move_back").length;

			anim.CrossFade ("move_back", 0.3f);

			// Rotate to face player and move back (if enough space behind enemy)
			for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime) {
				if (!Physics.Raycast (adjustedEnemyPos, trans.forward * -1, 5)) {
					trans.Translate (0, 0, -3f * Time.deltaTime);
				}
				yield return new WaitForEndOfFrame ();
			}
		}
	}

	// Move forward a short distance, provided that there is sufficient distance in front of it
	IEnumerator MoveForward () {

		// Determine if enemy should move back or not (true if no object is behind enemy)
		Vector3 adjustedEnemyPos = new Vector3 (trans.position.x, trans.position.y + 2.5f, trans.position.z);
		if (!Physics.Raycast (adjustedEnemyPos, trans.forward, 5)) {

			// Move for the duration of the indicated clip
			float duration = anim.GetClip ("move_forward").length;

			anim.CrossFade ("move_forward", 0.3f);

			// Rotate to face player and move forward (if enough space)
			for (float elapsed = 0; elapsed < duration; elapsed += Time.deltaTime) {

				if (!Physics.Raycast (adjustedEnemyPos, trans.forward, 5)) {
					trans.Translate (0, 0, 4 * Time.deltaTime);
				}
				yield return new WaitForEndOfFrame ();
			}
		}
	}
		
	// If all health missing, kill this enemy; otherwise just wait
	void LateUpdate () {
		if (health <= 0) {
			OnDeath ();
		} else {
			anim.PlayQueued ("wait");
		}
	}

	// When the player dies, send signal to room 4 to reset the room, and move this enemy
	// back into its original position.
	void OnPlayerDeath () {
		this.GetComponentInParent<Room4Listener> ().SendMessage ("Room4Exited");
		trans.position = new Vector3 (7, 0, 100);
	}


	// When this enemy dies, play death animation and trigger victory effects in room4
	void OnDeath () {
		StopAllCoroutines ();
		anim.Play ("dead");
		this.GetComponent<BoxCollider> ().enabled = false;

		this.GetComponentInParent<Room4Listener> ().SendMessage ("GameOver");
		UIManager.GameOver ();

		Destroy (this, anim.GetClip ("dead").length);
	}

	void OnDrawGizmos () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		Vector3 adjustedEnemyPos = new Vector3 (this.transform.position.x, this.transform.position.y + 2.5f, this.transform.position.z);	
		Gizmos.DrawRay (adjustedEnemyPos, this.transform.forward * 5);
	}
}