using UnityEngine;

public class PlayerChargerInteraction : MonoBehaviour {

    #region Private Fields
    private float _launchDistance;
    private Rigidbody2D _playerBody2d;
    private Rigidbody2D _chargerBody2d;

    #endregion Private Fields

    #region Private Initializers
    private void Start()
    {
        _launchDistance = 20000.0f;
        _playerBody2d = GetComponent<Rigidbody2D>();

        PlayerKnockedUp?.Invoke();
    }

    #endregion Private Initializers

    #region Delegates
    public delegate void PlayerKnockUpEvent();

    #endregion Delegates

    #region Events
    public static event PlayerKnockUpEvent PlayerKnockedUp;

    #endregion Events
}
