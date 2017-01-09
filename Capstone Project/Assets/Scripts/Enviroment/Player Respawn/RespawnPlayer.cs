using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

    [SerializeField]
    private SORespawn _respawnContainer;

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            otherGO.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            otherGO.transform.position = _respawnContainer.respawnPos;
        }
    }
}
