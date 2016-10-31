using UnityEngine;
using System.Collections;

public class ShotgunShake : CameraShake {

	private void OnEnable() {
        Shotgun.Fire += ShakeScreen;
    }

    private void OnDisable() {
        Shotgun.Fire -= ShakeScreen;
    }
}
