using UnityEngine;
using System.Collections;

public class ChargerLockOn : LockOntoPlayer {

    //public static event LockOntoPlayerEvent Attack;

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

    protected override void OnEnable() {
        _body2d = GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _hit = Physics2D.Raycast(transform.position, _direction, _attackRange, _whatToHit);


        base.OnEnable();
    }

    protected override IEnumerator LockOn() {


        while (true) {

            if (_hit.collider != null && _hit.collider.tag == "Player") {
                _body2d.velocity = Vector2.zero;
                yield return new WaitForSeconds(_timer);
                _body2d.AddForce(Vector2.right * _chargeSpeed * _direction.x, ForceMode2D.Impulse);
                break;
            }


            _direction = (_player.position - transform.position).normalized;
            _hit = Physics2D.Raycast(transform.position, _direction, _attackRange, _whatToHit);
            Debug.DrawLine(transform.position, transform.position + (Vector3.right * _attackRange * _direction.x), Color.yellow);
            _body2d.velocity = new Vector2(_walkSpeed * _direction.x, _body2d.velocity.y);




            yield return 0;
        }
        StartCoroutine(AttackReset());

    }

    private IEnumerator AttackReset() {
        _hit = new RaycastHit2D();
        _direction = (_player.position - transform.position).normalized;

        yield return new WaitForSeconds(_resetTimer);

        StartCoroutine(LockOn());
    }


}
