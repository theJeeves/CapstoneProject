using UnityEngine;
using System.Collections;
using SpriterDotNetUnity;
using System.Collections.Generic;
using System.Linq;

public class CheckpointBehavior : MonoBehaviour {

    private UnityAnimator _animator;                // Refernce to the SpriterDotNetUnity animator to animate the checkpoint
    private bool _activated = false;                // Determines if the player has reached hit a given checkpoint
    [SerializeField]
    private SOCheckpoint _SOCheckpoint;             // Reference to the checkpoint manager to set and get values from it.

    private void Update() {
        // Keep looking for the animator component unit it is found.
        if (_animator == null) {
            _animator = GetComponent<SpriterDotNetBehaviour>().Animator;
        }

        // If a player has reached a new checkpoint, reset the old checkpoint to a ready state (not a checked state)
        if (_activated && _SOCheckpoint.checkpointGO != gameObject) {
            _activated = false;
            _animator.Play(GetAnimation(-1));
        }
    }

    private void OnTriggerEnter2D(Collider2D otherGo) {

        // If the player has reached a new checkpoint, set the checkpoint to a checked state and record their position.
        // Also, record which checkpoint the player has just reached.
        if (!_activated && otherGo.gameObject.tag == "Player") {
            _activated = true;
            _animator.Play(GetAnimation(1));
            _SOCheckpoint.checkpointPosition = transform.position;
            _SOCheckpoint.checkpointGO = gameObject;
        }
    }

    // This is not Unity. This is SpriterDotNetUnity. This code is based on an example
    // straight from the developer. 1. generate a list of all the avaialbe animations.
    // 2. get the index of the animation you are looking for. 3. Offset the index to find the
    // correct animation.
    private string GetAnimation(int offset) {

        List<string> animations = _animator.GetAnimations().ToList();
        int index = animations.IndexOf(_animator.CurrentAnimation.Name);
        index += offset;
        if (index >= animations.Count) index = 0;
        if (index < 0) index = animations.Count - 1;
        return animations[index];
    }
}
