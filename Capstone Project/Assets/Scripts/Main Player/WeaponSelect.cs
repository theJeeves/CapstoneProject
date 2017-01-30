using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*
 * This script is designed to switch between the shotgun and the machine, depending on which one is currently enabled.
 * This includes the weapon's sprite, ammo sprite, and their respective scripts.
 */ 

public class WeaponSelect : MonoBehaviour {

    [SerializeField]
    private Image _shotgunAmmo;
    [SerializeField]
    private Image _machineGunAmmo;

    private Shotgun _shotgun;
    private MachineGun _machineGun;
    //private SpriteRenderer _renderer;

    private void OnEnable() {
        ControllableObject.OnButtonDown += OnButtonDown;

        _shotgun = GetComponent<Shotgun>();
        _machineGun = GetComponent<MachineGun>();

        EnableShotgun();
    }

    private void OnDisable() {
        ControllableObject.OnButtonDown -= OnButtonDown;
    }

    // Only perform the weapon swap one per button press.
    // Do not continually swap between weapons if the player holds the button down.
    private void OnButtonDown(Buttons button) {
        
        if (button == Buttons.WeaponSwap) {

            if (_shotgun.isActiveAndEnabled) {
                EnableMachineGun();
            }
            else if (_machineGun.isActiveAndEnabled) {
                EnableShotgun();
            }
        }
    }

    private void EnableShotgun() {
        _machineGun.enabled = false;
        _machineGunAmmo.enabled = false;

        _shotgun.enabled = true;
        _shotgunAmmo.enabled = true;
    }

    private void EnableMachineGun() {
        _shotgun.enabled = false;
        _shotgunAmmo.enabled = false;

        _machineGun.enabled = true;
        _machineGunAmmo.enabled = true;
    }
}
