using UnityEngine;

[CreateAssetMenu(menuName ="Screen Shake/Damage")]
public class DamageSSRequest : ScreenShakeRequest
{
    #region Constants
    public const string ENQUEUE_MESSAGE = "Enqueue";
    public const string PLAYER_DAMAGED_MESSAGE = "PlayerDamaged";

    #endregion Constants

    #region Public Methods

    /// <summary>
    /// Immediately have the camera perform a screen shake.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public override Vector3 Shake(int key = 0)
    {
        return new Vector3(Random.insideUnitCircle.x * _shakeAmount, Random.insideUnitCircle.y * _shakeAmount, 0.0f);
    }

    /// <summary>
    /// Request the camera perform a screen shake.
    /// </summary>
    public override void ShakeRequest()
    {
        Camera.main.SendMessage(ENQUEUE_MESSAGE, this);

        // This calls for the chromatic effect
        Camera.main.SendMessage(PLAYER_DAMAGED_MESSAGE, 1);
    }

    #endregion Public Methods
}
