﻿using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {


    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private float _health;
    public float Health {
        get { return _health; }
        set { _health = value; }
    }

    private void OnEnable() {
        ShotgunBlast.DamageEnemy += DecrementHealth;
        CrystalBullet.DamageEnemy += DecrementHealth;
    }

    private void OnDisable() {
        ShotgunBlast.DamageEnemy -= DecrementHealth;
        CrystalBullet.DamageEnemy -= DecrementHealth;
    }

    private void Start() {
        _health = _maxHealth;
    }

    private void DecrementHealth(int damage) {

        _health -= damage;

        if (_health <= 0.0f) {

            Destroy(gameObject);
        }
    }
}
