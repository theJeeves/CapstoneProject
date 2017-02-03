﻿using UnityEngine;
using System.Collections;

public enum EnemyType {
    Sniper,
    Swarmer,
    AcidSwarmer,
    ExplodingSwamer,
    Charger
}

public class EnemyHealth : MonoBehaviour {

    public EnemyType enemyType;
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _health;
    public float Health {
        get { return _health; }
        set { _health = value; }
    }

    [SerializeField]
    private SOEffects[] _SOEffects;
    private GameObject _effect;

    private int length = 0;
    private float _timer = 0.0f;
    private float _effectDelay = 0.0f;

    private void OnEnable() {

        if (enemyType == EnemyType.AcidSwarmer) {
            _timer = Time.time;
            _effectDelay = Random.Range(1.0f, 3.0f);
        }
        else if (enemyType == EnemyType.ExplodingSwamer) {
            _effect = _SOEffects[0].PlayEffect(transform.position);
        }

        length = _SOEffects.Length;
    }

    private void Start() {
        _health = _maxHealth;
    }

    private void OnDisable() {
        _SOEffects[0].StopEffect(_effect);
    }

    private void Update() {

        Vector3 position = transform.position;

        if (enemyType == EnemyType.AcidSwarmer) {

            if (Time.time - _timer > _effectDelay) {
                _SOEffects[0].PlayEffect(position, transform.eulerAngles.z + 90.0f);

                _SOEffects[1].PlayEffect(position);
                _effectDelay = Random.Range(1.0f, 3.0f);
                _timer = Time.time;
            }
        }

        else if (enemyType == EnemyType.ExplodingSwamer && _effect != null) {
            _effect.transform.position = new Vector3(position.x, position.y + 15.0f, position.z);
        }
    }

    public void DecrementHealth(int damage) {

        _health -= damage;
        if (_health <= 0.0f) {

            switch (enemyType) {

                case EnemyType.Sniper:
                    _SOEffects[0].PlayEffect(transform.position); break;

                case EnemyType.Swarmer:
                case EnemyType.AcidSwarmer:
                case EnemyType.ExplodingSwamer:
                    _SOEffects[length - 1].PlayEffect(transform.position); break;
            }
            Destroy(gameObject);
        }
        else {
            GetComponentInChildren<EnemyDamageEffect>().DamageEffect();
        }
    }
}
