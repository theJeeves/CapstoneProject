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
    protected float _recoil;

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
    protected System.Action[] _gunActions = new System.Action[8];

    protected float _addVel = 0.45f;
    protected float _setVel = 0.75f;

    protected float _xVel;
    protected float _yVel;

    private GameObject _player;
    protected ControllableObject _controller;
    protected Rigidbody2D _body2d;
    protected PlayerCollisionState _collisionState;

    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private Transform _mgBarrel;

    protected virtual void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _controller = _player.GetComponent<ControllableObject>();
        _body2d = _player.GetComponent<Rigidbody2D>();
        _collisionState = _player.GetComponent<PlayerCollisionState>();

        _gunActions[0] = AimRight;
        _gunActions[1] = AimUpAndRight;
        _gunActions[2] = AimUp;
        _gunActions[3] = AimUpAndLeft;
        _gunActions[4] = AimLeft;
        _gunActions[5] = AimDownAndLeft;
        _gunActions[6] = AimDown;
        _gunActions[7] = AimDownAndRight;

        numOfRounds = _clipSize;
    }

    protected virtual void OnEnable()
    {
        ControllableObject.OnButtonDown += OnButtonDown;
        PlayerCollisionState.OnHitGround += Reload;
        ReloadWeapon.Reload += ManualReload;
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
        ReloadWeapon.Reload -= ManualReload;
        ChargerDealDamage.DecrementPlayerHealth -= DamageReceived;

        _grounded = _collisionState.OnSolidGround ? true : false;
    }

    protected void SetVeloctiy(float xVel, float yVel)
    {
        _body2d.velocity = new Vector2(xVel, yVel);
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

    // The player cannot attach for a brief amount of time after they have received damage.
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


    /* Directional methods to be overriden by individual weapons
    *  and their unique properties of movement
    */
    protected virtual void AimDownAndRight() {
    }

    protected virtual void AimDownAndLeft() {
    }

    protected virtual void AimDown() {
    }

    protected virtual void AimUpAndRight() {
    }

    protected virtual void AimUpAndLeft() {
    }

    protected virtual void AimUp() {
    }

    protected virtual void AimRight() {
    }

    protected virtual void AimLeft() {
    }

    protected virtual void OnButtonDown(Buttons button)
    {
    }
}

