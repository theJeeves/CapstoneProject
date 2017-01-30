using UnityEngine;
using System.Collections;

public class PlayerWalking : MonoBehaviour {

    [SerializeField]
    private MovementRequest _moveRequest;

    private ControllableObject _controller;
    //private PlayerCollisionState _collisionState;

    //private bool _grounded;

    private void OnEnable() {

        _controller = GetComponent<ControllableObject>();
        //_collisionState = GetComponent<PlayerCollisionState>();

        ControllableObject.OnButton += OnButton;
        //ControllableObject.OnButtonUp += OnButtonUp;
    }

    private void OnDisable() {

        ControllableObject.OnButton -= OnButton;
        //ControllableObject.OnButtonUp -= OnButtonUp;
    }

    private void OnButton(Buttons button) {

        //_grounded = _collisionState.OnSolidGround;

        //AS LONG AS THE PLAYER IS NOT AIMING IN A DOWN DIRECTION AND FIRING, THEY CAN WALK
        // THIS IS TO PREVENT THE WALKING SCRIPT FROM OVERRIDING THE WEAPON RECOIL MOVEMENT
        if (_controller.GetButtonPress(Buttons.AimDown) && _controller.GetButtonPress(Buttons.Shoot)) {
        }
        else {
            if (button == Buttons.MoveRight || button == Buttons.MoveLeft) {
                _moveRequest.RequestMovement(button);
            }
        }
    }
}
