using UnityEngine;
using System.Collections;

public class SGReloadAnim : ReloadAnimation {

    private void OnEnable() {
        Shotgun.StartReloadAnimation += Reload;
    }

    protected override void OnDisable() {
        Shotgun.StartReloadAnimation -= Reload;
        base.OnDisable();
    }
}
