using UnityEngine;
using System.Collections;

public class GunTest : AbstractPlayerActions {

    [SerializeField]
    private float _blowBack;

    private float _xBlowBack;
    private float _yBlowBack;


	protected override void OnEnable() {
        ControllableObject.OnButtonDown += OnButton;
    }

    protected override void OnDisable() {
        ControllableObject.OnButtonDown -= OnButton;
    }

    private void OnButton(Buttons button) {

        if (button == Buttons.Shoot && !_collisionState.OnSolidGround) {

            _xBlowBack = _body2d.velocity.x;
            _yBlowBack = _body2d.velocity.y;

            if (_controller._down) {
                _xBlowBack = _controller._right ? _blowBack * -1.0f : _body2d.velocity.x;
                _xBlowBack = _controller._left ? _blowBack : _xBlowBack;
                _yBlowBack = _controller._right ? _blowBack *  0.75f : _blowBack;
                _yBlowBack = _controller._left ? _blowBack * 0.75f : _yBlowBack;
            }
            else if (_controller._up) {
                _xBlowBack = _body2d.velocity.x;
                _yBlowBack = _blowBack * -1.0f;
            }
            else if (_controller._right) {
                _xBlowBack = _blowBack * -0.5f;
                _yBlowBack = _body2d.velocity.y;
            }
            else if (_controller._left) {
                _xBlowBack = _blowBack * 0.5f;
                _yBlowBack = _body2d.velocity.y;
            }

            _body2d.velocity = new Vector2(_xBlowBack, _yBlowBack);
        }
    }
}
