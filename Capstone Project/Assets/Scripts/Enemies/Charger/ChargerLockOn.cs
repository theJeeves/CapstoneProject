using UnityEngine;
using System.Collections;

public class ChargerLockOn : MonoBehaviour {

    public delegate void ChargerLockOnEvent(float time);
    public delegate void ChargerLockOnEvent2();
    public static event ChargerLockOnEvent RocketAnim;
    public static event ChargerLockOnEvent2 ResetRockets;

    [SerializeField]
    protected float _timer;

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

    private void OnEnable() {
        _body2d = GetComponentInParent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _hit = Physics2D.Raycast(transform.position, _direction, _attackRange, _whatToHit);
        _animationManager = GetComponentInParent<ChargerAnimations>();

        StartCoroutine(LockOn());
    }

    private IEnumerator LockOn() {

        while (true) {

            if (_hit.collider != null && _hit.collider.tag == "Player") {

                _animationManager.Play(3);
                _body2d.velocity = Vector2.zero;
                //Start the "tell" animation for this enemy.
                if (RocketAnim != null) {
                    RocketAnim(_timer);
                }
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

        if (ResetRockets != null) {
            ResetRockets();
        }

        _animationManager.Play(2);
        StartCoroutine(LockOn());
    }


}
