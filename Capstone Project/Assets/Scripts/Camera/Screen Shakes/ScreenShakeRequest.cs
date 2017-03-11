using UnityEngine;

[CreateAssetMenu(menuName ="Screen Shake/New Screen Shake Request")]
public abstract class ScreenShakeRequest : ScriptableObject {

    [SerializeField]
    protected float _shakeAmount = 0.0f;

    public abstract Vector3 Shake(int key = 0);

    public virtual void ShakeRequest() {
        
    }
}
