using UnityEngine;
using System.Collections;

public class PlayerWalking : AbstractPlayerActions {

    //[SerializeField]
    //private float _xVelocity = 75.0f;

    [SerializeField]
    private float _walkSpeed;

    protected override void OnEnable() {
        //base.OnEnable();

        ControllableObject.OnButton += OnButton;
        ControllableObject.OnButtonUp += OnButtonUp;
    }

    protected override void OnDisable() {
        //base.OnDisable();

        ControllableObject.OnButton -= OnButton;
        ControllableObject.OnButtonUp -= OnButtonUp;
    }

    private void OnButton(Buttons button) {

        //if (button == Buttons.MoveRight || button == Buttons.MoveLeft) {
        //    _body2d.velocity = new Vector2(_xVelocity * (float)_controller.Direction, _body2d.velocity.y);
        //}

        if (button == Buttons.MoveRight && _collisionState.OnSolidGround) {
            //transform.Translate(Vector2.right * _walkSpeed * Time.deltaTime * 5);
            _body2d.velocity = new Vector2(_walkSpeed * Mathf.Clamp(_controller.GetButtonPressTime(button) * 4.5f, 0, 1), _body2d.velocity.y);
        }
        else if (button == Buttons.MoveLeft && _collisionState.OnSolidGround) {
            //transform.Translate(Vector2.left * -_walkSpeed * Time.deltaTime * 5);
            _body2d.velocity = new Vector2(-(_walkSpeed) * Mathf.Clamp(_controller.GetButtonPressTime(button) * 4.5f, 0, 1), _body2d.velocity.y);
        }
    }

    private void OnButtonUp(Buttons button) {

        if ((button == Buttons.MoveRight || button == Buttons.MoveLeft) && _collisionState.OnSolidGround) {
            _body2d.velocity = new Vector2(0, _body2d.velocity.y);
        }
    }
}
