using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Scripted Camera/New Scripted Camera")]
public class ScriptedCamera : ScriptableObject {

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

    [SerializeField]
    private bool _rightTrigger = false;
    public bool IsRightTrigger {
        set { _rightTrigger = value; }
    }

    private bool _fovDone = false;
    private bool _xDone = false;

    private void OnEnable() {
        _camera = GameObject.FindGameObjectWithTag("SmartCamera");
    }

    public bool MoveCamera(Vector2 playerPos) {

        Debug.Log("move camera");

        _time = _time == 0.0f ? Time.time : _time;
        float fromFOV = Camera.main.orthographicSize;
        //Vector3 fromPos = Camera.main.WorldToViewportPoint(playerPos);

        if (_adjustFOV && Camera.main.orthographicSize < _toFOV) {
            Camera.main.orthographicSize = Mathf.SmoothStep(fromFOV, _toFOV + 2.0f, (Time.time - _time) / _FOVAdjustSpeed);
            Debug.Log("adjusting fov");
        }
        else { _fovDone = true; }

        switch (_rightTrigger) {
            case true:
                if (_adjustXPosition && Camera.main.WorldToViewportPoint(playerPos).x < 1.0f - _toXPos) {
                    _camera.transform.position += Vector3.left * _XAdjustSpeed;
                }
                else { _xDone = true; }
                break;

            case false:
                if (_adjustXPosition && Camera.main.WorldToViewportPoint(playerPos).x > _toXPos) {
                    _camera.transform.position += Vector3.right * _XAdjustSpeed;
                }
                else { _xDone = true; }
                break;
        }

        return _fovDone == true && _xDone == true ? true : false;
    }

    public void EnableScripts() {
        if (_adjustFOV) {
            _camera.GetComponent<SmartCameraFOV>().enabled = true;
        }
        if (_adjustXPosition) {
            _camera.GetComponent<SmartCameraXPosition>().enabled = true;
        }
        if (_adjustYPosition) {
            _camera.GetComponent<SmartCameraYPosition>().enabled = true;
        }
        _time = 0.0f;
        _xDone = false;
        _fovDone = false;
    }

    public void DisableScripts() {
        if (_adjustFOV) {
            _camera.GetComponent<SmartCameraFOV>().enabled = false;
        }
        if (_adjustXPosition) {
            _camera.GetComponent<SmartCameraXPosition>().enabled = false;
        }
        if (_adjustYPosition) {
            _camera.GetComponent<SmartCameraYPosition>().enabled = false;
        }
        _time = 0.0f;
        _xDone = false;
        _fovDone = false;
    }
}
