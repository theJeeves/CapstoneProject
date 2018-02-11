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

    private float _walkingSpeed;
    private Vector2 _origin;
    private RaycastHit2D _hit;
    private BoxCollider2D _GOBox;
    private Rigidbody2D _body;
    private bool _movingRight = true;
    private float _offset;
    private EnemyType _enemyType;
    private UnityAnimator _animator;
    private float _timer = 0.0f;
    private float _landingDuration = 0.0f;

    #endregion Private Fields

    #region Private Initializers
    private void OnEnable() {
        _GOBox = GetComponent<BoxCollider2D>();
        _body = GetComponent<Rigidbody2D>();
        _enemyType = GetComponent<EnemyBasicBehaviors>().enemyType;

        _body.AddForce(new Vector2(Random.Range(_xMinVelocity, _xMaxVelocity), Random.Range(_yMinVelocity, _yMaxVelocity)), ForceMode2D.Impulse);
        _walkingSpeed = Random.Range(_walkingSpeedRange.Min, _walkingSpeedRange.Max);
    }

    #endregion Private Initializers

    #region Private Methods
    private void FixedUpdate() {
        if (_canMove) {
            _body.velocity = new Vector2(_walkingSpeed, _body.velocity.y);
        }
    }

    private void Update() {

        if (_animator == null) {
            _animator = GetComponent<SpriterDotNetUnity.SpriterDotNetBehaviour>().Animator;
            _animator.Play(GetAnimation(1));
        }

        if (_canMove) {

            if (_timer != 0.0f && TimeTools.TimeExpired(ref _timer)) {
                _animator.Play(GetAnimation(2));
                GetComponent<AudioSource>().Play();
                _timer = 0.0f;
            }

            _offset = _movingRight ? _GOBox.bounds.max.x : _GOBox.bounds.min.x;

            _origin = _GOBox.bounds.center;
            _hit = Physics2D.Raycast(new Vector2(_offset, _origin.y), _direction, _distance);
            Debug.DrawRay(new Vector2(_offset, _origin.y), new Vector3(_direction.x * _distance, _direction.y * _distance, 0.0f), Color.green);

            if (Mathf.Abs(_body.velocity.x) <= DIRECTION_TOGGLE) {
                _movingRight = _movingRight ? false : true;
                _walkingSpeed *= -DIRECTION_TOGGLE;
                _direction.x *= -DIRECTION_TOGGLE;
            }

            if (_hit.collider == null) {
                   
                    _movingRight = _movingRight ? false : true;
                    _walkingSpeed *= -DIRECTION_TOGGLE;
                    _direction.x *= -DIRECTION_TOGGLE;
                }
               
                
            
            else {
                _body.MoveRotation(0.0f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D otherGO) {

        if (otherGO.gameObject.tag == StringConstantUtility.SOLID_GROUND_TAG && !_canMove) {
            _animator.Play(GetAnimation(0));
            _canMove = true;
            _timer = _landingDuration;
        }
        else if (_enemyType == EnemyType.ExplodingSwamer && otherGO.gameObject.tag == StringConstantUtility.PLAYER_TAG) {
            otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(DAMAGE_AMOUNT, DAMAGE_DURATION, DamageEnum.Explosion);
            GetComponent<EnemyBasicBehaviors>().DecrementHealth(100);
        }
    }

    private string GetAnimation(int animationNum) {

        List<string> animations = _animator.GetAnimations().ToList();
        _landingDuration = animations[0].Length * MS_CONVERSION;
        return animations[animationNum];
    }

    #endregion Private Methods

    #region Private Structures
    [System.Serializable]
    private struct Range {

        #region Properties
        [SerializeField]
        private float _min;
        public float Min {
            get { return _min; }
            set { _min = value; }
        }

        [SerializeField]
        private float _max;
        public float Max {
            get { return _max; }
            set { _max = value; }
        }

        #endregion Properties
    }

    #endregion Private Structures
}
