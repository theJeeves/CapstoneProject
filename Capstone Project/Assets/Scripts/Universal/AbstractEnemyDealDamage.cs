using UnityEngine;
using System.Collections;

public class AbstractEnemyDealDamage : MonoBehaviour {

    public delegate void AbstractEnemyDealDamageEvent(int damage);
    public static event AbstractEnemyDealDamageEvent DecrementHealth;

    [SerializeField]
    protected int _damage;

    protected virtual void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag == "Player" && DecrementHealth != null) {

            DecrementHealth(_damage);
        }
    }
}
