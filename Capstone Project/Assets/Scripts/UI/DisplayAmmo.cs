using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayAmmo : MonoBehaviour {

    private Text _ammoText;

    private void Awake() {
        _ammoText = GetComponent<Text>();
    }

    private void OnEnable() {
        Shotgun.UpdateNumOfRounds += UpdateText;
        MachineGun.UpdateNumOfRounds += UpdateText;
        PlayerHealth.UpdateHealth += ShowHideText;
    }

    private void OnDisable() {
        Shotgun.UpdateNumOfRounds -= UpdateText;
        MachineGun.UpdateNumOfRounds -= UpdateText;
        PlayerHealth.UpdateHealth -= ShowHideText;
    }

    private void UpdateText(int numOfRounds) {
        if (numOfRounds >= 10) {
            _ammoText.text = string.Format("{0}", numOfRounds);
        }
        else {
            _ammoText.text = string.Format("{0}  ", numOfRounds);
        }
    }

    private void ShowHideText(object sender, int health) {
        if (health > 0) {
            _ammoText.enabled = true;
        }
        else {
            _ammoText.enabled = false;
        }
    }
}
