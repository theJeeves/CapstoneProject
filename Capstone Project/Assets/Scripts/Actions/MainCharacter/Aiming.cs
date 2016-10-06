using UnityEngine;
using System.Collections;

public class Aiming : AbstractPlayerActions {

    protected override void OnEnable() {
        ControllableObject.OnButton += OnButton;
        ControllableObject.OnButtonUp += OnButtonUp;
    }

    protected override void OnDisable() {
        ControllableObject.OnButton -= OnButton;
        ControllableObject.OnButtonUp -= OnButtonUp;
    }

    private void OnButton(Buttons button) {
        if (button == Buttons.AimRight) {
            _controller._right = true;
        }
        else if (button == Buttons.AimLeft) {
            _controller._left = true;
        }
        else if (button == Buttons.AimUp) {
            _controller._up = true;
        }
        else if (button == Buttons.AimDown) {
            _controller._down = true;
        }
    }

    private void OnButtonUp(Buttons button) {
        if (button == Buttons.AimRight) {
            _controller._right = false;
        }
        else if (button == Buttons.AimLeft) {
            _controller._left = false;
        }
        else if (button == Buttons.AimUp) {
            _controller._up = false;
        }
        else if (button == Buttons.AimDown) {
            _controller._down = false;
        }
    }
}
