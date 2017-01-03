using UnityEngine;
using System.Collections;

public class SetAimingDirection : MonoBehaviour {

    private ControllableObject _controller;

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

        if (button == Buttons.AimDown || button == Buttons.AimLeft || 
            button == Buttons.AimRight || button == Buttons.AimUp) {

            byte value = 0;

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
            else if (_controller.GetButtonPress(Buttons.AimRight)) {
                value = 0;
            }
            else if (_controller.GetButtonPress(Buttons.AimLeft)) {
                value = 4;
            }

            _controller.SetAimDirection(value);
        }
    }

    private void OnButtonUp(Buttons button) {

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
