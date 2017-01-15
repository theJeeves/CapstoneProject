using UnityEngine;
using System.Collections;

public class DestroyAfterAnimation : MonoBehaviour {

    [SerializeField]
    private AnimationClip _animation;

    private void OnEnable() {
        Destroy(gameObject, _animation.length);
    }
}
