using UnityEngine;

public class SniperLockOn : MonoBehaviour {

    #region Constant Fields
    private const float MIN_PITCH = 0.75f;
    private const float MAX_PITCH = 1.5f;
    private const float SHOT_DELAY_DURATION = -2.0f;
    private const float X_SCALE = 1.0f;

    #endregion Constant Fields

    #region Private Fields
    [SerializeField]
    private SOEffects _SOEffectHandler = null;
    [SerializeField]
    private GameObject _endOfBarrel = null;
    [SerializeField]
    private float m_defaultShotDelay = 0.0f;

    private float m_shotDelay;
    private BoxCollider2D _playerPos;
    private Vector3 _direction;
    private Vector3 _localScale;
    private AudioSource _audioSource;
    private bool _canAttack;
    private GameObject _tellEffect;
    private SniperAnimations _animationManager;

    #endregion Private Fields

    #region Private Initializers
    private void Awake() {
        _animationManager = GetComponentInParent<SniperAnimations>();
    }

    private void OnEnable() {

        _audioSource = GetComponent<AudioSource>();
        _canAttack = true;
        RestartAttack();
    }

    #endregion Private Initializers

    #region Private Finalizers
    private void OnDisable() {
        _SOEffectHandler.StopEffect(_tellEffect);
    }

    #endregion Private Finalizers

    #region Private Methods
    private void Update() {

        if (_playerPos == null) {
            _playerPos = GameObject.FindGameObjectWithTag(StringConstantUtility.PLAYER_TAG).GetComponent<BoxCollider2D>();
        }

        _direction = (_playerPos.bounds.center - transform.position);
        _localScale = transform.localScale;
        transform.localScale = _direction.x > 0 ? new Vector3(X_SCALE, _localScale.y, _localScale.z)
            : new Vector3(-X_SCALE, _localScale.y, _localScale.z);

        if (_tellEffect != null) {
            _tellEffect.transform.position = _endOfBarrel.transform.position;
        }

        if (TimeTools.TimeExpired(ref m_shotDelay)) {
            Fire();
                
            if (TimeTools.TimeExpired(ref m_shotDelay, SHOT_DELAY_DURATION)) {
                RestartAttack();
            }
        }
    }

    private void Fire() {
        if (_canAttack) {
            _SOEffectHandler.PlayEffect(EffectEnums.SniperBullet, _endOfBarrel.transform.position);
            _audioSource.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
            _audioSource.Play();
            _animationManager.Play(3);
            _canAttack = false;
        }
    }

    private void RestartAttack() {
        m_shotDelay = m_defaultShotDelay;
        _animationManager.Play(2);
        _tellEffect = _SOEffectHandler.PlayEffect(EffectEnums.SniperTellEffect, _endOfBarrel.transform.position);
        _canAttack = true;
    }

    #endregion Private Methods
}