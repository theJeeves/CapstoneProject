using UnityEngine;
using System.Collections;

public class SetRespawnPosition : MonoBehaviour {

    [SerializeField]
    private SORespawn _respawnContainer;

    private Vector3 _boxPosition;

    private void Awake() {
        _boxPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            _respawnContainer.respawnPos = transform.position;
        }
    }
}
