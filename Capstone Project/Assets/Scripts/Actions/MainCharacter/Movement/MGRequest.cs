using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Movement Request/Machine Gun")]
public class MGRequest : MovementRequest {

    private float _xVel;
    private float _yVel;

    [SerializeField]
    private float _recoil;
    [SerializeField]
    private float _setVel;
    [SerializeField]
    private float _addVel;
    [SerializeField]
    private float _xMultiplier;

    protected System.Action<float, float>[] _gunActions = new System.Action<float, float>[8];

    protected override void OnEnable() {
        base.OnEnable();

        _gunActions[0] = AimRight;
        _gunActions[1] = AimUp;
        _gunActions[2] = AimUp;
        _gunActions[3] = AimUp;
        _gunActions[4] = AimLeft;
        _gunActions[5] = AimDownAndLeft;
        _gunActions[6] = AimDown;
        _gunActions[7] = AimDownAndRight;
    }

    public override Vector2 Move(Vector3 values, bool grounded = false, byte key = 0) {

        _xVel = values.x;
        _yVel = values.y;

        _gunActions[key].Invoke(values.x, values.y);

        return new Vector2(_xVel, _yVel);
    }

    public override void RequestMovement() {
        _player.SendMessage("Enqueue", this);
    }

    private void AimRight(float bodyXvel, float bodyYvel) {

        _xVel += (_recoil * -0.25f);
        _yVel = bodyYvel;
    }

    private void AimUp(float bodyXvel, float bodyYvel) {
        _xVel = bodyXvel;
        _yVel = bodyYvel;
    }

    private void AimLeft(float bodyXvel, float bodyYvel) {

        _xVel += (_recoil * 0.25f);
        _yVel = bodyYvel;
    }

    private void AimDownAndLeft(float bodyXvel, float bodyYvel) {

        //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
        if (_yVel <= _recoil && bodyYvel < 0) {

            //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
            _yVel += Mathf.Abs(_recoil * (1.3f * (bodyYvel / _yVel)));

            if (_xVel <= _recoil * _xMultiplier) {
                _xVel += _recoil;
            }
        }
    }

    private void AimDown(float bodyXvel, float bodyYvel) {

        //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
        if (_yVel <= _recoil && bodyYvel < 0) {

            //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
            _yVel += Mathf.Abs(_recoil * (1.3f * (bodyYvel / _yVel)));
        }

        //AIMING STRIGHT DOWN AND MOVING RIGHT
        else if (bodyXvel > 0) {
            _xVel -= 2.0f;
        }

        //AIMING STRIGHT DOWN AND MOVING LEFT
        else if (bodyXvel < 0) {
            _xVel += 2.0f;
        }
    }

    private void AimDownAndRight(float bodyXvel, float bodyYvel) {
        //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
        if (_yVel <= _recoil && bodyYvel < 0) {

            //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
            _yVel += Mathf.Abs(_recoil * (1.3f * (bodyYvel / _yVel)));

            if (_xVel >= _recoil * -_xMultiplier) {
                _xVel -= _recoil;
            }
        }
    }
}
