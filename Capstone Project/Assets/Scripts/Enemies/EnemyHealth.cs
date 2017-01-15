using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public delegate void EnemyHealthEvent(GameObject thisEnemy);
    public static event EnemyHealthEvent Damaged;


    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _health;
    public float Health {
        get { return _health; }
        set { _health = value; }
    }

    [SerializeField]
    private SOEffects _SOEffect;

    private void Start() {
        _health = _maxHealth;
    }

    public void DecrementHealth(int damage) {

        _health -= damage;
        if (_health <= 0.0f) {
            _SOEffect.PlayEffect(EffectEnum.SniperDeathExplosion, transform.position);
            Destroy(gameObject);
        }
        else {
            GetComponentInChildren<EnemyDamageEffect>().DamageEffect();
        }


        ////whatGotHit is technically the enemy body where the collider is located.
        ////We look to its parent, which is where the health script is located.
        //if (whatGotHit.transform.parent.gameObject == gameObject) {
        //    _health -= damage;

        //    if (_health <= 0.0f) {

        //        Destroy(gameObject);
        //    }
        //    else if (Damaged != null) {
        //        Damaged(gameObject);
        //    }
        //}
    }
}
