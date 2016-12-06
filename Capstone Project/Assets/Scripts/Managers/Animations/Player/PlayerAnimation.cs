using UnityEngine;
using System.Collections;

/* This script is looking for a AimWeapon Event to work properly. 
 * When the player changes aim directions, the amination will change as well.
 * This way we do not have to look for aim directions and button presses twice.
 */

public class PlayerAnimation : MonoBehaviour {

    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }


    private void OnEnable() {
        AimWeapon.AimDirectionChanged += ChangeAnimationState;
        PlayerWalking.Walking += Walking;
    }

    private void OnDisable() {
        AimWeapon.AimDirectionChanged -= ChangeAnimationState;
        PlayerWalking.Walking -= Walking;
    }

    private void ChangeAnimationState(int degree) {
        _animator.SetInteger("AimDirection", degree);
    }

    private void Walking(bool isWalking) {
        _animator.SetBool("Walking", isWalking);
    }
}
