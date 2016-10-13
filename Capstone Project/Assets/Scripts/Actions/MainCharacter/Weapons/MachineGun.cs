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
                if (_yBlowBack <= _blowBack && _body2d.velocity.y < 0)
                {
                    float multiplier = 1.3f*(_body2d.velocity.y / _yBlowBack);
                    //float multiplier = _yBlowBack > 0 ? Mathf.Abs(_body2d.velocity.y) / _yBlowBack : 1;
                    _yBlowBack += Mathf.Abs(_blowBack * multiplier);
                }

                if (_controller.AimDirection.Right) {
                    if (_xBlowBack >= _blowBack * -_xMultiplier)
                    {
                        _xBlowBack -= _blowBack;
                    }
                    _yBlowBack *= 0.1f;
                }
                else if (_controller.AimDirection.Left) {
                    if (_xBlowBack <= _blowBack * _xMultiplier)
                    {
                        _xBlowBack += _blowBack;
                    }
                    _yBlowBack *= 0.1f;
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
