using UnityEngine;

[CreateAssetMenu(menuName ="Screen Shake/Weapon")]
public class WeaponSSRequest : ScreenShakeRequest {

    #region Private Fields
    private Vector3[] _directions = new Vector3[8];     // All possible angles which can be used by the player

    #endregion Private Fields

    #region Private Initializers
    private void OnEnable() {
        // Define all the possible angles based.
        AssignDirections(0, 1.0f, 0.0f);
        AssignDirections(1, 0.7f, 0.7f);
        AssignDirections(2, 0.0f, 1.0f);
        AssignDirections(3, -0.7f, 0.7f);
        AssignDirections(4, -1.0f, 0.0f);
        AssignDirections(5, -0.7f, -0.7f);
        AssignDirections(6, 0.0f, -1.0f);
        AssignDirections(7, 0.7f, -0.7f);
    }

    #endregion Private Initializers

    #region Public Methods
    /// <summary>
    /// Immediately have the camera perform a screen shake.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public override Vector3 Shake(int key) {
        return _directions[key] * _shakeAmount;
    }

    /// <summary>
    /// Request the camera perform a screen shake.
    /// </summary>
    public override void ShakeRequest() {
        Camera.main.SendMessage(StringConstantUtility.ENQUEUE_MESSAGE, this);
    }

    #endregion Public Methods

    #region Private Methods
    private void AssignDirections(int angle, float x, float y, float z = 0.0f) {
        _directions[angle] = new Vector3(x, y, z);
    }

    #endregion Private Methods
}