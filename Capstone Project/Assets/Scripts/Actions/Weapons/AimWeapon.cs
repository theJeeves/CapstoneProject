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
        ControllableObject.OnButton += OnButtonDown;
        ControllableObject.OnButtonUp -= OnButtonUp;
    }

    private void OnButtonDown(Buttons button) {

        int degree = 0;


        if (button == Buttons.AimDown) {
            if (_controller.AimDirection.Right) {
                degree = 315;
            }
            else if (_controller.AimDirection.Left) {
                degree = 225;
            }
            else {
                degree = 270;
            }
        }
        else if (button == Buttons.AimUp) {
            if (_controller.AimDirection.Right) {
                degree = 45;
            }
            else if (_controller.AimDirection.Left) {
                degree = 135;
            }
            else {
                degree = 90;
            }
        }

        else if (button == Buttons.AimRight) {
            degree = 0;
        }
        else if (button == Buttons.AimLeft) {
            degree = 180;
        }

        // These tweaks are to take into account for negating the X-scale on the sprite.
        if (button == Buttons.AimDown || button == Buttons.AimUp || button == Buttons.AimRight || button == Buttons.AimLeft) {
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

        //if (angle < -90 && transform.localScale.y > 0) {
        //    transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        //}
        //else if (angle >= -90 && transform.localScale.y < 0) {
        //    transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        //}
    }
}
