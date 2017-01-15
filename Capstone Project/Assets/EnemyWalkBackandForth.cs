using UnityEngine;
using System.Collections;

public class EnemyWalkBackandForth : MonoBehaviour {

    [SerializeField]
    private Range _walkingSpeedRange;
    [SerializeField]
    private Vector2 _direction;
    [SerializeField]
    private float _distance;

    private float _walkingSpeed;
    private Vector2 _origin;
    private RaycastHit2D _hit;
    private SpriteRenderer _sprite;
    private Rigidbody2D _body;
    private bool _movingRight = true;
    private float _offset;

    [SerializeField]
    private bool _canMove = false;

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

    private void OnEnable() {
        _sprite = GetComponent<SpriteRenderer>();
        _body = GetComponent<Rigidbody2D>();

        _body.AddForce(new Vector2(Random.Range(-150.0f, 150.0f), Random.Range(100.0f, 500.0f)), ForceMode2D.Impulse);
        _walkingSpeed = Random.Range(_walkingSpeedRange.Min, _walkingSpeedRange.Max);
    }


    private void FixedUpdate() {
        if (_canMove) {
            _body.velocity = new Vector2(_walkingSpeed, _body.velocity.y);
        }
    }

    private void Update() {

        if (_canMove) {
            _offset = _movingRight ? _sprite.bounds.max.x : _sprite.bounds.min.x;

            _origin = _sprite.bounds.center;
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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "SolidGround") {
            _canMove = true;
        }
    }

}
