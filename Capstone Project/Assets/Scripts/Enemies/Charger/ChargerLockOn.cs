using UnityEngine;
using System.Collections;
using System;

public class ChargerLockOn : MonoBehaviour {

    #region Fields

    #region Protected Fields
    [SerializeField]
    protected float _timer;

    #endregion Protected Fields


    #region Private Fields
    [SerializeField]
    private float _resetTimer;
    [SerializeField]
    private LayerMask _whatToHit;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _chargeSpeed;

    private Rigidbody2D _body2d;
    private RaycastHit2D _hit;
    private Transform _player;
    private Vector2 _direction;
    private ChargerAnimations _animationManager;

    #endregion Private Fields

    #endregion Fields

    #region Private Initializers
    private void OnEnable() {
        _body2d = GetComponentInParent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag(StringConstantUtility.PLAYER_TAG).transform;
        _hit = Physics2D.Raycast(transform.position, _direction, _attackRange, _whatToHit);
        _animationManager = GetComponentInParent<ChargerAnimations>();

        StartCoroutine(LockOn());
    }

    #endregion Private Initializers

    #region Events
    public static event EventHandler<float> RocketAnim;

    #endregion Events

    #region Private Mehtods
    private IEnumerator LockOn() {

        while (true) {

            if (_hit.collider != null && _hit.collider.tag == StringConstantUtility.PLAYER_TAG) {

                _animationManager.Play(3);
                _body2d.velocity = Vector2.zero;
                //Start the "tell" animation for this enemy.
                RocketAnim?.Invoke(this, _timer);
 
                yield return new WaitForSeconds(_timer);

                // Charge toward the player.
                _animationManager.Play(4);
                _body2d.AddForce(Vector2.right * _chargeSpeed * _direction.x, ForceMode2D.Impulse);
                break;
            }

            // Raycast to determine if we are in range of the player. Continually walk toward the player if not.
            _direction = (_player.position - transform.position).normalized;
            _direction = new Vector2(_direction.x, 0.0f);
            _hit = Physics2D.Raycast(transform.position, _direction, _attackRange, _whatToHit);
            Debug.DrawLine(transform.position, transform.position + (Vector3.right * _attackRange * _direction.x), Color.yellow);
            _body2d.velocity = new Vector2(_walkSpeed * _direction.x, _body2d.velocity.y);

            // Correct facing direction while walking toward the player
            Vector3 localScale = transform.localScale;
            transform.localScale = _direction.x > 0 ? new Vector3(1.0f, localScale.y, localScale.z)
                : new Vector3(-1.0f, localScale.y, localScale.z);

            yield return 0;
        }
        StartCoroutine(AttackReset());
    }

    private IEnumerator AttackReset() {

        _hit = new RaycastHit2D();
        _direction = (_player.position - transform.position).normalized;

        yield return new WaitForSeconds(_resetTimer);

        _animationManager.Play(2);
        StartCoroutine(LockOn());
    }

    #endregion Private Methods
}
