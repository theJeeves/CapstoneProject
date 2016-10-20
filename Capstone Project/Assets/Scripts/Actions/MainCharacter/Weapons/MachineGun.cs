using UnityEngine;
using System.Collections;

public class MachineGun : AbstractGun {
    [SerializeField]
    private float _initalYVelocity;

    [SerializeField]
    private float _xMultiplier;

    private float _xRecoil;
    private float _yRecoil;

    protected override void OnEnable() {
        base.OnEnable();

        ControllableObject.OnButton += OnButton;
    }

    protected override void OnDisable() {
        base.OnDisable();

        ControllableObject.OnButton -= OnButton;
    }

    protected override void OnButtonDown(Buttons button) {

        if (button == Buttons.Shoot) {
            //_body2d.MovePosition(_body2d.position + new Vector2(_body2d.velocity.x, 150) * Time.deltaTime);
            _body2d.velocity = new Vector2(_body2d.velocity.x, _initalYVelocity);
        }
    }

    private void OnButton(Buttons button) {

        if (button == Buttons.Shoot) {

            _xRecoil = _body2d.velocity.x;
            _yRecoil = _body2d.velocity.y;

            if (_controller.AimDirection.Down) {
                _yRecoil = _body2d.velocity.y > _recoil ? _body2d.velocity.y : _recoil;

                if (_controller.AimDirection.Right && _xRecoil >= _recoil * -(_xMultiplier)) {
                    _xRecoil -= _recoil;
                }
                else if (_controller.AimDirection.Left && _xRecoil <= _recoil * (_xMultiplier)) {
                    _xRecoil += _recoil;
                }
                else if (_body2d.velocity.x > 0) {

                    _xRecoil -= 1.0f;
                }
                else if (_body2d.velocity.x < 0) {
                    _xRecoil += 1.0f;
                }
            }

            else if (_controller.AimDirection.Right) {
                _xRecoil = -(_recoil);
            }
            else if (_controller.AimDirection.Left) {
                _xRecoil = _recoil;
            }

            _body2d.velocity = new Vector2(_xRecoil, _yRecoil);
        }
    }
}
