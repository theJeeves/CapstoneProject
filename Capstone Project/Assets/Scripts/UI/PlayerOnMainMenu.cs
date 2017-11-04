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

    private float m_time = 0.0f;
    private float m_defaultTime = 1.0f;

    private void OnEnable() {
        _collisionState = GetComponent<PlayerCollisionState>();
        _controller = GetComponent<ControllableObject>();

        // START THE GAME MANAGER, WINDOW MANAGER, AND THE EVENT SYSTEM
        // They are not used in this script, but they started for the reset of the game.
        GameManager GM = GameManager.Instance.GetComponent<GameManager>() ;
        WindowManager WM = WindowManager.Instance.GetComponent<WindowManager>();
        EventSystemSingleton ESS = EventSystemSingleton.Instance.GetComponent<EventSystemSingleton>();
    }

    private void Update() {

        if (_spriterAnimator == null) {
            _spriterAnimator = GetComponent<SpriterDotNetBehaviour>().Animator;
            _animationList = _spriterAnimator.GetAnimations().ToList();
        }

        if (_collisionState.OnSolidGround) {

            if (TimeTools.TimeExpired(ref m_time)) {

                _controller.SetButtonPressTime(Buttons.MoveRight);
                _walkingMovementRequest.RequestMovement(Buttons.MoveRight);
                if (_currentIndex != _previousIndex) {
                    Play(_currentIndex);
                    _previousIndex = _currentIndex;
                }
            }
        }
        else {
            m_time = m_defaultTime;
        }
    }

    private void Play(int index) {
        _spriterAnimator.Play(_animationList[index]);
    }
}
