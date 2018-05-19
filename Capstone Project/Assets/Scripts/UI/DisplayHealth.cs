using UnityEngine;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour {

    private Text _healthText;
    private Image _healthIcon;

    private void Awake() {
        _healthText = GetComponent<Text>();
        _healthIcon = GetComponentInChildren<Image>();
    }

    private void OnEnable() {
        PlayerHealth.UpdateHealth += UpdateText;
    }

    private void OnDisable() {
        PlayerHealth.UpdateHealth -= UpdateText;
    }

    private void UpdateText(int num) {

        _healthText.text = string.Format("{0}", num);

        if (num > 0) {
            _healthText.enabled = true;
            _healthIcon.enabled = true;
        }
        else {
            _healthText.enabled = false;
            _healthIcon.enabled = false;
        }
    }
}
