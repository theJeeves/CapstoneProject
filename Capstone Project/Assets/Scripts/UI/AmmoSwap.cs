using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AmmoSwap : MonoBehaviour {

    [SerializeField]
    private GameObject _shotgun;
    [SerializeField]
    private GameObject _machineGun;

	private void OnEnable() {
        WeaponSelect.SwapWeapon += SwapAmmoType;
    }

    private void OnDisable() {
        WeaponSelect.SwapWeapon -= SwapAmmoType;
    }

    /*
     * We have to change the positions and the scale at this point because
     * I just grabbed some clip art form online and they are not the same size.
     * This should not be an issue when we have Hao create the art assest in house.
     */
    private void SwapAmmoType(int weapon) {

        if (_shotgun.activeInHierarchy) {
            _machineGun.transform.localPosition = new Vector3(-16.0f, 0.0f, 0.0f);
            //_machineGun.transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);
            _machineGun.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -90.0f);

            _shotgun.SetActive(false);
            _machineGun.SetActive(true);
        }
        else if (_machineGun.activeInHierarchy) {
            _shotgun.transform.localPosition = new Vector3(-13.0f, 0.0f, 0.0f);
            //_shotgun.transform.localScale = new Vector3(0.25f, 0.25f, 1.0f);
            _shotgun.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

            _machineGun.SetActive(false);
            _shotgun.SetActive(true);
        }
    }
}
