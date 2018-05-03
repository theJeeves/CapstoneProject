﻿using UnityEngine;
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
    public static event AbstractGunEvent2 ShotFired;

    protected override void Awake() {
        base.Awake();

        _weaponManager.SetAmmoCapacity(_type, _ammoCapacity);

        numOfRounds = _weaponManager.GetNumOfRounds(_type);
        if (numOfRounds > 0 && DisplayAmmo != null) {
            DisplayAmmo();
        }
    }

    private void OnEnable() {
        ControllableObject.OnButtonDown += OnButtonDown;
        PlayerActions.ButtonPressed += OnPlayerActionButtonPress;
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
    }

    private void OnDisable() {
        ControllableObject.OnButtonDown -= OnButtonDown;
        PlayerActions.ButtonPressed -= OnPlayerActionButtonPress;
        PlayerCollisionState.OnHitGround -= Reload;

        _grounded = _collisionState.OnSolidGround ? true : false;

        StopAllCoroutines();
    }

    private void OnPlayerActionButtonPress(object sender, Buttons button) {
        if (button == Buttons.Shoot && numOfRounds > 0 && _canShoot) {

            StartCoroutine(ShotDelay());

            _moveRequest.RequestMovement();
            _SSRequest.ShakeRequest();

            if (UpdateNumOfRounds != null) {
                UpdateNumOfRounds(numOfRounds);
            }
        }
    }

    protected override void OnButtonDown(object sender, Buttons button) {
        if (button == Buttons.Reload) { ManualReload(); }
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

        // Prevent the gun from reloading until the player is back on the ground.
        while (!_collisionState.OnSolidGround) {
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
    }

    private IEnumerator ShotDelay() {

        _canShoot = false;

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

        _SOEffectHandler.PlayEffect(EffectEnums.ShotgunBlast, _barrelNorm.transform.position, _controller.AimDirection, _direction.x, _direction.y);

        if (ShotFired != null) { ShotFired(); }     // this is here to tell the save file another shot is fired and to record it.
        _grounded = false;

        // Make the ammo sprite disappear when the ammo is depleted while in the air.
        if (--numOfRounds <= 0) {
            if (EmptyClip != null) { EmptyClip(); }

            // Determine if the player was on the ground when they shot the last round in the chamber.
            _grounded = _collisionState.OnSolidGround ? true : false;
            yield return new WaitForSeconds(0.1f);
            Reload();
        }
        else {
            yield return new WaitForSeconds(m_shotDelay);
        }

        _canShoot = true;
    }
}
