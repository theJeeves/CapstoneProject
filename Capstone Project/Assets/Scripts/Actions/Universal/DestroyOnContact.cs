using UnityEngine;
using System.Collections;

public class DestroyOnContact : MonoBehaviour {

    [SerializeField]
    private GameObject _otherObject;


    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag == "Player") {
            Destroy(gameObject);
        }
    }
}