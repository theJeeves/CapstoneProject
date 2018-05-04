using UnityEngine;
using UnityEngine.UI;

public class DisplayAmmo : MonoBehaviour {

    #region Private Fields
    private Text m_AmmoText;

    #endregion Private Fields

    #region Initializers
    private void Awake() {
        m_AmmoText = GetComponent<Text>();
    }

    private void OnEnable() {

        // Wire-up Event
        Shotgun.UpdateNumOfRounds += OnUpdateText;
        MachineGun.UpdateNumOfRounds += OnUpdateText;
        PlayerHealth.UpdateHealth += OnShowHideText;
    }

    #endregion Initializers

    #region Finalizers
    private void OnDisable() {

        // Unwire event
        Shotgun.UpdateNumOfRounds -= OnUpdateText;
        MachineGun.UpdateNumOfRounds -= OnUpdateText;
        PlayerHealth.UpdateHealth -= OnShowHideText;
    }

    #endregion Finalizers

    #region Private Methods
    private void OnUpdateText(object sender, int numOfRounds) {
        if (numOfRounds >= 10) {
            m_AmmoText.text = string.Format("{0}", numOfRounds);
        }
        else {
            m_AmmoText.text = string.Format("{0}  ", numOfRounds);
        }
    }

    private void OnShowHideText(object sender, int health)
    {
        m_AmmoText.enabled = health > 0 ? true : false;
    }

    #endregion Private Methods
}
