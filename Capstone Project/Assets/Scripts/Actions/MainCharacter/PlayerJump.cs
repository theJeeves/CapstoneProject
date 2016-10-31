using UnityEngine;
using System.Collections;

public class PlayerJump : AbstractPlayerActions {

    [SerializeField]
    private float _jumpSpeed;

    protected override void OnEnable() {
        base.OnEnable();

        ControllableObject.OnButtonUp += OnButtonUp;
    }

    protected override void OnDisable() {
        base.OnDisable();

        ControllableObject.OnButtonUp -= OnButtonUp;
    }

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Jump && _collisionState.OnSolidGround) {
            _body2d.velocity = new Vector2(_body2d.velocity.x, _jumpSpeed);
        }
    }

    private void OnButtonUp(Buttons button) {
        if (button == Buttons.Jump) {
            //_body2d.velocity = new Vector2(_body2d.velocity.x, _body2d.velocity.y);
        }
    }
}
