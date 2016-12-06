using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public delegate void PlayerHealthEvent(int _health);
    public delegate void PlayerHealthEvent2(float time);

    public static event PlayerHealthEvent UpdateHealth;
    public static event PlayerHealthEvent2 StartInvulnAnim;

    [SerializeField]
    private int _maxHealth;

    [SerializeField]
    private int _health;

    [SerializeField]
    private float _recoveryTime;

    private bool _canTakeDamage;

    public int Health {
        get { return _health; }
        set { _health = value; }
    }

    private void Awake() {
        _canTakeDamage = true;
    }

    private void OnEnable() {
        SniperDealDamage.DecrementPlayerHealth += DecrementPlayerHealth;
        ChargerDealDamage.DecrementPlayerHealth += DecrementPlayerHealth;
        laserBeam.DecrementPlayerHealth += DecrementPlayerHealth;
    }

    private void OnDisable() {
        SniperDealDamage.DecrementPlayerHealth -= DecrementPlayerHealth;
        ChargerDealDamage.DecrementPlayerHealth -= DecrementPlayerHealth;
        laserBeam.DecrementPlayerHealth -= DecrementPlayerHealth;
    }

    void Start() {
        _health = _maxHealth;

        if (UpdateHealth != null) {
            UpdateHealth(_health);
        }
    }

    private void DecrementPlayerHealth(int damage) {
        if (_canTakeDamage) {

            StartCoroutine(RecoveryDelay());

            _health -= damage;

            if (UpdateHealth != null) {
                UpdateHealth(_health);
            }
        }
    }

    private IEnumerator RecoveryDelay() {
        _canTakeDamage = false;

        // Send the event so the character sprite can flash for a short time.
        if (StartInvulnAnim != null) {
            StartInvulnAnim(_recoveryTime);
        }

        yield return new WaitForSeconds(_recoveryTime);
        _canTakeDamage = true;
    }
}
