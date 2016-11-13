using UnityEngine;
using System.Collections;

public class AimWeapon : MonoBehaviour {

    public delegate void AimWeaponEvent(int degree);
    public static event AimWeaponEvent AimDirectionChanged;

    private ControllableObject _controller;

	// Use this for initialization
	private void Awake () {
        _controller = GetComponentInParent<ControllableObject>();
	}

    private void OnEnable() {
        ControllableObject.OnButton += OnButtonDown;
        ControllableObject.OnButtonUp += OnButtonUp;
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

            if (AimDirectionChanged != null) {
                AimDirectionChanged(degree);
            }
        }
    }

    private void OnButtonUp(Buttons button) {
        if (button == Buttons.AimDown || button == Buttons.AimUp ||
            button == Buttons.AimRight || button == Buttons.AimLeft) {

            RotateGun(0);
            if (AimDirectionChanged != null) {
                AimDirectionChanged(0);
            }
        }
    }

    private void RotateGun(int angle) {

        transform.localEulerAngles = new Vector3(0, 0, angle);

        Vector2 position = Vector3.zero;
        switch (angle) {
            case 0:
            case 180:
                position = new Vector2(82.0f, 2.3f);
                break;
            case 45:
            case 135:
                position = new Vector2(64.5f, 121.0f);
                break;
            case -45:
            case 315:
                position = new Vector2(40.0f, -106.0f);
                break;
            case 90:
                position = new Vector2(-21.0f, 175.0f);
                break;
            case 270:
                position = new Vector2(-45.0f, -155.0f);
                break;
        }

        transform.localPosition = position;
    }
}
