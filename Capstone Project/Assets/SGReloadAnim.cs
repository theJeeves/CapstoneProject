using UnityEngine;
using System.Collections;

public class SGReloadAnim : ReloadAnimation {

    protected override void OnEnable() {
        Shotgun.StartReloadAnimation += Reload;
        base.OnEnable();
    }

    protected override void OnDisable() {
        Shotgun.StartReloadAnimation -= Reload;
        base.OnDisable();
    }
}
