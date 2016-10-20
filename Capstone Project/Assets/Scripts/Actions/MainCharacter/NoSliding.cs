using UnityEngine;
using System.Collections;

public class NoSliding : AbstractPlayerActions {

    protected override void OnEnable() {
        PlayerCollisionState.OnHitGround += OnHitGround;
    }

    protected override void OnDisable() {
        PlayerCollisionState.OnHitGround -= OnHitGround;
    }

    private void OnHitGround() {

        if (!_controller.GetButtonPress(Buttons.MoveLeft) &&
            !_controller.GetButtonPress(Buttons.MoveRight) &&
            !_controller.GetButtonPress(Buttons.Shoot)  ) {

            _body2d.velocity = new Vector2(0, _body2d.velocity.y);
        }
    }
}
