using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Weapon based on the AbstractGun class which gives the player a "jump" like ability.
 */

public class Shotgun : AbstractGun {

    // This is put into the shotgun script so the shotgun screen shake script
    // knows which weapon called this event.
    //public static event AbstractGunEvent2 Fire;
    public static event AbstractGunEvent2 EmptyClip;
    public static event AbstractGunEvent3 StartReloadAnimation;
    public static event AbstractGunEvent UpdateNumOfRounds;
    public static event AbstractGunEvent2 DisplayAmmo;

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
    }

    private void OnDisable() {
        ControllableObject.OnButtonDown -= OnButtonDown;
        PlayerCollisionState.OnHitGround -= Reload;
        ChargerDealDamage.DecrementPlayerHealth -= DamageReceived;

        _grounded = _collisionState.OnSolidGround ? true : false;

        StopAllCoroutines();
    }

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot && numOfRounds > 0 && _canShoot) {

            StartCoroutine(ShotDelay());

            _moveRequest.RequestMovement();
            _SSRequest.ShakeRequest();
            //_SOEffect.PlayEffect(EffectEnum.SGMuzzleFlash, _barrel.transform.position, _controller.AimDirection - 90.0f);

            _grounded = false;

            // Make the ammo sprite disappear when the ammo is depleted while in the air.
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
        }

        if (button == Buttons.Reload) {
            ManualReload();
        }
    }

    private void Reload() {
        if (!_reloading && numOfRounds < _ammoCapacity) {
            StartCoroutine(ReloadDelay());
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

    private IEnumerator ShotDelay() {

        if (!_damaged) {
            _canShoot = false;
            Instantiate(_bullet, _barrel.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(_shotDelay);
            _canShoot = true;
        }
    }
}
