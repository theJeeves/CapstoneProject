using UnityEngine;
using System.Collections;

public class MGReloadAnim : ReloadAnimation {

    protected override void OnEnable() {
        base.OnEnable();

        MachineGun.StartReloadAnimation += Reload;
        MachineGun.EmptyClip += ZeroFillAmount;
    }

    protected override void OnDisable() {
        MachineGun.StartReloadAnimation -= Reload;
        MachineGun.EmptyClip -= ZeroFillAmount;
    }
}
