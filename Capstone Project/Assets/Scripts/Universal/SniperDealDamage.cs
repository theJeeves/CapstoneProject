using UnityEngine;
using System.Collections;

public class SniperDealDamage : MonoBehaviour {

    [SerializeField]
    private int _damage;
    [SerializeField]
    private float _knockBack;
    [SerializeField]
    private SOEffects _SOEffectHandler;

    private void OnCollisionEnter2D(Collision2D otherGO) {

        string tag = otherGO.gameObject.tag;

        if (tag != "Enemy") {

            if (tag == "Player") {
                otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(_damage);
                Vector2 direction = new Vector2(transform.position.x, transform.position.y) - otherGO.gameObject.GetComponent<PolygonCollider2D>().offset;
                otherGO.gameObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * _knockBack, ForceMode2D.Impulse);
            }

            _SOEffectHandler.PlayEffect(EffectEnums.SniperBulletImpact, transform.position);
            Destroy(gameObject);
        }
    }
}