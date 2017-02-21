using UnityEngine;
using System.Collections;

public class SniperLockOn : MonoBehaviour {

    [SerializeField]
    private SOEffects _SOEffectHandler;

    [SerializeField]
    private GameObject _endOfBarrel;

    [SerializeField]
    private float _shotDelay;
    private PolygonCollider2D _playerPos;
    private Vector3 _direction;
    private Vector3 _localScale;

    private bool _canAttack;
    private float _startTime;
    private GameObject _tellEffect;
    private SniperAnimations _animationManager;

    private void Awake() {
        _playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<PolygonCollider2D>();
        _animationManager = GetComponentInParent<SniperAnimations>();
    }

    private void OnEnable() {
        _canAttack = true;
        RestartAttack();
    }

    private void OnDisable() {
        _SOEffectHandler.StopEffect(_tellEffect);
    }

    private void Update() {

        _direction = (_playerPos.bounds.center - transform.position);
        _localScale = transform.localScale;
        transform.localScale = _direction.x > 0 ? new Vector3(1.0f, _localScale.y, _localScale.z)
            : new Vector3(-1.0f, _localScale.y, _localScale.z);

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
            _SOEffectHandler.PlayEffect(EffectEnums.SniperBullet, _endOfBarrel.transform.position);
            _animationManager.Play(3);
            _canAttack = false;
        }
    }

    private void RestartAttack() {
        _startTime = Time.time;
        _animationManager.Play(2);
        _tellEffect = _SOEffectHandler.PlayEffect(EffectEnums.SniperTellEffect, _endOfBarrel.transform.position);
        _canAttack = true;
    }
}
