using UnityEngine;
using System.Collections;
using System;

[CreateAssetMenu(menuName ="Scripted Camera/Basic")]
public class BasicScriptedCamera : ScriptedCamera {

    public override bool MoveCamera(Vector2 playerPos) {

        DisableScripts();

        _time = _time == 0.0f ? Time.time : _time;
        float fromFOV = Camera.main.orthographicSize;
        Vector3 fromPos = Camera.main.WorldToViewportPoint(playerPos);

        if (_adjustFOV && Camera.main.orthographicSize < _toFOV) {
            Camera.main.orthographicSize = Mathf.SmoothStep(fromFOV, _toFOV + 2.0f, (Time.time - _time) / _FOVAdjustSpeed);
            return false;
        }
        if (_adjustXPosition && Camera.main.WorldToViewportPoint(playerPos).x > _toXPos) {
            _camera.transform.position += Vector3.right * _XAdjustSpeed;
            return false;
        }
        else {
            EnableScripts();
            _time = 0.0f;
            return true;
        }
    }
}
