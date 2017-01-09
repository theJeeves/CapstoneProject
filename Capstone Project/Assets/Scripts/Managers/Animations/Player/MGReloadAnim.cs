using UnityEngine;
using System.Collections;

public class MGReloadAnim : ReloadAnimation {

    private void OnEnable() {

        MachineGun.StartReloadAnimation += Reload;
        MachineGun.EmptyClip += ZeroFillAmount;
    }

    protected override void OnDisable() {
        MachineGun.StartReloadAnimation -= Reload;
        MachineGun.EmptyClip -= ZeroFillAmount;
        base.OnDisable();
    }
}
