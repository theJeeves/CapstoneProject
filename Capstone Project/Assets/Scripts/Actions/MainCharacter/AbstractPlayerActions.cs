using UnityEngine;
using System.Collections;

public abstract class AbstractPlayerActions : MonoBehaviour {

    protected ControllableObject _controller;
    protected Rigidbody2D _body2d;

	// Use this for initialization
	void Awake () {
        _controller = GetComponent<ControllableObject>();
        _body2d = GetComponent<Rigidbody2D>();
	}
}
