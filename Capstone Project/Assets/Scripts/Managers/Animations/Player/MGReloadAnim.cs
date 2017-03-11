using UnityEngine;
using System.Collections;

public class MGReloadAnim : ReloadAnimation {

    private void OnEnable() {

        MachineGun.StartReloadAnimation += Reload;
        MachineGun.EmptyClip += ZeroFillAmount;
        MachineGun.DisplayAmmo += DisplayAmmo;
    }

    protected override void OnDisable() {
        MachineGun.StartReloadAnimation -= Reload;
        MachineGun.EmptyClip -= ZeroFillAmount;
        MachineGun.DisplayAmmo -= DisplayAmmo;
        base.OnDisable();
    }
}
