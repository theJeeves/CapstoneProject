using UnityEngine;
using System.Collections;

public class FacingDirection : AbstractPlayerActions {

    protected override void OnButtonDown(Buttons button) {

        switch (button) {
            case Buttons.MoveRight:
                _controller.Direction = Facing.Right;
                break;
            case Buttons.MoveLeft:
                _controller.Direction = Facing.Left;
                break;
            default:
                break;
        }

        // In 2D games, to get the characters to chagnes directions, all we have to do is
        // to change the 'x' localScale from 1 or -1. This will flip the sprite. 
        // Note: even though we are working in 2D, if we were to call Vector2, it might set the z
        // axis to 0, which would cause the sprite to disappear entirely.
        transform.localScale = new Vector3((float)_controller.Direction, 1, 1);
    }
}
