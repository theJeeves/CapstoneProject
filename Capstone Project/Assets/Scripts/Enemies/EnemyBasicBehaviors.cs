using UnityEngine;
using System.Collections;

/// <summary>
/// WHEN TALKING ABOUT THE EFFECTS THE THREE DIFFERNT TYPES OF SWARMERS WILL HAVE,
/// THE LAST ELEMENT IN THE ARRAY WILL ALWAYS BE THE DEATH EFFECT. THE ONES BEFORE IT
/// WILL BE THE ATTACKS.
/// </summary>

public enum EnemyType {
    Sniper,
    Swarmer,
    AcidSwarmer,
    ExplodingSwamer,
    Charger,
    Flying
}

public class EnemyBasicBehaviors : MonoBehaviour {

    public delegate void EnemyBasicBehaviorEvent(EnemyType type);
    public static event EnemyBasicBehaviorEvent OnDeath;

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
    private SOEffects _SOEffectHandler;
    private GameObject _effect;

    [Header("Body Parts")]
    [SerializeField]
    private SOEnemyBodyParts _SOBodyParts;


    private Transform[] _effectPositions;
    private float _timer = 0.0f;
    private float _effectDelay = 0.0f;

    private void OnEnable() {

        if (enemyType == EnemyType.AcidSwarmer) {
            _timer = Time.time;
            _effectDelay = Random.Range(1.0f, 3.0f);
        }
        else if (enemyType == EnemyType.ExplodingSwamer) {
            _effect = _SOEffectHandler.PlayEffect(EffectEnums.SwarmerExplosiveEffect, transform.position);
        }
        else if (enemyType == EnemyType.Flying) {
            _effect = _SOEffectHandler.PlayEffect(EffectEnums.Flying_Swarmer_Exhaust, transform.position);
        }
        else if (enemyType == EnemyType.Charger) {
            _effect = _SOEffectHandler.PlayEffect(EffectEnums.ChargerExhaust, transform.position);
        }

        _effectPositions = GetComponentsInChildren<Transform>();
    }

    private void Start() {
        _health = _maxHealth;
    }

    private void OnDisable() {
        if (_SOEffectHandler != null) {
            _SOEffectHandler.StopEffect(_effect);
        }
    }

    private void Update() {

        if (enemyType == EnemyType.AcidSwarmer) {

            if (Time.time - _timer > _effectDelay) {
                _SOEffectHandler.PlayEffect(EffectEnums.AcidSquirt, _effectPositions[1].position, transform.eulerAngles.z + 90.0f);

                _SOEffectHandler.PlayEffect(EffectEnums.AcidBall, _effectPositions[1].position);
                _effectDelay = Random.Range(1.0f, 3.0f);
                _timer = Time.time;
            }
        }

        else if (_effect != null && enemyType == EnemyType.ExplodingSwamer) {
            _effect.transform.position = _effectPositions[1].position;
        }
        else if (_effectPositions != null && enemyType == EnemyType.Flying) {
            _effect.transform.position = new Vector3(_effectPositions[1].position.x, _effectPositions[1].position.y, 1.0f);
        }
        else if (_effectPositions != null && enemyType == EnemyType.Charger) {

            _effect.transform.position = new Vector3(_effectPositions[2].position.x, _effectPositions[2].position.y, 1.0f);
            _effect.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -13.5f * _effectPositions[1].localScale.x);
        }
    }

    // IN THIS CASE, COLLISIONS ARE PRIMARILY USED TO SEE IF CERTAIN TYPES OF ENEMIES SHOULD
    // DAMAGE THE PLAYER.
    private void OnCollisionEnter2D(Collision2D otherGO) {

        if (otherGO.gameObject.tag == "Player") {

            if (enemyType == EnemyType.Flying) {
                otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(7);
                int direction = otherGO.gameObject.transform.position.x > transform.position.x ? 1 : -1;
                otherGO.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(175.0f * direction, 100.0f);
            }
        }
    }

    public void DecrementHealth(int damage) {

        _health -= damage;
        if (_health <= 0.0f) {

            switch (enemyType) {

                case EnemyType.Sniper:
                case EnemyType.Charger:
                    _SOEffectHandler.PlayEffect(EffectEnums.SniperDeathExplosion, transform.position);
                    break;

                case EnemyType.Swarmer:
                case EnemyType.AcidSwarmer:
                case EnemyType.ExplodingSwamer:
                case EnemyType.Flying:
                    _SOEffectHandler.PlayEffect(EffectEnums.SwarmerDeathExplosion, transform.position); break;
            }
      
            if (OnDeath != null) { OnDeath(enemyType); }
            DeployBodyParts();
            Destroy(gameObject);
        }
        else {
            GetComponentInChildren<EnemyDamageEffect>().DamageEffect();
        }
    }

    private void DeployBodyParts() {

        for (int i = 0; i < _SOBodyParts.bodyParts.Length; ++i) {
            GameObject instance = Instantiate(_SOBodyParts.bodyParts[i], transform.position, Quaternion.identity) as GameObject;
            instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-275.0f, 275.0f), Random.Range(100.0f, 300.0f)), ForceMode2D.Impulse);
            instance.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-500.0f, 500.0f));
        }
    }
}
