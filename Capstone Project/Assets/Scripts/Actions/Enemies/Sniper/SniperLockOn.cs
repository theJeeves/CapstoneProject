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

    private void Awake() {
        _playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void OnEnable() {
        StartCoroutine(LockOn());
    }

    private void Update() {
        _direction = (_playerPos.position - transform.position);
        _localScale = transform.localScale;
        transform.localScale = _direction.x > 0 ? new Vector3(0.12f, _localScale.y, _localScale.z)
            : new Vector3(-0.12f, _localScale.y, _localScale.z);
    }

    private IEnumerator LockOn() {

        while (true) {
            yield return new WaitForSeconds(_timer);

            if (Attack != null) {
                Attack();
            }
        }
    }
}
