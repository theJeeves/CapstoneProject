using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Screen Shake/Damage")]
public class DamageSSRequest : ScreenShakeRequest {

    public override Vector3 Shake() {
        Camera.main.SendMessage("PlayerDamaged", 1);
        return new Vector3(Random.insideUnitCircle.x * _shakeAmount, Random.insideUnitCircle.y * _shakeAmount, 0.0f);
    }
}
