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
    private int _maxHealth;

    [SerializeField]
    private int _health;

    [SerializeField]
    private float _recoveryTime;

    [Space]

    [Header("Effects")]

    [SerializeField]
    private SOEffects _acidDamageEfect;
    [SerializeField]
    private SOEffects _explosionDamageEffect;
    [SerializeField]
    private ScreenShakeRequest _scrnShkRequest;
    [SerializeField]
    private SOCheckpoint _SOCheckpoint;

    private Transform[] _effectPositions;
    private GameObject _effect;
    private DamageEnum _damageType;
    private SpriteRenderer[] _spriteRenderer;
    private bool _canTakeDamage;
    private float _timer = 0.0f;
    private int _damageReceived = 0;
    private float _duration = 0.0f;

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
        if (_health <= 0)
        {
            gameObject.transform.position = _SOCheckpoint.checkpointPosition;
            _health = _maxHealth;
            UpdateHealth(_health);
        }

        if (_effect != null) {
            _effect.transform.position = transform.position + new Vector3(0.0f, 25.0f, 0.0f);
        }

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
                _acidDamageEfect.StopEffect(_effect);
            }
        }
    }

    public void DecrementPlayerHealth(int damage, float duration = 0.0f, DamageEnum damageType = DamageEnum.None) {

        if (_canTakeDamage) {

            if (duration > 0.0f) {
                _damageReceived = damage;
                _duration = duration;

                if (damageType == DamageEnum.Acid) {
                    AcidDamageEffect();
                    _effect = _acidDamageEfect.PlayEffect(_effectPositions[1].position);
                    _damageType = damageType;
                }
                else if (damageType == DamageEnum.Explosion) {
                    ExplosionDamageEffect();
                    _effect = _explosionDamageEffect.PlayEffect(_effectPositions[1].position);
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

    private void AcidDamageEffect() {

        for (int i = 0; i < _spriteRenderer.Length; ++i) {
            _spriteRenderer[i].color = Color.green;
        }
    }

    private void ExplosionDamageEffect() {
        for (int i = 0; i < _spriteRenderer.Length; ++i) {
            _spriteRenderer[i].color = new Color(0.18f, 0.18f, 0.18f);
        }
    }

    private void ReturnToNormalColor() {

        float t = Time.deltaTime * (1.0f / _duration);

        for (int i = 0; i < _spriteRenderer.Length - 1; ++i) {

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
