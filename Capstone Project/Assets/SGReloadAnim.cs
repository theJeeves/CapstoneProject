using UnityEngine;
using System.Collections;

public class SGReloadAnim : ReloadAnimation {

    private void OnEnable() {
        Shotgun.StartReloadAnimation += Reload;
        Shotgun.EmptyClip += ZeroFillAmount;
    }

    protected override void OnDisable() {
        Shotgun.StartReloadAnimation -= Reload;
        Shotgun.EmptyClip -= ZeroFillAmount;
        base.OnDisable();
    }
}
