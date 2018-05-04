using UnityEngine;
using System;

public class CallTextHUD : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private float _dropDownSpeed = 0.0f;

    private bool m_Display = false;
    private RectTransform m_Transform = null;

    #endregion Private Fields

    #region Initializers
    private void OnEnable() {

        // Wire-up events
        InstructionText.DisplayHint += OnDisplayHint;
        InstructionText.HideHint += OnHideHint;

        m_Transform = GetComponent<RectTransform>();
    }

    #endregion Initializers

    #region Finalizers
    private void OnDisable() {

        // Unwire events
        InstructionText.DisplayHint -= OnDisplayHint;
        InstructionText.HideHint -= OnHideHint;
    }

    #endregion Finalizers

    #region Private Methods
    private void Update() {
        
        if (m_Display && m_Transform.anchoredPosition.y > -200.0f) {
            m_Transform.anchoredPosition = Vector3.MoveTowards(m_Transform.anchoredPosition, new Vector3(0.0f, -200.0f, 0), Time.deltaTime * _dropDownSpeed);
        }
        else if (!m_Display && m_Transform.anchoredPosition.y < 155.0f) {
            m_Transform.anchoredPosition = Vector3.MoveTowards(m_Transform.anchoredPosition, new Vector3(0.0f, 155.0f, 0), Time.deltaTime * _dropDownSpeed);
        }
    }

    private void OnDisplayHint(object sender, string hint) {
        m_Display = true;
    }

    private void OnHideHint(object sender, EventArgs args) {
        m_Display = false;
    }

    #endregion Private Methods
}
