
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
        }

        if (button == Buttons.Reload) {
            ManualReload();
        }
    }

    private void OnButton(Buttons button) {

        if (button == Buttons.Shoot && numOfRounds > 0) {

            if (_canLift && _controller.GetButtonPress(Buttons.AimDown)) {
                OnButtonDown(button);
            }
            if (!_reloading) {

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

            _SOEffect.PlayEffect(EffectEnum.MGMuzzelFlash, _barrel.transform.position, _controller.AimDirection);
            GameObject instance = Instantiate(_bullet, _barrel.transform.position, Quaternion.identity) as GameObject;

            // Angle the crystal according the the angle of the gun's direction
            instance.transform.localEulerAngles = new Vector3(0.0f, 0.0f, _controller.AimDirection);

            _SSRequest.ShakeRequest();
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
}
