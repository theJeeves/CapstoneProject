using UnityEngine;
using System.Collections;

public class PlayerAnimationLegs : MonoBehaviour {

    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        PlayerWalking.Walking += Walking;
        PlayerCollisionState.OnHitGround += Grounded;
        PlayerCollisionState.OnLifted += NotGrounded;
    }

    private void OnDisable() {
        PlayerWalking.Walking -= Walking;
        PlayerCollisionState.OnHitGround -= Grounded;
        PlayerCollisionState.OnLifted -= NotGrounded;
    }

    private void NotGrounded() {
        _animator.SetBool("Grounded", false);
    }

    private void Grounded() {
        _animator.SetBool("Grounded", true);
    }

    private void Walking(bool isWalking) {
        _animator.SetBool("Walking", isWalking);
    }
}
