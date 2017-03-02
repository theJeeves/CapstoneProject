using UnityEngine;
using System.Collections;
using SpriterDotNetUnity;
using System.Collections.Generic;
using System.Linq;

public class PlayerAnimations : MonoBehaviour {

    private UnityAnimator _spriterAnimator;
    private List<string> _animationList;

    private ControllableObject _controller;
    private PlayerCollisionState _collisionState;

    private int _currentIndex;
    private int _previousIndex = -100;
    private Shotgun _shotgunGO;
    private bool _grounded;

    private void OnEnable() {

        _controller = GetComponent<ControllableObject>();
        _collisionState = GetComponent<PlayerCollisionState>();

        _shotgunGO = GetComponentInChildren<Shotgun>();
    }

    private void Update() {

        if (_spriterAnimator == null) {
            _spriterAnimator = GetComponent<SpriterDotNetBehaviour>().Animator;
            _animationList = _spriterAnimator.GetAnimations().ToList();
        }

        _grounded = _collisionState.OnSolidGround;

        _currentIndex = _shotgunGO.isActiveAndEnabled ? 0 : 23;         // Check if the shotgun or the machine gun is active
        _currentIndex += !_grounded ? 0 : 6;        // Check if the player is falling or is on the ground.

        // If the player is on the ground, check if the player is standing still or running.
        if (_currentIndex == 6 || _currentIndex == 29) {
            _currentIndex += _controller.GetButtonPress(Buttons.MoveLeft) || _controller.GetButtonPress(Buttons.MoveRight) ? 6 : 0;
        }

        if (_controller.GetButtonPress(Buttons.AimDown) || _controller.GetButtonPress(Buttons.AimLeft) || 
            _controller.GetButtonPress(Buttons.AimRight) || _controller.GetButtonPress(Buttons.AimUp) || _controller.GetButtonPress(Buttons.Shoot)) {

            switch (_controller.AimDirection) {
                case 0:
                case 180:
                    _currentIndex += 3; break;
                case 45:
                case 135:
                    _currentIndex += 2; break;
                case 90:
                    _currentIndex += 1; break;
                case 270:
                    _currentIndex += 5; break;
                case 225:
                case 315:
                    _currentIndex += 4; break;
                default:
                    _currentIndex += 3; break;
            }
        }

        if (_grounded) {
            if (_controller.GetButtonPress(Buttons.MoveLeft) && _controller.GetButtonPress(Buttons.AimRight) ||
                _controller.GetButtonPress(Buttons.MoveRight) && _controller.GetButtonPress(Buttons.AimLeft)) {
                _currentIndex += 5;
            }
        }

        if (_currentIndex != _previousIndex) {
            Play(_currentIndex);
            _previousIndex = _currentIndex;
        }
    }

    private void Play(int index) {
        _spriterAnimator.Play(_animationList[index]);
    }
}
