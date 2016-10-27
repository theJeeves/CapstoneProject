using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayAmmo : MonoBehaviour {

    private Text _ammoText;
    private AbstractGun _shotGun;
    private AbstractGun _machineGun;

    private void Awake() {
        _ammoText = GetComponent<Text>();
        _shotGun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Shotgun>();
        _machineGun = GameObject.FindGameObjectWithTag("Gun").GetComponent<MachineGun>();
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
