using UnityEngine;
using System.Collections;

public class MachineGunShake : CameraShake {

	private void OnEnable() {
        MachineGun.Fire += ShakeScreen;
    }

    private void OnDisable() {
        MachineGun.Fire -= ShakeScreen;
    }
}
