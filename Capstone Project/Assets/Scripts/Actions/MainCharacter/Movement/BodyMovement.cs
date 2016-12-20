using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName = "Movement Request/Body Movement")]
public class BodyMovement : MovementRequest {

    [SerializeField]
    private float _walkSpeed;

    protected override void OnEnable() {
        base.OnEnable();

        _type = MovementType.Walking;
    }

    public override Vector2 Move(Vector3 values, bool grounded = false, byte key = 0) {

        if (grounded) {
            if (_button == Buttons.MoveRight) {
                return new Vector2(_walkSpeed * Mathf.Clamp(values.z * 4.5f, 0, 1), values.y);
            }
            else {
                return new Vector2(-(_walkSpeed) * Mathf.Clamp(values.z * 4.5f, 0, 1), values.y);
            }
        }
        else {
            return new Vector2(values.x, values.y);
        }
    }

    public override void RequestMovement(Buttons input) {

        _button = input;
        _player.SendMessage("Enqueue", this);
    }
}
