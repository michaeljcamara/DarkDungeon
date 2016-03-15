using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	private static Text timer, youWin;
	private static Scrollbar healthBar;
	public static int maxHealth = 5;

	// Delegate for relaying player death
	public delegate void PlayerDeathAction ();

	public static event PlayerDeathAction OnPlayerDeath;

	void Start () {
		Text[] textList = this.GetComponentsInChildren<Text> (true);
		timer = textList [0];
		youWin = textList [1];
		healthBar = this.GetComponentInChildren<Scrollbar> ();
	}

	// Change the current label for the timer in the top-center of the screen
	public static void UpdateTimer (string seconds) {
		timer.text = seconds;
	}

	// Change the current amount of player health, indicated by a red scroll bar
	// in the upper left corner
	public static void AdjustHealth (float value) {
		healthBar.size += value / maxHealth;

		if (healthBar.size <= 0.001) { // Size can never reach 0 for scrollbars

			// Create event for the death of the player and reset health to max
			OnPlayerDeath ();
			healthBar.size = 1;
		}
	}

	// Show the "You Win!" text at the end of the game
	public static void GameOver () {
		youWin.enabled = true;
	}
}