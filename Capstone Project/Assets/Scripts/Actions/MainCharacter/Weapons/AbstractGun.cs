using UnityEngine;
using System.Collections;

public abstract class AbstractGun : AbstractPlayerActions {

    [SerializeField]
    protected float _blowBack;
    [SerializeField]
    protected float _XYRatio;

    protected float _xBlowBack = 0.0f;
    protected float _yBlowBack = 0.0f;

    protected override void Awake() {
        _controller = GetComponentInParent<ControllableObject>();
        _body2d = GetComponentInParent<Rigidbody2D>();
        _collisionState = GetComponentInParent<PlayerCollisionState>();
    }

    protected override void OnButtonDown(Buttons button) {

        _xBlowBack = _body2d.velocity.x;
        _yBlowBack = _body2d.velocity.y;

        if (_controller.AimDirection.Down) {
            _yBlowBack = _blowBack;

            if (_controller.AimDirection.Right || _controller.AimDirection.Left) {
                _xBlowBack = _controller.AimDirection.Left ? _blowBack : - (_blowBack);
                _xBlowBack *= _XYRatio;
            }
        }

        else if (_controller.AimDirection.Right) {
            _xBlowBack = _blowBack * -1.0f;
        }
        else if (_controller.AimDirection.Left) {
            _xBlowBack = _blowBack;
        }

        _body2d.velocity = new Vector2(_xBlowBack, _yBlowBack);
    }
}
