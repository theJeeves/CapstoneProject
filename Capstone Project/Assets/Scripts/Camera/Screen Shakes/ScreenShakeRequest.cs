using UnityEngine;

[CreateAssetMenu(menuName ="Screen Shake/New Screen Shake Request")]
public abstract class ScreenShakeRequest : ScriptableObject {

    #region Fields
    [SerializeField]
    protected float _shakeAmount = 0.0f;

    #endregion Fields

    #region Public Methods
    /// <summary>
    /// Immediately have the camera perform a screen shake.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public abstract Vector3 Shake(int key = 0);

    /// <summary>
    /// Request the camera perform a screen shake.
    /// </summary>
    public virtual void ShakeRequest() {
        
    }

    #endregion Public Methods
}
