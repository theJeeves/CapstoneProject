using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Screen Shake/Weapon")]
public class WeaponSSRequest : ScreenShakeRequest {

    private Vector3[] _directions = new Vector3[8];     // All possible angles which can be used by the player

    private void OnEnable() {
        // Define all the possible angles based.
        AssignDirections(0, 1.0f, 0.0f);
        AssignDirections(1, 0.7f, 0.7f);
        AssignDirections(2, 0.0f, 1.0f);
        AssignDirections(3, -0.7f, 0.7f);
        AssignDirections(4, -1.0f, 0.0f);
        AssignDirections(5, -0.7f, -0.7f);
        AssignDirections(6, 0.0f, -1.0f);
        AssignDirections(7, 0.7f, -0.7f);
    }

    private void AssignDirections(int angle, float x, float y, float z = 0.0f) {
        _directions[angle] = new Vector3(x, y, z);
    }

    public override Vector3 Shake(int key) {
        return _directions[key] * _shakeAmount;
    }

    public override void ShakeRequest() {
        Camera.main.SendMessage("Enqueue", this);
    }
}
