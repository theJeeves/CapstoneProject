using UnityEngine;
using System.Collections;

public class MGReloadAnim : ReloadAnimation {

    protected override void OnEnable() {
        MachineGun.StartReloadAnimation += Reload;
        base.OnEnable();
    }

    protected override void OnDisable() {
        MachineGun.StartReloadAnimation -= Reload;
        base.OnDisable();
    }
}
