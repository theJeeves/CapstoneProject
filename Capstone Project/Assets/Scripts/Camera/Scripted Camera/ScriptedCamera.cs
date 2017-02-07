using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Scripted Camera/New Scripted Camera")]
public class ScriptedCamera : ScriptableObject {

    // Bool to determine if the script should adjust the FOV.
    [SerializeField]
    protected bool _adjustFOV;
    // Allows the developer to define how close or far the camera should pan in/out.
    [SerializeField, Range(25.0f, 350.0f)]
    protected float _toFOV = 150.0f;
    //The speed at which the FOV will adjust
    [SerializeField]
    protected float _FOVAdjustSpeed = 10.0f;

    [SerializeField]
    protected bool _adjustPosition;
    [SerializeField]
    protected Vector2 _target;
    [SerializeField]
    protected float _positionAdjustSpeed = 7.0f;

    protected GameObject _camera;       // Reference to the main Camera
    protected float _time = 0.0f;       // Used for timing and delay purposes

    // This is used to determine how the camera should move based on if the player will approach this trigger from the
    // left or right side. Just need one boolean to keep track of both.
    private bool _rightTrigger = false;
    public bool IsRightTrigger {
        set { _rightTrigger = value; }
    }

    private bool _fovDone = false;
    private bool _positionDone = false;
    //private bool _xDone = false;
    //private bool _yDone = false;

    private void OnEnable() {
        // Get a reference to the main camera to be used with the rest of the script.
        _camera = GameObject.FindGameObjectWithTag("SmartCamera");
    }

    public bool MoveCamera(Vector2 playerPos) {

        // If adjustment is needed, get the current time and the current orthographic size (aka FOV)
        _time = _time == 0.0f ? Time.time : _time;
        float fromFOV = Camera.main.orthographicSize;

        // Using a smooth transition, keep adjusting the fov until the target number has been reached.
        if (_adjustFOV && Camera.main.orthographicSize < _toFOV) {
            Camera.main.orthographicSize = Mathf.SmoothStep(fromFOV, _toFOV + 2.0f, (Time.time - _time) / _FOVAdjustSpeed);
        }
        else { _fovDone = true; Debug.Log("fov done"); }

        //if ( (_camera.transform.position.x < _target.x - 0.5f && _camera.transform.position.x > _target.x + 0.5f) || 
        //    (_camera.transform.position.y < _target.y - 0.5f && _camera.transform.position.y > _target.y + 0.5f)) {
        if ((Vector2)_camera.transform.position != _target) { 
            _camera.transform.position = new Vector3(Mathf.SmoothStep(_camera.transform.position.x, _target.x, (Time.time - _time) / _positionAdjustSpeed),
                Mathf.SmoothStep(_camera.transform.position.y, _target.y, (Time.time - _time) / _positionAdjustSpeed), -10.0f);
        }
        else {
            _positionDone = true; Debug.Log("position done");
        }

        return _fovDone && _positionDone ? true : false;
    }

    //Depending on what the developer wants to do, EnableScripts and DisableScripts will turn on/off
    //when a scripted camera event is needed. 
    public void EnableScripts() {
        if (_adjustFOV) {
            _camera.GetComponent<SmartCameraFOV>().enabled = true;
        }
        if (_adjustPosition) {
            _camera.GetComponent<SmartCameraXPosition>().enabled = true;
            _camera.GetComponent<SmartCameraYPosition>().enabled = true;
        }
        _time = 0.0f;
        _positionDone = false;
        _fovDone = false;
    }

    public void DisableScripts() {
        if (_adjustFOV) {
            _camera.GetComponent<SmartCameraFOV>().enabled = false;
        }
        if (_adjustPosition) {
            _camera.GetComponent<SmartCameraXPosition>().enabled = false;
            _camera.GetComponent<SmartCameraYPosition>().enabled = false;
        }
        _time = 0.0f;
        _positionDone = false;
        _fovDone = false;
    }
}
