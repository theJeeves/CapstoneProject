using UnityEngine;
using System.Collections;

public class RespawnPlayer : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.tag == "Player") {
            otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(100);
        }
    }
}
