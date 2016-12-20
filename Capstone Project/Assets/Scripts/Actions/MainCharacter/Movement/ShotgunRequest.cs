using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName ="Movement Request/Shotgun")]
public class ShotgunRequest : MovementRequest {

    private float _xVel;
    private float _yVel;

    [SerializeField]
    private float _recoil;
    [SerializeField]
    private float _setVel;
    [SerializeField]
    private float _addVel;

    private bool _grounded;

    protected System.Action<float, float>[] _gunActions = new System.Action<float, float>[8];

    protected override void OnEnable() {
        base.OnEnable();

        _gunActions[0] = AimRight;
        _gunActions[1] = AimUpAndRight;
        _gunActions[2] = AimUp;
        _gunActions[3] = AimUpAndLeft;
        _gunActions[4] = AimLeft;
        _gunActions[5] = AimDownAndLeft;
        _gunActions[6] = AimDown;
        _gunActions[7] = AimDownAndRight;
    }

    public override Vector2 Move(Vector3 values, bool grounded = false, byte key = 0) {

        _grounded = grounded;
        _gunActions[key].Invoke(values.x, values.y);

        return new Vector2(_xVel, _yVel);
    }

    public override void RequestMovement() {
        _player.SendMessage("Enqueue", this);
    }

    private void AimRight(float bodyXvel, float bodyYvel) {

        _xVel = !_grounded ? -_recoil : -_recoil * 0.45f;

        // MOVING LEFT OR MOVING RIGHT OR STANDING STILL
        _yVel = bodyYvel;
    }

    private void AimUpAndRight(float bodyXvel, float bodyYvel) {

        //FALLING
        if (bodyYvel < 0) {
            _yVel = bodyYvel + _recoil * -(_addVel);
        }

        //MOVING LEFT
        if (bodyXvel < 0) {

            _xVel = bodyXvel + _recoil * -(_addVel);

            //RISING
            if (bodyYvel >= 0) {
                _yVel = _recoil * -(_addVel);
            }
        }

        //MOVING RIGHT OR NO X VELOCITY
        else if (bodyXvel >= 0) {

            _xVel = _recoil * -(_setVel);

            //RISING
            if (bodyYvel >= 0) {
                _yVel = _recoil * -(_setVel);
            }
        }
    }

    private void AimUp(float bodyXvel, float bodyYvel) {

        _xVel = bodyXvel;

        //FALLING
        if (bodyYvel < 0) {
            _yVel = bodyYvel + _recoil * -(_setVel);
        }

        //RISING OR STILL
        else if (bodyYvel >= 0) {
            _yVel = -(_recoil);
        }
    }

    private void AimUpAndLeft(float bodyXvel, float bodyYvel) {

        //FALLING
        if (bodyYvel < 0) {
            _yVel = bodyYvel + _recoil * -(_addVel);
        }

        //MOVING LEFT
        if (bodyXvel <= 0) {

            _xVel = _recoil * (_setVel);

            //RISING
            if (bodyYvel >= 0) {
                _yVel = _recoil * -(_setVel);
            }
        }

        //MOVING RIGHT
        else if (bodyXvel > 0) {

            _xVel = bodyXvel + _recoil * _addVel;

            //RISING
            if (bodyYvel >= 0) {
                _yVel = _recoil * -(_addVel);
            }
        }
    }

    private void AimLeft(float bodyXvel, float bodyYvel) {
        _xVel = !_grounded ? _recoil : _recoil * 0.45f;

        //MOVING RIGHT OR MOVING LEFT OR STANDING STILL
        _yVel = bodyYvel;
    }

    private void AimDownAndLeft(float bodyXvel, float bodyYvel) {
        //ON THE GROUND
        if (_grounded) {

            _yVel = _recoil * _setVel;

            //MOVING RIGHT
            if (bodyXvel > 0) {
                _xVel = Mathf.Clamp(bodyXvel + _recoil * _addVel, _recoil * _addVel, _recoil);
            }

            // MOVING LEFT OR STANDING STILL
            else if (bodyXvel <= 0) {
                _xVel = _recoil * _setVel;
            }
        }

        //IN THE AIR
        else if (!_grounded) {

            //MOVING RIGHT
            if (bodyXvel > 0) {

                _xVel = Mathf.Clamp(bodyXvel + _recoil * _addVel, _recoil * _addVel, _recoil);

                //FALLING (NEGATIVE Y VELOCITY)
                if (bodyYvel < 0) {
                    _yVel = _recoil * _setVel;
                }

                //RISING OR ZERO Y VELOCITY
                else if (bodyYvel >= 0) {
                    _yVel = _recoil * _setVel;
                }
            }

            // MOVING LEFT OR STANDING STILL
            else if (bodyXvel <= 0) {
                _xVel = _recoil * _setVel;
                _yVel = _recoil * _setVel;
            }
        }
    }

    private void AimDown(float bodyXvel, float bodyYvel) {

        _yVel = _recoil;

        //FALLING (NEGATVIE Y VELOCITY) OR RISING OR ZERO Y VELOCITY
        _xVel = bodyXvel;
    }

    private void AimDownAndRight(float bodyXvel, float bodyYvel) {
        //ON THE GROUND
        if (_grounded) {

            _yVel = _recoil * _setVel;

            // MOVING LEFT
            if (bodyXvel < 0) {
                _xVel = -1.0f * Mathf.Clamp(Mathf.Abs(bodyXvel) + _recoil * _addVel, _recoil * _addVel, _recoil);
            }

            //MOVING RIGHT OR STANDING STILL
            else if (bodyXvel >= 0) {
                _xVel = _recoil * -_setVel;
            }
        }

        //IN THE AIR
        else if (!_grounded) {

            //MOVING LEFT
            if (bodyXvel < 0) {

                _xVel = -1.0f * Mathf.Clamp(Mathf.Abs(bodyXvel) + _recoil * _addVel, _recoil * _addVel, _recoil);

                //FALLING (NEGATIVE Y VELOCITY)
                if (bodyYvel < 0) {
                    _yVel = _recoil * _setVel;
                }

                //RISING OR ZERO Y VELOCITY
                else if (bodyYvel >= 0) {
                    _yVel = Mathf.Clamp(bodyYvel, _recoil * _addVel, _recoil * 2);
                }
            }

            //MOVING RIGHT OR STANDING STILL
            else if (bodyXvel >= 0) {
                _xVel = _recoil * -_setVel;
                _yVel = _recoil * _setVel;
            }
        }
    }
}
