using UnityEngine;
using System.Collections;

public abstract class AbstractGun : AbstractPlayerActions {
    [SerializeField]
    protected int _version;

    [SerializeField]
    protected float _recoil;

    protected float _addVel = 0.35f;
    protected float _setVel = 0.6f;

    [SerializeField]
    private float _doubleTapTime;

    private float _startTime;

    private Vector2 _down;
    private Vector2 _right;
    private Vector2 _left;

    protected override void Awake() {
        _controller = GetComponentInParent<ControllableObject>();
        _body2d = GetComponentInParent<Rigidbody2D>();
        _collisionState = GetComponentInParent<PlayerCollisionState>();

        _down = new Vector2(_body2d.velocity.x, _recoil);
        _right = new Vector2(-(_recoil), _body2d.velocity.y);
        _left = new Vector2(_recoil, _body2d.velocity.y);
    }

    protected override void OnEnable() {
        base.OnEnable();

        ControllableObject.OnButtonUp += OnButtonUp;
        PlayerCollisionState.OnHitGround += OnHitGround;
    }

    protected override void OnDisable() {
        base.OnDisable();

        ControllableObject.OnButtonUp -= OnButtonUp;
        PlayerCollisionState.OnHitGround -= OnHitGround;
    }

    private void OnHitGround() {

        _startTime = 0.0f;
    }

    private void OnButtonUp(Buttons button) {
        if (button == Buttons.Jump && Time.time - _startTime > _doubleTapTime) {
            _startTime = 0.0f;
        }
    }

    protected override void OnButtonDown(Buttons button) {

        if (_version == 1) {
            if (_startTime == 0.0f) {

                _startTime = Time.time;
            }
            else if (Time.time - _startTime < _doubleTapTime) {

                _body2d.velocity = new Vector2(_body2d.velocity.x, _recoil);
                Debug.Log("fire");
            }
        }

        if (_version == 2) {
            if (_controller.AimDirection.Down) {

                //AIMING DOWN AND RIGHT
                if (_controller.AimDirection.Right) {

                    //MOVING LEFT
                    if (_body2d.velocity.x < 0) {

                        //FALLING
                        if (_body2d.velocity.y < 0) {
                            _body2d.velocity = new Vector2(_body2d.velocity.x + _recoil * -(_addVel), _recoil * _setVel);
                        }

                        //RISING
                        else if (_body2d.velocity.y >= 0) {
                            float temp = 0.0f;
                            _body2d.velocity = new Vector2(_body2d.velocity.x + _recoil * -_addVel, Mathf.Clamp(temp, _recoil * _addVel, _recoil * 2));
                        }
                    }
                    else if (_body2d.velocity.x >= 0) {
                        _body2d.velocity = new Vector2(_recoil * -_setVel, _recoil * _setVel);
                    }
                }
                //AIMING DOWN AND LEFT
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
                //AIMING STRAIGHT DOWN
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
            //AIMING RIGHT
            else if (_controller.AimDirection.Right) {
                if (_body2d.velocity.x < 0) {
                    _body2d.velocity += new Vector2(_recoil * -_addVel, 0);
                }
                else if (_body2d.velocity.x >= 0) {
                    _body2d.velocity = new Vector2(_recoil * -_setVel, _body2d.velocity.y);
                }
            }
            //AIMING LEFT
            else if (_controller.AimDirection.Left) {
                if (_body2d.velocity.x > 0) {
                    _body2d.velocity += new Vector2(_recoil * _addVel, 0);
                }
                else if (_body2d.velocity.x <= 0) {
                    _body2d.velocity = new Vector2(_recoil * _setVel, _body2d.velocity.y);
                }
            }

        }
    }
}
