using UnityEngine;
using System.Collections;
using System;

public class PlayerActions : MonoBehaviour {
    
    [Header("Walking")]
    [SerializeField]
    private MovementRequest _moveRequest;

    [Space]
    [Header("Facing Direction")]
    private ControllableObject _controller;

    [Space]
    [Header("Set Aiming Direction")]
    private int value = 0;

    #region Initializers
    private void OnEnable() {
        ControllableObject.OnButton += OnButton;
        ControllableObject.OnButtonDown += OnButtonDown;

        _controller = GetComponent<ControllableObject>();
    }

    #endregion Initializers

    #region Finalizers
    private void OnDisable() {
        ControllableObject.OnButton -= OnButton;
        ControllableObject.OnButtonDown -= OnButtonDown;
    }

    #endregion Finalizers

    #region Events
    public static event EventHandler<Buttons> ButtonPressed;
    public static event EventHandler<Buttons> ButtonHeld;

    #endregion Events

    #region Private Methods
    private void Update() {

        // THIS IS HERE TO ENSURE THE MUZZLE FIRE ON THE MACHINE GUN ALWAYS PLAYS IN THE CORRECT DIRECTION
        if (!_controller.GetButtonPress(Buttons.AimDown) && !_controller.GetButtonPress(Buttons.AimUp) &&
            !_controller.GetButtonPress(Buttons.AimLeft) && !_controller.GetButtonPress(Buttons.AimRight)) {

            if (_controller.FacingDirection == Facing.Right) {
                _controller.SetAimDirection(0);
            }
            else if (_controller.FacingDirection == Facing.Left) {
                _controller.SetAimDirection(4);
            }
        }
    }

    private void OnButton(object sender, Buttons button) {

        /*
         * PLAYER WALKING
         */
        //AS LONG AS THE PLAYER IS NOT AIMING IN A DOWN DIRECTION AND FIRING, THEY CAN WALK
        // THIS IS TO PREVENT THE WALKING SCRIPT FROM OVERRIDING THE WEAPON RECOIL MOVEMENT
        if (_controller.GetButtonPress(Buttons.AimDown) && _controller.GetButtonPress(Buttons.Shoot) && _controller.GetButtonPressTime(Buttons.Shoot) < 0.25f) {
        }
        else {
            if (button == Buttons.MoveRight || button == Buttons.MoveLeft) {
                _moveRequest.RequestMovement(button);

                if (button == Buttons.MoveRight) { _controller.SetAimDirection(0); }
                else if (button == Buttons.MoveLeft) { _controller.SetAimDirection(4); }
            }
        }

        /*
         * FACING DIRECTION
         */
        switch (button) {
            case Buttons.MoveRight:
            case Buttons.AimRight:
                _controller.FacingDirection = Facing.Right;
                break;
            case Buttons.MoveLeft:
            case Buttons.AimLeft:
                _controller.FacingDirection = Facing.Left;
                break;
            default:
                break;
        }

        // In 2D games, to get the characters to chagnes directions, all we have to do is
        // to change the 'x' localScale from 1 or -1. This will flip the sprite. 
        // Note: even though we are working in 2D, if we were to call Vector2, it might set the z
        // axis to 0, which would cause the sprite to disappear entirely.
        if (button == Buttons.MoveLeft || button == Buttons.MoveRight ||
            button == Buttons.AimLeft || button == Buttons.AimRight) {

            if (_controller.FacingDirection == Facing.Right && transform.localScale.x < 0) {
                transform.localScale = new Vector3(-transform.localScale.x,
                    transform.localScale.y, transform.localScale.z);
            }
            else if (_controller.FacingDirection == Facing.Left && transform.localScale.x > 0) {
                transform.localScale = new Vector3(-transform.localScale.x,
                    transform.localScale.y, transform.localScale.z);
            }
        }

        /*
         * SET AIM DIRECTION
         */
         // Check the incoming button and see if it is any of the aiming directions.
         // For the appropriate direction, set the correct value and update the controller.
        if (button == Buttons.AimDown || button == Buttons.AimLeft ||
            button == Buttons.AimRight || button == Buttons.AimUp) {

            if (_controller.GetButtonPress(Buttons.AimDown)) {
                if (_controller.GetButtonPress(Buttons.AimRight)) {
                    value = 7;
                }
                else if (_controller.GetButtonPress(Buttons.AimLeft)) {
                    value = 5;
                }
                else {
                    value = 6;
                }
            }
            else if (_controller.GetButtonPress(Buttons.AimUp)) {
                if (_controller.GetButtonPress(Buttons.AimRight)) {
                    value = 1;
                }
                else if (_controller.GetButtonPress(Buttons.AimLeft)) {
                    value = 3;
                }
                else {
                    value = 2;
                }
            }
            else if (_controller.GetButtonPress(Buttons.AimLeft)) {
                value = 4;
            }
            else if (_controller.GetButtonPress(Buttons.AimRight)) {
                value = 0;
            }

            _controller.SetAimDirection(value);
        }


        // THIS STATEMENT IS TO MITIGATE A SMALL DELAY BETWEEN WHEN THE PLAYER SHOOTS AND THE CHARACTER MOVING INTO A AIMING POSITION
        // WITHOUT THE PLAYER ACTUALLY AIMING.
        if (button == Buttons.Shoot && !_controller.GetButtonPress(Buttons.AimDown) && !_controller.GetButtonPress(Buttons.AimUp) &&
            !_controller.GetButtonPress(Buttons.AimLeft) && !_controller.GetButtonPress(Buttons.AimRight)) {

            if (_controller.FacingDirection == Facing.Right) {
                _controller.SetButtonPressed(Buttons.AimRight);
            }
            else if (_controller.FacingDirection == Facing.Left) {
                _controller.SetButtonPressed(Buttons.AimLeft);
            }

            StartCoroutine(PersuaderDelay());
        }
        // IF THE PLAYER IS AIMING, THEN THERE IS NO NEED FOR THE SMALL DELAY AND FIRE IMMEDIATELY.
        else if (button == Buttons.Shoot && (_controller.GetButtonPress(Buttons.AimDown) || _controller.GetButtonPress(Buttons.AimUp) ||
            _controller.GetButtonPress(Buttons.AimLeft) || _controller.GetButtonPress(Buttons.AimRight)) ) {

            ButtonHeld?.Invoke(this, Buttons.Shoot);
        }
    }


