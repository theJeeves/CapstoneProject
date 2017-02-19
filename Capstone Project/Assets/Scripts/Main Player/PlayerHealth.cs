using UnityEngine;
using System.Collections;

public enum DamageEnum {
    None,
    Acid,
    Explosion
}

public class PlayerHealth : MonoBehaviour {

    public delegate void PlayerHealthEvent(int _health);
    public static event PlayerHealthEvent UpdateHealth;

    [Header("Health Variables")]
    [SerializeField]
    private int _maxHealth;                 // Max health the player can have at any time. Default value when the player respawns.
    [SerializeField]
    private int _health;                    // The health the player current has.
    [SerializeField]
    private float _recoveryTime;            // How long before the player can receive damage again.

    [Space]

    [Header("Effects")]

    [SerializeField]
    private SOEffects _SOEffectHandler;                 // Reference to the SOEffectHandler so this gameobject can request effects.
    [SerializeField]
    private ScreenShakeRequest _scrnShkRequest;         // Screenshake request to show damage has been done to the player.
    [SerializeField]
    private SOCheckpoint _SOCheckpointHandler;                 // Reference to the checkpoint system. This is where the player will respawn on death.

    private Transform[] _effectPositions;           // Positions where effects will be played.
    private GameObject _effect;                     // GameObject to reference a called effect. This is so the effect may be manipulated.
    private DamageEnum _damageType;                 // Type of damage so this script can call different effect types.
    private SpriteRenderer[] _spriteRenderer;       // Refernece to all the sprites associated with the player for acid and explosion effects.
    private bool _canTakeDamage;                    // Bool determining if the player is able to take damage or not. Tied with _recoveryTime.
    private float _timer = 0.0f;                    // Timer is used to determine how much time has passed for effect durations.
    private int _damageReceived = 0;                // How much damage has the player received.
    private float _duration = 0.0f;                 // Duration for an effect to last.

    // Getter/Setter for other scripts to refernce the player's health.
    public int Health {
        get { return _health; }
        set { _health = value; }
    }

    private void Awake() {
        _canTakeDamage = true;
    }

    private void OnEnable() {
        _spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        _effectPositions = GetComponentsInChildren<Transform>();
    }

    void Start() {
        _health = _maxHealth;

        if (UpdateHealth != null) {
            UpdateHealth(_health);
        }
    }

    private void Update() {

        // if the player dies, stop any velocity the player had, place them at the last checkpoint, and reset their health to 100%.
        if (_health <= 0) {
            GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            transform.position = _SOCheckpointHandler.checkpointPosition;

            if (Camera.main.WorldToViewportPoint(_SOCheckpointHandler.checkpointPosition).x > 0.0f &&
                Camera.main.WorldToViewportPoint(_SOCheckpointHandler.checkpointPosition).x < 1.0f) {

                GetComponent<Rigidbody2D>().gravityScale = 40.0f;
                _SOEffectHandler.PlayEffect(EffectEnums.PlayerRespawn, _SOCheckpointHandler.checkpointPosition);
                _health = _maxHealth;
                UpdateHealth(_health);
            }
        }

        // if there is any type of effect associated with the player, ensure it is following them with a slight offset.
        if (_effect != null) {
            _effect.transform.position = transform.position + new Vector3(0.0f, 25.0f, 0.0f);
        }

        // If an enemy has damaged the player and their is an effect to be played,
        // keep calling the damage and effect to do damage over time.
        if (_duration != 0.0f) {
            if (Time.time - _timer < _duration) {

                if (_damageType == DamageEnum.Acid && _canTakeDamage) {
                    _health -= _damageReceived;
                    _scrnShkRequest.ShakeRequest();
                    StartCoroutine(RecoveryDelay());
                    if (UpdateHealth != null) {
                        UpdateHealth(_health);
                    }
                }

                ReturnToNormalColor();
            }
            else {
                _duration = 0.0f;
                _SOEffectHandler.StopEffect(_effect);
            }
        }
    }

    /// <summary>
    /// The function is generally usesd by other game objects which can damage the player. This function handles various damage types.
    /// Some attacks will have an instant damage on the health while other may have a damage over time. 
    /// Each one will damage the player and, if applicable, call the SOEffectHandler to play the appropriate effect.
    /// </summary>
    public void DecrementPlayerHealth(int damage, float duration = 0.0f, DamageEnum damageType = DamageEnum.None) {

        if (_canTakeDamage) {

            if (duration > 0.0f) {
                _damageReceived = damage;
                _duration = duration;

                if (damageType == DamageEnum.Acid) {
                    AcidDamageEffect();
                    _effect = _SOEffectHandler.PlayEffect(EffectEnums.AcidDamageEffect, _effectPositions[1].position);
                    _damageType = damageType;
                }
                else if (damageType == DamageEnum.Explosion) {
                    ExplosionDamageEffect();
                    _effect = _SOEffectHandler.PlayEffect(EffectEnums.ExplosionDamageEffect, _effectPositions[1].position);
                    _damageType = damageType;
                    _health -= damage;
                    _scrnShkRequest.ShakeRequest();
                    StartCoroutine(RecoveryDelay());

                    if (UpdateHealth != null) {
                        UpdateHealth(_health);
                    }
                }

                _timer = Time.time;
            }
            else {
                StartCoroutine(RecoveryDelay());

                _health -= damage;
                _scrnShkRequest.ShakeRequest();

                if (UpdateHealth != null) {
                    UpdateHealth(_health);
                }
            }
        }
    }

    // Go through all the sprites for the player and render them green
    // for an acid damage effect.
    private void AcidDamageEffect() {

        for (int i = 0; i < _spriteRenderer.Length; ++i) {
            _spriteRenderer[i].color = Color.green;
        }
    }

    // Go through all the sprites for the player and render them
    // dark grey for an explosion effect
    private void ExplosionDamageEffect() {
        for (int i = 0; i < _spriteRenderer.Length; ++i) {
            _spriteRenderer[i].color = new Color(0.18f, 0.18f, 0.18f);
        }
    }

    // Regardless of what color a damage effect has turned the player,
    // this goes through all the sprites for the player and renders them
    // back to their normal state.
    private void ReturnToNormalColor() {

        float t = Time.deltaTime * (1.0f / _duration);

        for (int i = 0; i < _spriteRenderer.Length; ++i) {

            _spriteRenderer[i].color = new Color(Mathf.Clamp01(_spriteRenderer[i].color.r + t), Mathf.Clamp01(_spriteRenderer[i].color.g + t),
                Mathf.Clamp01(_spriteRenderer[i].color.b + t));
        }
    }

    private IEnumerator RecoveryDelay() {
        _canTakeDamage = false;

        float timer = 0.0f;
        while (timer < _recoveryTime) {

            //_damageAnimation.PlayAnimation();
            yield return new WaitForSeconds(_recoveryTime / 10.0f);
            //_damageAnimation.StopAnimation();
            yield return new WaitForSeconds(_recoveryTime / 10.0f);

            timer += _recoveryTime / 5.0f;
        }

        _canTakeDamage = true;
    }
}
