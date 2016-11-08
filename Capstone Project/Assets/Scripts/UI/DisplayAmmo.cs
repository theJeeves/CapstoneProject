using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayAmmo : MonoBehaviour {

    private Text _ammoText;

    private void Awake() {
        _ammoText = GetComponent<Text>();
    }

    private void OnEnable() {
        AbstractGun.UpdateNumOfRounds += UpdateText;
    }

    private void OnDisable() {
        AbstractGun.UpdateNumOfRounds -= UpdateText;
    }

    private void UpdateText(int numOfRounds) {
        if (numOfRounds >= 10) {
            _ammoText.text = string.Format("{0}", numOfRounds);
        }
        else {
            _ammoText.text = string.Format("{0}  ", numOfRounds);
        }
    }
}
