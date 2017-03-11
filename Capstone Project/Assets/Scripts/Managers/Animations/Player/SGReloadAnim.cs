using UnityEngine;
using System.Collections;

public class SGReloadAnim : ReloadAnimation {

    private void OnEnable() {
        Shotgun.StartReloadAnimation += Reload;
        Shotgun.EmptyClip += ZeroFillAmount;
        Shotgun.DisplayAmmo += DisplayAmmo;
    }

    protected override void OnDisable() {
        Shotgun.StartReloadAnimation -= Reload;
        Shotgun.EmptyClip -= ZeroFillAmount;
        Shotgun.DisplayAmmo -= DisplayAmmo;
        base.OnDisable();
    }
}
