using UnityEngine;
using System.Collections;

/*
 * This class will act as the base class for all weapons in the game.
 */

public abstract class AbstractGun : MonoBehaviour {

    // Delegate to work with UI
    public delegate void AbstractGunEvent(int numOfRounds);
    // Delegate to work with ScreenShake feature
    public delegate void AbstractGunEvent2();
    public delegate void AbstractGunEvent3(float reloadTime);

    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite {
        get { return _sprite; }
    }
    [SerializeField]
    protected MovementRequest _moveRequest;
    [SerializeField]
    protected ScreenShakeRequest _SSRequest;

    [SerializeField]
    protected int _clipSize;
    
    public int numOfRounds;

    [SerializeField]
    protected float _shotDelay;
    [SerializeField]
    protected float _normReloadTime;
    [SerializeField]
    protected float _fastReloadTime;

    protected float _reloadTime;

    protected bool _reloading = false;
    // This keeps a state of whether the player was in the air or not
    // when the reload was called. This is to prevent the player from calling
    // reload when they landed, switching weapons, and the the slow reload is called.
    protected bool _grounded;

    protected bool _canShoot = true;
    protected bool _damaged = false;

    protected ControllableObject _controller;
    protected PlayerCollisionState _collisionState;

    [SerializeField]
    protected GameObject _bullet;
    [SerializeField]
    protected Transform _mgBarrel;

    protected virtual void Awake() {

        _controller = GameObject.FindGameObjectWithTag("Player").GetComponent<ControllableObject>();
        _collisionState = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionState>();

        numOfRounds = _clipSize;
    }

    protected virtual void OnEnable()
    {
        ControllableObject.OnButtonDown += OnButtonDown;
        PlayerCollisionState.OnHitGround += Reload;
        //ReloadWeapon.Reload += ManualReload;
        //PlayerActions.Reload += ManualReload;
        ChargerDealDamage.DecrementPlayerHealth += DamageReceived;

        _reloading = false;
        _canShoot = true;

        if (numOfRounds <= 0) {
            Reload();
        }

        if (numOfRounds == _clipSize) {
            _canShoot = true;
        }
    }

    protected virtual void OnDisable()
    {
        ControllableObject.OnButtonDown -= OnButtonDown;
        PlayerCollisionState.OnHitGround -= Reload;
        //ReloadWeapon.Reload -= ManualReload;
        //PlayerActions.Reload -= ManualReload;
        ChargerDealDamage.DecrementPlayerHealth -= DamageReceived;

        _grounded = _collisionState.OnSolidGround ? true : false;
    }
    
    protected virtual void Reload() {

        if (!_reloading && numOfRounds < _clipSize) {
            StartCoroutine(ReloadDelay());
        }
    }

    protected virtual void ManualReload() {
        if (!_reloading && numOfRounds < _clipSize && _collisionState.OnSolidGround) {
            _grounded = true;
            StartCoroutine(ReloadDelay());
        }
    }

    protected virtual IEnumerator ReloadDelay() {
        yield return 0;
    }

    protected virtual IEnumerator ShotDelay() {

        if (!_damaged) {
            _canShoot = false;
            Instantiate(_bullet, _mgBarrel.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(_shotDelay);
            _canShoot = true;
        }
    }

    // The player cannot attack for a brief amount of time after they have received damage.
    protected virtual void DamageReceived(int ignore) {
        StartCoroutine(DamageDelay());
    }

    protected virtual IEnumerator DamageDelay() {

        _damaged = true;
        _canShoot = false;
        yield return new WaitForSeconds(1.0f);
        _damaged = false;
        _canShoot = true;
    }

    protected virtual void OnButtonDown(Buttons button)
    {
    }
}

