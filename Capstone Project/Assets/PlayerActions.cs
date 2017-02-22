using UnityEngine;
using System.Collections;

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

    private void OnEnable() {
        ControllableObject.OnButton += OnButton;
        ControllableObject.OnButtonUp += OnButtonUp;

        _controller = GetComponent<ControllableObject>();
    }

    private void OnDisable() {
        ControllableObject.OnButton -= OnButton;
        ControllableObject.OnButtonUp -= OnButtonUp;
    }

    private void OnButton(Buttons button) {


        /*
         * PLAYER WALKING
         */
        //AS LONG AS THE PLAYER IS NOT AIMING IN A DOWN DIRECTION AND FIRING, THEY CAN WALK
        // THIS IS TO PREVENT THE WALKING SCRIPT FROM OVERRIDING THE WEAPON RECOIL MOVEMENT
        if (_controller.GetButtonPress(Buttons.AimDown) && _controller.GetButtonPress(Buttons.Shoot)) {
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

            // Aiming down and possibly another direciton
            if (_controller.GetButtonPress(Buttons.AimDown) && _controller.GetButtonPress(Buttons.AimRight)) {
                value = 7;
            }
            else if (_controller.GetButtonPress(Buttons.AimDown) && _controller.GetButtonPress(Buttons.AimLeft)) {
                value = 5;
            }
            else if (_controller.GetButtonPress(Buttons.AimDown)) {
                value = 6;
            }

            // Aiming up and possibly another direction
            else if (_controller.GetButtonPress(Buttons.AimUp) && _controller.GetButtonPress(Buttons.AimRight)) {
                value = 1;
            }
            else if (_controller.GetButtonPress(Buttons.AimUp) && _controller.GetButtonPress(Buttons.AimLeft)) {
                value = 3;
            }
            else if (_controller.GetButtonPress(Buttons.AimUp)) {
                value = 2;
            }

            // Aiming Right
            else if (_controller.GetButtonPress(Buttons.AimRight)) {
                value = 0;
            }

            // Aiming Left
            else if (_controller.GetButtonPress(Buttons.AimLeft)) {
                value = 4;
            }

            _controller.SetAimDirection(value);
        }
    }

    private void OnButtonUp(Buttons button) {

        /*
        * SET AIM DIRECTION
        */
        if (button != Buttons.Shoot) {
            if (_controller.FacingDirection == Facing.Right) {
                _controller.SetAimDirection(0);
            }
            else if (_controller.FacingDirection == Facing.Left) {
                _controller.SetAimDirection(4);
            }
        }
    }
}
