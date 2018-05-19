using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour {

    #region Fields
    private Text m_HealthText;
    private Image m_HealthIcon;

    #endregion Fields

    #region Initializers
    private void Awake() {
        m_HealthText = GetComponent<Text>();
        m_HealthIcon = GetComponentInChildren<Image>();
    }

    private void OnEnable() {
        PlayerHealth.UpdateHealth += UpdateText;
    }

    #endregion Initializers

    #region Finalizers
    private void OnDisable() {
        PlayerHealth.UpdateHealth -= UpdateText;
    }

    #endregion Finalizers

    #region Private Methods
    private void UpdateText(int num) {

        m_HealthText.text = string.Format("{0}", num);

        if (num > 0) {
            m_HealthText.enabled = true;
            m_HealthIcon.enabled = true;
        }
        else {
            m_HealthText.enabled = false;
            m_HealthIcon.enabled = false;
        }
    }

    #endregion Private Methods
}
