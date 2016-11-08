using UnityEngine;
using System.Collections;

public class ChargerAttack : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D otherObject) {

        if(otherObject.gameObject.tag == "Player") {

        }
    }
}
