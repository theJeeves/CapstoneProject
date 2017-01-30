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
    private Rigidbody2D _body2D;

    private int _currentIndex;
    private int _previousIndex = -100;
    private Shotgun _shotgunGO;
    private bool _grounded;

    private void OnEnable() {

        _controller = GetComponent<ControllableObject>();
        _collisionState = GetComponent<PlayerCollisionState>();
        _body2D = GetComponent<Rigidbody2D>();

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
        _spriterAnimator.Play(_animationList[(int)index]);
    }
}

//public enum AnimationParameters {
//    None,
//    Walking,
//    AimDirection,
//    Grounded,
//    Damaged
//}

//[CreateAssetMenu(menuName =("SO Animatoins/New SO Animation"))]
//public class SOAnimations : ScriptableObject {

//    [SerializeField]
//    private AnimationParameters _animationParameter;

//    private Animator[] _animators;
//    private SpriteRenderer[] _sprites;
//    private int _numOfSprites = 0;


//    private void OnEnable() {

//        switch (_animationParameter) {
//            case AnimationParameters.Walking:
//                _animators = new Animator[2];
//                _animators[0] = GameObject.FindGameObjectWithTag("PlayerUpperBody").GetComponent<Animator>();
//                _animators[1] = GameObject.FindGameObjectWithTag("PlayerLowerBody").GetComponent<Animator>();
//                break;

//            case AnimationParameters.AimDirection:
//                _animators = new Animator[1];
//                _animators[0] = GameObject.FindGameObjectWithTag("PlayerUpperBody").GetComponent<Animator>();
//                break;

//            case AnimationParameters.Grounded:
//                _animators = new Animator[1];
//                _animators[0] = GameObject.FindGameObjectWithTag("PlayerLowerBody").GetComponent<Animator>();
//                break;

//            case AnimationParameters.Damaged:
//                _numOfSprites = 3;
//                _sprites = new SpriteRenderer[3];
//                _sprites[0] = GameObject.FindGameObjectWithTag("PlayerUpperBody").GetComponent<SpriteRenderer>();
//                _sprites[1] = GameObject.FindGameObjectWithTag("PlayerLowerBody").GetComponent<SpriteRenderer>();
//                _sprites[2] = GameObject.FindGameObjectWithTag("Weapons").GetComponent<SpriteRenderer>();
//                Show();
//                break;
//        }
//    }

//    public void PlayAnimation(int value = 0) {

//        switch (_animationParameter) {

//            case AnimationParameters.Walking:
//                SetBool(true); break;

//            case AnimationParameters.AimDirection:
//                SetInteger(value); break;

//            case AnimationParameters.Grounded:
//                SetBool(true); break;

//            case AnimationParameters.Damaged:
//                Hide(); break;
//        }
//    }

//    public void StopAnimation() {
//        switch (_animationParameter) {

//            case AnimationParameters.Walking:
//                SetBool(false); break;

//            case AnimationParameters.AimDirection:
//                SetInteger(0); break;

//            case AnimationParameters.Grounded:
//                SetBool(false); break;

//            case AnimationParameters.Damaged:
//                Show(); break;
//        }
//    }

//    private void SetBool(bool value) {

//        for (byte i = 0; i < _animators.Length; ++i) {
//            _animators[i].SetBool(_animationParameter.ToString(), value);
//        }
//    }

//    private void SetInteger(int value) {
//        for (byte i = 0; i < _animators.Length; ++i) {
//            _animators[i].SetInteger(_animationParameter.ToString(), value);
//        }
//    }

//    // This does not use Unity's Animator. This is changing sprite properties directly to animate
//    private void Hide() {
//        for (int i = 0; i < _numOfSprites; ++i) {
//            _sprites[i].enabled = false;
//        }
//    }

//    // This does not use Unity's Animator. This is changing sprite properties directly to animate
//    private void Show() {
//        for (int i = 0; i < _numOfSprites; ++i) {
//            _sprites[i].enabled = true;
//        }
//    }
//}
