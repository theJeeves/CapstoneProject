
using UnityEngine;
using System.Collections;

/*
 * Weapon based on the AbstractGun class which gives the player a "float" like ability.
 */

public class MachineGun : AbstractGun {

    // This is put into the shotgun script so the shotgun screen shake script
    // knows which weapon called this event.
    //public static event AbstractGunEvent2 Fire;
    public static event AbstractGunEvent2 EmptyClip;
    public static event AbstractGunEvent3 StartReloadAnimation;
    public static event AbstractGunEvent UpdateNumOfRounds;

    [SerializeField]
    private MovementRequest _initialMoveRequest;

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

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot && _canShoot && _collisionState.OnSolidGround && numOfRounds > 0) {

            _initialMoveRequest.RequestMovement();
            _canLift = false;

            //float xVel = _body2d.velocity.x;

            ////STANDING STILL
            //if (xVel > -0.5f && xVel < 0.5f) {

            //    //AIMING DOWN
            //    if (_controller.CurrentKey == 6) {

            //        //AIMING STRAIGHT DOWN AND STANDING STILL
            //        _body2d.AddForce(new Vector2(0, 10000), ForceMode2D.Impulse);
            //        _canLift = false;
            //    }

            //    //AIMING DOWN AND RIGHT AND STANDING STILL
            //    else if (_controller.CurrentKey == 7) {
            //        _body2d.AddForce(new Vector2(-5000, 7500), ForceMode2D.Impulse);
            //        _canLift = false;
            //    }

            //    //AIMING DOWN AND LEFT AND STANDING STILL
            //    else if (_controller.CurrentKey == 5) {
            //        _body2d.AddForce(new Vector2(5000, 7500), ForceMode2D.Impulse);
            //        _canLift = false;
            //    }
            //}

            ////MOVING LEFT OR RIGHT
            //else {

            //    //AIMING DOWN AND RIGHT AND MOVING
            //    if (_controller.CurrentKey == 6) {

            //        //AIMING STRAIGHT DOWN AND MOVINGf
            //        _body2d.AddForce(new Vector2(0, 10000), ForceMode2D.Impulse);
            //        _canLift = false;
            //    }

            //    else if (_controller.CurrentKey == 7) {
            //        _body2d.AddForce(new Vector2(0, 7500), ForceMode2D.Impulse);
            //        _canLift = false;
            //    }

            //    //AIMING DOWN AND LEFT AND MOVING
            //    else if (_controller.CurrentKey == 5) {
            //        _body2d.AddForce(new Vector2(0, 7500), ForceMode2D.Impulse);
            //        _canLift = false;
            //    }
            //}
        }
    }

    private void OnButton(Buttons button) {

        if (button == Buttons.Shoot && numOfRounds > 0) {

            if (_canLift && _controller.GetButtonPress(Buttons.AimDown)) {
                OnButtonDown(button);
            }
            if (!_reloading) {

                //_xVel = _body2d.velocity.x;
                //_yVel = _body2d.velocity.y;

                //// Call the appropriate function based on the player's aim direction.
                //_gunActions[_controller.CurrentKey].Invoke();
                //SetVeloctiy(_xVel, _yVel);

                _moveRequest.RequestMovement();

                _grounded = false;
            }
            if (_canShoot & !_reloading) {

                // If the last round has gone out, send out the event to hide the ammo type display.
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

                StartCoroutine(ShotDelay());
            }
        }
        
        // Only call for the ammo to hide if the player has no more bullets in the clip
        // and are in the air. Otherwise, it is handle in the if statement above.
        else if (numOfRounds <= 0 && !_collisionState.OnSolidGround) {
            if (EmptyClip != null) {
                EmptyClip();
            }
        }
    }

    protected override IEnumerator ShotDelay() {
        if (!_damaged) {
            _canShoot = false;
            Instantiate(_bullet, _mgBarrel.transform.position, Quaternion.identity);
            //if (Fire != null) {
            //    Fire();
            //}
            _SSRequest.ShakeRequest(_controller.CurrentKey);
            yield return new WaitForSeconds(_shotDelay);
            _canShoot = true;
        }
    }

    protected override void Reload() {

        // This ensures the player will be lifted by the initial shot every time.
        // Machine Gun specific.
        _canLift = true;

        if (numOfRounds > 0 && _controller.GetButtonPress(Buttons.Shoot)) { }
        else {
            base.Reload();
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

    //protected override void AimDown() {

    //    //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
    //    if (_yVel <= _recoil && _body2d.velocity.y < 0) {

    //        //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
    //        _yVel += Mathf.Abs(_recoil * (1.3f * (_body2d.velocity.y / _yVel)));
    //    }

    //    //AIMING STRIGHT DOWN AND MOVING RIGHT
    //    else if (_body2d.velocity.x > 0) {
    //        _xVel -= 2.0f;
    //    }

    //    //AIMING STRIGHT DOWN AND MOVING LEFT
    //    else if (_body2d.velocity.x < 0) {
    //        _xVel += 2.0f;
    //    }
    //}

    //protected override void AimDownAndRight() {

    //    //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
    //    if (_yVel <= _recoil && _body2d.velocity.y < 0) {

    //        //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
    //        _yVel += Mathf.Abs(_recoil * (1.3f * (_body2d.velocity.y / _yVel)));

    //        if (_xVel >= _recoil * -_xMultiplier) {
    //            _xVel -= _recoil;
    //        }
    //    }
    //}

    //protected override void AimDownAndLeft() {
    //    //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
    //    if (_yVel <= _recoil && _body2d.velocity.y < 0) {

    //        //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
    //        _yVel += Mathf.Abs(_recoil * (1.3f * (_body2d.velocity.y / _yVel)));

    //        if (_xVel <= _recoil * _xMultiplier) {
    //            _xVel += _recoil;
    //        }
    //    }
    //}

    //protected override void AimRight() {
    //    _xVel += (_recoil * -0.25f);
    //}

    //protected override void AimLeft() {
    //    _xVel += (_recoil * 0.25f);
    //}
}
