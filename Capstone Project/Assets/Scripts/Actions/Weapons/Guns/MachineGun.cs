
using UnityEngine;
using System.Collections;

/*
 * Weapon based on the AbstractGun class which gives the player a "float" like ability.
 */

public class MachineGun : AbstractGun {

    // This is put into the shotgun script so the shotgun screen shake script
    // knows which weapon called this event.
    public static event AbstractGunEvent2 Fire;
    public static event AbstractGunEvent2 EmptyClip;
    public static event AbstractGunEvent3 StartReloadAnimation;
    public static event AbstractGunEvent UpdateNumOfRounds;

    [SerializeField]
    protected float _xMultiplier;

    private bool _canLift = true;

    protected override void OnEnable() {
        base.OnEnable();

        if (UpdateNumOfRounds != null) {
            UpdateNumOfRounds(numOfRounds);
        }

        ControllableObject.OnButton += OnButton;

        _canLift = _collisionState.OnSolidGround ? true : false;
    }

    protected override void OnDisable() {
        base.OnDisable();
        ControllableObject.OnButton -= OnButton;
    }

    protected override void Reload() {
        _canLift = true;

        if (_body2d.velocity.y <= 0.0f && numOfRounds <= 0) {
            StartCoroutine(ReloadDelay());
        }
    }

    private IEnumerator ReloadDelay() {

        _canShoot = false;
        while (!_collisionState.OnSolidGround) {
            yield return 0;
        }

        if (StartReloadAnimation != null) {
            StartReloadAnimation(_reloadTime);
        }

        float timer = Time.time;
        while (Time.time - timer < _reloadTime) {
            yield return 0;
        }

        numOfRounds = _clipSize;

        // UPDATE THE UI
        if (UpdateNumOfRounds != null) {
            UpdateNumOfRounds(numOfRounds);
        }

        _canShoot = true;
    }

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot && _canShoot && _collisionState.OnSolidGround && numOfRounds > 0) {

            float xVel = _body2d.velocity.x;

            //STANDING STILL
            if (xVel > -0.5f && xVel < 0.5f) {

                //AIMING DOWN
                if (_controller.CurrentKey == 6) {

                    //AIMING STRAIGHT DOWN AND STANDING STILL
                    _body2d.AddForce(new Vector2(0, 10000), ForceMode2D.Impulse);
                    _canLift = false;
                }

                //AIMING DOWN AND RIGHT AND STANDING STILL
                else if (_controller.CurrentKey == 7) {
                    _body2d.AddForce(new Vector2(-5000, 7500), ForceMode2D.Impulse);
                    _canLift = false;
                }

                //AIMING DOWN AND LEFT AND STANDING STILL
                else if (_controller.CurrentKey == 5) {
                    _body2d.AddForce(new Vector2(5000, 7500), ForceMode2D.Impulse);
                    _canLift = false;
                }
            }

            //MOVING LEFT OR RIGHT
            else {

                //AIMING DOWN AND RIGHT AND MOVING
                if (_controller.CurrentKey == 6) {

                    //AIMING STRAIGHT DOWN AND MOVINGf
                    _body2d.AddForce(new Vector2(0, 10000), ForceMode2D.Impulse);
                    _canLift = false;
                }

                else if (_controller.CurrentKey == 7) {
                    _body2d.AddForce(new Vector2(0, 7500), ForceMode2D.Impulse);
                    _canLift = false;
                }

                //AIMING DOWN AND LEFT AND MOVING
                else if (_controller.CurrentKey == 5) {
                    _body2d.AddForce(new Vector2(0, 7500), ForceMode2D.Impulse);
                    _canLift = false;
                }
            }
        }
    }

    private void OnButton(Buttons button) {

        if (button == Buttons.Shoot && numOfRounds > 0) {

            _xVel = _body2d.velocity.x;
            _yVel = _body2d.velocity.y;

            _gunActions[_controller.CurrentKey].Invoke();
            SetVeloctiy(_xVel, _yVel);

            if (Fire != null) {
                Fire();
            }

            if (_canLift && _controller.GetButtonPress(Buttons.AimDown)) {
                OnButtonDown(button);
            }
            else {
                if (_canShoot) {
                    if (--numOfRounds <= 0) {
                        if (EmptyClip != null) {
                            EmptyClip();
                        }
                        Reload();
                    }
                    if (UpdateNumOfRounds != null) {
                        UpdateNumOfRounds(numOfRounds);
                    }

                    StartCoroutine(ShotDelay());
                }
            }
        }
        else if (numOfRounds <= 0) {
            if (EmptyClip != null) {
                EmptyClip();
            }
        }
    }

    protected override void AimDown() {

        //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
        if (_yVel <= _recoil && _body2d.velocity.y < 0) {

            //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
            _yVel += Mathf.Abs(_recoil * (1.3f * (_body2d.velocity.y / _yVel)));
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

    protected override void AimDownAndRight() {

        //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
        if (_yVel <= _recoil && _body2d.velocity.y < 0) {

            //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
            _yVel += Mathf.Abs(_recoil * (1.3f * (_body2d.velocity.y / _yVel)));

            if (_xVel >= _recoil * -_xMultiplier) {
                _xVel -= _recoil;
            }
        }
    }

    protected override void AimDownAndLeft() {
        //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
        if (_yVel <= _recoil && _body2d.velocity.y < 0) {

            //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
            _yVel += Mathf.Abs(_recoil * (1.3f * (_body2d.velocity.y / _yVel)));

            if (_xVel <= _recoil * _xMultiplier) {
                _xVel += _recoil;
            }
        }
    }

    protected override void AimRight() {
        _xVel += (_recoil * -0.25f);
    }

    protected override void AimLeft() {
        _xVel += (_recoil * 0.25f);
    }
}
