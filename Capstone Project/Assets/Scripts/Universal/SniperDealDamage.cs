using UnityEngine;
using System.Collections;

public class SniperDealDamage : MonoBehaviour {

    public delegate void SniperDealDamageEvent(int damage);
    public static event SniperDealDamageEvent DecrementPlayerHealth;

    [SerializeField]
    private int _damage;

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag == "Player") {

            if (DecrementPlayerHealth != null) {
                DecrementPlayerHealth(_damage);
            }
            Destroy(gameObject);
        }
    }
}