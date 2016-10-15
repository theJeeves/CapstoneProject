using UnityEngine;
using System.Collections;

public class PlayerJump : AbstractPlayerActions {

    [SerializeField]
    private float _jumpHeight;
    [SerializeField]
    private float _jumpSpeed;

    protected override void OnEnable() {
        ControllableObject.OnButton += OnButtonDown;
    }

    protected override void OnDisable() {
        ControllableObject.OnButton -= OnButtonDown;
    }
    protected override void OnButtonDown(Buttons button) {
        //if (button == Buttons.Jump && _collisionState.OnSolidGround) {
        //    _body2d.velocity = new Vector2(_body2d.velocity.x, _jumpSpeed);
        //}
        while (button == Buttons.Jump) {
            if (button == Buttons.Jump && _collisionState.OnSolidGround) {
                transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(0, _jumpHeight, 0), _jumpSpeed * Time.deltaTime);
            }
        }
    }
}
