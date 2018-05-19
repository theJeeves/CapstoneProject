using UnityEngine;
using UnityEngine.UI;

/*
 * This script is designed to switch between the shotgun and the machine, depending on which one is currently enabled.
 * This includes the weapon's sprite, ammo sprite, and their respective scripts.
 */

public class WeaponSelect : MonoBehaviour
{
    #region Constants
    private const string SG_AMMO = "SGAmmoType";
    private const string MG_AMMO = "MGAmmoType";

    #endregion Constants

    private Image _shotgunAmmo;
    private Image _machineGunAmmo;

    [SerializeField]
    private bool _MGAvailable = true;
    public bool MGAvailable
    {
        set { _MGAvailable = value; }
    }
    [SerializeField]
    private bool _SGAvailable = false;
    public bool SGAvailable
    {
        set { _SGAvailable = value; }
    }

    private Shotgun _shotgun;
    private MachineGun _machineGun;

    private void OnEnable()
    {
        ControllableObject.ButtonDown += OnButtonDown;
        PlayerHealth.UpdateHealth += OnUpdateHealth;

        _shotgunAmmo = GameObject.Find(SG_AMMO).GetComponent<Image>();
        _machineGunAmmo = GameObject.Find(MG_AMMO).GetComponent<Image>();

        _shotgun = GetComponent<Shotgun>();
        _machineGun = GetComponent<MachineGun>();

        if (_shotgun.enabled)
        {
            EnableShotgun();
        }
        else if (_machineGun.enabled)
        {
            EnableMachineGun();
        }
    }

    private void OnDisable() {
        ControllableObject.ButtonDown -= OnButtonDown;
        PlayerHealth.UpdateHealth -= OnUpdateHealth;
    }

    // Only perform the weapon swap one per button press.
    // Do not continually swap between weapons if the player holds the button down.
    private void OnButtonDown(Buttons button)
    {        
        if (button == Buttons.WeaponSwap)
        {
            if (_shotgun.isActiveAndEnabled && _MGAvailable)
            {
                EnableMachineGun();
            }
            else if (_machineGun.isActiveAndEnabled && _SGAvailable)
            {
                EnableShotgun();
            }
        }
    }

    private void EnableShotgun()
    {
        _machineGun.enabled = false;
        _machineGunAmmo.enabled = false;

        _shotgun.enabled = true;
        _shotgunAmmo.enabled = true;
    }

    private void EnableMachineGun()
    {
        _shotgun.enabled = false;
        _shotgunAmmo.enabled = false;

        _machineGun.enabled = true;
        _machineGunAmmo.enabled = true;
    }

    private void OnUpdateHealth(int health)
    {
        if (health > 0)
        {
            if (_shotgun.isActiveAndEnabled && _MGAvailable)
            {
                _shotgunAmmo.enabled = true;
            }
            else if (_machineGun.isActiveAndEnabled && _SGAvailable)
            {
                _machineGunAmmo.enabled = true;
            }
        }
        else
        {
            if (_shotgun.isActiveAndEnabled)
            {
                _shotgunAmmo.enabled = false;
            }
            else if (_machineGun.isActiveAndEnabled)
            {
                _machineGunAmmo.enabled = false;
            }
        }
    }
}
