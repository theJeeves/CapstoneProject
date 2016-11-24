using UnityEngine;
using System.Collections;

public class MGReloadAnim : ReloadAnimation {

    private void OnEnable() {
        MachineGun.StartReloadAnimation += Reload;
    }

    protected override void OnDisable() {
        MachineGun.StartReloadAnimation -= Reload;
        base.OnDisable();
    }
}
