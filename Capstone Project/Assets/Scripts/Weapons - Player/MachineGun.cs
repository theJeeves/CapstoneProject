
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
    public static event AbstractGunEvent2 ShotFired;

    [SerializeField]
    private MovementRequest _initialMoveRequest;

    private GameObject _muzzleFlashGO;
    private bool _canLift = true;
    private float _timer = 0.0f;

    protected override void Awake() {
        base.Awake();

        _weaponManager.SetAmmoCapacity(_type, _ammoCapacity);

        numOfRounds = _weaponManager.GetNumOfRounds(_type);
        if (numOfRounds > 0 && DisplayAmmo != null) {
            DisplayAmmo();
        }
    }

    private void OnEnable() {
        PlayerCollisionState.OnHitGround += Reload;
        ControllableObject.OnButtonDown += ManualReload;

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

        PlayerActions.ButtonHeld += OnPlayerActionButtonPress;

        _canLift = _collisionState.OnSolidGround ? true : false;
    }

    private void OnDisable() {
        PlayerCollisionState.OnHitGround -= Reload;
        ControllableObject.OnButtonDown -= ManualReload;

        _grounded = _collisionState.OnSolidGround ? true : false;

        StopAllCoroutines();
        PlayerActions.ButtonHeld -= OnPlayerActionButtonPress;
    }

    private void Update() {
        if (_muzzleFlashGO != null) {
            _muzzleFlashGO.transform.position = _barrelNorm.transform.position;
        }
        if (!_canShoot && Time.time - _timer >= _shotDelay) {
            _canShoot = true;
        }

        _grounded = _collisionState.OnSolidGround;
    }

    private void Lift() {

        if (_canLift && _controller.GetButtonPress(Buttons.AimDown) && _direction.y == -1.0f) {
            _initialMoveRequest.RequestMovement();
            _canLift = false;
        }
    }

    private void OnPlayerActionButtonPress(Buttons button) {

        if (button == Buttons.Shoot && numOfRounds > 0) {

            if (!_reloading) {

                _moveRequest.RequestMovement();

                if (_canShoot) {
                    Fire();
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
                }
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

            int firingAngle = FiringAngle();

            _muzzleFlashGO = _SOEffectHandler.PlayEffect(EffectEnums.MGMuzzleFlash, _barrelNorm.transform.position, firingAngle);

            if (firingAngle == 270) {
                _SOEffectHandler.PlayEffect(EffectEnums.CrystalBullet, _barrelAlt.transform.position, firingAngle, _direction.x, _direction.y);
            }
            else {
                _SOEffectHandler.PlayEffect(EffectEnums.CrystalBullet, _barrelNorm.transform.position, firingAngle, _direction.x, _direction.y);
            }
            Lift();
            _SSRequest.ShakeRequest();
            _timer = Time.time;

            if (ShotFired != null) { ShotFired(); }     // this is here to tell the save file another shot is fired and to record it.
        }
    }

    private int FiringAngle() {

        if (_controller.GetButtonPress(Buttons.AimDown)) {

            if (_controller.GetButtonPress(Buttons.AimRight) || _controller.GetButtonPress(Buttons.AimLeft)) {
                _direction = (_endNorm.position - _barrelNorm.position).normalized;
            }
            else {
                _direction = (_endAlt.position - _barrelAlt.position).normalized;
            }
        }
        else {
            _direction = (_endNorm.position - _barrelNorm.position).normalized;
        }
        _direction = new Vector2(Mathf.Round(_direction.x), Mathf.Round(_direction.y));

        if (_direction.x == 1.0f && _direction.y == 0.0f)           return 0;
        else if (_direction.x == 1.0f && _direction.y == 1.0f)      return 45;
        else if (_direction.x == 0.0f && _direction.y == 1.0f)      return 90;
        else if (_direction.x == -1.0f && _direction.y == 1.0f)     return 135;
        else if (_direction.x == -1.0f && _direction.y == 0.0f)     return 180;
        else if (_direction.x == -1.0f && _direction.y == -1.0f)    return 225;
        else if (_direction.x == 0.0f && _direction.y == -1.0f)     return 270;
        else if (_direction.x == 1.0f && _direction.y == -1.0f)     return 315;
        else return -1;
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

    private void ManualReload(Buttons button) {

        if (button == Buttons.Reload && !_reloading && numOfRounds < _ammoCapacity && _grounded) {
            _grounded = true;
            StartCoroutine(ReloadDelay());
        }
    }

    private IEnumerator ReloadDelay() {

        // Prevent the gun from trying to reload multiple times and
        // prevent the player from firing while reloading.
        _reloading = true;
        _canShoot = false;

        // Prevent the gun from reloading until the player is back on the ground.
        while (!_grounded) {
            yield return 0;
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
        // This ensures the player will be lifted by the initial shot every time.
        // Machine Gun specific.
        _canLift = true;
    }
}
