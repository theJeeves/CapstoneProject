using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementManager : MonoBehaviour {

    private Rigidbody2D _body2d;
    private ControllableObject _controller;
    private PlayerCollisionState _collisionState;

    private bool _grounded = false;

    // The key is whether or not the movement should be additive or override the current movement.
    private Queue<MovementRequest> _moveQ = new Queue<MovementRequest>();

    private void OnEnable() {
        _body2d = GetComponent<Rigidbody2D>();
        _controller = GetComponent<ControllableObject>();
        _collisionState = GetComponent<PlayerCollisionState>();
    }

    // Update is called once per frame
    private void Update () {
        if (_moveQ.Count > 0) {

            if (!_moveQ.Peek().SpecialRequest) {
                _grounded = _collisionState.OnSolidGround;
                Vector3 values = new Vector3(_body2d.velocity.x, _body2d.velocity.y, _controller.GetButtonPressTime(_moveQ.Peek().Button));

                _body2d.velocity = _moveQ.Dequeue().Move(values, _grounded, _controller.CurrentKey);
            }
            else {
                _moveQ.Dequeue().Move(Vector3.zero, false, _controller.CurrentKey);
            }
        }
	}

    public void Enqueue(MovementRequest request) {

        _moveQ.Enqueue(request);
    }

    //Skip the queue and instantly move the player (Mainly for world/enemy factors)
    public void AddImpulseForce(Vector2 force) {
        _body2d.AddForce(force, ForceMode2D.Impulse);
    }
}
