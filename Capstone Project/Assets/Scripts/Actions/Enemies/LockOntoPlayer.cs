using UnityEngine;
using System.Collections;

public abstract class LockOntoPlayer : MonoBehaviour {

    public delegate void LockOntoPlayerEvent();

    [SerializeField]
    protected float _timer;

    protected virtual void OnEnable() {

        StartCoroutine(LockOn());
    }

    protected virtual IEnumerator LockOn() {
        yield return 0;
    }
}
