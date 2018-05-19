using UnityEngine;
using SpriterDotNetUnity;
using System.Collections.Generic;
using System.Linq;

public class SwarmerBehavior : MonoBehaviour {

    #region Constants
    private const float MS_CONVERSION = 0.001f;
    private const int DAMAGE_AMOUNT = 25;
    private const float DAMAGE_DURATION = 3.0f;
    private const float DIRECTION_TOGGLE = 1.0f;
    private const int FULL_HEALTH = 100;
    #endregion Constants

    #region Private Fields
    [SerializeField]
    private float _xMinVelocity = -150.0f;
    [SerializeField]
    private float _xMaxVelocity = 150.0f;
    [SerializeField]
    private float _yMinVelocity = 100.0f;
    [SerializeField]
    private float _yMaxVelocity = 500.0f;
    [SerializeField]
    private Range _walkingSpeedRange;
    [SerializeField]
    private Vector2 _direction;
    [SerializeField]
    private float _distance;
    [SerializeField]
    private bool _canMove = false;

    private float m_WalkingSpeed = float.NaN;
    private Vector2 m_Origin = Vector2.zero;
    private RaycastHit2D m_Hit;
    private BoxCollider2D m_GOBox = null;
    private Rigidbody2D m_Body = null;
    private bool m_MovingRight = true;
    private float m_Offset = float.NaN;
    private EnemyType m_EnemyType;
    private UnityAnimator m_Animator = null;
    private float m_Timer = 0.0f;
    private float m_LandingDuration = 0.0f;

    #endregion Private Fields

    #region Initializers
    private void OnEnable() {
        m_GOBox = GetComponent<BoxCollider2D>();
        m_Body = GetComponent<Rigidbody2D>();
        m_EnemyType = GetComponent<EnemyBasicBehaviors>().enemyType;

        m_Body.AddForce(new Vector2(Random.Range(_xMinVelocity, _xMaxVelocity), Random.Range(_yMinVelocity, _yMaxVelocity)), ForceMode2D.Impulse);
        m_WalkingSpeed = Random.Range(_walkingSpeedRange.Min, _walkingSpeedRange.Max);
    }

    #endregion Initializers

    #region Private Methods
    private void FixedUpdate() {
        if (_canMove) {
            m_Body.velocity = new Vector2(m_WalkingSpeed, m_Body.velocity.y);
        }
    }

    private void Update() {

        if (m_Animator == null) {
            m_Animator = GetComponent<SpriterDotNetUnity.SpriterDotNetBehaviour>().Animator;
            m_Animator.Play(GetAnimation(1));
        }

        if (_canMove) {

            if (m_Timer != 0.0f && TimeTools.TimeExpired(ref m_Timer))
            {
                m_Animator.Play(GetAnimation(2));
                GetComponent<AudioSource>().Play();
                m_Timer = 0.0f;
            }

            m_Offset = m_MovingRight ? m_GOBox.bounds.max.x : m_GOBox.bounds.min.x;

            m_Origin = m_GOBox.bounds.center;
            m_Hit = Physics2D.Raycast(new Vector2(m_Offset, m_Origin.y), _direction, _distance);
            Debug.DrawRay(new Vector2(m_Offset, m_Origin.y), new Vector3(_direction.x * _distance, _direction.y * _distance, 0.0f), Color.green);

            if (Mathf.Abs(m_Body.velocity.x) <= DIRECTION_TOGGLE)
            {
                m_MovingRight = m_MovingRight ? false : true;
                m_WalkingSpeed *= -DIRECTION_TOGGLE;
                _direction.x *= -DIRECTION_TOGGLE;
            }

            if (m_Hit.collider == null)
            {                   
                m_MovingRight = m_MovingRight ? false : true;
                m_WalkingSpeed *= -DIRECTION_TOGGLE;
                _direction.x *= -DIRECTION_TOGGLE;
            }          
            else
            {
                m_Body.MoveRotation(0.0f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D otherGO)
    {
        if (otherGO?.gameObject.tag == Tags.SolidGroundTag && !_canMove) {
            m_Animator.Play(GetAnimation(0));
            _canMove = true;
            m_Timer = m_LandingDuration;
        }
        else if (m_EnemyType == EnemyType.ExplodingSwamer && otherGO?.gameObject.tag == Tags.PlayerTag)
        {
            otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(DAMAGE_AMOUNT, DAMAGE_DURATION, DamageEnum.Explosion);
            GetComponent<EnemyBasicBehaviors>().DecrementHealth(FULL_HEALTH);
        }
    }

    private string GetAnimation(int animationNum)
    {
        List<string> animations = m_Animator.GetAnimations().ToList();
        m_LandingDuration = animations[0].Length * MS_CONVERSION;
        return animations[animationNum];
    }

    #endregion Private Methods

    #region Private Structures
    [System.Serializable]
    private struct Range
    {
        #region Properties
        [SerializeField]
        private float _min;
        public float Min
        {
            get { return _min; }
            set { _min = value; }
        }

        [SerializeField]
        private float _max;
        public float Max
        {
            get { return _max; }
            set { _max = value; }
        }

        #endregion Properties
    }

    #endregion Private Structures
}
