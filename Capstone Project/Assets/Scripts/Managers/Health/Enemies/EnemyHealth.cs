using UnityEngine;
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
    }

    private void OnDisable() {
        ShotgunBlast.DamageEnemy -= DecrementHealth;
    }

    private void Start() {
        _health = _maxHealth;
    }

    private void DecrementHealth(float damage) {

        _health -= damage;

    }
}
