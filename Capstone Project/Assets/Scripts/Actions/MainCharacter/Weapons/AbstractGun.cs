using UnityEngine;
using System.Collections;

public abstract class AbstractGun : AbstractPlayerActions {

    [SerializeField]
    protected float _recoil;

    protected float _addVel = 0.35f;
    protected float _setVel = 0.6f;

    protected override void Awake() {
        _controller = GetComponentInParent<ControllableObject>();
        _body2d = GetComponentInParent<Rigidbody2D>();
        _collisionState = GetComponentInParent<PlayerCollisionState>();
    }

    protected override void OnEnable() {
        base.OnEnable();
    }

    protected override void OnDisable() {
        base.OnDisable();
    }

    protected override void OnButtonDown(Buttons button) {

        //AIMING DOWN
        if (_controller.AimDirection.Down) {

            //AIMING DOWN AND RIGHT
            if (_controller.AimDirection.Right) {

                //ON THE GROUND
                if (_collisionState.OnSolidGround) {

                    // MOVING LEFT
                    if (_body2d.velocity.x < 0) {
                        _body2d.velocity = new Vector2(_recoil * -_addVel + _body2d.velocity.x, _recoil * _setVel);
                    }

                    //MOVING RIGHT OR STANDING STILL
                    else if (_body2d.velocity.x >= 0) {
                        _body2d.velocity = new Vector2(_recoil * -_setVel, _recoil * _setVel);
                    }
                }

                //IN THE AIR
                else if (!_collisionState.OnSolidGround) {

                    //MOVING LEFT
                    if (_body2d.velocity.x < 0) {

                        //FALLING (NEGATIVE Y VELOCITY)
                        if (_body2d.velocity.y < 0) {
                            _body2d.velocity = new Vector2(_body2d.velocity.x + _recoil * -(_addVel), _recoil * _setVel);
                        }

                        //RISING OR ZERO Y VELOCITY
                        else if (_body2d.velocity.y >= 0) {
                            float temp = 0.0f;
                            _body2d.velocity = new Vector2(_body2d.velocity.x + _recoil * -_addVel, Mathf.Clamp(temp, _recoil * _addVel, _recoil * 2));
                        }
                    }

                    //MOVING RIGHT OR STANDING STILL
                    else if (_body2d.velocity.x >= 0) {
                        _body2d.velocity = new Vector2(_recoil * -_setVel, _recoil * _setVel);
                    }
                }
            }

            //AIMING DOWN AND LEFT
            else if (_controller.AimDirection.Left) {

                //ON THE GROUND
                if (_collisionState.OnSolidGround) {

                    //MOVING RIGHT
                    if (_body2d.velocity.x > 0) {
                        _body2d.velocity = new Vector2(_recoil * _addVel + _body2d.velocity.x, _recoil * _setVel);
                    }

                    // MOVING LEFT OR STANDING STILL
                    else if (_body2d.velocity.x <= 0) {
                        _body2d.velocity = new Vector2(_recoil * _setVel, _recoil * _setVel);
                    }
                }

                //IN THE AIR
                else if (!_collisionState.OnSolidGround) {

                    //MOVING RIGHT
                    if (_body2d.velocity.x > 0) {

                        //FALLING (NEGATIVE Y VELOCITY)
                        if (_body2d.velocity.y < 0) {
                            _body2d.velocity = new Vector2(_body2d.velocity.x + _recoil * _addVel, _recoil * _setVel);
                        }

                        //RISING OR ZERO Y VELOCITY
                        else if (_body2d.velocity.y >= 0) {
                            //_body2d.velocity = new Vector2(_recoil * _addVel, _recoil * _addVel);
                            _body2d.velocity = new Vector2(_recoil * _setVel + _body2d.velocity.x, _recoil * _setVel);
                        }
                    }

                    // MOVING LEFT OR STANDING STILL
                    else if (_body2d.velocity.x <= 0) {
                        _body2d.velocity = new Vector2(_recoil * _setVel, _recoil * _setVel);
                    }
                }
            }

            //AIMING STRAIGHT DOWN
            else {

                //FALLING (NEGATVIE Y VELOCITY
                if (_body2d.velocity.y < 0) {
                    _body2d.velocity = new Vector2(_body2d.velocity.x, _recoil);
                }

                //RISING OR ZERO Y VELOCITY
                else if (_body2d.velocity.y >= 0) {

                    _body2d.velocity = new Vector2(_body2d.velocity.x, _recoil);

                    //float temp1 = _body2d.velocity.y;

                    //float temp2 = Mathf.Clamp(_body2d.velocity.y + _recoil, _recoil, _recoil * 2.0f);
                    //_body2d.velocity = new Vector2(_body2d.velocity.x, _recoil);
                }
            }
        }

        //AIMING RIGHT
        else if (_controller.AimDirection.Right) {

            //IN AIR CONTROLLS ONLY
            if (!_collisionState.OnSolidGround) {

                // MOVING LEFT
                if (_body2d.velocity.x < 0) {
                    _body2d.velocity += new Vector2(_recoil * -_addVel, 0);
                }

                // MOVING RIGHT OR STANDING STILL
                else if (_body2d.velocity.x >= 0) {
                    _body2d.velocity = new Vector2(_recoil * -_setVel, _body2d.velocity.y);
                }
            }
        }
        //AIMING LEFT
        else if (_controller.AimDirection.Left) {

            //IN AIR CONTROLLS ONLY
            if (!_collisionState.OnSolidGround) {
                //MOVING RIGHT
                if (_body2d.velocity.x > 0) {
                    _body2d.velocity += new Vector2(_recoil * _addVel, 0);
                }

                //MOVING LEFT OR STANDING STILL
                else if (_body2d.velocity.x <= 0) {
                    _body2d.velocity = new Vector2(_recoil * _setVel, _body2d.velocity.y);
                }
            }
        }
    }
}
