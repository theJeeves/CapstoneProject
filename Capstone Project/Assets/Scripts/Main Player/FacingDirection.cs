using UnityEngine;
using System.Collections;

public class FacingDirection : MonoBehaviour {

    private ControllableObject _controller;

    private void OnEnable() {
        ControllableObject.OnButton += OnButton;

        _controller = GetComponent<ControllableObject>();
    }

    private void OnDisable() {
        ControllableObject.OnButton -= OnButton;
    }

    private void OnButton(Buttons button) {

        switch (button) {
            case Buttons.MoveRight:
            case Buttons.AimRight:
                _controller.FacingDirection = Facing.Right;
                break;
            case Buttons.MoveLeft:
            case Buttons.AimLeft:
                _controller.FacingDirection = Facing.Left;
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

            if (_controller.FacingDirection == Facing.Right && transform.localScale.x < 0) {
                transform.localScale = new Vector3(-transform.localScale.x,
                    transform.localScale.y, transform.localScale.z);
            }
            else if (_controller.FacingDirection == Facing.Left && transform.localScale.x > 0) {
                transform.localScale = new Vector3(-transform.localScale.x,
                    transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
