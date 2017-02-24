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
    [SerializeField]
    private bool _MGAvailable = true;
    public bool MGAvailable {
        set { _MGAvailable = value; }
    }
    [SerializeField]
    private bool _SGAvailable = false;
    public bool SGAvailable {
        set { _SGAvailable = value; }
    }

    private Shotgun _shotgun;
    private MachineGun _machineGun;

    private void OnEnable() {
        ControllableObject.OnButtonDown += OnButtonDown;

        _shotgun = GetComponent<Shotgun>();
        _machineGun = GetComponent<MachineGun>();

        if (_shotgun.enabled) {
            EnableShotgun();
        }
        else if (_machineGun.enabled) {
            EnableMachineGun();
        }
    }

    private void OnDisable() {
        ControllableObject.OnButtonDown -= OnButtonDown;
    }

    // Only perform the weapon swap one per button press.
    // Do not continually swap between weapons if the player holds the button down.
    private void OnButtonDown(Buttons button) {
        
        if (button == Buttons.WeaponSwap) {

            if (_shotgun.isActiveAndEnabled && _MGAvailable) {
                EnableMachineGun();
            }
            else if (_machineGun.isActiveAndEnabled && _SGAvailable) {
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
