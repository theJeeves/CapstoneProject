using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AmmoSwap : MonoBehaviour {

    [SerializeField]
    private Image _shotgun;
    [SerializeField]
    private Image _machineGun;

    private void Awake() {
        _machineGun.enabled = false;
    }

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

        if (_shotgun.enabled) {
            _shotgun.enabled = false;
            _machineGun.enabled = true; ;
        }
        else if (_machineGun.enabled) {
            _machineGun.enabled = false;
            _shotgun.enabled = true;
        }
    }
}
