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
    }

    private void OnDisable() {
        Shotgun.UpdateNumOfRounds -= UpdateText;
        MachineGun.UpdateNumOfRounds -= UpdateText;
    }

    private void UpdateText(int numOfRounds) {
        _ammoText.text = string.Format("{0}", numOfRounds);
    }
}
