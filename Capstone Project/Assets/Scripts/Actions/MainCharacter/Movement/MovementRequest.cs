using UnityEngine;
using System.Collections;

public abstract class MovementRequest : ScriptableObject {

    protected bool _specialRequest = false;
    public bool SpecialRequest {
        get { return _specialRequest; }
    }

    protected Buttons _button;
    public Buttons Button {
        get { return _button; }
    }

    protected GameObject _player;

    protected virtual void OnEnable() {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public abstract Vector2 Move(Vector3 values, bool grounded = false, byte key = 0);

    public virtual void RequestMovement(Buttons button) { }

    public virtual void RequestMovement() { }
}
