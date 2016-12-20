using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName ="Movement Request/Machine Gun Initial")]
public class MGInitialRequest : MovementRequest {

    protected System.Action<float>[] _gunActions = new System.Action<float>[3];

    private Vector2 _forceRequest = new Vector2(0.0f, 0.0f);
    private byte _key;

    protected override void OnEnable() {
        base.OnEnable();

        _gunActions[0] = AimDownAndLeft;
        _gunActions[1] = AimDown;
        _gunActions[2] = AimDownAndRight;

        _type = MovementType.AddForce;
    }

    public override Vector2 Move(Vector3 values, bool grounded = false, byte key = 0) {

        switch (key) {
            case 5:
                _key = 0; break;
            case 6:
                _key = 1; break;
            case 7:
                _key = 2; break;
            default:
                return new Vector2(0.0f, 0.0f);
        }

        _gunActions[_key].Invoke(values.x);
        _player.SendMessage("AddImpulseForce", _forceRequest);

        return new Vector2(0.0f, 0.0f);
    }

    public override void RequestMovement() {
        _player.SendMessage("Enqueue", this);
    }

    private void AimDownAndLeft(float xVel) {
        // CHANGE THE FORCE DEPENDING ON IF THE PLAYER IS MOVING IN THE XDIRECTION OR NOT
        _forceRequest = xVel > -0.5f || xVel < 0.5f ? new Vector2(5000, 7500) : new Vector2(0, 7500);
    }

    private void AimDown(float ignore) {
        // CHANGE THE FORCE DEPENDING ON IF THE PLAYER IS MOVING IN THE XDIRECTION OR NOT
        _forceRequest = new Vector2(0, 10000);
    }

    private void AimDownAndRight(float xVel) {
        // CHANGE THE FORCE DEPENDING ON IF THE PLAYER IS MOVING IN THE XDIRECTION OR NOT
        _forceRequest = xVel > -0.5f || xVel < 0.5f ? new Vector2(-5000, 7500) : new Vector2(0, 7500);
    }
}
