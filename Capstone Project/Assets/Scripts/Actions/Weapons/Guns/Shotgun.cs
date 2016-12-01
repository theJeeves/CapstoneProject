using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Weapon based on the AbstractGun class which gives the player a "jump" like ability.
 */

public class Shotgun : AbstractGun {

    // This is put into the shotgun script so the shotgun screen shake script
    // knows which weapon called this event.
    public static event AbstractGunEvent2 Fire;
    public static event AbstractGunEvent2 EmptyClip;
    public static event AbstractGunEvent3 StartReloadAnimation;
    public static event AbstractGunEvent UpdateNumOfRounds;

    protected override void OnEnable() {
        base.OnEnable();

        if (UpdateNumOfRounds != null) {
            UpdateNumOfRounds(numOfRounds);
        }
    }

    protected override void OnButtonDown(Buttons button) { 

        if (button == Buttons.Shoot && numOfRounds > 0 && _canShoot) {

            StartCoroutine(ShotDelay());

            _xVel = _body2d.velocity.x;
            _yVel = _body2d.velocity.y;

            _gunActions[_controller.CurrentKey].Invoke();
            SetVeloctiy(_xVel, _yVel);

            // Call the screen shake class.
            if (Fire != null) {
                Fire();
            }
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
    }
    
    protected override IEnumerator ReloadDelay() {

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

        numOfRounds = _clipSize;

        // UPDATE THE UI
        if (UpdateNumOfRounds != null) {
            UpdateNumOfRounds(numOfRounds);
        }

        // The player can now fire again.
        _reloading = false;
        _canShoot = true;
    }

    protected override void AimDown() {

        _yVel = _recoil;

        //FALLING (NEGATVIE Y VELOCITY
        if (_body2d.velocity.y < 0) {
            _xVel = _body2d.velocity.x;
        }

        //RISING OR ZERO Y VELOCITY
        else if (_body2d.velocity.y >= 0) {
            _xVel = _body2d.velocity.x;
        }

        //SetVeloctiy(_xVel, _yVel);
    }

    protected override void AimDownAndRight() {

        //ON THE GROUND
        if (_collisionState.OnSolidGround) {

            _yVel = _recoil * _setVel;

            // MOVING LEFT
            if (_body2d.velocity.x < 0) {
                _xVel = -1.0f * Mathf.Clamp(Mathf.Abs(_body2d.velocity.x) + _recoil * _addVel, _recoil * _addVel, _recoil);
            }

            //MOVING RIGHT OR STANDING STILL
            else if (_body2d.velocity.x >= 0) {
                _xVel = _recoil * -_setVel;
            }
        }

        //IN THE AIR
        else if (!_collisionState.OnSolidGround) {

            //MOVING LEFT
            if (_body2d.velocity.x < 0) {

                _xVel = -1.0f * Mathf.Clamp(Mathf.Abs(_body2d.velocity.x) + _recoil * _addVel, _recoil * _addVel, _recoil);

                //FALLING (NEGATIVE Y VELOCITY)
                if (_body2d.velocity.y < 0) {
                    _yVel = _recoil * _setVel;
                }

                //RISING OR ZERO Y VELOCITY
                else if (_body2d.velocity.y >= 0) {
                    _yVel = Mathf.Clamp(_body2d.velocity.y, _recoil * _addVel, _recoil * 2);
                }
            }

            //MOVING RIGHT OR STANDING STILL
            else if (_body2d.velocity.x >= 0) {
                _xVel = _recoil * -_setVel;
                _yVel = _recoil * _setVel;
            }
        }
    }

    protected override void AimDownAndLeft() {

        //ON THE GROUND
        if (_collisionState.OnSolidGround) {

            _yVel = _recoil * _setVel;

            //MOVING RIGHT
            if (_body2d.velocity.x > 0) {
                _xVel = Mathf.Clamp(_body2d.velocity.x + _recoil * _addVel, _recoil * _addVel, _recoil);
            }

            // MOVING LEFT OR STANDING STILL
            else if (_body2d.velocity.x <= 0) {
                _xVel = _recoil * _setVel;
            }
        }

        //IN THE AIR
        else if (!_collisionState.OnSolidGround) {

            //MOVING RIGHT
            if (_body2d.velocity.x > 0) {

                _xVel = Mathf.Clamp(_body2d.velocity.x + _recoil * _addVel, _recoil * _addVel, _recoil);

                //FALLING (NEGATIVE Y VELOCITY)
                if (_body2d.velocity.y < 0) {
                    _yVel = _recoil * _setVel;
                }

                //RISING OR ZERO Y VELOCITY
                else if (_body2d.velocity.y >= 0) {
                    _yVel = _recoil * _setVel;
                }
            }

            // MOVING LEFT OR STANDING STILL
            else if (_body2d.velocity.x <= 0) {
                _xVel = _recoil * _setVel;
                _yVel = _recoil * _setVel;
            }
        }
    }

    protected override void AimUp() {

        _xVel = _body2d.velocity.x;

        //FALLING
        if (_body2d.velocity.y < 0) {
            _yVel = _body2d.velocity.y + _recoil * -(_setVel);
        }

        //RISING OR STILL
        else if (_body2d.velocity.y >= 0) {
            _yVel = -(_recoil);
        }
    }

    protected override void AimUpAndRight() {

        //MOVING LEFT
        if (_body2d.velocity.x < 0) {

            _xVel = _body2d.velocity.x + _recoil * -(_addVel);

            //FALLING
            if (_body2d.velocity.y < 0) {
                _yVel = _body2d.velocity.y + _recoil * -(_addVel);
            }
            //RISING
            else if (_body2d.velocity.y >= 0) {
                _yVel = _recoil * -(_addVel);
            }
        }

        //MOVING RIGHT NO X VELOCITY
        else if (_body2d.velocity.x >= 0) {

            _xVel = _recoil * -(_setVel);

            //FALLING
            if (_body2d.velocity.y < 0) {
                _yVel = _body2d.velocity.y + _recoil * -(_addVel);
            }
            //RISING
            else if (_body2d.velocity.y >= 0) {
                _yVel = _recoil * -(_setVel);
            }
        }
    }

    protected override void AimUpAndLeft() {

        //MOVING LEFT
        if (_body2d.velocity.x < 0) {

            _xVel = _recoil * (_setVel);

            //FALLING
            if (_body2d.velocity.y < 0) {
                _yVel = _body2d.velocity.y + _recoil * -(_addVel);
            }
            //RISING
            else if (_body2d.velocity.y >= 0) {
                _yVel = _recoil * -(_addVel);
            }
        }

        //MOVING RIGHT
        else if (_body2d.velocity.x >= 0) {

            _xVel = _body2d.velocity.x + _recoil * _addVel;

            //FALLING
            if (_body2d.velocity.y < 0) {
                _yVel = _body2d.velocity.y + _recoil * -(_addVel);
            }
            //RISING
            if (_body2d.velocity.y >= 0) {
                _yVel = _recoil * -(_addVel);
            }
        }
    }

    protected override void AimRight() {
        //IN AIR CONTROLLS ONLY
        if (!_collisionState.OnSolidGround) {

            _xVel = -_recoil;

            // MOVING LEFT
            if (_body2d.velocity.x < 0) {
                _yVel = _body2d.velocity.y;
            }

            // MOVING RIGHT OR STANDING STILL
            else if (_body2d.velocity.x >= 0) {
                _yVel = _body2d.velocity.y;
            }
        }
    }

    protected override void AimLeft() {

        //IN AIR CONTROLLS ONLY
        if (!_collisionState.OnSolidGround) {

            _xVel = _recoil;

            //MOVING RIGHT
            if (_body2d.velocity.x > 0) {
                _yVel = _body2d.velocity.y;
            }

            //MOVING LEFT OR STANDING STILL
            else if (_body2d.velocity.x <= 0) {
                _yVel = _body2d.velocity.y;
            }
        }
    }
}
