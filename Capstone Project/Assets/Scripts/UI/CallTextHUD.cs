using UnityEngine;

public class CallTextHUD : MonoBehaviour {

    #region Private Fields
    [SerializeField]
    private float _dropDownSpeed = 0.0f;

    private RectTransform m_Transform = null;
    private Vector3 m_DownPosition = Vector3.zero;
    private Vector3 m_UpPosition = Vector3.zero;

    private bool m_Display = false;
    #endregion Private Fields

    #region Initializers
    private void Start()
    {
        m_DownPosition = new Vector3(0.0f, -200.0f, 0);
        m_UpPosition = new Vector3(.0f, 155.0f, 0);
    }

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
            m_Transform.anchoredPosition = Vector3.MoveTowards(m_Transform.anchoredPosition, m_DownPosition, Time.deltaTime * _dropDownSpeed);
        }
        else if (!m_Display && m_Transform.anchoredPosition.y < 155.0f) {
            m_Transform.anchoredPosition = Vector3.MoveTowards(m_Transform.anchoredPosition, m_UpPosition, Time.deltaTime * _dropDownSpeed);
        }
    }

    private void OnDisplayHint(string hint) {
        m_Display = true;
    }

    private void OnHideHint() {
        m_Display = false;
    }

    #endregion Private Methods
}
