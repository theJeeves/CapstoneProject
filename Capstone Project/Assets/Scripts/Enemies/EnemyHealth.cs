﻿using UnityEngine;
using System.Collections;

/// <summary>
/// WHEN TALKING ABOUT THE EFFECTS THE THREE DIFFERNT TYPES OF SWARMERS WILL HAVE,
/// THE LAST ELEMENT IN THE ARRAY WILL ALWAYS BE THE DEATH EFFECT. THE ONES BEFORE IT
/// WILL BE THE ATTACKS.
/// </summary>

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
    private SOEffects _SOEffectHandler;
    private GameObject _effect;


    private Transform[] _effectPositions;
    private float _timer = 0.0f;
    private float _effectDelay = 0.0f;

    private void OnEnable() {

        if (enemyType == EnemyType.AcidSwarmer) {
            _timer = Time.time;
            _effectDelay = Random.Range(1.0f, 3.0f);
        }
        else if (enemyType == EnemyType.ExplodingSwamer) {
            _effect = _SOEffectHandler.PlayEffect(EffectEnums.SwarmerExplosiveEffect, transform.position);
        }

        _effectPositions = GetComponentsInChildren<Transform>();
    }

    private void Start() {
        _health = _maxHealth;
    }

    private void OnDisable() {
        if (_SOEffectHandler != null) {
            _SOEffectHandler.StopEffect(_effect);
        }
    }

    private void Update() {

        //Vector3 position = transform.position;

        if (enemyType == EnemyType.AcidSwarmer) {

            if (Time.time - _timer > _effectDelay) {
                _SOEffectHandler.PlayEffect(EffectEnums.AcidSquirt, _effectPositions[1].position, transform.eulerAngles.z + 90.0f);

                _SOEffectHandler.PlayEffect(EffectEnums.AcidBall, _effectPositions[1].position);
                _effectDelay = Random.Range(1.0f, 3.0f);
                _timer = Time.time;
            }
        }

        else if (enemyType == EnemyType.ExplodingSwamer && _effect != null) {
            _effect.transform.position = _effectPositions[1].position;
        }
    }

    public void DecrementHealth(int damage) {

        _health -= damage;
        if (_health <= 0.0f) {

            switch (enemyType) {

                case EnemyType.Sniper:
                    _SOEffectHandler.PlayEffect(EffectEnums.SniperDeathExplosion, transform.position); break;

                case EnemyType.Swarmer:
                case EnemyType.AcidSwarmer:
                case EnemyType.ExplodingSwamer:
                    _SOEffectHandler.PlayEffect(EffectEnums.SwarmerDeathExplosion, transform.position); break;
            }
            Destroy(gameObject);
        }
        else {
            GetComponentInChildren<EnemyDamageEffect>().DamageEffect();
        }
    }
}
