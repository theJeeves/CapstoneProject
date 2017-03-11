using UnityEngine;
using System.Collections;

public abstract class LockOntoPlayer : MonoBehaviour {

    public delegate void LockOntoPlayerEvent();

    protected virtual void OnEnable() {

    }

    protected virtual IEnumerator LockOn() {
        yield return 0;
    }
}
