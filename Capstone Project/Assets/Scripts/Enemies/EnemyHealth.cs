using UnityEngine;
using System.Collections;

public enum EnemyType {
    Sniper,
    Swarmer,
    AcidSwarmer,
    ExplodingSwamer,
    Charger
}

public class EnemyHealth : MonoBehaviour {

    public EnemyType enemyType;
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
    private GameObject _effect;

    private float _timer = 0.0f;
    private float _effectDelay = 0.0f;

    private void OnEnable() {

        if (enemyType == EnemyType.AcidSwarmer) {
            _timer = Time.time;
            _effectDelay = Random.Range(1.0f, 3.0f);
        }
        else if (enemyType == EnemyType.ExplodingSwamer) {
            _effect = _SOEffect.PlayEffect(EffectEnum.ExplosiveSwarmerEffect, transform.position);
        }

    }

    private void Start() {
        _health = _maxHealth;
    }

    private void OnDisable() {
        _SOEffect.StopEffect(_effect);
    }

    private void Update() {
        if (enemyType == EnemyType.AcidSwarmer) {

            if (Time.time - _timer > _effectDelay) {
                _SOEffect.PlayEffect(EffectEnum.AcidSwarmerSpill, transform.position, transform.eulerAngles.z + 90.0f);

                _SOEffect.PlayEffect(EffectEnum.AcidSwarmerBall, transform.position);
                _effectDelay = Random.Range(1.0f, 3.0f);
                _timer = Time.time;
            }
        }

        else if (enemyType == EnemyType.ExplodingSwamer && _effect != null) {
            _effect.transform.position = transform.position + new Vector3(0.0f, 15.0f, 0.0f);
        }
    }

    public void DecrementHealth(int damage) {

        _health -= damage;
        if (_health <= 0.0f) {

            switch (enemyType) {

                case EnemyType.Sniper:
                    _SOEffect.PlayEffect(EffectEnum.SniperDeathExplosion, transform.position); break;

                case EnemyType.Swarmer:
                case EnemyType.AcidSwarmer:
                case EnemyType.ExplodingSwamer:
                    _SOEffect.PlayEffect(EffectEnum.SwarmerDeathExplosion, transform.position); break;
            }
            Destroy(gameObject);
        }
        else {
            GetComponentInChildren<EnemyDamageEffect>().DamageEffect();
        }
    }
}
