using UnityEngine;
using System.Collections;

public class WeaponSelect : AbstractPlayerActions {

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
            }
            else if (_machineGun.isActiveAndEnabled) {
                _machineGun.enabled = false;
                _shotgun.enabled = true;
            }
        }
    }
}
