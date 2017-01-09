using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Screen Shake/Damage")]
public class DamageSSRequest : ScreenShakeRequest {

    public override Vector3 Shake(byte key = 0) {
        return new Vector3(Random.insideUnitCircle.x * _shakeAmount, Random.insideUnitCircle.y * _shakeAmount, 0.0f);
    }

    public override void ShakeRequest() {
        Camera.main.SendMessage("Enqueue", this);

        // This calls for the chromatic effect
        Camera.main.SendMessage("PlayerDamaged", 1);
    }
}
