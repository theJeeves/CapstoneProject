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
    protected int _reloadTime;

    protected bool _canShoot;
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

        if (numOfRounds <= 0) {
            Reload();
        }
        else {
            _canShoot = true;
        }
    }

    protected virtual void OnDisable()
    {
        ControllableObject.OnButtonDown -= OnButtonDown;
        PlayerCollisionState.OnHitGround -= Reload;
    }

    protected void SetVeloctiy(float xVel, float yVel)
    {
        _body2d.velocity = new Vector2(xVel, yVel);
    }

    protected virtual void Reload() {
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

    protected virtual IEnumerator ShotDelay() {

        _canShoot = false;
        Instantiate(_bullet, _mgBarrel.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_shotDelay);
        _canShoot = true;
    }
}

