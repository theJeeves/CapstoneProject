using UnityEngine;
using System.Collections;

public class EnemyWalkBackandForth : MonoBehaviour {
    [SerializeField]
    private float _walkingSpeed;
    [SerializeField]
    private Vector2 _direction;
    [SerializeField]
    private float _distance;


    private Vector2 _origin;
    private RaycastHit2D _hit;
    private SpriteRenderer _sprite;
    private Rigidbody2D _body;
    private bool _movingRight = true;
    private float _offset;

    private bool _canMove = false;

    private void OnEnable() {
        _sprite = GetComponent<SpriteRenderer>();
        _body = GetComponent<Rigidbody2D>();

        StartCoroutine(Delay());
    }


    private void FixedUpdate() {
        if (_canMove) {
            //_body.velocity = new Vector2(_walkingSpeed, _body.velocity.y);
        }
    }

    private void Update() {

        _offset = _movingRight ? _sprite.bounds.max.x : _sprite.bounds.min.x;

        _origin = _sprite.bounds.center;
        _hit = Physics2D.Raycast(new Vector2(_offset, _origin.y), _direction, _distance);
        Debug.DrawRay(new Vector2(_offset, _origin.y), new Vector3(_direction.x * _distance, _direction.y * _distance, 0.0f), Color.green);

        if (_hit.collider.tag == "SolidGround") {
            _canMove = true;

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
        else {
            _canMove = false;
        }
    }

    private IEnumerator Delay() {
        yield return new WaitForSeconds(3.0f);
        _body.AddForce(new Vector2(Random.Range(-150.0f, 150.0f), Random.Range(100.0f, 500.0f)), ForceMode2D.Impulse);
        _canMove = true;
    }

}
