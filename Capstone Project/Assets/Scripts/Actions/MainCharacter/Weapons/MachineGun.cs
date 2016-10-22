
using UnityEngine;
using System.Collections;

public class MachineGun : AbstractGun {

    [SerializeField]
    protected float _xMultiplier;

    protected override void OnEnable() {
        base.OnEnable();
        ControllableObject.OnButton += OnButton;
    }

    protected override void OnDisable() {
        base.OnDisable();
        ControllableObject.OnButton -= OnButton;
    }

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot && _collisionState.OnSolidGround) {

            //STANDING STILL
            if (_body2d.velocity.x > -0.5f && _body2d.velocity.x < 0.5f) {

                //AIMING DOWN
                if (_controller.AimDirection.Down) {

                    //AIMING DOWN AND RIGHT AND STANDING STILL
                    if (_controller.AimDirection.Right) {
                        _body2d.AddForce(new Vector2(-5000, 7500), ForceMode2D.Impulse);
                    }

                    //AIMING DOWN AND LEFT AND STANDING STILL
                    else if (_controller.AimDirection.Left) {
                        _body2d.AddForce(new Vector2(5000, 7500), ForceMode2D.Impulse);
                    }

                    //AIMING STRAIGHT DOWN AND STANDING STILL
                    else {
                        _body2d.AddForce(new Vector2(0, 10000), ForceMode2D.Impulse);
                    }
                }
            }

            //MOVING LEFT OR RIGHT
            else {
                //AIMING DOWN AND RIGHT AND MOVING
                if (_controller.AimDirection.Right) {
                    _body2d.AddForce(new Vector2(0, 7500), ForceMode2D.Impulse);
                }

                //AIMING DOWN AND LEFT AND MOVING
                else if (_controller.AimDirection.Left) {
                    _body2d.AddForce(new Vector2(0, 7500), ForceMode2D.Impulse);
                }

                //AIMING STRAIGHT DOWN AND MOVING
                else {
                    _body2d.AddForce(new Vector2(0, 10000), ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnButton(Buttons button) {

        if (button == Buttons.Shoot && !_collisionState.OnSolidGround) {

            _xVel = _body2d.velocity.x;
            _yVel = _body2d.velocity.y;

            //AIMING DOWN
            if (_controller.AimDirection.Down) {

                //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
                if (_yVel <= _recoil && _body2d.velocity.y < 0) {

                    //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
                    _yVel += Mathf.Abs(_recoil * (1.3f * (_body2d.velocity.y / _yVel) ) );

                    //AIMING DOWN AND RIGHT
                    if (_controller.AimDirection.Right) {
                        if (_xVel >= _recoil * -_xMultiplier) {
                            _xVel -= _recoil;
                        }
                    }

                    //AIMING DOWN AND LEFT
                    else if (_controller.AimDirection.Left) {
                        if (_xVel <= _recoil * _xMultiplier) {
                            _xVel += _recoil;
                        }
                    }
                }

                //AIMING STRIGHT DOWN AND MOVING RIGHT
                else if (_body2d.velocity.x > 0) {
                    _xVel -= 2.0f;
                }

                //AIMING STRIGHT DOWN AND MOVING LEFT
                else if (_body2d.velocity.x < 0) {
                    _xVel += 2.0f;
                }
            }

            //AIMING RIGHT
            else if (_controller.AimDirection.Right) {
                _xVel += (_recoil * -0.25f);
            }

            //AIMING LEFT
            else if (_controller.AimDirection.Left) {
                _xVel += (_recoil * 0.25f);
            }

            //SET THE VELOCITY
            _body2d.velocity = new Vector2(_xVel, _yVel);
        }
    }
}
