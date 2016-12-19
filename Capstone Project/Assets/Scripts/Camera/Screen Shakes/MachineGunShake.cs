using UnityEngine;
using System.Collections;

public class MachineGunShake : CameraShake {

	protected override void OnEnable() {
        base.OnEnable();
        MachineGun.Fire += ShakeScreen;
    }

    private void OnDisable() {
        MachineGun.Fire -= ShakeScreen;
    }
}
