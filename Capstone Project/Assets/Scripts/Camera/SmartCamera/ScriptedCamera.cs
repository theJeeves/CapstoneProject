using UnityEngine;
using System.Collections;

public abstract class ScriptedCamera : ScriptableObject {

    [SerializeField]
    protected bool _adjustFOV;
    [SerializeField, Range(150.0f, 250.0f)]
    protected float _toFOV = 150.0f;
    [SerializeField]
    protected float _FOVAdjustSpeed = 10.0f;

    [SerializeField]
    protected bool _adjustXPosition;
    [SerializeField, Range(0.0f, 1.0f)]
    protected float _toXPos = 0.5f;
    [SerializeField]
    protected float _XAdjustSpeed = 10.0f;

    [SerializeField]
    protected bool _adjustYPosition;
    [SerializeField, Range(0.0f, 1.0f)]
    protected float _toYPos = 0.5f;
    [SerializeField]
    protected float _YAdjustSpeed = 10.0f;

    protected GameObject _camera;
    protected float _time = 0.0f;

    private void OnEnable() {
        _camera = GameObject.FindGameObjectWithTag("SmartCamera");
    }

    public abstract bool MoveCamera(Vector2 playerPos);

    protected virtual void EnableScripts() {
        if (_adjustFOV) {
            _camera.GetComponent<SmartCameraFOV>().enabled = true;
        }
        if (_adjustXPosition) {
            _camera.GetComponent<SmartCameraXPosition>().enabled = true;
        }
        if (_adjustYPosition) {
            _camera.GetComponent<SmartCameraYPosition>().enabled = true;
        }
    }

    protected virtual void DisableScripts() {
        if (_adjustFOV) {
            _camera.GetComponent<SmartCameraFOV>().enabled = false;
        }
        if (_adjustXPosition) {
            _camera.GetComponent<SmartCameraXPosition>().enabled = false;
        }
        if (_adjustYPosition) {
            _camera.GetComponent<SmartCameraYPosition>().enabled = false;
        }
    }
}
