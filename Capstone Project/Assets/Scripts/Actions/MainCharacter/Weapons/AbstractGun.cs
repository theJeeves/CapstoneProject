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

        //_down = new Vector2(_body2d.velocity.x, _recoil);
        //_right = new Vector2(-(_recoil), _body2d.velocity.y);
        //_left = new Vector2(_recoil, _body2d.velocity.y);
    }

    protected override void OnButtonDown(Buttons button) {

        //_newVelocity.x = _body2d.velocity.x;
        //_newVelocity.y = _body2d.velocity.y;

        //if (_controller.AimDirection.Down) {

        //    if (_controller.AimDirection.Right) {
        //        _body2d.velocity = (_down + _right) * 0.7f;
        //    }
        //    else if (_controller.AimDirection.Left) {
        //        _body2d.velocity = (_down + _left) * 0.7f;
        //    }
        //    else {
        //        _body2d.velocity = _down;
        //    }

            //if (_controller.AimDirection.Right || _controller.AimDirection.Left) {
            //    _xRecoil = _controller.AimDirection.Left ? _recoil * 0.5f : _recoil * -0.5f;
            //    _yRecoil =  _recoil * 0.5f;
            //}
        //}

        //else if (_controller.AimDirection.Right) {
        //    _body2d.velocity = _right;
        //}
        //else if (_controller.AimDirection.Left) {
        //    _body2d.velocity = _left;
        //}

        if (_controller.AimDirection.Down) {

            // AIMING DOWN AND RIGHT
            if (_controller.AimDirection.Right) {
                
                // MOVING LEFT
                if (_body2d.velocity.x < 0) {

                    // FALLING
                    if (_body2d.velocity.y < 0) {
                        _body2d.velocity = new Vector2(_body2d.velocity.x + _recoil * -(_addVel), _recoil * _setVel);
                    }

                    // RISING
                    else if (_body2d.velocity.y >= 0) {
                        float temp = 0.0f;
                        _body2d.velocity = new Vector2(_body2d.velocity.x + _recoil * -_addVel, Mathf.Clamp(temp,_recoil * _addVel,_recoil * 2));
                    }
                }
                else if (_body2d.velocity.x >= 0) {
                    _body2d.velocity = new Vector2(_recoil * -_setVel, _recoil * _setVel);
                }
            }
            // AIMING DOWN AND LEFT
            else if (_controller.AimDirection.Left) {
                if (_body2d.velocity.x > 0) {
                    if (_body2d.velocity.y < 0) {
                        _body2d.velocity = new Vector2(_body2d.velocity.x + _recoil * _addVel, _recoil * _setVel);
                    }
                    else if (_body2d.velocity.y >= 0) {
                        _body2d.velocity += new Vector2(_recoil * _addVel, _recoil * _addVel);
                    }
                }
                else if (_body2d.velocity.x <= 0) {
                    _body2d.velocity = new Vector2(_recoil * _setVel, _recoil * _setVel);
                }
            }
            // AIMING STRAIGHT DOWN
            else {
                if (_body2d.velocity.y < 0) {
                    _body2d.velocity = new Vector2(_body2d.velocity.x, _recoil);
                }
                else if (_body2d.velocity.y >= 0) {
                    float temp1 = _body2d.velocity.y;

                    float temp2 = Mathf.Clamp(_body2d.velocity.y + _recoil, _recoil, _recoil * 2.0f);
                    _body2d.velocity = new Vector2(_body2d.velocity.x, _recoil);
                }
            }
        }
        // AIMING RIGHT
        else if (_controller.AimDirection.Right) {
            if (_body2d.velocity.x < 0) {
                _body2d.velocity += new Vector2(_recoil * -_addVel, 0);
            }
            else if (_body2d.velocity.x >= 0) {
                _body2d.velocity = new Vector2(_recoil * -_setVel, _body2d.velocity.y);
            }
        }
        // AIMING LEFT
        else if (_controller.AimDirection.Left) {
            if (_body2d.velocity.x > 0) {
                _body2d.velocity += new Vector2(_recoil * _addVel, 0);
            }
            else if (_body2d.velocity.x <= 0) {
                _body2d.velocity = new Vector2(_recoil * _setVel, _body2d.velocity.y);
            }
        }
        //_body2d.velocity += _newVelocity;
    }
}
