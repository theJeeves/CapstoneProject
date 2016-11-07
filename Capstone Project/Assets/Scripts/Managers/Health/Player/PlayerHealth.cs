using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public delegate void PlayerHealthEvent(int _health);
    public static event PlayerHealthEvent UpdateHealth;

    [SerializeField]
    private int _health;

    [SerializeField]
    private int _maxHealth;

    public int Health {
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

        if (UpdateHealth != null) {
            UpdateHealth(_health);
        }
    }

    private void decrementPlayerHealth(int damage) {
        _health -= damage;

        if (UpdateHealth != null) {
            UpdateHealth(_health);
        }
    }
}
