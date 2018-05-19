using UnityEngine;

public class SniperDealDamage : MonoBehaviour {

    [SerializeField]
    private int _damage;
    [SerializeField]
    private float _knockBack;
    [SerializeField]
    private SOEffects _SOEffectHandler;

    private void OnCollisionEnter2D(Collision2D otherGO) {

        string tag = otherGO.gameObject.tag;

        if (tag == "Player") {
            otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(_damage);
            Vector2 direction = otherGO.gameObject.GetComponent<BoxCollider2D>().bounds.center - transform.position;
            //otherGO.gameObject.GetComponent<Rigidbody2D>().AddForce(direction.normalized * _knockBack, ForceMode2D.Impulse);
            otherGO.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.normalized.x * _knockBack, direction.normalized.y * _knockBack);
        }

        _SOEffectHandler.PlayEffect(EffectEnums.SniperBulletImpact, transform.position);
        Destroy(gameObject);
    }

    public void Destroy() {
        _SOEffectHandler.PlayEffect(EffectEnums.SniperBulletImpact, transform.position);
        Destroy(gameObject);
    }
}