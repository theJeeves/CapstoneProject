using System;
using UnityEngine;

public class PlayerChargerInteraction : MonoBehaviour {

    #region Private Fields
    private float m_LaunchDistance = float.NaN;
    private Rigidbody2D m_PlayerBody2d = null;
    private Rigidbody2D m_ChargerBody2d = null;

    #endregion Private Fields

    #region Initializers
    private void Start()
    {
        m_LaunchDistance = 20000.0f;
        m_PlayerBody2d = GetComponent<Rigidbody2D>();

        PlayerKnockedUp?.Invoke(this, null);
    }

    #endregion Initializers

    #region Events
    public static event EventHandler PlayerKnockedUp;

    #endregion Events
}
