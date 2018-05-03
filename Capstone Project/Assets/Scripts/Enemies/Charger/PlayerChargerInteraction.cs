using System;
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

        PlayerKnockedUp?.Invoke(this, null);
    }

    #endregion Private Initializers

    #region Events
    public static event EventHandler PlayerKnockedUp;

    #endregion Events
}
