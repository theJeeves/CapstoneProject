using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayHealth : MonoBehaviour {

    private Text _healthText;

    private void Awake() {
        _healthText = GetComponent<Text>();
    }

    private void OnEnable() {
        PlayerHealth.UpdateHealth += UpdateText;
    }

    private void OnDisable() {
        PlayerHealth.UpdateHealth -= UpdateText;
    }

    private void UpdateText(int num) {

        _healthText.text = string.Format("{0}", num);
    }
}
