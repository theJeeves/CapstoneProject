using UnityEngine;
using System.Collections;

public class InAirMovement : AbstractPlayerActions {

    [SerializeField]
    private float _movementSpeed;

    protected override void OnEnable() {
        base.OnEnable();

        ControllableObject.OnButton += OnButton;
    }

    protected override void OnDisable() {
        base.OnDisable();

        ControllableObject.OnButton += OnButton;
    }

    private void OnButton(Buttons button) {

        if (!_collisionState.OnSolidGround) {

            // if the player was moving left and switched directions in air
            if (button == Buttons.MoveRight && _body2d.velocity.x <= 0) {
                //_body2d.velocity = new Vector2(_movementSpeed, _body2d.velocity.y);
                _body2d.velocity += new Vector2(_movementSpeed, 0);
            }
            // if the player was moving right and switched directions in air
            else if (button == Buttons.MoveLeft && _body2d.velocity.x >= 0) {
                //_body2d.velocity += new Vector2(-(_movementSpeed), _body2d.velocity.y);
                _body2d.velocity += new Vector2(-(_movementSpeed), 0);
            }
        }
    }
}
