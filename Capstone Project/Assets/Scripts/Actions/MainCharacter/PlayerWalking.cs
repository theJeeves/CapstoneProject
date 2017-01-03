using UnityEngine;
using System.Collections;

public class PlayerWalking : MonoBehaviour {

    [SerializeField]
    private MovementRequest _moveRequest;

    private ControllableObject _controller;

    public delegate void PlayerWalkingEvent(bool isWalking);
    public static event PlayerWalkingEvent Walking;

    private bool _canWalk;
    private bool _isWalking;
    private bool _grounded;

    private void OnEnable() {

        _controller = GetComponent<ControllableObject>();

        ControllableObject.OnButton += OnButton;
        ControllableObject.OnButtonUp += OnButtonUp;
        //SniperPushBack.Stun += Stun;

        _canWalk = true;
    }

    private void OnDisable() {

        ControllableObject.OnButton -= OnButton;
        ControllableObject.OnButtonUp -= OnButtonUp;
        //SniperPushBack.Stun -= Stun;
    }

    //private void Stun() {
    //    StartCoroutine(Delay());
    //}

    //private IEnumerator Delay() {
    //    if (_canWalk) {
    //        _canWalk = false;
    //        yield return new WaitForSeconds(1.0f);
    //        _canWalk = true;
    //    }
    //}

    private void OnButton(Buttons button) {

        //_grounded = _collisionState.OnSolidGround;

        //AS LONG AS THE PLAYER IS NOT AIMING IN A DOWN DIRECTION AND FIRING, THEY CAN WALK
        // THIS IS TO PREVENT THE WALKING SCRIPT FROM OVERRIDING THE WEAPON RECOIL MOVEMENT
        if (_controller.GetButtonPress(Buttons.AimDown) && _controller.GetButtonPress(Buttons.Shoot)) {
        }
        else {
            if (_canWalk) {
                if (button == Buttons.MoveRight || button == Buttons.MoveLeft) {
                    _moveRequest.RequestMovement(button);
                }
                //else if (!_grounded && (button == Buttons.MoveLeft || button == Buttons.MoveRight)) {
                //    if (Walking != null && _isWalking) {
                //        _isWalking = false;
                //        Walking(_isWalking);
                //    }
                //}
                //UpdateWalking(button);
            }
        }
    }

    private void OnButtonUp(Buttons button) {

        //if ((button == Buttons.MoveRight || button == Buttons.MoveLeft) && _collisionState.OnSolidGround) {
        //    if (Walking != null && _isWalking) {
        //        _isWalking = false;
        //        Walking(_isWalking);
        //    }
        //}
    }

    //// For the animator, ensure the player is not walking first to ensure we are not telling the animator
    //// to switch to itself and make sure the player is grounded. It would look funny if the walkiing animation
    //// played while the player was in mid-air.
    //private void UpdateWalking(Buttons button) {
    //    if (button == Buttons.MoveLeft || button == Buttons.MoveRight) {
    //        if (Walking != null && !_isWalking && _grounded) {
    //            _isWalking = true;
    //            Walking(_isWalking);
    //        }
    //    }
    //}
}
