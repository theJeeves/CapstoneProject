﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementManager : MonoBehaviour {

    private Rigidbody2D _body2d;
    private ControllableObject _controller;
    private PlayerCollisionState _collisionState;

    private bool _grounded = false;
    private Vector3 _values;
    private float m_time = 0.0f;

    private AudioSource _audioSource;
    private bool _isPlaying = false;

    // The key is whether or not the movement should be additive or override the current movement.
    private Queue<MovementRequest> _moveQ = new Queue<MovementRequest>();

    private void OnEnable() {
        _body2d = GetComponent<Rigidbody2D>();
        _controller = GetComponent<ControllableObject>();
        _collisionState = GetComponent<PlayerCollisionState>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update () {

        _grounded = _collisionState.OnSolidGround;

        if (_moveQ.Count == 0 && _grounded && Mathf.Abs(_body2d.velocity.x) > 0.5f) {
            _body2d.velocity = new Vector2(Mathf.SmoothStep(_body2d.velocity.x, 0.0f, TimeTools.TimeElapsed(ref m_time) / 1.00f), _body2d.velocity.y);
            _audioSource.Stop();
            _isPlaying = false;
        }
        else {
            m_time = 0.0f;
        }

        while (_moveQ.Count > 0) {
            _values = new Vector3(_body2d.velocity.x, _body2d.velocity.y, _controller.GetButtonPressTime(_moveQ.Peek().Button));

            switch (_moveQ.Peek().MovementType) {
                case MovementType.MainMenuWalking:
                case MovementType.Walking:
                    _body2d.velocity = _moveQ.Dequeue().Move(_values, _grounded, _controller.CurrentKey);
                    if (!_isPlaying) {
                        _audioSource.Play();
                        _isPlaying = true;
                    }
                    break;
                case MovementType.Shotgun:
                case MovementType.MachineGun:
                    _body2d.velocity = _moveQ.Dequeue().Move(_values, _grounded, _controller.CurrentKey);
                    break;
                case MovementType.AddForce:
                    _moveQ.Dequeue().Move(Vector3.zero, false, _controller.CurrentKey);
                    break;
            }
        }
    }

    public void Enqueue(MovementRequest request) {
        _moveQ.Enqueue(request);
    }

    //Instantly move the player (Mainly for world/enemy factors)
    public void AddImpulseForce(Vector2 force) {
        _body2d.AddForce(force, ForceMode2D.Impulse);
    }

    public void ClearQueue() {
        _moveQ.Clear();
    }
}
