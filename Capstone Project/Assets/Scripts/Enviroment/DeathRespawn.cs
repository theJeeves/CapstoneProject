using UnityEngine;
using System.Collections;

public class DeathRespawn : MonoBehaviour {

    private Vector3 _respawnPoint;

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            otherGO.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            otherGO.transform.position = _respawnPoint;
        }
    }

    private void LastGrounded(Vector3 playerPos) {
        _respawnPoint = playerPos;
    }
}
