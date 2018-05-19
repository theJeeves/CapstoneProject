using UnityEngine;
using UnityEngine.Events;

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

    #region Constants
    private const float MIN_EFFECT_DELAY = 1.0f;
    private const float MAX_EFFECT_DELAY = 3.0f;
    private const float Z_ANGLE_OFFSET = 90.0f;
    private const float CHARGER_Z_ANGLE_OFFSET = -13.5f;
    private const float FLYING_X_FORCE = 175.0f;
    private const float FLYING_Y_FORCE = 100.0f;
    private const int FLYING_DAMAGE_AMOUNT = 15;
    private const float MIN_X_BODY_PARTS = -275.0f;
    private const float MAX_X_BODY_PARTS = 275.0f;
    private const float MIN_Y_BODY_PARTS = 100.0f;
    private const float MAX_Y_BODY_PARTS = 300.0f;
    private const float MIN_TORQUE = -500.0f;
    private const float MAX_TORQUE = 500.0f;

    #endregion Constants

    #region Fields

    #region Public Fields
    public EnemyType enemyType;

    #endregion Public Fields

    #region Private Fields
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private SOEffects _SOEffectHandler;

    private GameObject _effect;

    [Header("Body Parts")]
    [SerializeField]
    private SOEnemyBodyParts _SOBodyParts;

    private Transform[] _effectPositions;
    private float m_effectDelay = 0.0f;

    #endregion Private Fields

    #endregion Fields

    #region Private Initializers
    private void OnEnable() {

        if (enemyType == EnemyType.AcidSwarmer) {
            m_effectDelay = UnityEngine.Random.Range(MIN_EFFECT_DELAY, MAX_EFFECT_DELAY);
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

    #endregion Private Initializers

    #region Private Finalizers
    private void OnDisable() {

        _SOEffectHandler?.StopEffect(_effect);
    }

    #endregion Private Finalizers

    #region Events
    public static event UnityAction<EnemyType> EnemyDeath;

    #endregion Events

    #region Properties
    [SerializeField]
    private float _health;
    public float Health {
        get { return _health; }
        set { _health = value; }
    }

    #endregion Properties

    #region Public Methods
    /// <summary>
    /// Decrement the health of an instantiated enemy by an integer value.
    /// </summary>
    /// <param name="damage"></param>
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
                    _SOEffectHandler.PlayEffect(EffectEnums.SwarmerDeathExplosion, transform.position);
                    break;
            }

            EnemyDeath?.Invoke(enemyType);
            DeployBodyParts();
            Destroy(gameObject);
        }
        else {
            GetComponentInChildren<EnemyDamageEffect>().DamageEffect();
        }
    }

    #endregion Public Methods

    #region Private Methods
    private void LateUpdate() {

        if (enemyType == EnemyType.AcidSwarmer) {

            if (TimeTools.TimeExpired(ref m_effectDelay)) {
                _SOEffectHandler.PlayEffect(EffectEnums.AcidSquirt, _effectPositions[1].position, transform.eulerAngles.z + Z_ANGLE_OFFSET);

                _SOEffectHandler.PlayEffect(EffectEnums.AcidBall, _effectPositions[1].position);
                m_effectDelay = UnityEngine.Random.Range(MIN_EFFECT_DELAY, MAX_EFFECT_DELAY);
            }
        }

        else if (_effect != null && enemyType == EnemyType.ExplodingSwamer) {
            _effect.transform.position = _effectPositions[1].position;
        }
        else if (_effectPositions != null && enemyType == EnemyType.Flying) {
            _effect.transform.position = new Vector3(_effectPositions[1].position.x, _effectPositions[1].position.y, 0.0f);

            if (TimeTools.TimeExpired(ref m_effectDelay)) {
                _SOEffectHandler.PlayEffect(EffectEnums.AcidSquirt, _effectPositions[2].position, transform.eulerAngles.z + Z_ANGLE_OFFSET);

                _SOEffectHandler.PlayEffect(EffectEnums.AcidBall, _effectPositions[2].position);
                m_effectDelay = UnityEngine.Random.Range(MIN_EFFECT_DELAY, MAX_EFFECT_DELAY);
            }

        }
        else if (_effectPositions != null && enemyType == EnemyType.Charger) {

            _effect.transform.position = new Vector3(_effectPositions[2].position.x, _effectPositions[2].position.y, 1.0f);
            _effect.transform.localEulerAngles = new Vector3(0.0f, 0.0f, CHARGER_Z_ANGLE_OFFSET * _effectPositions[1].localScale.x);
        }
    }

    // IN THIS CASE, COLLISIONS ARE PRIMARILY USED TO SEE IF CERTAIN TYPES OF ENEMIES SHOULD
    // DAMAGE THE PLAYER.
    private void OnCollisionEnter2D(Collision2D otherGO) {

        if (otherGO.gameObject.tag == Tags.PlayerTag) {

            if (enemyType == EnemyType.Flying) {
                otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(FLYING_DAMAGE_AMOUNT);
                int direction = otherGO.gameObject.transform.position.x > transform.position.x ? 1 : -1;
                otherGO.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(FLYING_X_FORCE * direction, FLYING_Y_FORCE);
            }
        }
    }

    private void DeployBodyParts() {

        for (int i = 0; i < _SOBodyParts.bodyParts.Length; ++i) {
            GameObject instance = Instantiate(_SOBodyParts.bodyParts[i], transform.position, Quaternion.identity) as GameObject;
            instance.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(MIN_X_BODY_PARTS, MAX_X_BODY_PARTS), UnityEngine.Random.Range(MIN_Y_BODY_PARTS, MAX_Y_BODY_PARTS)), ForceMode2D.Impulse);
            instance.GetComponent<Rigidbody2D>().AddTorque(UnityEngine.Random.Range(MIN_TORQUE, MAX_TORQUE));
        }
    }

    #endregion Private Methods
}
