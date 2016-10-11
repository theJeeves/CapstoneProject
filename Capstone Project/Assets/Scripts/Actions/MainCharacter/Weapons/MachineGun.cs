using UnityEngine;
using System.Collections;

public class MachineGun : AbstractGun {

    [SerializeField]
    protected float _xMultiplier;

    protected override void OnEnable() {
        ControllableObject.OnButton += OnButton;
    }

    protected override void OnDisable() {
        ControllableObject.OnButton -= OnButton;
    }

    private void OnButton(Buttons button) {
        if (button == Buttons.Shoot && !_collisionState.OnSolidGround) {

            _xBlowBack = _body2d.velocity.x;
            _yBlowBack = _body2d.velocity.y;

            if (_controller.AimDirection.Down) {
                _yBlowBack = _blowBack;

                if (_controller.AimDirection.Right && _xBlowBack >= _blowBack * -(_xMultiplier)) {
                    _xBlowBack -= _blowBack;
                }
                else if (_controller.AimDirection.Left && _xBlowBack <= _blowBack * (_xMultiplier)) {
                    _xBlowBack += _blowBack;
                }
                else if (_body2d.velocity.x > 0) {

                    _xBlowBack -= 1.0f;
                }
                else if (_body2d.velocity.x < 0) {
                    _xBlowBack += 1.0f;
                }
            }

            else if (_controller.AimDirection.Right) {
                _xBlowBack = -(_blowBack);
            }
            else if (_controller.AimDirection.Left) {
                _xBlowBack = _blowBack;
            }

            _body2d.velocity = new Vector2(_xBlowBack, _yBlowBack);
        }
    }
}
