using UnityEngine;
using System.Collections;

public class PlayerWalking : AbstractPlayerActions {

    [SerializeField]
    private float _xVelocity = 75.0f;

    protected override void OnEnable() {
        ControllableObject.OnButton += OnButton;
        ControllableObject.OnButtonUp += OnButtonUp;
    }

    protected override void OnDisable() {
        ControllableObject.OnButton -= OnButton;
        ControllableObject.OnButtonUp -= OnButtonUp;
    }

    private void OnButton(Buttons button) {

        if ((button == Buttons.MoveRight || button == Buttons.MoveLeft) && _collisionState.OnSolidGround) {
            _body2d.velocity = new Vector2(_xVelocity * (float)_controller.Direction, _body2d.velocity.y);
        }
    }

    private void OnButtonUp(Buttons button) {

        if ((button == Buttons.MoveRight || button == Buttons.MoveLeft) && _collisionState.OnSolidGround) {
            _body2d.velocity = new Vector2(0, _body2d.velocity.y);
        }
    }
}
