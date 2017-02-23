
using UnityEngine;
using System.Collections;

/*
 * Weapon based on the AbstractGun class which gives the player a "float" like ability.
 */

public class MachineGun : AbstractGun {

    public static event AbstractGunEvent2 EmptyClip;
    public static event AbstractGunEvent3 StartReloadAnimation;
    public static event AbstractGunEvent UpdateNumOfRounds;
    public static event AbstractGunEvent2 DisplayAmmo;

    [SerializeField]
    private MovementRequest _initialMoveRequest;

    private GameObject _muzzleFlashGO;
    private bool _canLift = true;
    private float _timer = 0.0f;
    private Vector2 _direction = Vector2.zero;

    protected override void Awake() {
        base.Awake();

        _weaponManager.SetAmmoCapacity(_type, _ammoCapacity);

        numOfRounds = _weaponManager.GetNumOfRounds(_type);
        if (numOfRounds > 0) {
            DisplayAmmo();
        }
    }

    private void OnEnable() {
        ControllableObject.OnButtonDown += OnButtonDown;
        PlayerCollisionState.OnHitGround += Reload;

        GetComponent<AudioSource>().clip = _audioClip;
        GetComponent<AudioSource>().Play();

        _reloading = false;
        _canShoot = true;

        if (_weaponManager.reloaded) {
            numOfRounds = _ammoCapacity;
            DisplayAmmo();
            _weaponManager.reloaded = false;
        }
        else if (numOfRounds <= 0) {
            Reload();
        }

        if (numOfRounds == _ammoCapacity) {
            _canShoot = true;
        }

        if (UpdateNumOfRounds != null) {
            UpdateNumOfRounds(numOfRounds);
        }

        ControllableObject.OnButton += OnButton;

        _canLift = _collisionState.OnSolidGround ? true : false;
    }

    private void OnDisable() {
        ControllableObject.OnButtonDown -= OnButtonDown;
        PlayerCollisionState.OnHitGround -= Reload;

        _grounded = _collisionState.OnSolidGround ? true : false;

        StopAllCoroutines();
        ControllableObject.OnButton -= OnButton;
    }

    private void Update() {
        if (_muzzleFlashGO != null) {
            _muzzleFlashGO.transform.position = _barrel.transform.position;
        }
        if (!_canShoot && Time.time - _timer >= _shotDelay) {
            _canShoot = true;
        }

        _grounded = _collisionState.OnSolidGround;
    }

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot && _canShoot && _grounded && numOfRounds > 0) {
            _initialMoveRequest.RequestMovement();
            _canLift = false;
        }

        if (button == Buttons.Reload) {
            ManualReload();
        }
    }

    private void OnButton(Buttons button) {

        if (button == Buttons.Shoot && numOfRounds > 0) {
            if (_canLift && _controller.GetButtonPress(Buttons.AimDown)) {
                OnButtonDown(button);
            }

            //_canLift = _grounded ? true : false;

            if (!_reloading) {

                _moveRequest.RequestMovement();
                _grounded = false;
            }
            if (_canShoot & !_reloading) {

                // If the last round has gone out, send out the event to hide the ammo type display.
                if (--numOfRounds <= 0) {
                    if (EmptyClip != null) {
                        EmptyClip();
                    }

                    // Determine if the player was on the ground when they shot the last round in the chamber.
                    //_grounded = _collisionState.OnSolidGround ? true : false;
                    Reload();
                }
                if (UpdateNumOfRounds != null) {
                    UpdateNumOfRounds(numOfRounds);
                }

                Fire();
            }
        }

        // Only call for the ammo to hide if the player has no more bullets in the clip
        // and are in the air. Otherwise, it is handle in the if statement above.
        else if (numOfRounds <= 0 && !_grounded) {
            if (EmptyClip != null) {
                EmptyClip();
            }
        }
    }

    private void Fire() {
        // Set can shoot to false for a delay between shots, ask for the muzzle flash, ask for the crystal bullet, and request a screen shake.
        // Lastly, start the timer for the shot delay
        if (_canShoot) {
            _canShoot = false;
            _muzzleFlashGO = _SOEffectHandler.PlayEffect(EffectEnums.MGMuzzleFlash, _barrel.transform.position, FiringAngle());
            _SOEffectHandler.PlayEffect(EffectEnums.CrystalBullet, _barrel.transform.position, FiringAngle());
            _SSRequest.ShakeRequest();
            _timer = Time.time;
        }
    }

    private int FiringAngle() {

        _direction = (GameObject.FindGameObjectWithTag("End").transform.position - _barrel.position).normalized;
        _direction = new Vector2(Mathf.Round(_direction.x), Mathf.Round(_direction.y));

        if (_direction.x == 1.0f && _direction.y == 0.0f) {
            return 0;
        }
        else if (_direction.x == 1.0f && _direction.y == 1.0f) {
            return 45;
        }
        else if (_direction.x == 0.0f && _direction.y == 1.0f) {
            return 90;
        }
        else if (_direction.x == -1.0f && _direction.y == 1.0f) {
            return 135;
        }
        else if (_direction.x == -1.0f && _direction.y == 0.0f) {
            return 180;
        }
        else if (_direction.x == -1.0f && _direction.y == -1.0f) {
            return 225;
        }
        else if (_direction.x == 0.0f && _direction.y == -1.0f) {
            return 270;
        }
        else if (_direction.x == 1.0f && _direction.y == -1.0f) {
            return 315;
        }
        else {
            return -1;
        }
    }

    private void Reload() {

        // This ensures the player will be lifted by the initial shot every time.
        // Machine Gun specific.
        _canLift = true;

        if (numOfRounds > 0 && _controller.GetButtonPress(Buttons.Shoot)) { }
        else {
            if (!_reloading && numOfRounds < _ammoCapacity) {
                StartCoroutine(ReloadDelay());
            }
        }
    }

    private void ManualReload() {
        if (!_reloading && numOfRounds < _ammoCapacity && _grounded) {
            _grounded = true;
            StartCoroutine(ReloadDelay());
        }
    }

    private IEnumerator ReloadDelay() {

        // Prevent the gun from trying to reload multiple times and
        // prevent the player from firing while reloading.
        _reloading = true;
        _canShoot = false;

        // The player will have the same reload time if they stay on the ground or if they
        // shot themselves up into the air. The reload times balance out.
        if (_grounded) {
            _reloadTime = _normReloadTime;
        }
        else {
            _reloadTime = _fastReloadTime;

            // Prevent the gun from reloading until the player is back on the ground.
            while (!_grounded) {
                yield return 0;
            }
        }



        if (StartReloadAnimation != null) {
            StartReloadAnimation(_reloadTime);
        }

        yield return new WaitForSeconds(_reloadTime);

        _weaponManager.Reload();
        numOfRounds = _weaponManager.GetNumOfRounds(_type);

        // UPDATE THE UI
        if (UpdateNumOfRounds != null) {
            UpdateNumOfRounds(numOfRounds);
        }

        // The player can now fire again.
        _reloading = false;
        _canShoot = true;
    }
}
