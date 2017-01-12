using UnityEngine;
using System.Collections;

public class AimWeapon : MonoBehaviour {

    [SerializeField]
    private SOAnimations _aimingDirection;

    private ControllableObject _controller;

	// Use this for initialization
	private void Awake () {
        _controller = GetComponentInParent<ControllableObject>();
	}

    private void OnEnable() {
        ControllableObject.OnButton += OnButtonDown;
        ControllableObject.OnButtonUp += OnButtonUp;

        RotateGun(0);
        _aimingDirection.StopAnimation();
    }
	
    private void OnDisable() {
        ControllableObject.OnButton -= OnButtonDown;
        ControllableObject.OnButtonUp -= OnButtonUp;
    }

    private void OnButtonDown(Buttons button) {

        short degree = 0;

        if (button == Buttons.AimDown || button == Buttons.AimLeft ||
            button == Buttons.AimRight || button == Buttons.AimUp) {

            degree = _controller.AimDirection;

            switch (degree) {
                case 225:
                    RotateGun(-45); break;
                case 135:
                    RotateGun(45); break;
                case 180:
                    RotateGun(0); break;
                default:
                    RotateGun(degree); break;
            }

            _aimingDirection.PlayAnimation(degree);
        }
    }

    private void OnButtonUp(Buttons button) {
        if (button == Buttons.AimDown || button == Buttons.AimUp ||
            button == Buttons.AimRight || button == Buttons.AimLeft &&
            (!_controller.GetButtonPress(Buttons.AimDown) || !_controller.GetButtonPress(Buttons.AimUp) ||
             !_controller.GetButtonPress(Buttons.AimRight) || !_controller.GetButtonPress(Buttons.AimLeft) )) {

            RotateGun(0);

            _aimingDirection.StopAnimation();
        }
    }

    private void RotateGun(int angle) {

        transform.localEulerAngles = new Vector3(0, 0, angle);

        Vector2 position = Vector3.zero;
        switch (angle) {
            case 0:
            case 180:
                position = new Vector2(101.0f, 0.0f);
                break;
            case 45:
            case 135:
                position = new Vector2(96.0f, 75.0f);
                break;
            case -45:
            case 315:
                position = new Vector2(92.0f, -68.0f);
                break;
            case 90:
                position = new Vector2(20.0f, 98.0f);
                break;
            case 270:
                position = new Vector2(42.0f, -95.0f);
                break;
        }

        transform.localPosition = position;
    }
}
