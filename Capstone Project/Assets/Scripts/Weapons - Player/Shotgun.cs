using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Events;

/*
 * Weapon based on the AbstractGun class which gives the player a "jump" like ability.
 */

public class Shotgun : AbstractGun {

    #region Initializers
    protected override void Awake() {
        base.Awake();

        _weaponManager.SetAmmoCapacity(_type, _ammoCapacity);

        numOfRounds = _weaponManager.GetNumOfRounds(_type);
        if (numOfRounds > 0) {
            DisplayAmmo?.Invoke();
        }
    }

    private void OnEnable() {
        ControllableObject.ButtonDown += OnButtonDown;
        PlayerActions.ButtonPressed += OnPlayerActionButtonPress;
        PlayerCollisionState.HitGround += Reload;

        GetComponent<AudioSource>().clip = _audioClip;
        GetComponent<AudioSource>().Play();

        _reloading = false;
        _canShoot = true;

        if (_weaponManager.reloaded) {
            numOfRounds = _ammoCapacity;
            DisplayAmmo?.Invoke();
            _weaponManager.reloaded = false;
        }
        else if (numOfRounds <= 0) {
            Reload();
        }

        if (numOfRounds == _ammoCapacity) {
            _canShoot = true;
        }

        UpdateNumOfRounds?.Invoke(numOfRounds);
    }

    #endregion Initializers

    #region Finalizers
    private void OnDisable() {
        ControllableObject.ButtonDown -= OnButtonDown;
        PlayerActions.ButtonPressed -= OnPlayerActionButtonPress;
        PlayerCollisionState.HitGround -= Reload;

        _grounded = _collisionState.OnSolidGround ? true : false;

        StopAllCoroutines();
    }

    #endregion Finalizers

    #region Events
    public static event UnityAction EmptyClip;
    public static event UnityAction<float> StartReloadAnimation;
    public static event UnityAction<int> UpdateNumOfRounds;
    public static event UnityAction DisplayAmmo;
    public static event UnityAction ShotFired;

    #endregion Events

    #region Protected Methods
    protected override void OnButtonDown(Buttons button)
    {
        if (button == Buttons.Reload)
        {
            ManualReload();
        }
    }

    #endregion Protected Methods

    #region Private Methods
    private void OnPlayerActionButtonPress(Buttons button) {
        if (button == Buttons.Shoot && numOfRounds > 0 && _canShoot) {

            StartCoroutine(ShotDelay());

            _moveRequest.RequestMovement();
            _SSRequest.ShakeRequest();

            UpdateNumOfRounds?.Invoke(numOfRounds);
        }
    }

    private void Reload(EventArgs args)
    {
        Reload();
    }

    private void Reload()
    {
        if (!_reloading && numOfRounds < _ammoCapacity)
        {
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

        StartReloadAnimation?.Invoke(_reloadTime);

        yield return new WaitForSeconds(_reloadTime);

        _weaponManager.Reload();
        numOfRounds = _weaponManager.GetNumOfRounds(_type);

        // UPDATE THE UI
        UpdateNumOfRounds?.Invoke(numOfRounds);

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

        // this is here to tell the save file another shot is fired and to record it.
        ShotFired?.Invoke();
        _grounded = false;

        // Make the ammo sprite disappear when the ammo is depleted while in the air.
        if (--numOfRounds <= 0) {
            EmptyClip?.Invoke();

            // Determine if the player was on the ground when they shot the last round in the chamber.
            _grounded = _collisionState.OnSolidGround ? true : false;
            yield return new WaitForSeconds(0.1f);
            Reload();
        }
        else
        {
            yield return new WaitForSeconds(m_defaultShotDelay);
        }

        _canShoot = true;
    }

    #endregion Private Methods
}
