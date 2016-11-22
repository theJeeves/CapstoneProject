using UnityEngine;
using System.Collections;

public class SniperLockOn : MonoBehaviour {

    public delegate void SniperLockOnEvent();
    public static event SniperLockOnEvent Attack;

    [SerializeField]
    private float _timer;
    private Transform _playerPos;
    private Vector3 _direction;
    private Vector3 _localScale;

    private bool _canAttack;
    private float _startTime;

    private void Awake() {
        _playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void OnEnable() {
        _canAttack = true;
        _startTime = Time.time;
        SniperPushBack.Stun += PauseAttack;
    }

    private void OnDisable() {
        SniperPushBack.Stun -= PauseAttack;
    }

    private void PauseAttack() {
        StartCoroutine(StunResetAttack());
    }

    private void Update() {
        _direction = (_playerPos.position - transform.position);
        _localScale = transform.localScale;
        transform.localScale = _direction.x > 0 ? new Vector3(0.12f, _localScale.y, _localScale.z)
            : new Vector3(-0.12f, _localScale.y, _localScale.z);

        if (_canAttack) {

            if (Time.time - _startTime >= _timer) {
                if (Attack != null) {
                    Attack();
                    _startTime = Time.time;
                }
            }
        }
        else {
            _startTime = Time.time;
        }
    }

    private IEnumerator StunResetAttack() {

        _canAttack = false;
        yield return new WaitForSeconds(1.0f);
        _canAttack = true;
    }
}
