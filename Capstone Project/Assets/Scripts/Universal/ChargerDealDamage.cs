using UnityEngine;
using System.Collections;

public class ChargerDealDamage : MonoBehaviour {

    public delegate void ChargerDealDamageEvent(int damage);
    public static event ChargerDealDamageEvent DecrementPlayerHealth;

    public delegate void KnockPlayerUpEvent();
    public static event KnockPlayerUpEvent KnockUp;

    [SerializeField]
    private int _damage;

    private void OnCollisionEnter2D(Collision2D collider) {

        if (collider.gameObject.tag == "Player" && DecrementPlayerHealth != null && KnockUp != null) {
            DecrementPlayerHealth(_damage);
            KnockUp();
        }
    }
}
