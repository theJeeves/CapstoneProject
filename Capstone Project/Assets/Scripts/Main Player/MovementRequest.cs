using UnityEngine;
using System.Collections;

public enum MovementType {
    None,
    Walking,
    Shotgun,
    MachineGun,
    AddForce
}

[CreateAssetMenu(menuName ="Movement Request/New Movement")]
public class MovementRequest : ScriptableObject {

    private GameObject _player;
    private Buttons _button;
    public Buttons Button {
        get { return _button; }
    }

    public MovementType _type = MovementType.None;
    public MovementType MovementType {
        get { return _type; }
    }

    [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _recoil;
    [SerializeField]
    private float _setVel;
    [SerializeField]
    private float _addVel;
    [SerializeField]
    private float _xMultiplier;
    [SerializeField]
    private float _xImpulse;
    [SerializeField]
    private float _yImpulse;

    private float _xVel;
    private float _yVel;
    private bool _grounded;
    private Vector2 _forceRequest = new Vector2(0.0f, 0.0f);

    private System.Action<float, float>[] _gunActions = new System.Action<float, float>[8];

    protected virtual void OnEnable() {
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_type == MovementType.Shotgun || _type == MovementType.MachineGun) {
            _gunActions[0] = AimRight;
            _gunActions[1] = AimUpAndRight;
            _gunActions[2] = AimUp;
            _gunActions[3] = AimUpAndLeft;
            _gunActions[4] = AimLeft;
            _gunActions[5] = AimDownAndLeft;
            _gunActions[6] = AimDown;
            _gunActions[7] = AimDownAndRight;
        }
    }

