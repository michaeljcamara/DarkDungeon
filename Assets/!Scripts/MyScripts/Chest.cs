using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

    private AudioSource audioSource;
    private bool isOpened = false;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Trigger room animation on click, which will open chest and reveal sword
    void OnMouseDown () {

        if (!isOpened) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, 5)) {
                SendMessageUpwards("ChestOpened");
                audioSource.Play();
                isOpened = true;
            }
        }
	}
}