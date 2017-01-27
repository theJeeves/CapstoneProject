using UnityEngine;
using System.Collections;

public class SniperLockOn : MonoBehaviour {

    [SerializeField]
    private SOEffects _SOEffects;

    [SerializeField]
    private GameObject _endOfBarrel;
    [SerializeField]
    private GameObject _bullet;

    [SerializeField]
    private float _shotDelay;
    private Transform _playerPos;
    private Vector3 _direction;
    private Vector3 _localScale;

    private bool _canAttack;
    private float _startTime;
    private GameObject _tellEffect;

    private void Awake() {
        _playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void OnEnable() {
        _canAttack = true;
        RestartAttack();
    }

    private void OnDisable() {
        _SOEffects.StopEffect(_tellEffect);
    }

    private void Update() {

        _direction = (_playerPos.position - transform.position);
        _localScale = transform.localScale;
        transform.localScale = _direction.x > 0 ? new Vector3(0.12f, _localScale.y, _localScale.z)
            : new Vector3(-0.12f, _localScale.y, _localScale.z);

        if (_tellEffect != null) {
            _tellEffect.transform.position = _endOfBarrel.transform.position;
        }


        if (Time.time - _startTime >= _shotDelay) {
            Fire();
                
            if (Time.time - _startTime >= _shotDelay + 2.0f) {
                RestartAttack();
            }
        }
    }

    private void Fire() {
        if (_canAttack) {
            Instantiate(_bullet, _endOfBarrel.transform.position, Quaternion.identity);
            _canAttack = false;
        }
    }

    private void RestartAttack() {
        _startTime = Time.time;
        _tellEffect = _SOEffects.PlayEffect(EffectEnum.SniperTellEffect, _endOfBarrel.transform.position);
        _canAttack = true;
    }
}
