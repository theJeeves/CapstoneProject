using UnityEngine;
using System.Collections;
using SpriterDotNetUnity;
using System.Collections.Generic;
using System.Linq;

public class CheckpointBehavior : MonoBehaviour {

    private UnityAnimator _animator;
    private bool _activated = false;
    [SerializeField]
    private SOCheckpoint _SOCheckpoint;

    private void Update() {
        if (_animator == null) {
            _animator = GetComponent<SpriterDotNetBehaviour>().Animator;
        }

        if (_activated && _SOCheckpoint.checkpointGO != gameObject) {
            _activated = false;
            _animator.Play(GetAnimation(-1));
        }
    }

    private void OnTriggerEnter2D(Collider2D otherGo) {

        if (!_activated && otherGo.gameObject.tag == "Player") {
            _activated = true;
            _animator.Play(GetAnimation(1));
            _SOCheckpoint.checkpointPosition = transform.position;
            _SOCheckpoint.checkpointGO = gameObject;
        }
    }

    private string GetAnimation(int offset) {

        List<string> animations = _animator.GetAnimations().ToList();
        int index = animations.IndexOf(_animator.CurrentAnimation.Name);
        index += offset;
        if (index >= animations.Count) index = 0;
        if (index < 0) index = animations.Count - 1;
        return animations[index];
    }
}
