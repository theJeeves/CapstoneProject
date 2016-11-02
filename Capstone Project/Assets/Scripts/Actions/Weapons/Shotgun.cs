using UnityEngine;
using System.Collections;

/*
 * Weapon based on the AbstractGun class which gives the player a "jump" like ability.
 */

public class Shotgun : AbstractGun {

    // This is put into the shotgun script so the shotgun screen shake script
    // knows which weapon called this event.
    public static event AbstractGunEvent2 Fire;

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot && _numOfRounds > 0 && _canShoot) {

            if (Fire != null) {
                Fire();
            }

            base.OnButtonDown(button);
        }
    }

    protected override void AimDown() {

        //AIMING DOWN AND RIGHT
        if (_controller.AimDirection.Right) {
            AimDownAndRight();
        }

        //AIMING DOWN AND LEFT
        else if (_controller.AimDirection.Left) {
            AimDownAndLeft();
        }

        //AIMING STRAIGHT DOWN
        else {
            //FALLING (NEGATVIE Y VELOCITY
            if (_body2d.velocity.y < 0) {
                _xVel = _body2d.velocity.x;
                _yVel = _recoil;
            }

            //RISING OR ZERO Y VELOCITY
            else if (_body2d.velocity.y >= 0) {
                _xVel = _body2d.velocity.x;
                _yVel = _recoil;
            }
        }
    }

    protected override void AimDownAndRight() {

        //ON THE GROUND
        if (_collisionState.OnSolidGround) {

            // MOVING LEFT
            if (_body2d.velocity.x < 0) {
                _xVel = _recoil * -_addVel + _body2d.velocity.x;
                _yVel = _recoil * _setVel;
            }

            //MOVING RIGHT OR STANDING STILL
            else if (_body2d.velocity.x >= 0) {
                _xVel = _recoil * -_setVel;
                _yVel = _recoil * _setVel;
            }
        }

        //IN THE AIR
        else if (!_collisionState.OnSolidGround) {

            //MOVING LEFT
            if (_body2d.velocity.x < 0) {

                //FALLING (NEGATIVE Y VELOCITY)
                if (_body2d.velocity.y < 0) {
                    _xVel = _body2d.velocity.x + _recoil * -(_addVel);
                    _yVel = _recoil * _setVel;
                }

                //RISING OR ZERO Y VELOCITY
                else if (_body2d.velocity.y >= 0) {
                    _xVel = _body2d.velocity.x + _recoil * -_addVel;
                    _xVel = -_recoil;
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

            //MOVING RIGHT
            if (_body2d.velocity.x > 0) {
                _xVel = _recoil * _addVel + _body2d.velocity.x;
                _yVel = _recoil * _setVel;
            }

            // MOVING LEFT OR STANDING STILL
            else if (_body2d.velocity.x <= 0) {
                _xVel = _recoil * _setVel;
                _yVel = _recoil * _setVel;
            }
        }

        //IN THE AIR
        else if (!_collisionState.OnSolidGround) {

            //MOVING RIGHT
            if (_body2d.velocity.x > 0) {

                //FALLING (NEGATIVE Y VELOCITY)
                if (_body2d.velocity.y < 0) {
                    _xVel = _body2d.velocity.x + _recoil * _addVel;
                    _yVel = _recoil * _setVel;
                }

                //RISING OR ZERO Y VELOCITY
                else if (_body2d.velocity.y >= 0) {
                    _xVel = _recoil * _setVel + _body2d.velocity.x;
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

        //AIMING UP AND RIGHT IN AIR
        if (_controller.AimDirection.Right) {
            AimUpAndRight();
        }
        //AIMING UP AND LEFT IN AIR
        else if (_controller.AimDirection.Left) {
            AimUpAndLeft();
        }
        //AIMMING STRAIGHT UP
        else {
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
    }

    protected override void AimUpAndRight() {

        //MOVING LEFT
        if (_body2d.velocity.x < 0) {
            //FALLING
            if (_body2d.velocity.y < 0) {
                _xVel = _body2d.velocity.x + _recoil * -(_addVel);
                _yVel = _body2d.velocity.y + _recoil * -(_addVel);
            }
            //RISING
            else if (_body2d.velocity.y >= 0) {
                _xVel = _body2d.velocity.x + _recoil * -(_addVel);
                _yVel = _recoil * -(_addVel);
            }
        }

        //MOVING RIGHT NO X VELOCITY
        else if (_body2d.velocity.x >= 0) {
            //FALLING
            if (_body2d.velocity.y < 0) {
                _xVel = _recoil * -(_setVel);
                _yVel = _body2d.velocity.y + _recoil * -(_addVel);
            }
            //RISING
            else if (_body2d.velocity.y >= 0) {
                _xVel = _recoil * -(_setVel);
                _yVel = _recoil * -(_setVel);
            }
        }
    }

    protected override void AimUpAndLeft() {

        //MOVING LEFT
        if (_body2d.velocity.x < 0) {
            //FALLING
            if (_body2d.velocity.y < 0) {
                _xVel = _recoil * (_setVel);
                _yVel = _body2d.velocity.y + _recoil * -(_addVel);
            }
            //RISING
            else if (_body2d.velocity.y >= 0) {
                _xVel = _recoil * (_setVel);
                _yVel = _recoil * -(_addVel);
            }
        }

        //MOVING RIGHT
        else if (_body2d.velocity.x >= 0) {
            //FALLING
            if (_body2d.velocity.y < 0) {
                _xVel = _body2d.velocity.x + _recoil * _addVel;
                _yVel = _body2d.velocity.y + _recoil * -(_addVel);
            }
            //RISING
            if (_body2d.velocity.y >= 0) {
                _xVel = _body2d.velocity.x + _recoil * _addVel;
                _yVel = _recoil * -(_addVel);
            }
        }
    }

    protected override void AimRight() {
        //IN AIR CONTROLLS ONLY
        if (!_collisionState.OnSolidGround) {

            // MOVING LEFT
            if (_body2d.velocity.x < 0) {
                _xVel = -_recoil;
                _yVel = _body2d.velocity.y;
            }

            // MOVING RIGHT OR STANDING STILL
            else if (_body2d.velocity.x >= 0) {
                _xVel = -_recoil;
                _yVel = _body2d.velocity.y;
            }
        }
    }

    protected override void AimLeft() {

        //IN AIR CONTROLLS ONLY
        if (!_collisionState.OnSolidGround) {

            //MOVING RIGHT
            if (_body2d.velocity.x > 0) {
                _xVel = _recoil;
                _yVel = _body2d.velocity.y;
            }

            //MOVING LEFT OR STANDING STILL
            else if (_body2d.velocity.x <= 0) {
                _xVel = _recoil;
                _yVel = _body2d.velocity.y;
            }
        }
    }
}
