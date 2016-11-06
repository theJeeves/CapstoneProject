using UnityEngine;
using System.Collections;

/* This script is looking for a AimWeapon Event to work properly. 
 * When the player changes aim directions, the amination will change as well.
 * This way we do not have to look for aim directions and button presses twice.
 */

public class PlayerAnimation : MonoBehaviour {

    private Animator _animator;
    //private ControllableObject _controller;

    private void Awake() {
        _animator = GetComponent<Animator>();
        //_controller = GetComponent<ControllableObject>();
    }


    private void OnEnable() {
        AimWeapon.AimDirectionChanged += ChangeAnimationState;
    }

    private void OnDisable() {
        AimWeapon.AimDirectionChanged -= ChangeAnimationState;
    }

    private void ChangeAnimationState(int degree) {
        _animator.SetInteger("AimDirection", degree);
    }
}
