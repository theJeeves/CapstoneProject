using UnityEngine;
using System.Collections;

public class AimWeapon : MonoBehaviour {

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

        if (button == Buttons.AimDown) {
            if (_controller.AimDirection.Right) {
                RotateGun(-45);
            }
            else if (_controller.AimDirection.Left) {
                RotateGun(-135);
            }
            else {
                RotateGun(-90);
            }
        }
        else if (button == Buttons.AimRight) {
            RotateGun(0);
        }
        else if (button == Buttons.AimLeft) {
            RotateGun(-180);
        }
    }

    private void OnButtonUp(Buttons button) {
        if (button == Buttons.AimDown || button == Buttons.AimRight || button == Buttons.AimLeft) {
            RotateGun(0);
        }
    }

    private void RotateGun(int angle) {
        transform.eulerAngles = new Vector3(0, 0, angle);

        if (angle < -90 && transform.localScale.y > 0) {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
        else if (angle >= -90 && transform.localScale.y < 0) {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
        }
    }
}
