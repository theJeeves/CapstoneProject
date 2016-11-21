using UnityEngine;
using System.Collections;

public class SGReloadAnim : ReloadAnimation {

    protected override void OnEnable() {
        base.OnEnable();

        Shotgun.StartReloadAnimation += Reload;
        Shotgun.EmptyClip += ZeroFillAmount;
    }

    protected override void OnDisable() {
        Shotgun.StartReloadAnimation -= Reload;
        Shotgun.EmptyClip -= ZeroFillAmount;
    }
}
