using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

    public delegate void PlayerHealthEvent(int _health);
    public static event PlayerHealthEvent UpdateHealth;

    [SerializeField]
    private ScreenShakeRequest _scrnShkRequest;
    [SerializeField]
    private SOAnimations _damageAnimation;

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
            _scrnShkRequest.ShakeRequest();

            if (UpdateHealth != null) {
                UpdateHealth(_health);
            }
        }
    }

    private IEnumerator RecoveryDelay() {
        _canTakeDamage = false;

        float timer = 0.0f;
        while (timer < _recoveryTime) {

            _damageAnimation.PlayAnimation();
            yield return new WaitForSeconds(_recoveryTime / 10.0f);
            _damageAnimation.StopAnimation();
            yield return new WaitForSeconds(_recoveryTime / 10.0f);

            timer += _recoveryTime / 5.0f;
        }

        _canTakeDamage = true;
    }
}
