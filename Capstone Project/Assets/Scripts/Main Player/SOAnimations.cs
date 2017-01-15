using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AnimationParameters {
    None,
    Walking,
    AimDirection,
    Grounded,
    Damaged,
}

[CreateAssetMenu(menuName =("SO Animatoins/New SO Animation"))]
public class SOAnimations : ScriptableObject {

    [SerializeField]
    private AnimationParameters _animationParameter;

    private Animator[] _animators = null;
    private SpriteRenderer[] _sprites = null;
    private int _numOfSprites = 0;

    private void OnEnable() {

        switch (_animationParameter) {
            case AnimationParameters.Walking:
                _animators = new Animator[2];
                _animators[0] = GameObject.FindGameObjectWithTag("PlayerUpperBody").GetComponent<Animator>();
                _animators[1] = GameObject.FindGameObjectWithTag("PlayerLowerBody").GetComponent<Animator>();
                break;

            case AnimationParameters.AimDirection:
                _animators = new Animator[1];
                _animators[0] = GameObject.FindGameObjectWithTag("PlayerUpperBody").GetComponent<Animator>();
                break;

            case AnimationParameters.Grounded:
                _animators = new Animator[1];
                _animators[0] = GameObject.FindGameObjectWithTag("PlayerLowerBody").GetComponent<Animator>();
                break;

            case AnimationParameters.Damaged:
                _numOfSprites = 3;
                _sprites = new SpriteRenderer[3];
                _sprites[0] = GameObject.FindGameObjectWithTag("PlayerUpperBody").GetComponent<SpriteRenderer>();
                _sprites[1] = GameObject.FindGameObjectWithTag("PlayerLowerBody").GetComponent<SpriteRenderer>();
                _sprites[2] = GameObject.FindGameObjectWithTag("Weapons").GetComponent<SpriteRenderer>();
                Show();
                break;
        }
    }

    public void PlayAnimation(int value = 0) {

        switch (_animationParameter) {

            case AnimationParameters.Walking:
                SetBool(true); break;

            case AnimationParameters.AimDirection:
                SetInteger(value); break;

            case AnimationParameters.Grounded:
                SetBool(true); break;

            case AnimationParameters.Damaged:
                Hide(); break;
        }
    }

    public void StopAnimation() {
        switch (_animationParameter) {

            case AnimationParameters.Walking:
                SetBool(false); break;

            case AnimationParameters.AimDirection:
                SetInteger(0); break;

            case AnimationParameters.Grounded:
                SetBool(false); break;

            case AnimationParameters.Damaged:
                Show(); break;
        }
    }

    private void SetBool(bool value) {

        for (byte i = 0; i < _animators.Length; ++i) {
            _animators[i].SetBool(_animationParameter.ToString(), value);
        }
    }

    private void SetInteger(int value) {
        for (byte i = 0; i < _animators.Length; ++i) {
            _animators[i].SetInteger(_animationParameter.ToString(), value);
        }
    }

    // This does not use Unity's Animator. This is changing sprite properties directly to animate
    private void Hide() {
        for (int i = 0; i < _numOfSprites; ++i) {
            _sprites[i].enabled = false;
        }
    }

    // This does not use Unity's Animator. This is changing sprite properties directly to animate
    private void Show() {
        for (int i = 0; i < _numOfSprites; ++i) {
            _sprites[i].enabled = true;
        }
    }
}
