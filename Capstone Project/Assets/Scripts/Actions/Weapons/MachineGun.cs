
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
    private GameObject _bullet;
    [SerializeField]
    private Transform _mgBarrel;

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

        if (button == Buttons.Shoot && _collisionState.OnSolidGround && _numOfRounds > 0) {

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
                if (_controller.AimDirection.Down) {
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
    }

    private void OnButton(Buttons button) {

        if (button == Buttons.Shoot && !_collisionState.OnSolidGround && _numOfRounds > 0) {

            if (Fire != null) {
                Fire();
            }

            base.OnButtonDown(button);
        }
    }

    protected override void AimDown() {

        //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
        if (_yVel <= _recoil && _body2d.velocity.y < 0) {

            //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
            _yVel += Mathf.Abs(_recoil * (1.3f * (_body2d.velocity.y / _yVel)));

            //AIMING DOWN AND RIGHT
            if (_controller.AimDirection.Right) {
                AimDownAndRight();
            }

            //AIMING DOWN AND LEFT
            else if (_controller.AimDirection.Left) {
                AimDownAndLeft();
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

    protected override void AimDownAndRight() {

        if (_xVel >= _recoil * -_xMultiplier) {
            _xVel -= _recoil;
        }
    }

    protected override void AimDownAndLeft() {

        if (_xVel <= _recoil * _xMultiplier) {
            _xVel += _recoil;
        }
    }

    protected override void AimRight() {
        _xVel += (_recoil * -0.25f);
    }

    protected override void AimLeft() {
        _xVel += (_recoil * 0.25f);
    }
}
