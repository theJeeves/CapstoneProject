using UnityEngine;
using System.Collections;

public class CrystalBulletBehavior : MonoBehaviour {

    public delegate void CystalBulletBehaviorEvent(float damage);
    public static event CystalBulletBehaviorEvent DamageEnemy;

    [SerializeField]
    private float _damageAmount;

    private void OnCollisionEnter2D(Collision2D otherObject) { 

        if (otherObject.gameObject.tag == "Enemy" && DamageEnemy != null) {
            DamageEnemy(_damageAmount);
            Destroy(gameObject);
        }
    }
}
