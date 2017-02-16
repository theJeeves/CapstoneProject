using UnityEngine;
using System.Collections;

public class EnableShotgun : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.gameObject.tag == "Player") {
            otherGO.gameObject.GetComponentInChildren<WeaponSelect>().SGAvailable = true;
        }
    }
}
