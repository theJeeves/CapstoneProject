using UnityEngine;
using System.Collections;

public class ShotgunShake : CameraShake {

	protected override void OnEnable() {
        base.OnEnable();
        Shotgun.Fire += ShakeScreen;
    }

    private void OnDisable() {
        Shotgun.Fire -= ShakeScreen;
    }
}
