using UnityEngine;
using System.Collections;

public abstract class AbstractGun : AbstractPlayerActions {

    [SerializeField]
    protected float _blowBack;
    [SerializeField]
    protected float _XYRatio;

    protected float _xBlowBack;
    protected float _yBlowBack;

    protected override void OnButtonDown(Buttons button) {

        _xBlowBack = _body2d.velocity.x;
        _yBlowBack = _body2d.velocity.y;

        if (_controller.AimDirection.Down) {
            _xBlowBack = _controller.AimDirection.Right ? _blowBack * -1.0f : _body2d.velocity.x;
            _xBlowBack = _controller.AimDirection.Left ? _blowBack : _xBlowBack;
            _yBlowBack = _controller.AimDirection.Right ? _blowBack * _XYRatio : _blowBack;
            _yBlowBack = _controller.AimDirection.Left ? _blowBack * _XYRatio : _yBlowBack;
        }

        else if (_controller.AimDirection.Right) {
            _xBlowBack = _blowBack * -1.0f;
            _yBlowBack = _body2d.velocity.y;
        }
        else if (_controller.AimDirection.Left) {
            _xBlowBack = _blowBack;
            _yBlowBack = _body2d.velocity.y;
        }

        _body2d.velocity = new Vector2(_xBlowBack, _yBlowBack);
    }
}
