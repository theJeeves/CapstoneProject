using UnityEngine;
using System;

public class InstructionText : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private string _ds4Instructions;
    [SerializeField]
    private string _xboxInstructions;

    [SerializeField]
    private bool _enableInput = false;

    [SerializeField]
    private float m_displayTime = 3.0f;

    private float m_DefaultDisplayTime = 3.0f;
    private bool m_InTrigger = false;
    private bool m_Ds4 = false;

    #endregion Private Fields

    #region Initializers
    private void Start() {
        m_displayTime = m_DefaultDisplayTime;
    }

    #endregion Initializers

    #region Events
    public static event EventHandler<string> DisplayHint;
    public static event EventHandler HideHint;

    #endregion Events

    #region Private Methods
    private void Update() {

        if (m_InTrigger && m_displayTime != float.NegativeInfinity) {
            TimeTools.TimeExpired(ref m_displayTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D player) {

        m_InTrigger = true;

        if (m_displayTime != float.NegativeInfinity && player.gameObject.tag == StringConstantUtility.PLAYER_TAG) {

            m_Ds4 = InputManager.Instance.GetComponent<InputManager>().controllerType == 0 ? true : false;
            if (m_Ds4) {
                DisplayHint?.Invoke(this, _ds4Instructions);
            }
            else {
                DisplayHint?.Invoke(this, _xboxInstructions);
            }

            if (_enableInput) { InputManager.Instance.GetComponent<InputManager>().StartInput(); }
        }
    }

    private void OnTriggerExit2D(Collider2D player) {

        m_InTrigger = false;

        if (m_displayTime != float.NegativeInfinity && player.gameObject.tag == StringConstantUtility.PLAYER_TAG) {

            HideHint?.Invoke(this, null);

            m_displayTime = m_displayTime <= 0.0f ? float.NegativeInfinity : m_DefaultDisplayTime;
        }
    }

    #endregion Private Methods
}
