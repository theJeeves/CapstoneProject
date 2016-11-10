using UnityEngine;
using System.Collections;

public class ChargerDealDamage : MonoBehaviour {

    public delegate void ChargerDealDamageEvent(int damage);
    public static event ChargerDealDamageEvent DecrementPlayerHealth;

    [SerializeField]
    private int _damage;

    private void OnCollisionEnter2D(Collision2D collider) {

        if (collider.gameObject.tag == "Player" && DecrementPlayerHealth != null) {
            DecrementPlayerHealth(_damage);
        }
    }
}
