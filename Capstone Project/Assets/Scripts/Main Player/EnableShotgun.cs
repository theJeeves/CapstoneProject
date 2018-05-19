using UnityEngine;

public class EnableShotgun : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D otherGO) {

        if (otherGO.gameObject.tag == Tags.PlayerTag) {
            otherGO.gameObject.GetComponentInChildren<WeaponSelect>().SGAvailable = true;
        }
    }
}
