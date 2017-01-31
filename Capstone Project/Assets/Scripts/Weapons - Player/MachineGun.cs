
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
        ChargerDealDamage.DecrementPlayerHealth += DamageReceived;

        _audioSource = GetComponent<AudioSource>();

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
        ChargerDealDamage.DecrementPlayerHealth -= DamageReceived;

        _grounded = _collisionState.OnSolidGround ? true : false;

        StopAllCoroutines();
        ControllableObject.OnButton -= OnButton;
    }

    private void Update() {
        if (_muzzleFlashGO != null) {
            _muzzleFlashGO.transform.position = _barrel.transform.position;
        }
    }

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot && _canShoot && _collisionState.OnSolidGround && numOfRounds > 0) {

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

            _canLift = _collisionState.OnSolidGround ? true : false;

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
                    _grounded = _collisionState.OnSolidGround ? true : false;
                    Reload();
                }
                if (UpdateNumOfRounds != null) {
                    UpdateNumOfRounds(numOfRounds);
                }

                StartCoroutine(ShotDelay());
            }
        }
        
        // Only call for the ammo to hide if the player has no more bullets in the clip
        // and are in the air. Otherwise, it is handle in the if statement above.
        else if (numOfRounds <= 0 && !_collisionState.OnSolidGround) {
            if (EmptyClip != null) {
                EmptyClip();
            }
        }
    }

    private IEnumerator ShotDelay() {
        if (!_damaged) {
            _canShoot = false;

            _muzzleFlashGO = _SOEffect.PlayEffect(EffectEnum.MGMuzzelFlash, _barrel.transform.position, _controller.AimDirection);
            GameObject instance = Instantiate(_bullet, _barrel.transform.position, Quaternion.identity) as GameObject;

            // Angle the crystal according the the angle of the gun's direction
            instance.transform.localEulerAngles = new Vector3(0.0f, 0.0f, _controller.AimDirection);

            _SSRequest.ShakeRequest();
            _SOAudio.Play(_audioSource, AudioTypeEnum.MachineGunFire);

            yield return new WaitForSeconds(_shotDelay);
            _canShoot = true;
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
        if (!_reloading && numOfRounds < _ammoCapacity && _collisionState.OnSolidGround) {
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
            while (!_collisionState.OnSolidGround) {
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
