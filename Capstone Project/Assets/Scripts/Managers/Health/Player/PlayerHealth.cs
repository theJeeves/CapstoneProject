using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public delegate void PlayerHealthEvent();
    public static event PlayerHealthEvent UpdateHealth;

    [SerializeField]
    private float _health;

    [SerializeField]
    private float _maxHealth;

    public float Health {
        get { return _health; }
        set { _health = value; }
    }

    private void OnEnable() {
        DestroyOnContact.DecrementHealth += decrementPlayerHealth;
    }

    private void OnDisable() {
        DestroyOnContact.DecrementHealth -= decrementPlayerHealth;
    }

    void Start() {
        _health = _maxHealth;
    }

    private void decrementPlayerHealth(float damage) {
        _health -= damage;
    }
}
