using UnityEngine;
using System.Collections;

public abstract class LockOntoPlayer : MonoBehaviour {

    #region Protected Initializers
    protected virtual void OnEnable() {

    }

    #endregion Protected Initializers

    #region Protected Methods
    protected virtual IEnumerator LockOn() {
        yield return 0;
    }

    #endregion Protected Methods
}
