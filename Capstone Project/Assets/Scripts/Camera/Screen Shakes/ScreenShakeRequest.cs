using UnityEngine;

public abstract class ScreenShakeRequest : ScriptableObject {

    [SerializeField]
    protected float _shakeAmount = 0.0f;

    public abstract Vector3 Shake();
    public virtual void ShakeRequest(byte key = 0) {}
}
