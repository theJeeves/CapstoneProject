using UnityEngine;
using System.Collections;

public class RespawnPoint : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D otherGO) {
        if (otherGO.tag == "Player") {
            SendMessageUpwards("LastGrounded", otherGO.transform.position);
        }
    }
}
