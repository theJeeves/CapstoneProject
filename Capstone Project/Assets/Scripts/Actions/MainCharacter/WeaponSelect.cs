using UnityEngine;
using System.Collections;

public class WeaponSelect : AbstractPlayerActions {

    public delegate void WeaponSelectEvent(int weapon);
    public static event WeaponSelectEvent SwapWeapon;

    private AbstractGun _shotgun;
    private AbstractGun _machineGun;

    protected override void Awake() {
        base.Awake();

        _shotgun = GetComponent<Shotgun>();
        _machineGun = GetComponent<MachineGun>();
    }

    protected override void OnButtonDown(Buttons button) {
        
        if (button == Buttons.WeaponSwap) {

            if (_shotgun.isActiveAndEnabled) {
                _shotgun.enabled = false;
                _machineGun.enabled = true;
                if (SwapWeapon != null) {
                    SwapWeapon(1);
                }
            }
            else if (_machineGun.isActiveAndEnabled) {
                _machineGun.enabled = false;
                _shotgun.enabled = true;
                if (SwapWeapon != null) {
                    SwapWeapon(0);
                }
            }
        }
    }
}
