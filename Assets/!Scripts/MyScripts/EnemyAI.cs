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

    // How quickly the enemy turns to face the player
    public float turnSpeed = 1;

    // How quickly the enemy moves toward player
    public float moveSpeed = 1;

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

        RotateToPlayer();

		// Only excecute action if currently in the waiting animation
		if (anim.IsPlaying ("wait")) {

            // Check how far away the player is from the enemy
            float distanceToPlayer = Vector3.Magnitude(trans.position - player.transform.position);

            // If Player is too close, move away
            if (distanceToPlayer < 4.5f) {
                MoveBackward();
			}

			// If player is within range, Attack!
			else if (distanceToPlayer < 6.5f) {
				Swing ();
			} 

			// If player is too far away, move closer
			else {
                MoveForward();
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
    void RotateToPlayer() {
        Vector3 targetDir = player.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, Time.deltaTime * turnSpeed, 0.0f);
        newDir.y = 0;
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    // Move forward a short distance, provided that there is sufficient distance in front of it
    void MoveForward() {

        anim.CrossFade("move_forward", 0.3f);

        Vector3 adjustedEnemyPos = new Vector3(trans.position.x, trans.position.y + 2.5f, trans.position.z);
        if (!Physics.Raycast(adjustedEnemyPos, trans.forward, Time.deltaTime * moveSpeed)) {
            transform.Translate(0, 0, Time.deltaTime * moveSpeed);
        }
    }

    // Move backward a short distance, provided that there is sufficient distance in back of it
    void MoveBackward() {

        anim.CrossFade("move_back", 0.3f);

        Vector3 adjustedEnemyPos = new Vector3(trans.position.x, trans.position.y + 2.5f, trans.position.z);
        if (!Physics.Raycast(adjustedEnemyPos, trans.forward * -1, Time.deltaTime * moveSpeed)) {
            transform.Translate(0, 0, -1 * Time.deltaTime * moveSpeed);
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