    // THIS IS PRIMARILY FOR THE SHOTGUN SINCE WE DO NOT WANT THE GUN SHOOTING THE ENTIRE CLIP IF THE BUTTON IS HELD DOWN
    private void OnButtonDown(object sender, Buttons button) {

        // THIS STATEMENT IS TO MITIGATE A SMALL DELAY BETWEEN WHEN THE PLAYER SHOOTS AND THE CHARACTER MOVING INTO A AIMING POSITION
        // WITHOUT THE PLAYER ACTUALLY AIMING.
        if (button == Buttons.Shoot && !_controller.GetButtonPress(Buttons.AimDown) && !_controller.GetButtonPress(Buttons.AimUp) &&
            !_controller.GetButtonPress(Buttons.AimLeft) && !_controller.GetButtonPress(Buttons.AimRight)) {

            StartCoroutine(JouleDelay());
        }
        // IF THE PLAYER IS AIMING, THEN THERE IS NO NEED FOR THE SMALL DELAY AND FIRE IMMEDIATELY.
        else if (button == Buttons.Shoot && (_controller.GetButtonPress(Buttons.AimDown) || _controller.GetButtonPress(Buttons.AimUp) ||
            _controller.GetButtonPress(Buttons.AimLeft) || _controller.GetButtonPress(Buttons.AimRight))) {

            ButtonPressed?.Invoke(this, Buttons.Shoot);
        }
    }

    // EXTREMELY SMALL DELAY SO IT LOOKS INSTANT
    private IEnumerator PersuaderDelay() {
        yield return new WaitForSeconds(0.01f);
        ButtonHeld?.Invoke(this, Buttons.Shoot);
    }

    private IEnumerator JouleDelay() {
        yield return new WaitForSeconds(0.01f);
       ButtonPressed?.Invoke(this, Buttons.Shoot);
    }

    #endregion Private Methods
}
