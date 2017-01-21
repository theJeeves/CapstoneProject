using UnityEngine;
using System.Collections;

public enum EnemyType {
    Sniper,
    Swarmer,
    Charger
}

public class EnemyHealth : MonoBehaviour {

    [SerializeField]
    private EnemyType _enemyType;
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

            switch (_enemyType) {

                case EnemyType.Sniper:
                    _SOEffect.PlayEffect(EffectEnum.SniperDeathExplosion, transform.position); break;

                case EnemyType.Swarmer:
                    _SOEffect.PlayEffect(EffectEnum.SwarmerDeathExplosion, transform.position); break;
            }
            Destroy(gameObject);
        }
        else {
            GetComponentInChildren<EnemyDamageEffect>().DamageEffect();
        }
    }
}
