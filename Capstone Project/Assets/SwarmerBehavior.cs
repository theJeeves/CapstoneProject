using UnityEngine;
using System.Collections;

public class SwarmerBehavior : MonoBehaviour {
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

    private float _walkingSpeed;
    private Vector2 _origin;
    private RaycastHit2D _hit;
    private BoxCollider2D _GOBox;
    private Rigidbody2D _body;
    private bool _movingRight = true;
    private float _offset;

    [SerializeField]
    private bool _canMove = false;

    private EnemyType _enemyType;

    [System.Serializable]
    private struct Range {

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
    }

    private SpriterDotNetUnity.SpriterDotNetBehaviour _animator;
    private float _timer = 0.0f;
    private float _landingDuration = 0.0f;

    private void OnEnable() {
        _GOBox = GetComponent<BoxCollider2D>();
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<SpriterDotNetUnity.SpriterDotNetBehaviour>();
        _enemyType = GetComponent<EnemyHealth>().enemyType;

        _body.AddForce(new Vector2(Random.Range(_xMinVelocity, _xMaxVelocity), Random.Range(_yMinVelocity, _yMaxVelocity)), ForceMode2D.Impulse);
        _walkingSpeed = Random.Range(_walkingSpeedRange.Min, _walkingSpeedRange.Max);
    }


    private void FixedUpdate() {
        if (_canMove) {
            _body.velocity = new Vector2(_walkingSpeed, _body.velocity.y);
        }
    }

    private void Update() {

        if (_canMove) {

            if (Time.time - _timer > _landingDuration && _timer != 0.0f) {
                _animator.Animator.Play(_animator.SpriterData.Spriter.Entities[0].Animations[3]);
                _timer = 0.0f;
            }

            _offset = _movingRight ? _GOBox.bounds.max.x : _GOBox.bounds.min.x;

            _origin = _GOBox.bounds.center;
            _hit = Physics2D.Raycast(new Vector2(_offset, _origin.y), _direction, _distance);
            Debug.DrawRay(new Vector2(_offset, _origin.y), new Vector3(_direction.x * _distance, _direction.y * _distance, 0.0f), Color.green);

            if (Mathf.Abs(_body.velocity.x) <= 1.0f) {
                _movingRight = _movingRight ? false : true;
                _walkingSpeed *= -1.0f;
                _direction.x *= -1.0f;
            }

            if (_hit.collider == null) {
                _movingRight = _movingRight ? false : true;
                _walkingSpeed *= -1.0f;
                _direction.x *= -1.0f;
            }
            else {
                _body.MoveRotation(0.0f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D otherGO) {
        if (otherGO.gameObject.tag == "SolidGround" && !_canMove) {
            _animator.Animator.Play(_animator.SpriterData.Spriter.Entities[0].Animations[1]);
            _landingDuration = _animator.SpriterData.Spriter.Entities[0].Animations[1].Length * 0.001f;
            _canMove = true;
            _timer = Time.time;
        }
        else if (_enemyType == EnemyType.ExplodingSwamer && otherGO.gameObject.tag == "Player") {
            otherGO.gameObject.GetComponent<PlayerHealth>().DecrementPlayerHealth(15, 3.0f, DamageEnum.Explosion);
            GetComponent<EnemyHealth>().DecrementHealth(100);
        }
    }

}
