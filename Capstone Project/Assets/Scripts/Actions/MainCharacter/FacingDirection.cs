using UnityEngine;
using System.Collections;

public class FacingDirection : AbstractPlayerActions {

    protected override void OnEnable() {
        base.OnEnable();
        ControllableObject.OnButton += OnButton;
    }

    protected override void OnDisable() {
        base.OnDisable();
        ControllableObject.OnButton -= OnButton;
    }

    private void OnButton(Buttons button) {

        switch (button) {
            case Buttons.MoveRight:
            case Buttons.AimRight:
                _controller.Direction = Facing.Right;
                break;
            case Buttons.MoveLeft:
            case Buttons.AimLeft:
                _controller.Direction = Facing.Left;
                break;
            default:
                break;
        }

        // In 2D games, to get the characters to chagnes directions, all we have to do is
        // to change the 'x' localScale from 1 or -1. This will flip the sprite. 
        // Note: even though we are working in 2D, if we were to call Vector2, it might set the z
        // axis to 0, which would cause the sprite to disappear entirely.
        if (button == Buttons.MoveLeft || button == Buttons.MoveRight ||
            button == Buttons.AimLeft || button == Buttons.AimRight) {

            if (_controller.Direction == Facing.Right && transform.localScale.x < 0) {
                transform.localScale = new Vector3(-transform.localScale.x,
                    transform.localScale.y, transform.localScale.z);
            }
            else if (_controller.Direction == Facing.Left && transform.localScale.x > 0) {
                transform.localScale = new Vector3(-transform.localScale.x,
                    transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
