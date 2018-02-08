using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2ExitTrigger : MonoBehaviour {

    // Cause countdown to stop when player leaves room2
    void OnTriggerEnter(Collider collider) {
        if (collider.tag == "Player") {
            SendMessageUpwards("Room2Exited");
        }
    }
}
