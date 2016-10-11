using UnityEngine;
using System.Collections;

public abstract class AbstractPlayerActions : MonoBehaviour {

    protected ControllableObject _controller;
    protected Rigidbody2D _body2d;
    protected PlayerCollisionState _collisionState;

	// Use this for initialization
	protected virtual void Awake () {
        _controller = GetComponent<ControllableObject>();
        _body2d = GetComponent<Rigidbody2D>();
        _collisionState = GetComponent<PlayerCollisionState>();
	}

    protected virtual void OnEnable() {
        ControllableObject.OnButtonDown += OnButtonDown;
    }

    protected virtual void OnDisable() {
        ControllableObject.OnButtonDown -= OnButtonDown;
    }

    protected virtual void OnButtonDown(Buttons button) {
    }
}