    public Vector2 Move(Vector3 values, bool grounded = false, int key = 0) {

        if (_player == null) {
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        switch (_type) {
            case MovementType.Walking:

                if (grounded) {
                    if (_button == Buttons.MoveRight) {

                        return new Vector2(Mathf.Clamp( values.x + (_walkSpeed * Mathf.Clamp(values.z * 2.0f , 0, 1)), 0.0f, _walkSpeed), values.y);
                    }
                    else {
                        return new Vector2(Mathf.Clamp(values.x - (_walkSpeed * Mathf.Clamp(values.z * 2.0f, 0, 1)), -_walkSpeed, 0.0f), values.y);
                    }
                }
                else {
                    return new Vector2(values.x, values.y);
                }

            case MovementType.Shotgun:
                _grounded = grounded;
                _gunActions[key].Invoke(values.x, values.y);

                return new Vector2(_xVel, _yVel);

            case MovementType.MachineGun:
                _xVel = values.x;
                _yVel = values.y;

                _gunActions[key].Invoke(values.x, values.y);

                return new Vector2(_xVel, _yVel);


            case MovementType.AddForce:
                if (key == 5 || key == 6 || key == 7) {

                    if (key == 5) { ImpulseDownAndLeft(values.x); }
                    else if (key == 6) { ImpulseDown(); }
                    else if (key == 7) { ImpulseDownAndRight(values.x); }

                    _player.SendMessage("AddImpulseForce", _forceRequest);
                }
                return new Vector2(0.0f, 0.0f);

            default:
                return new Vector2(values.x, values.y);
        }
    }

    public virtual void RequestMovement(Buttons button) {
        _button = button;
        _player.SendMessage("Enqueue", this);
    }

    public virtual void RequestMovement() {
        _player.SendMessage("Enqueue", this);
    }

    private void AimRight(float bodyXvel, float bodyYvel) {

        switch (_type) {
            case MovementType.Shotgun:
                _xVel = !_grounded ? -_recoil : -_recoil * 0.5f;

                // MOVING LEFT OR MOVING RIGHT OR STANDING STILL
                _yVel = bodyYvel;
                break;

            case MovementType.MachineGun:
                if (_xVel >= -_recoil) {
                    _xVel -= _recoil / 2.0f;
                }

                _yVel = bodyYvel;
                break;
        }
    }

    private void AimUpAndRight(float bodyXvel, float bodyYvel) {

        switch (_type) {

            case MovementType.Shotgun:
                //FALLING
                if (bodyYvel < 0) {
                    _yVel = bodyYvel + _recoil * -(_addVel);
                }

                //MOVING LEFT
                if (bodyXvel < 0) {

                    _xVel = bodyXvel + _recoil * -(_addVel);

                    //RISING
                    if (bodyYvel >= 0) {
                        _yVel = _recoil * -(_addVel);
                    }
                }

                //MOVING RIGHT OR NO X VELOCITY
                else if (bodyXvel >= 0) {

                    _xVel = _recoil * -(_setVel);

                    //RISING
                    if (bodyYvel >= 0) {
                        _yVel = _recoil * -(_setVel);
                    }
                }
                break;
        }
    }

    private void AimUp(float bodyXvel, float bodyYvel) {

        switch (_type) {

            case MovementType.Shotgun:
                _xVel = bodyXvel;

                //FALLING
                if (bodyYvel < 0) {
                    _yVel = bodyYvel + _recoil * -(_setVel);
                }

                //RISING OR STILL
                else if (bodyYvel >= 0) {
                    _yVel = -(_recoil);
                }
                break;
        }
    }

    private void AimUpAndLeft(float bodyXvel, float bodyYvel) {

        switch (_type) {

            case MovementType.Shotgun:
                //FALLING
                if (bodyYvel < 0) {
                    _yVel = bodyYvel + _recoil * -(_addVel);
                }

                //MOVING LEFT
                if (bodyXvel <= 0) {

                    _xVel = _recoil * (_setVel);

                    //RISING
                    if (bodyYvel >= 0) {
                        _yVel = _recoil * -(_setVel);
                    }
                }

                //MOVING RIGHT
                else if (bodyXvel > 0) {

                    _xVel = bodyXvel + _recoil * _addVel;

                    //RISING
                    if (bodyYvel >= 0) {
                        _yVel = _recoil * -(_addVel);
                    }
                }
                break;
        }
    }

    private void AimLeft(float bodyXvel, float bodyYvel) {

        //SHOTGUN
        if (_type == MovementType.Shotgun) {
            _xVel = !_grounded ? _recoil : _recoil * 0.5f;

            //MOVING RIGHT OR MOVING LEFT OR STANDING STILL
            _yVel = bodyYvel;
        }
        //MACHINEGUN
        else if (_type == MovementType.MachineGun) {

            if (_xVel <= _recoil) {
                _xVel += _recoil / 2.0f;
            }
            _yVel = bodyYvel;
        }
    }

    private void AimDownAndLeft(float bodyXvel, float bodyYvel) {

        switch (_type) {

            case MovementType.Shotgun:
                //ON THE GROUND
                if (_grounded) {

                    _yVel = _recoil * _setVel;

                    //MOVING RIGHT
                    if (bodyXvel > 0) {
                        _xVel = Mathf.Clamp(bodyXvel + _recoil * _addVel, _recoil * _addVel, _recoil);
                    }

                    // MOVING LEFT OR STANDING STILL
                    else if (bodyXvel <= 0) {
                        _xVel = _recoil * _setVel;
                    }
                }

                //IN THE AIR
                else if (!_grounded) {

                    //MOVING RIGHT
                    if (bodyXvel > 0.0f) {

                        _xVel = Mathf.Clamp(bodyXvel + _recoil * _addVel, _recoil * _addVel, _recoil);

                        //FALLING (NEGATIVE Y VELOCITY)
                        if (bodyYvel < 0.0f) {
                            _yVel = _recoil * _setVel;
                        }

                        //RISING OR ZERO Y VELOCITY
                        else if (bodyYvel >= 0.0f) {
                            _yVel = _recoil * _setVel;
                        }
                    }

                    // MOVING LEFT OR STANDING STILL
                    else if (bodyXvel <= 0.0f) {
                        _xVel = _recoil * _setVel;
                        _yVel = _recoil * _setVel;
                    }
                }
                break;

            case MovementType.MachineGun:
                //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
                if (_yVel <= _recoil && bodyYvel < 0) {

                    //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
                    _yVel += Mathf.Abs(_recoil * (1.3f * (bodyYvel / _yVel)));

                    if (_xVel <= _recoil * _xMultiplier) {
                        _xVel += _recoil;
                    }
                }
                break;
        }
    }

    private void AimDown(float bodyXvel, float bodyYvel) {

        switch (_type) {

            case MovementType.Shotgun:
                _yVel = _recoil;

                //FALLING (NEGATVIE Y VELOCITY) OR RISING OR ZERO Y VELOCITY
                _xVel = bodyXvel;
                break;

            case MovementType.MachineGun:
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
                break;
        }
    }

    private void AimDownAndRight(float bodyXvel, float bodyYvel) {

        switch (_type) {

            //SHOTGUN
            case MovementType.Shotgun:
                //ON THE GROUND
                if (_grounded) {

                    _yVel = _recoil * _setVel;

                    // MOVING LEFT
                    if (bodyXvel < 0) {
                        _xVel = -1.0f * Mathf.Clamp(Mathf.Abs(bodyXvel) + _recoil * _addVel, _recoil * _addVel, _recoil);
                    }

                    //MOVING RIGHT OR STANDING STILL
                    else if (bodyXvel >= 0) {
                        _xVel = _recoil * -_setVel;
                    }
                }

                //IN THE AIR
                else if (!_grounded) {

                    //MOVING LEFT
                    if (bodyXvel < 0) {

                        _xVel = -1.0f * Mathf.Clamp(Mathf.Abs(bodyXvel) + _recoil * _addVel, _recoil * _addVel, _recoil);

                        //FALLING (NEGATIVE Y VELOCITY)
                        if (bodyYvel < 0) {
                            _yVel = _recoil * _setVel;
                        }

                        //RISING OR ZERO Y VELOCITY
                        else if (bodyYvel >= 0) {
                            _yVel = Mathf.Clamp(bodyYvel, _recoil * _addVel, _recoil * 2);
                        }
                    }

                    //MOVING RIGHT OR STANDING STILL
                    else if (bodyXvel >= 0) {
                        _xVel = _recoil * -_setVel;
                        _yVel = _recoil * _setVel;
                    }
                }
                break;

            //MACHINE GUN
            case MovementType.MachineGun:
                //FALLING DOWN AND THE Y-VELOCITY IS LESS THAN THE SET RECOIL
                if (_yVel <= _recoil && bodyYvel < 0) {

                    //THIS QUICKLY SLOWS DOWN THE PLAYER FROM FALLING (IRON MAN EFFECT)
                    _yVel += Mathf.Abs(_recoil * (1.3f * (bodyYvel / _yVel)));

                    if (_xVel >= _recoil * -_xMultiplier) {
                        _xVel -= _recoil;
                    }
                }
                break;
        }
    }


    //THE NEXT THREE FUCNTIONS ARE FOR IMPULSE TYPES ONLY
    private void ImpulseDownAndLeft(float xVel) {
        // CHANGE THE FORCE DEPENDING ON IF THE PLAYER IS MOVING IN THE XDIRECTION OR NOT
        _forceRequest = xVel > -0.5f || xVel < 0.5f ? new Vector2(_xImpulse, _yImpulse) : new Vector2(0, _yImpulse);
    }

    private void ImpulseDown() {
        // CHANGE THE FORCE DEPENDING ON IF THE PLAYER IS MOVING IN THE XDIRECTION OR NOT
        _forceRequest = new Vector2(0, _yImpulse + 2500.0f);
    }

    private void ImpulseDownAndRight(float xVel) {
        // CHANGE THE FORCE DEPENDING ON IF THE PLAYER IS MOVING IN THE XDIRECTION OR NOT
        _forceRequest = xVel > -0.5f || xVel < 0.5f ? new Vector2(-_xImpulse, _yImpulse) : new Vector2(0, _yImpulse);
    }
}
