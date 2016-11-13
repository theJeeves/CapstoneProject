
using UnityEngine;
using System.Collections;

/*
 * Weapon based on the AbstractGun class which gives the player a "float" like ability.
 */ 

public class MachineGun : AbstractGun {

    // This is put into the shotgun script so the shotgun screen shake script
    // knows which weapon called this event.
    public static event AbstractGunEvent2 Fire;

    [SerializeField]
    protected float _xMultiplier;

    private bool _canLift = true;

    protected override void OnEnable() {
        base.OnEnable();

        ControllableObject.OnButton += OnButton;
        PlayerCollisionState.OnHitGround += OnHitSolidGround;

        _canLift = _collisionState.OnSolidGround ? true : false;
    }

    protected override void OnDisable() {
        base.OnDisable();
        ControllableObject.OnButton -= OnButton;
        PlayerCollisionState.OnHitGround -= OnHitSolidGround;
    }

    private void OnHitSolidGround() {

        _canLift = true;
        
        if (_body2d.velocity.y <= 0.0f) {
            Reload();
        }
    }

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot && _collisionState.OnSolidGround && _numOfRounds > 0) {

            //STANDING STILL
            if (_body2d.velocity.x > -0.5f && _body2d.velocity.x < 0.5f) {

                //AIMING DOWN
                //if (_controller.AimDirection.Down) {
                if (_controller.CurrentKey == 6) {

                    //AIMING STRAIGHT DOWN AND STANDING STILL
                    _body2d.AddForce(new Vector2(0, 10000), ForceMode2D.Impulse);
                    _canLift = false;
                }

                //AIMING DOWN AND RIGHT AND STANDING STILL
                //if (_controller.AimDirection.Right) {
                else if (_controller.CurrentKey == 7) {
                    _body2d.AddForce(new Vector2(-5000, 7500), ForceMode2D.Impulse);
                    _canLift = false;
                }

                //AIMING DOWN AND LEFT AND STANDING STILL
                //else if (_controller.AimDirection.Left) {
                else if (_controller.CurrentKey == 5) {
                    _body2d.AddForce(new Vector2(5000, 7500), ForceMode2D.Impulse);
                    _canLift = false;
                }
            }

            //MOVING LEFT OR RIGHT
            else {

                //AIMING DOWN AND RIGHT AND MOVING
                //if (_controller.AimDirection.Down) {
                if (_controller.CurrentKey == 6) {

                    //AIMING STRAIGHT DOWN AND MOVINGf
                    _body2d.AddForce(new Vector2(0, 10000), ForceMode2D.Impulse);
                    _canLift = false;
                }

                //if (_controller.AimDirection.Right) {
                else if (_controller.CurrentKey == 7) {
                    _body2d.AddForce(new Vector2(0, 7500), ForceMode2D.Impulse);
                    _canLift = false;
                }

                //AIMING DOWN AND LEFT AND MOVING
                //else if (_controller.AimDirection.Left) {
                else if (_controller.CurrentKey == 5) {
                    _body2d.AddForce(new Vector2(0, 7500), ForceMode2D.Impulse);
                    _canLift = false;
                }
            }
        }
    }

    private void OnButton(Buttons button) {

        if (button == Buttons.Shoot && _numOfRounds > 0) {

            if (Fire != null) {
                Fire();
            }

            //if (_canLift && _controller.AimDirection.Down) {
            if (_canLift && _controller.GetButtonPress(Buttons.AimDown) ) { 
                OnButtonDown(button);
            }
            else {
                base.OnButtonDown(button);
                _gunActions[_controller.CurrentKey].Invoke();
                SetVeloctiy(_xVel, _yVel);
            }
        }
        else if (_numOfRounds <= 0 && _collisionState.OnSolidGround) {
            Reload();
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
