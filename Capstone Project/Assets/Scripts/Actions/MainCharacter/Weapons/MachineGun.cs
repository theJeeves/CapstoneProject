
using UnityEngine;
using System.Collections;

public class MachineGun : AbstractGun {

    [SerializeField]
    protected float _xMultiplier;

    private float _xRecoil;
    private float _yRecoil;

    protected override void OnEnable() {
        ControllableObject.OnButton += OnButton;
    }

    protected override void OnDisable() {
        ControllableObject.OnButton -= OnButton;
    }

    private void OnButton(Buttons button) {
        if (button == Buttons.Shoot && !_collisionState.OnSolidGround) {

            _xRecoil = _body2d.velocity.x;
            _yRecoil = _body2d.velocity.y;

            if (_controller.AimDirection.Down) {
                if (_yRecoil <= _recoil && _body2d.velocity.y < 0) {
                    float multiplier = 1.3f * (_body2d.velocity.y / _yRecoil);
                    //float multiplier = _yBlowBack > 0 ? Mathf.Abs(_body2d.velocity.y) / _yBlowBack : 1;
                    _yRecoil += Mathf.Abs(_recoil * multiplier);


                    if (_controller.AimDirection.Right) {
                        if (_xRecoil >= _recoil * -_xMultiplier) {
                            _xRecoil -= _recoil;
                        }
                        //_yBlowBack *= 0.1f;
                    }
                    else if (_controller.AimDirection.Left) {
                        if (_xRecoil <= _recoil * _xMultiplier) {
                            _xRecoil += _recoil;
                        }
                        //_yBlowBack *= 0.1f;
                    }
                }
                else if (_body2d.velocity.x > 0) {

                    _xRecoil -= 1.0f;
                }
                else if (_body2d.velocity.x < 0) {
                    _xRecoil += 1.0f;
                }
            }

            else if (_controller.AimDirection.Right) {
                _xRecoil += (_recoil * -0.25f);
                //_xBlowBack = -(_blowBack);
            }
            else if (_controller.AimDirection.Left) {
                _xRecoil += (_recoil * 0.25f);
                //_xBlowBack = _blowBack;
            }

            _body2d.velocity = new Vector2(_xRecoil, _yRecoil);
        }
    }
}
//using UnityEngine;
//using System.Collections;

//public class MachineGun : AbstractGun {
//    [SerializeField]
//    private float _initalYVelocity;

//    [SerializeField]
//    private float _xMultiplier;

//    private float _xRecoil;
//    private float _yRecoil;

//    protected override void OnEnable() {
//        base.OnEnable();

//        ControllableObject.OnButton += OnButton;
//    }

//    protected override void OnDisable() {
//        base.OnDisable();

//        ControllableObject.OnButton -= OnButton;
//    }

//    protected override void OnButtonDown(Buttons button) {

//        if (button == Buttons.Shoot) {
//            //_body2d.MovePosition(_body2d.position + new Vector2(_body2d.velocity.x, 150) * Time.deltaTime);
//            _body2d.velocity = new Vector2(_body2d.velocity.x, _initalYVelocity);
//        }
//    }

//    private void OnButton(Buttons button) {

//        if (button == Buttons.Shoot) {

//            _xRecoil = _body2d.velocity.x;
//            _yRecoil = _body2d.velocity.y;

//            //AIMING DOWN 
//            if (_controller.AimDirection.Down) {
//                _yRecoil = _body2d.velocity.y > _recoil ? _body2d.velocity.y : _recoil;

//                //AIMING DOWN AND RIGHT
//                if (_controller.AimDirection.Right && _xRecoil >= _recoil * -(_xMultiplier)) {
//                    _xRecoil -= _recoil;
//                }

//                //AIMING DOWN AND LEFT
//                else if (_controller.AimDirection.Left && _xRecoil <= _recoil * (_xMultiplier)) {
//                    _xRecoil += _recoil;
//                }

//                //AIMING DOWN AND WLAKING RIGHT
//                else if (_body2d.velocity.x > 0) {

//                    _xRecoil -= 1.0f;
//                }

//                //AIMING DOWN AND WALKING LEFT
//                else if (_body2d.velocity.x < 0) {
//                    _xRecoil += 1.0f;
//                }
//            }

//            //AIMING RIGHT
//            else if (_controller.AimDirection.Right) {

//                if (_xRecoil >= _recoil) {
//                    _xRecoil -= _recoil / 2;
//                }
//            }

//            //AIMING LEFT
//            else if (_controller.AimDirection.Left) {
//                if (_xRecoil <= _recoil) {
//                    _xRecoil += _recoil / 2;
//                }
//            }

//            _body2d.velocity = new Vector2(_xRecoil, _yRecoil);
//        }
//    }
//}
