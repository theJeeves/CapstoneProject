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

    private float m_ShotDelay = float.NaN;
    private BoxCollider2D m_PlayerPos = null;
    private Vector3 m_Direction = Vector3.zero;
    private Vector3 m_LocalScale = Vector3.zero;
    private AudioSource m_AudioSource = null;
    private bool m_CanAttack = false;
    private GameObject m_TellEffect = null;
    private SniperAnimations m_AnimationManager = null;

    #endregion Private Fields

    #region Initializers
    private void Awake() {
        m_AnimationManager = GetComponentInParent<SniperAnimations>();
    }

    private void OnEnable() {

        m_AudioSource = GetComponent<AudioSource>();
        m_CanAttack = true;
        RestartAttack();
    }

    #endregion Initializers

    #region Finalizers
    private void OnDisable() {
        _SOEffectHandler.StopEffect(m_TellEffect);
    }

    #endregion Finalizers

    #region Private Methods
    private void Update() {

        if (m_PlayerPos == null) {
            m_PlayerPos = GameObject.FindGameObjectWithTag(Tags.PlayerTag).GetComponent<BoxCollider2D>();
        }

        m_Direction = (m_PlayerPos.bounds.center - transform.position);
        m_LocalScale = transform.localScale;
        transform.localScale = m_Direction.x > 0 ? new Vector3(X_SCALE, m_LocalScale.y, m_LocalScale.z)
            : new Vector3(-X_SCALE, m_LocalScale.y, m_LocalScale.z);

        if (m_TellEffect != null) {
            m_TellEffect.transform.position = _endOfBarrel.transform.position;
        }

        if (TimeTools.TimeExpired(ref m_ShotDelay)) {
            Fire();
                
            if (TimeTools.TimeExpired(ref m_ShotDelay, SHOT_DELAY_DURATION)) {
                RestartAttack();
            }
        }
    }

    private void Fire() {
        if (m_CanAttack) {
            _SOEffectHandler.PlayEffect(EffectEnums.SniperBullet, _endOfBarrel.transform.position);
            m_AudioSource.pitch = Random.Range(MIN_PITCH, MAX_PITCH);
            m_AudioSource.Play();
            m_AnimationManager.Play(3);
            m_CanAttack = false;
        }
    }

    private void RestartAttack() {
        m_ShotDelay = m_defaultShotDelay;
        m_AnimationManager.Play(2);
        m_TellEffect = _SOEffectHandler.PlayEffect(EffectEnums.SniperTellEffect, _endOfBarrel.transform.position);
        m_CanAttack = true;
    }

    #endregion Private Methods
}