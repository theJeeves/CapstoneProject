using UnityEngine;
using System.Collections;
using SpriterDotNetUnity;
using System.Collections.Generic;
using System.Linq;

public class PlayerOnMainMenu : MonoBehaviour {

    [SerializeField]
    private MovementRequest _walkingMovementRequest;

    private ControllableObject _controller;
    private PlayerCollisionState _collisionState;
    private UnityAnimator _spriterAnimator;
    private List<string> _animationList;
    private int _currentIndex = 38;
    private int _previousIndex = -100;

    private float _timer = 0.0f;
    private bool _animating = false;

    private void OnEnable() {
        _collisionState = GetComponent<PlayerCollisionState>();
        _controller = GetComponent<ControllableObject>();
    }

    private void Update() {

        if (_spriterAnimator == null) {
            _spriterAnimator = GetComponent<SpriterDotNetBehaviour>().Animator;
            _animationList = _spriterAnimator.GetAnimations().ToList();
        }

        if (_collisionState.OnSolidGround) {

            if (Time.time - _timer > 1.0f) {

                _controller.SetButtonPressTime(Buttons.MoveRight);
                _walkingMovementRequest.RequestMovement(Buttons.MoveRight);
                if (_currentIndex != _previousIndex) {
                    Play(_currentIndex);
                    _previousIndex = _currentIndex;
                }
            }
        }
        else {
            _timer = Time.time;
        }
    }

    private void Play(int index) {
        _spriterAnimator.Play(_animationList[index]);
    }
}
